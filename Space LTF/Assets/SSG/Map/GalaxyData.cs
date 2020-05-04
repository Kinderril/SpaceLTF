using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GlobalMapCellType
{
    army,
    eventMap,
    shop,

    //    modif,
    repair,
    nothing,
}


[Serializable]
public class GalaxyData
{
    private CellsInGalaxy cells;
    public int SizeX => cells.SizeX;
    public int SizeZ => cells.SizeZ;
    public int VerticalCount => _verticalCount;
    public int SizeOfSector { get; protected set; }
    public int StartDeathStep { get; protected set; }
    public int CellsDestroyed { get; private set; }
    public GalaxyEnemiesArmyController GalaxyEnemiesArmyController { get; private set; }
    public string Name;
    public const int VERTICAL_COUNT = 4;
    private float _powerPerTurn = 0f;
    public List<SectorData> AllSectors = new List<SectorData>();
    [field: NonSerialized] public event Action<int, int> OnWayDelete;

    protected int _verticalCount;

    protected int _sectorsCount;

    private Dictionary<GlobalMapEventType, int> _eventsCount = new Dictionary<GlobalMapEventType, int>();

    private Dictionary<GlobalMapEventType, int> _maxCount = new Dictionary<GlobalMapEventType, int>()
    {
        {GlobalMapEventType.creditStorage, 2},
        {GlobalMapEventType.prisoner, 2},
        {GlobalMapEventType.asteroidsField, 3},
        {GlobalMapEventType.scienceLab, 3},
        {GlobalMapEventType.anomaly, 3},
        {GlobalMapEventType.spaceGarbage, 3},
        {GlobalMapEventType.excavation, 3},
        {GlobalMapEventType.secretDeal, 2},
    };

    public GlobalMapCell Init2(int sectorCount, int sizeSector, int startPower, int coreCells,
        int startDeathStep, ShipConfig playerShipConfig, int powerPerTurn)
    {
        _verticalCount = VERTICAL_COUNT;
        _powerPerTurn = ConfigurePowerPerTurn(powerPerTurn);
        StartDeathStep = startDeathStep;

        var startCell = ImpletemtSectors(sectorCount, sizeSector, startPower, coreCells, playerShipConfig,
            _verticalCount);
        GalaxyEnemiesArmyController = new GalaxyEnemiesArmyController(GetAllList());
        return startCell;
    }


