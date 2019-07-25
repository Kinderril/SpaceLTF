using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;
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



[System.Serializable]
public class GalaxyData
{
    private CellsInGalaxy cells;
    public int Size { get;private set;}
    public int StartDeathStep { get;private set;}
    public string Name;
//    public int startIndexX { get; private set; }
//    public int startIndexZ { get; private set; }
    private  const int zoneCount = 5;

    Dictionary<GlobalMapEventType, int> _eventsCount = new Dictionary<GlobalMapEventType, int>();

    private Dictionary<GlobalMapEventType, int> _maxCount = new Dictionary<GlobalMapEventType, int>()
    {
        {GlobalMapEventType.retranslatorMessage, 2},
        {GlobalMapEventType.prisoner, 2},
        {GlobalMapEventType.asteroidsField, 3},
        {GlobalMapEventType.scienceLab, 3},
        {GlobalMapEventType.anomaly, 3},
        {GlobalMapEventType.spaceGarbage, 3},
    };
            /*
    public static void InitExamples()
    {
        WDictionary<ShipConfig> lvl1 = new WDictionary<ShipConfig>(new Dictionary<ShipConfig, float>
        {
            {ShipConfig.raiders, 6},
            {ShipConfig.mercenary, 2},
            {ShipConfig.ocrons, 1},
        });
        WDictionary<ShipConfig> lvl2 = new WDictionary<ShipConfig>(new Dictionary<ShipConfig, float>
        {
            {ShipConfig.raiders, 3},
            {ShipConfig.mercenary, 3},
            {ShipConfig.federation, 1},
            {ShipConfig.ocrons, 3},
        });
        WDictionary<ShipConfig> lvl3= new WDictionary<ShipConfig>(new Dictionary<ShipConfig, float>
        {
            {ShipConfig.mercenary, 1},
            {ShipConfig.federation, 2},
            {ShipConfig.ocrons, 1},
        });
        WDictionary<ShipConfig> lvl4 = new WDictionary<ShipConfig>(new Dictionary<ShipConfig, float>
        {
            {ShipConfig.federation, 3},
            {ShipConfig.ocrons, 1},
        });

        var d = new Dictionary<int, WDictionary<ShipConfig>>
        {
            { 1,lvl1},
            { 2,lvl2},
            { 3,lvl3},
            { 4,lvl4},
        };

        ShipsDictionary = d;
    }

    private static Dictionary<int, WDictionary<ShipConfig>> ShipsDictionary;
          */
    public GlobalMapCell Init2(int sectorCount, int sizeSector,int startPower, int coreCells, int startDeathStep)
    {
        StartDeathStep = startDeathStep;
        var allSubSectors =new List<SectorData>();
        var unPopulatedSectors =new List<SectorData>();
        var step = (sizeSector + 1);
        var sectorsCount = sectorCount;
        var mapSize = sectorsCount * sizeSector;
        SectorData[,] sectors = new SectorData[sectorsCount, sectorsCount];
        int id = 0;
        int maxDist = 0;
        for (int i = 0; i < sectorsCount; i++)
        {
            for (int j = 0; j < sectorsCount; j++)
            {
                var xx = i * step;
                var zz = j * step;
                var distToStart = i + j;
                ShipConfig shipConfig = GetConfig(i,j, sectorsCount);
                var subSector = new SectorData(xx, zz, sizeSector,_eventsCount,  shipConfig,id);
                id++;
                allSubSectors.Add(subSector);
                unPopulatedSectors.Add(subSector);
                if (distToStart > maxDist)
                {
                    maxDist = distToStart;
                }
                sectors[i, j] = subSector;
            }
        }

        
        //Create start sector
        var startSector = allSubSectors.Where(x => x.StartX == 0).ToList().RandomElement();
        var xCell = startSector.StartX + RndIndex(sizeSector);
        var zCell = startSector.StartZ + RndIndex(sizeSector);
        var startCell = new StartGlobalCell(Utils.GetId(), xCell, zCell);
//        startSector.RandomizeBorders();
        startSector.SetCell(startCell,id);
        unPopulatedSectors.Remove(startSector);
        startSector.Populate(startPower,null);
        //CreateEndSector   

        var endSector = sectors[sectorsCount - 1, RndIndex(sectorsCount)];
        xCell = endSector.StartX + RndIndex(sizeSector);
        zCell = endSector.StartZ + RndIndex(sizeSector);
        var endCell = new EndGlobalCell(Utils.GetId(), xCell, zCell);
        endSector.SetCell(endCell,id);
        unPopulatedSectors.Remove(endSector);
        endSector.Populate(startPower,startSector);

        //Create core sectors
        coreCells = Mathf.Clamp(coreCells, 0, maxDist - 2);
        for (int i = 0; i < coreCells; i++)
        {
            var secrosDist = maxDist - 1 - i;
            var coreSector = allSubSectors.Where(x => x.StartX/ sizeSector + x.StartZ / sizeSector == secrosDist).ToList().RandomElement();
            if (coreSector == null)
            {
                Debug.LogError($"can't find sector with dist:{secrosDist}");
            }
            else
            {
                xCell = coreSector.StartX + RndIndex(sizeSector);
                zCell = coreSector.StartZ + RndIndex(sizeSector);
                var coreId = Utils.GetId();
                var coreCell = new CoreGlobalMapCell(xCell + zCell, coreId, xCell, zCell);
                coreSector.SetCell(coreCell, id);
                coreSector.Populate(startPower, startSector);
                unPopulatedSectors.Remove(coreSector);
                coreSector.MarkAsCore(coreId);
                Debug.Log($"Core created {coreSector.StartX} {coreSector.StartZ}.");
            }
        }


        foreach (var sectorData in unPopulatedSectors)
        {
            sectorData.Populate(startPower, startSector);
        }

        foreach (var sectorData in allSubSectors)
        {
            sectorData.CacheWays();
        }

        AddPortals(sectorsCount,sectors);
        PopulateBigMap(sectors, sizeSector, sectorsCount);


        Debug.Log("Population end");
#if UNITY_EDITOR
        var list = cells.GetAllList();
        var cores = list.Where(x => x is CoreGlobalMapCell).ToList();
        if (cores.Count != coreCells)
        {
            Debug.LogError($"Wrong count of core cells {cores.Count}/{coreCells}.");
        }
#endif
        //        for (int i = 0; i < cells.Size; i++)
        //        {
        //            for (int j = 0; j < cells.Size; j++)
        //            {
        //                var cell = cells.GetCell(i, j  );
        //                var str = String.Format("Cell:: {0} {1}     _   {2} ", i, j, cell);
        //                if (cell == null)
        //                {
        //                   Debug.LogError(str);
        //                }
        //                else
        //                {
        //                    Debug.Log(str);
        //                }
        //            }
        //        }
        return startCell;
    }
    public List<GlobalMapCell> GetAllList()
    {
        return cells.GetAllList();
    }

    private int RndIndex(int upsizeZoneSector)
    {
        return MyExtensions.Random(0, upsizeZoneSector-1);
    }

    private void AddPortals(int sectorsCount, SectorData[,] sectors)
    {

        for (int i = 0; i < sectorsCount; i++)
        {
            for (int j = 0; j < sectorsCount; j++)
            {
                var subSector = sectors[i, j];
                if (i + 1 < sectorsCount)
                {
                    var rightConnectSector = sectors[i + 1, j];
                    ConnectSectorsRight(subSector, rightConnectSector);
                }

                if (j + 1 < sectorsCount)
                {

                    var topConnectedSector = sectors[i, j + 1];
                    ConnectSectorTop(subSector, topConnectedSector);
                }
            }
        }
    }

    private void ConnectSectorsRight(SectorData left, SectorData right)
    {
        var leftCells = new HashSet<GlobalMapCell>();
        int moveIndex = 1;
        while (leftCells.Count < 2 && left.Size - moveIndex > 0)
        {
            for (int i = 0; i < left.Size; i++)
            {
                var cell = left.Cells[left.Size - moveIndex, i];
                if (!(cell.Data is GlobalMapNothing))
                {
                    leftCells.Add(cell.Data);
                }
            }

            moveIndex++;
        }


        var rightCells = new HashSet<GlobalMapCell>();
        moveIndex = 0;
        while (rightCells.Count < 2 && moveIndex < right.Size)
        {
            for (int i = 0; i < right.Size; i++)
            {
                var cell = right.Cells[moveIndex, i];
                if (!(cell.Data is GlobalMapNothing))
                {
                    rightCells.Add(cell.Data);
                }
            }
            moveIndex++;
        }


        ConnectedCellsListsFirst(rightCells, leftCells,"RL");

    }