    protected virtual StartGlobalCell ImpletemtSectors(int sectorCount, int sizeSector, int startPower, int coreCells,
        ShipConfig playerShipConfig, int verticalCount)
    {
        var allSubSectors = new List<SectorData>();
        var unPopulatedSectors = new List<SectorData>();
        var step = sizeSector + 1;
        SizeOfSector = step;
        _sectorsCount = sectorCount;
        Debug.Log($"global map size: {_sectorsCount} {verticalCount}");
        var sectors = new SectorData[_sectorsCount, verticalCount];
        var id = 0;
        var maxDist = 0;
        for (var i = 0; i < _sectorsCount; i++)
        for (var j = 0; j < verticalCount; j++)
        {
            var xx = i * step;
            var zz = j * step;
            var distToStart = i + j;
            SectorData subSector = null;
            var shipConfig = GetConfig(i, j, _sectorsCount);
            var upper = j == verticalCount - 1;
            var bot = j == 0;
            if (upper || bot)
            {
                if (MyExtensions.IsTrue01(.75f) && i < sectorCount - 1)
                {
                    var isUp = upper;
                    subSector = new SectorDungeon(xx, zz, sizeSector,
                        _eventsCount, shipConfig, id, i, _powerPerTurn, isUp, DeletedWays);
                }
            }
            else
            {
                subSector = new SectorData(xx, zz, sizeSector,
                    _eventsCount, shipConfig, id, i, _powerPerTurn, DeletedWays);
            }

            id++;
            if (subSector != null)
            {
                allSubSectors.Add(subSector);
                unPopulatedSectors.Add(subSector);
                if (distToStart > maxDist) maxDist = distToStart;
                sectors[i, j] = subSector;
            }
        }


        //Create start sector       
        var startSector = sectors[0, MyExtensions.Random(1, verticalCount - 2)];
        //        var startSector = allSubSectors.Where(x => x.StartX == 0 && x.S).ToList().RandomElement();
        startSector.ChangeSectorOwner(playerShipConfig);
        var xCell = startSector.StartX + sizeSector / 2;
        var zCell = startSector.StartZ + sizeSector / 2;
        var startCell = new StartGlobalCell(Utils.GetId(), xCell, zCell, startSector, playerShipConfig);
        startSector.SetCell(startCell, id);
        startSector.MarkAsStart();
        unPopulatedSectors.Remove(startSector);
        startSector.Populate(startPower);
        startSector.MarkAsVisited();

        //CreateEndSector   
        var endSector = sectors[_sectorsCount - 1, MyExtensions.Random(1, verticalCount - 2)];


        xCell = endSector.StartX + 1 + RndIndex(sizeSector - 1);
        zCell = endSector.StartZ + RndIndex(sizeSector);
        var endCell = new EndGlobalCell(startPower, Utils.GetId(), xCell, zCell, endSector);
        endSector.SetCell(endCell, id);
        unPopulatedSectors.Remove(endSector);
        endSector.Populate(startPower);
        Debug.Log($"CreateEndSector : {xCell} {zCell}");

        //Create core sectors
        var randomInts = new List<int>();
        for (var i = 2; i < sectorCount - 1; i++) randomInts.Add(i);

        //        var takeThisIndex = 1;

        var xIndexs = randomInts.RandomElement(coreCells - 1);
        xIndexs.Add(1);
        var goodCorePositions = xIndexs.Count;
        //        var goodCorePositions = Mathf.Clamp(coreCells, 0, sectorsCount - 2);
        int createdCoreSector = 0;
        for (var i = 0; i < xIndexs.Count; i++)
        {
            var secrosDist = xIndexs[i];
            List<SectorData> possibleSectors;
            if (createdCoreSector > 1 && MyExtensions.IsTrue01(.3f) && allSubSectors.Count > verticalCount - 2)
                //Only dungeons
                possibleSectors = allSubSectors.Where(x => x.XIndex == secrosDist && x is SectorDungeon).ToList();
            else
                //Only inner
                possibleSectors = allSubSectors.Where(x => x.XIndex == secrosDist && !(x is SectorDungeon)).ToList();

            var coreSector = possibleSectors.RandomElement();
            if (coreSector == null)
                Debug.LogError($"can't find sector with dist:{secrosDist}");
            else
            {
                createdCoreSector++;
                CreateCoreSector(coreSector, startSector, id++, unPopulatedSectors, startPower, sizeSector);
            }
        }

        var notGoodCores = coreCells - goodCorePositions;
        for (var i = 0; i < notGoodCores; i++)
        {
            var coreSector = allSubSectors.Where(x => !x.IsPopulated && x.StartX != 0 && x != null).ToList()
                .RandomElement();
            if (coreSector != null)
                CreateCoreSector(coreSector, startSector, id++, unPopulatedSectors, startPower, sizeSector);
        }

        foreach (var sectorData in unPopulatedSectors)
        {
            Debug.Log($"Populate others : {sectorData.StartX} {sectorData.StartZ}");
            sectorData.Populate(startPower);
        }

        foreach (var sectorData in allSubSectors)
        {
            AllSectors.Add(sectorData);
            sectorData.CacheWays();
        }

        AddPortals(_sectorsCount, sectors, verticalCount);
        AddExitsFromDungeons(sectorCount, sectors, verticalCount);
        InplementSectorToGalaxy(sectors, sizeSector, _sectorsCount, verticalCount);

        Debug.Log("Population end");
#if UNITY_EDITOR
        var list = cells.GetAllList();
        var cores = list.Where(x => x is CoreGlobalMapCell).ToList();
        if (cores.Count != coreCells)
            Debug.LogError(
                $"Wrong count of core cells {cores.Count}/{coreCells}. goodCorePositions:{goodCorePositions}. notGoodCores:{notGoodCores}");
#endif
        return startCell;
    }

    public void RemoveAllWaysFromAnotherSectorToCell(GlobalMapCell cell)
    {
        Debug.LogError($"RemoveAllWaysFromAnotherSectorToCell start  {cell.Id} indX:{cell.indX} indz:{cell.indZ}");
        var waysToRemove = new List<GlobalMapCell>();
        var cells = GetAllList();
        foreach (var globalMapCell in cells)
        {
            var ways = globalMapCell.GetCurrentPosibleWays();
            if (ways.Contains(cell) && globalMapCell.SectorId != cell.SectorId)
            {
                Debug.LogError(
                    $"RemoveAllWaysFromAnotherSectorToCell waysToRemove.Add(cell):{globalMapCell.Id}  indX:{globalMapCell.indX} indz:{globalMapCell.indZ}");
                waysToRemove.Add(globalMapCell);
            }
        }

        foreach (var globalMapCell in waysToRemove)
        {
            globalMapCell.RemoveWayTo(cell);
            OnWayDelete?.Invoke(globalMapCell.Id, cell.Id);
        }
    }