    private void ConnectSectorTop(SectorData bot, SectorData top)
    {
        HashSet< GlobalMapCell > botCells = new HashSet<GlobalMapCell>();
        int moveIndex = 1;
        while (botCells.Count < 2 && bot.Size - moveIndex > 0)
        {
            for (int i = 0; i < bot.Size; i++)
            {
                var cell = bot.Cells[i, bot.Size - moveIndex];
                if (!(cell.Data is GlobalMapNothing))
                {
                    botCells.Add(cell.Data);
                }
            }

            moveIndex++;
        }
        moveIndex = 0;
        HashSet<GlobalMapCell> topCells = new HashSet<GlobalMapCell>();
        while (topCells.Count < 2 && moveIndex < bot.Size)
        {
            for (int i = 0; i < top.Size; i++)
            {
                var cell = top.Cells[i, moveIndex];
                if (!(cell.Data is GlobalMapNothing))
                {
                    topCells.Add(cell.Data);
                }
            }

            moveIndex++;
        }
        
        ConnectedCellsListsFirst(topCells, botCells, "TB");

    }

    private void ConnectedCellsListsFirst(HashSet<GlobalMapCell> rightCells, HashSet<GlobalMapCell> leftCells,string info)
    {
        if (leftCells.Count < 2)
        {
            Debug.LogError("ConnectSectorsRight left cell less < 2....");
            return;
        }
        if (rightCells.Count < 2)
        {
            Debug.LogError("ConnectSectorsRight right cell less < 2...." );
            return;
        }

        if (MyExtensions.IsTrueEqual())
        {
            ConnectedCellsListsSecond(leftCells, rightCells, info);
        }
        else
        {
            ConnectedCellsListsSecond(rightCells, leftCells,info);
        }
    }


    private void ConnectedCellsListsSecond(HashSet<GlobalMapCell> rightCells, HashSet<GlobalMapCell> leftCells, string info)
    {
//        Debug.Log($"ConnectedCellsListsSecond rightCells:{rightCells.Count}. leftCells:{leftCells.Count}. {info}");
        if (rightCells.Count < 2 || rightCells.Count < 2)
        {
            var cellL = leftCells.ToList().RandomElement();
            var cellR = rightCells.ToList().RandomElement();
            ConnectCells(cellL, cellR,info);
        }
        else
        {
            var cellsL = leftCells.ToList().Suffle();
            var cellsR = rightCells.ToList().Suffle();
            for (int i = 0; i < 2; i++)
            {
                var c1 = cellsL[i];
                for (int j = 0; j < 2; j++)
                {
                    var c2 = cellsR[j];
                    ConnectCells(c1, c2, info);
                }
            }
        }
    }

    private void ConnectCells(GlobalMapCell cellL, GlobalMapCell cellR, string info)
    {
//        Debug.Log($"Connect cell ({cellL.indX};{cellL.indZ})<->({cellR.indX};{cellR.indZ}) __ info:{info}");
        cellL.AddWay(cellR);
        cellR.AddWay(cellL);
    }

    private void PopulateBigMap(SectorData[,] sectors, int sizeZoneSector,int sectorsCount)
    {
//        Debug.Log(String.Format("PopulateBigMap : sizeSector:{0}   sectorsCount:{1}", sizeSector, sectorsCount) );
        Size = sectorsCount * (sizeZoneSector + 1) - 1;
        cells = new CellsInGalaxy(Size);
        for (int i = 0; i < sectorsCount; i++)
        {
            for (int j = 0; j < sectorsCount; j++)
            {
                var subSector = sectors[i, j];
                SetNullsTo(cells,subSector, sizeZoneSector);
                subSector.ApplyPointsTo(cells);
            }
        }

    }