    private void DeletedWays(GlobalMapCell to)
    {
        RemoveAllWaysFromAnotherSectorToCell(to);
    }

    private float ConfigurePowerPerTurn(int powerPerTurn)
    {
        var d = (float) powerPerTurn * 0.03f;
        return d;
    }

    private void CreateCoreSector(SectorData coreSector, SectorData startSector, int id,
        List<SectorData> unPopulatedSectors,
        int startPower, int sizeSector)
    {
        var xCell = coreSector.StartX + RndIndex(sizeSector);
        var zCell = coreSector.StartZ + RndIndex(sizeSector);

        //#if UNITY_EDITOR      
        //        var s = _sectorsCount * (sizeSector + 1) - 1;
        //
        //        if (zCell >= s)
        //        {
        //            Debug.LogError($"How it posible core:{ coreSector.StartZ}  s:{s}");
        //            return;
        //        }
        //#endif
        var coreId = Utils.GetId();
        var coreCell = new CoreGlobalMapCell(xCell + zCell, coreId, xCell, zCell, coreSector);
        coreSector.Populate(startPower);
        coreSector.SetCell(coreCell, id);
        unPopulatedSectors.Remove(coreSector);
        coreSector.MarkAsCore(coreId, coreCell);
        Debug.Log($"Core created {coreSector.StartX} {coreSector.StartZ}.  coreCell:{coreCell.indX} {coreCell.indZ}");
    }

    public List<GlobalMapCell> GetAllList()
    {
        return cells.GetAllList();
    }

    private int RndIndex(int upsizeZoneSector)
    {
        return MyExtensions.Random(0, upsizeZoneSector - 1);
    }

    private void AddExitsFromDungeons(int sectorsCount, SectorData[,] sectors, int verticalCount)
    {
        for (var i = 0; i < sectorsCount; i++)
        for (var j = 0; j < verticalCount; j++)
        {
            var subSector = sectors[i, j];
            if (subSector == null) continue;

            var dungeon = subSector as SectorDungeon;
            if (dungeon != null)
            {
                //                    Debug.LogError("FINDED DUNGEON");
                SectorData sectorConnected = null;
                var pointToExit = dungeon.ExitCell;
                var connectedSecotAtBot = j == 0;
                var connectedSecotAtTop = j == verticalCount - 1;
                if (connectedSecotAtBot)
                    sectorConnected = sectors[i, 1];
                else if (connectedSecotAtTop) sectorConnected = sectors[i, verticalCount - 2];

                if (sectorConnected != null)
                {
                    var cellToConnect = FindEndExtraPortal(sectorConnected, connectedSecotAtBot);
                    if (cellToConnect != null && cellToConnect.Data != null)
                        //                            Debug.LogError($"Extra portal {pointToExit}   to {cellToConnect.Data}");
                        pointToExit.AddWay(cellToConnect.Data);
                    else
                        Debug.LogError("Can't find FindEndExtraPortal");
                }
                else
                {
                    Debug.LogError("Can't find sector to create exit of military portal");
                }
            }
        }
    }

    private SectorCellContainer FindEndExtraPortal(SectorData sectorConnected, bool reverse)
    {
        if (reverse)
            for (var k = 0; k < sectorConnected.Size; k++)
            for (var m = 0; m < sectorConnected.Size; m++)
            {
                var cellToTest = sectorConnected.Cells[m, k];
                if (cellToTest != null && cellToTest.Data != null &&
                    !(cellToTest.Data is GlobalMapNothing))
                    return cellToTest;
            }
        else
            for (var k = sectorConnected.Size - 1; k >= 0; k--)
            for (var m = 0; m < sectorConnected.Size; m++)
            {
                var cellToTest = sectorConnected.Cells[m, k];
                if (cellToTest != null && cellToTest.Data != null &&
                    !(cellToTest.Data is GlobalMapNothing))
                    return cellToTest;
            }


        return null;
    }