    private void SetNullsTo(CellsInGalaxy cellsInGalaxy, SectorData sector, int sizeZoneSector)
    {
        var verticalNullsStartX = sector.StartX + sizeZoneSector;
        if (sector.StartZ < cellsInGalaxy.Size)
        {
            for (int i = 0; i < sizeZoneSector + 1; i++)
            {
                var xx = verticalNullsStartX ;
                if (xx < cellsInGalaxy.Size)
                {
                    var zz = sector.StartZ + i;
                    if (zz < cellsInGalaxy.Size)
                    {
                        var nothing = new GlobalMapNothing(Utils.GetId(), xx, zz);
                        cellsInGalaxy.SetCell(nothing);
//                        Debug.Log(String.Format("Set nothing to {0}  {1}", nothing.indX, nothing.indZ));
                    }
                }
            }
        }

        var horizontalNullsStartZ = sector.StartZ + sizeZoneSector;
        if (sector.StartX < cellsInGalaxy.Size)
        {
            for (int i = 0; i < sizeZoneSector + 1; i++)
            {
                var zz = horizontalNullsStartZ;
                if (zz < cellsInGalaxy.Size)
                {
                    var xx = sector.StartX + i;
                    if (xx < cellsInGalaxy.Size)
                    {

                        var nothing = new GlobalMapNothing(Utils.GetId(), xx, zz);
                        cellsInGalaxy.SetCell(nothing);
//                        Debug.Log(String.Format("Set nothing to {0}  {1}", nothing.indX, nothing.indZ));
                    }
                }
            }
        }
    }

    private ShipConfig GetConfig(int i, int j, int subZoneCount)
    {
        Dictionary<ShipConfig, float> d;
        if (i == 0)
        {
//            configs = new WDictionary<ShipConfig>();
            d = new Dictionary<ShipConfig, float>() { {ShipConfig.mercenary,1},{ShipConfig.raiders,2},};
        }
        else
        {
            if (i == subZoneCount - 1)
            {
                d = new Dictionary<ShipConfig, float>() { { ShipConfig.federation, 3 }, { ShipConfig.krios, 1 },{ ShipConfig.ocrons, 1 }, };
            }
            else
            {
                if (i < subZoneCount / 2)
                {

                    d = new Dictionary<ShipConfig, float>() { { ShipConfig.mercenary, 2 }, { ShipConfig.krios, 1 }, { ShipConfig.ocrons, 1 }, { ShipConfig.federation, 1 }, };
                }
                else
                {
                    d = new Dictionary<ShipConfig, float>() { { ShipConfig.federation, 3 }, { ShipConfig.krios, 3 }, { ShipConfig.ocrons, 3 }, };

                }
            }

        }
        var configs = new WDictionary<ShipConfig>(d);
        return configs.Random();
    }

//    public GlobalMapCell Init(int mapSize, int startPower,int coreCells,int startDeathStep)
//    {
//        StartDeathStep = startDeathStep;
//        foreach (GlobalMapEventType d in Enum.GetValues(typeof(GlobalMapEventType)))
//        {
//            _eventsCount.Add(d,0);
//        }
//        Size = mapSize;
//        InitExamples();
//        int cellId = 0;
//        cells = new CellsInGalaxy(mapSize);
//        int delta = Mathf.Clamp((int)(mapSize*0.2f), 1, 5);
//        int half = mapSize/2;
//
//        startIndexX = 0;//MyExtensions.Random(half - delta, half + delta);
//        startIndexZ = MyExtensions.Random(half - delta, half + delta);
//        int endIndexX = mapSize - 1;
//        int endIndexZ = MyExtensions.Random(half - delta, half + delta);
//
//        Debug.LogFormat("Start cell: {0},{1}",startIndexX,startIndexZ);
//        Debug.LogFormat("End cell: {0},{1}", endIndexX, endIndexZ);
//
////        int maxDistanceX = Mathf.Max(Mathf.Abs(mapSize - startIndexX), startIndexX);
////        int maxDistanceZ = Mathf.Max(Mathf.Abs(mapSize - startIndexZ), startIndexZ);
//        int maxDistance = mapSize;// + maxDistanceZ;
//
//        var endCell = new EndGlobalCell(cellId++, endIndexX, endIndexZ);
//        cells.SetCell(endIndexX, endIndexZ,endCell);
//
//
//        List<int> vertical = new List<int>();
//        List<int> horizontal = new List<int>();
//
//        var notingPower = Size/3;
//        AddNothingPoints(notingPower);
//
//        for (int i = 0; i < mapSize; i++)
//        {
//            vertical.Add(i);
//            horizontal.Add(i);
//        }
//        List<CoreGlobalMapCell> cores = new List<CoreGlobalMapCell>();
//        for (int i = 0; i < coreCells; i++)
//        {
//            var coreX = vertical.RandomElement();
//            var coreZ = horizontal.RandomElement();
//            var deltaToStart = Mathf.Max(Mathf.Abs(startIndexX - coreX), Mathf.Abs(startIndexZ - coreZ));
//            var power = CalcPowerOfCell(startPower, deltaToStart, mapSize);
//            var coreCell = new CoreGlobalMapCell(power, cellId++, coreX, coreZ);
//            SubscribeCell(coreCell);
//            cells.SetCell(coreX, coreZ, coreCell);
//            vertical.Remove(coreX);
//            horizontal.Remove(coreZ);
//            Debug.LogFormat("coreCells {2} cell: {0},{1}", coreX, coreZ,i);
//            cores.Add(coreCell);
//        }
//
//        var counts = new Dictionary<GlobalMapCellType, int>()
//        {
//            {GlobalMapCellType.army,0},
//            {GlobalMapCellType.eventMap, 0},
//            {GlobalMapCellType.repair,0},
////            {GlobalMapCellType.modif, 0},
//            {GlobalMapCellType.shop, 0},
//            {GlobalMapCellType.nothing, 0},
//        };
//        for (int i = 0; i < mapSize; i++)
//        {
//            for (int j = 0; j < mapSize; j++)
//            {
//                var cell = cells.GetCell(i, j);
//                if (cell == null)
//                {
//                    var deltaToStart = Mathf.Max(Mathf.Abs(startIndexX - i) , Mathf.Abs(startIndexZ - j));
//                    cell = CreateRandomCell(deltaToStart,mapSize,startPower,maxDistance,ref cellId,i,j, counts);
//                    SubscribeCell(cell);
//                    cells.SetCell(i, j, cell);
//                }
//            }
//        }
//
//        InitKeysZones(cores);
//
//#if UNITY_EDITOR
//        string ss = "";
//        foreach (var count in counts)
//        {
//            ss += " (" + count.Key.ToString() + ":" + count.Value + ")";
//        }
//        Debug.Log(ss);
//#endif
//        var startCell = new StartGlobalCell(cellId++, startIndexX, startIndexZ);
//        cells.SetCell(startIndexX, startIndexZ, startCell);
//        return startCell;
//    }

//    private void InitKeysZones(List<CoreGlobalMapCell> cores)
//    {
//        int maxPosibleOffset = zoneCount / 2;
//        int coreIndex = 1;
//        foreach (var item in cores)
//        {
//            var xx = item.indX;
//            var zz = item.indZ;
//            int offsetX = MyExtensions.Random(-maxPosibleOffset, maxPosibleOffset);
//            int offsetZ = MyExtensions.Random(-maxPosibleOffset, maxPosibleOffset);
//            var centerX = Mathf.Clamp(xx + offsetX, maxPosibleOffset, Size - maxPosibleOffset-1);
//            var centerZ = Mathf.Clamp(zz + offsetZ, maxPosibleOffset, Size - maxPosibleOffset - 1);
//            for (int i = 0; i < zoneCount; i++)
//            {
//                var ix = centerX - maxPosibleOffset + i;
//                for (int j = 0; j < zoneCount; j++)
//                {
//                    var jz = centerZ - maxPosibleOffset + j;
//#if UNITY_EDITOR
//                    if (ix < 0 || ix >= Size)
//                    {
//                        Debug.LogErrorFormat("wrong cells {0} {1} {2} {3}" , centerX,maxPosibleOffset,i, Size);
//                    }
//                    if (jz < 0 || jz >= Size)
//                    {
//                        Debug.LogErrorFormat("wrong cells {0} {1} {2} {3}", centerZ, maxPosibleOffset, j, Size);
//                    }
//#endif
//                    var cell = cells.GetCell(ix, jz);
//                    if (cell != null)
//                    {
//                        cell.SetConnectedCell(coreIndex);
//                    }
//                }
//            }
//            coreIndex++;
//        }
//    }