    private void AddPortals(int sectorsCount, SectorData[,] sectors, int verticalCount)
    {
        for (var i = 0; i < sectorsCount; i++)
        for (var j = 0; j < verticalCount - 1; j++)
        {
            var subSector = sectors[i, j];
            if (subSector == null) continue;
            if (i + 1 < sectorsCount && j > 0)
            {
                var rightConnectSector = sectors[i + 1, j];
                if (rightConnectSector == null)
                    continue;
                ConnectSectorsRight(subSector, rightConnectSector);
            }

            if (j + 1 < verticalCount)
            {
                var topConnectedSector = sectors[i, j + 1];
                if (topConnectedSector == null)
                    continue;
                ConnectSectorTop(subSector, topConnectedSector);
            }
        }
    }

    private void ConnectSectorsRight(SectorData left, SectorData right)
    {
        var leftCells = new HashSet<GlobalMapCell>();
        var moveIndex = 1;
        while (leftCells.Count < 2 && left.Size - moveIndex > 0)
        {
            for (var i = 0; i < left.Size; i++)
            {
                var cell = left.Cells[left.Size - moveIndex, i];
                if (!(cell.Data is GlobalMapNothing)) leftCells.Add(cell.Data);
            }

            moveIndex++;
        }


        var rightCells = new HashSet<GlobalMapCell>();
        moveIndex = 0;
        while (rightCells.Count < 2 && moveIndex < right.Size)
        {
            for (var i = 0; i < right.Size; i++)
            {
                var cell = right.Cells[moveIndex, i];
                if (!(cell.Data is GlobalMapNothing)) rightCells.Add(cell.Data);
            }

            moveIndex++;
        }


        ConnectedCellsListsFirst(rightCells, leftCells, "RightLeft", 2);
    }

    private void ConnectSectorTop(SectorData bot, SectorData top)
    {
        var botCells = new HashSet<GlobalMapCell>();
        var moveIndex = 1;
        var targetConnections = bot is SectorDungeon || top is SectorDungeon ? 1 : 2;

        while (botCells.Count < targetConnections && bot.Size - moveIndex > 0)
        {
            for (var i = 0; i < bot.Size; i++)
            {
                var cell = bot.Cells[i, bot.Size - moveIndex];
                if (!(cell.Data is GlobalMapNothing))
                    if (cell.Data != null)
                        botCells.Add(cell.Data);
            }

            moveIndex++;
        }

        moveIndex = 0;
        var topCells = new HashSet<GlobalMapCell>();
        while (topCells.Count < targetConnections && moveIndex < bot.Size)
        {
            for (var i = 0; i < top.Size; i++)
            {
                var cell = top.Cells[i, moveIndex];
                if (!(cell.Data is GlobalMapNothing))
                    if (cell.Data != null)
                        topCells.Add(cell.Data);
            }

            moveIndex++;
        }

        if (topCells.Count == 0 || botCells.Count == 0) Debug.LogError("Sectors have not enought portals");

        //        if (targetConnections == 1)
        //        {
        ////            Debug.LogError($"Connected sectors {bot} ID:{bot.Id} Index:{bot.XIndex} and {top} ID:{top.Id} Index:{top.XIndex}   counts:{topCells.Count} {botCells.Count}");
        //
        //        }

        ConnectedCellsListsFirst(topCells, botCells, "TopBottom", targetConnections);
    }

    private void ConnectedCellsListsFirst(HashSet<GlobalMapCell> rightCells, HashSet<GlobalMapCell> leftCells,
        string info, int targetCount)
    {
        if (leftCells.Count < targetCount)
        {
            Debug.LogError($"{info} cell less < {targetCount}....");
            return;
        }

        if (rightCells.Count < targetCount)
        {
            Debug.LogError($"{info}t cell less < {targetCount}....");
            return;
        }

        if (MyExtensions.IsTrueEqual())
            ConnectedCellsListsSecond(leftCells, rightCells, info, targetCount);
        else
            ConnectedCellsListsSecond(rightCells, leftCells, info, targetCount);
    }