    private void SubscribeCell(GlobalMapCell cell)
    {
        cell.OnScouted += OnCellScouted;
    }

    private void OnCellScouted(GlobalMapCell obj)
    {
        if (obj is CoreGlobalMapCell)
        {
            int idToUnconnect = obj.ConnectedGates;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    var cell = cells.GetCell(i, j);
                    if (cell != null)
                    {
                        var shallUnconect = cell.ConnectedGates == idToUnconnect;
//                        Debug.LogErrorFormat(" cells: {0} {1} " , cell.ConnectedGates , obj.ConnectedGates);
                        if (shallUnconect)
                        {
//                            Debug.LogError("UNCONNECTED!!!");
                            cell.UnconnectAll();
                        }
                    }
                }
            }
        }
        else if (obj.ConnectedGates > 0)
        {
            obj.UnconnectAll();
        }
    }

//    private void AddNothingPoints(int notingPower)
//    {
//        for (int i = 0; i < Size; i++)
//        {
//            for (int j = 0; j < notingPower; j++)
//            {
//
//                var rndZ = MyExtensions.Random(0, Size-1);
//                var cell = cells.GetCell(i, rndZ);
//                if (cell == null)
//                {
//                    var connected = ConnectedCellsToCurrent(i,rndZ);
//                    int blockedCount = 0;
//                    foreach (var globalMapCell in connected)
//                    {
//                        if (globalMapCell != null)
//                        {
//                            if (globalMapCell is GlobalMapNothing)
//                            {
//                                blockedCount++;
//                            }
//                        }
//                    }
//                    if (blockedCount <= 2)
//                    {
//                        var c = new GlobalMapNothing(Utils.GetId(), i, rndZ);
//                        cells.SetCell( c);
//                    }
//                }
//            }
//        }
//
//    }

    public GlobalMapCell GetRandomClosestCellWithNoData(ShipConfig config, int indX, int indZ)
    {
        return cells.GetRandomClosestCellWithNoData(config, indX, indZ);
    }

    private bool CanAdd(GlobalMapEventType eventType)
    {
        if (_maxCount.ContainsKey(eventType))
        {
            return  _eventsCount[eventType] < _maxCount[eventType];
        }
        else
        {
            return true;
        }
    }

    private int CalcPowerOfCell(int startPower,int distToStart,int mapSize)
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
        if (step > StartDeathStep)
        {
            TryDestroyCell(curCell);
        }
    }

    public GlobalMapCell GetRandomCell()
    {
        var cells1 = this.cells.GetRandom();
        return cells1;
    }

    private void TryDestroyCell(GlobalMapCell curCell)
    {
        var rnds = cells.GetAllList().Where(x=>x.SectorId == curCell.SectorId && x.CanCellDestroy()).ToList();
        if (rnds.Count > 0)
        {
            var rnd = rnds.RandomElement();
            rnd.DestroyCell();
            var ways = rnd.GetCurrentPosibleWays();
            foreach (var globalMapCell in ways)
            {
                globalMapCell.RemoveWayTo(rnd);
            }
            MainController.Instance.MainPlayer.MessagesToConsole.AddMsg("Cell destroyed");
            
           
        }
    }


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
}