    private void ConnectedCellsListsSecond(HashSet<GlobalMapCell> rightCells, HashSet<GlobalMapCell> leftCells,
        string info, int targetCount)
    {
#if UNITY_EDITOR
        var connectedSomething = false;
#endif
        //        Debug.Log($"ConnectedCellsListsSecond rightCells:{rightCells.Count}. leftCells:{leftCells.Count}. {info}");
        if (rightCells.Count < targetCount || rightCells.Count < targetCount)
        {
            var cellL = leftCells.ToList().RandomElement();
            var cellR = rightCells.ToList().RandomElement();
            ConnectCells(cellL, cellR, info);
#if UNITY_EDITOR
            if (targetCount == 1 && cellL != null && cellR != null)
            {
                connectedSomething = true;
                Debug.LogError($"ConnectCells c1:{cellL.indX},{cellL.indZ}  =>  c2:{cellR.indX},{cellR.indZ}");
            }
#endif
        }
        else
        {
            var cellsL = leftCells.ToList().Suffle();
            var cellsR = rightCells.ToList().Suffle();
            for (var i = 0; i < targetCount; i++)
            {
                var c1 = cellsL[i];
                for (var j = 0; j < targetCount; j++)
                {
                    var c2 = cellsR[j];
                    ConnectCells(c1, c2, info);
#if UNITY_EDITOR
                    if (targetCount == 1 && c1 != null && c2 != null)
                        connectedSomething = true;
                    //                        Debug.LogError($"ConnectCells c1:{c1.indX},{c1.indZ}  =>  c2:{c2.indX},{c2.indZ}");
#endif
                }
            }
        }
#if UNITY_EDITOR
        if (targetCount == 1 && !connectedSomething) Debug.LogError($"Nothing CONNECTED!");
#endif
    }

    private void ConnectCells(GlobalMapCell cellL, GlobalMapCell cellR, string info)
    {
        //        Debug.Log($"Connect cell ({cellL.indX};{cellL.indZ})<->({cellR.indX};{cellR.indZ}) __ info:{info}");
        if (cellL != null && cellR != null)
        {
            cellL.AddWay(cellR);
            cellR.AddWay(cellL);
        }
    }

    protected void InplementSectorToGalaxy(SectorData[,] sectors, int sizeZoneSector, int sectorsCount, int verticalCount)
    {
        //        Debug.Log(Namings.TryFormat("InplementSectorToGalaxy : sizeSector:{0}   sectorsCount:{1}", sizeSector, sectorsCount) );
        var SizeX = sectorsCount * (sizeZoneSector + 1) - 1;
        var SizeZ = verticalCount * (sizeZoneSector + 1) - 1;
        cells = new CellsInGalaxy(SizeX, SizeZ);
        for (var i = 0; i < sectorsCount; i++)
        {
            for (var j = 0; j < verticalCount; j++)
            {
                var subSector = sectors[i, j];
                if (subSector != null)
                {
                    SetNullsTo(cells, subSector, sizeZoneSector);
                    subSector.ApplyPointsTo(cells);
                }
            }
        }
    }

    private void SetNullsTo(CellsInGalaxy cellsInGalaxy, SectorData sector, int sizeZoneSector)
    {
        var verticalNullsStartX = sector.StartX + sizeZoneSector;
        if (sector.StartZ < cellsInGalaxy.SizeZ)
            for (var i = 0; i < sizeZoneSector + 1; i++)
            {
                var xx = verticalNullsStartX;
                if (xx < cellsInGalaxy.SizeX)
                {
                    var zz = sector.StartZ + i;
                    if (zz < cellsInGalaxy.SizeZ)
                    {
                        var nothing = new GlobalMapNothing(Utils.GetId(), xx, zz, sector, sector.ShipConfig);
                        cellsInGalaxy.SetCell(nothing);
                        //                        Debug.Log(Namings.TryFormat("Set nothing to {0}  {1}", nothing.indX, nothing.indZ));
                    }
                }
            }

        var horizontalNullsStartZ = sector.StartZ + sizeZoneSector;
        if (sector.StartX < cellsInGalaxy.SizeX)
            for (var i = 0; i < sizeZoneSector + 1; i++)
            {
                var zz = horizontalNullsStartZ;
                if (zz < cellsInGalaxy.SizeZ)
                {
                    var xx = sector.StartX + i;
                    if (xx < cellsInGalaxy.SizeX)
                    {
                        var nothing = new GlobalMapNothing(Utils.GetId(), xx, zz, sector, sector.ShipConfig);
                        cellsInGalaxy.SetCell(nothing);
                        //                        Debug.Log(Namings.TryFormat("Set nothing to {0}  {1}", nothing.indX, nothing.indZ));
                    }
                }
            }
    }

    private ShipConfig GetConfig(int i, int j, int subZoneCount)
    {
        var configsList = new List<ShipConfig>()
        {
            ShipConfig.mercenary,
            ShipConfig.raiders, ShipConfig.krios, ShipConfig.ocrons, ShipConfig.federation,
        };

        return configsList.RandomElement();
        //
        // Dictionary<ShipConfig, float> d;
        // if (i == 0)
        // {
        //     d = new Dictionary<ShipConfig, float>() { { ShipConfig.mercenary, 1 }, { ShipConfig.raiders, 2 }, };
        // }
        // else
        // {
        //     if (i == subZoneCount - 1)
        //     {
        //         d = new Dictionary<ShipConfig, float>() { { ShipConfig.federation, 3 }, { ShipConfig.krios, 3 }, { ShipConfig.ocrons, 3 }, };
        //     }
        //     else
        //     {
        //         if (i < subZoneCount / 2)
        //         {
        //
        //             d = new Dictionary<ShipConfig, float>() { { ShipConfig.mercenary, 2 }, { ShipConfig.krios, 1 }, { ShipConfig.ocrons, 1 }, { ShipConfig.federation, 1 }, };
        //         }
        //         else
        //         {
        //             d = new Dictionary<ShipConfig, float>() { { ShipConfig.federation, 3 }, { ShipConfig.krios, 3 }, { ShipConfig.ocrons, 3 }, };
        //
        //         }
        //     }
        //
        // }
        // var configs = new WDictionary<ShipConfig>(d);
        // return configs.Random();
    }

    private void OnCellScouted(GlobalMapCell obj)
    {
        if (obj is CoreGlobalMapCell)
        {
            var idToUnconnect = obj.ConnectedGates;
            for (var i = 0; i < SizeX; i++)
            for (var j = 0; j < SizeZ; j++)
            {
                var cell = cells.GetCell(i, j);
                if (cell != null)
                {
                    var shallUnconect = cell.ConnectedGates == idToUnconnect;
                    //                        Debug.LogErrorFormat(" cells: {0} {1} " , cell.ConnectedGates , obj.ConnectedGates);
                    if (shallUnconect)
                        //                            Debug.LogError("UNCONNECTED!!!");
                        cell.UnconnectAll();
                }
            }
        }
        else if (obj.ConnectedGates > 0)
        {
            obj.UnconnectAll();
        }
    }

    public GlobalMapCell GetRandomClosestCellWithNoData(ShipConfig config, int indX, int indZ)
    {
        return cells.GetRandomClosestCellWithNoData(config, indX, indZ);
    }

    private bool CanAdd(GlobalMapEventType eventType)
    {
        if (_maxCount.ContainsKey(eventType))
            return _eventsCount[eventType] < _maxCount[eventType];
        else
            return true;
    }

    private int CalcPowerOfCell(int startPower, int distToStart, int mapSize)
    {
        var deltaPerCell = (Library.MAX_ARMY_POWER_MAP - startPower) / mapSize;
        return startPower + (int) (distToStart * deltaPerCell);
    }

    public GlobalMapCell GetRandomConnectedCell()
    {
        return cells.GetRandomConnectedCell();
    }

    public GalaxyData(string name)
    {
        Name = name;
    }

    public void StepComplete(int step, GlobalMapCell curCell)
    {
        GalaxyEnemiesArmyController.CheckEnemiesMovingArmies(step, curCell);
    }

    public void StepStart(int step, GlobalMapCell curCell)
    {
    }


    public GlobalMapCell GetRandomCell()
    {
        var cells1 = cells.GetRandom();
        return cells1;
    }

    // private void TryDestroyCell(GlobalMapCell curCell)
    // {
    //     var rnds = cells.GetAllList().Where(x => x.SectorId == curCell.SectorId && x.CanCellDestroy()).ToList();
    //     if (rnds.Count > 0)
    //     {
    //         var rnd = rnds.RandomElement();
    //         CellsDestroyed++;
    //         rnd.DestroyCell();
    //         var ways = rnd.GetCurrentPosibleWays();
    //         foreach (var globalMapCell in ways)
    //         {
    //             globalMapCell.RemoveWayTo(rnd);
    //         }
    //         MainController.Instance.MainPlayer.MessagesToConsole.AddMsg("Cell destroyed");
    //
    //
    //     }
    // }


    //    public List<GlobalMapCell> ConnectedCellsToCurrent(GlobalMapCell CurrentCell)
    //    {
    //        var XINdex = CurrentCell.indX;
    //        var ZINdex = CurrentCell.indZ;
    //        return ConnectedCellsToCurrent(XINdex, ZINdex);
    //    }


    public GlobalMapCell[,] AllCells()
    {
        return cells.GetAllCells();
    }

    public void Dispose()
    {
        GalaxyEnemiesArmyController.Dispose();
    }

    public int GetAllWaysCount()
    {
        var logic = 0;
        foreach (var globalMapCell in GetAllList()) logic += globalMapCell.GetCurrentPosibleWays().Count;

        return logic;
    }
}