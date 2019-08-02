using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

public class SectorData
{
    private const float COEF_POWER = 0.7f;
    public int StartX;
    public int StartZ;
    public int Size { get; private set; }
    public int XIndex { get; private set; }
    public int Id { get; private set; }
    public bool IsPopulated { get; private set; }
    private int _power;
    private bool _isVisited;
    public SectorCellContainer[,] Cells;
    private HashSet<SectorCellContainer> _listCells = new HashSet<SectorCellContainer>();

    Dictionary<GlobalMapEventType, int> _eventsCount = new Dictionary<GlobalMapEventType, int>();

//    private static Dictionary<int, WDictionary<ShipConfig>> ShipsDictionary;
    private ShipConfig _shipConfig;
    private Dictionary<GlobalMapEventType, int> _maxCount;

    public SectorData(int startX, int startZ, int size, Dictionary<GlobalMapEventType, int> maxCountEvents,
         ShipConfig shipConfig,int index,int xIndex)
    {
        XIndex = xIndex;
        Id = index;
        Cells = new SectorCellContainer[size, size];
        _shipConfig = shipConfig;
        _maxCount = maxCountEvents;
        StartX = startX;
        StartZ = startZ;
        Size = size;
//        Debug.Log(String.Format("Sub Sector X:{0} Z:{1}.     Congif:{2}", startX, startZ, shipConfig.ToString()));
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                var cell = new SectorCellContainer(i, j);
                Cells[i, j] = cell;
                _listCells.Add(cell);
            }
        }
    }

    public void SetCell(GlobalMapCell cell,int subSectotId)
    {
        var x = cell.indX - StartX;
        var z = cell.indZ - StartZ;
        cell.SectorId = subSectotId;
#if UNITY_EDITOR
        try
        {
            Cells[x, z].SetData(cell);
        }
        catch (Exception e)
        {
            Debug.LogErrorFormat("can't put cell {0}  {1}.  {2}   {3}." ,x,z, cell.indX, cell.indZ);
        }
#endif
        Cells[x, z].SetData(cell);
    }

    public static int CalcCellPower(int StartX, int Size, int startPowerGalaxy)
    {
       
        var additional = (int)(StartX * Size * COEF_POWER);
        var power = startPowerGalaxy + additional;
        return power;
    }

    public void Populate(int startPowerGalaxy,SectorData startSectorData)
    {
        IsPopulated = true;
        _power = CalcCellPower(startSectorData.StartX,startPowerGalaxy,startPowerGalaxy);
        RandomizeBorders();
        var remainFreeCells = _listCells.Where(x => x.IsFreeToPopulate()).ToList();
        //        Debug.Log($"populate cell. remainFreeCells {remainFreeCells.Count}.  all cells:{_listCells.Count}");
        //Shop cells
        int shopsCount;
        int repairCount;
//        int eventsCount;
        int bigEventsCount;
        int minorEventsCount;
        int armiesCount;
        int remainCount = remainFreeCells.Count;

        if (remainCount < 7)
        {
            if (MyExtensions.IsTrueEqual())
            {
                shopsCount = 1;
                repairCount = 0;
            }
            else
            {
                shopsCount = 0;
                repairCount = 1;
            }

        }
        else
        {
            shopsCount = remainFreeCells.Count > 12 ? MyExtensions.Random(1, 2) : 1;
            repairCount = 1;
        }
        remainCount = remainCount - shopsCount - repairCount;
        armiesCount = (int)(remainCount * MyExtensions.Random(0.6f, 0.8f));
        var allEventsCount = remainCount - armiesCount;
        bigEventsCount = Mathf.Clamp((int)(allEventsCount * 0.3f), 1, 4);
        minorEventsCount = allEventsCount - bigEventsCount;

        if (shopsCount > 0)
        {
            var cellsForShop = remainFreeCells.RandomElement(shopsCount);
            foreach (var container in cellsForShop)
            {
                var shopCell = new ShopGlobalMapCell(_power > 20, Utils.GetId(), StartX + container.indX,
                    StartZ + container.indZ,this);
                container.SetData(shopCell);
                remainFreeCells.Remove(container);
            }
        }

        if (repairCount == 1 && remainFreeCells.Count >= 1)
        {
            var cellForRepair = remainFreeCells.RandomElement();
            var cellRepair = new RepairStationGlobalCell(Utils.GetId(), StartX + cellForRepair.indX, StartZ + cellForRepair.indZ, this);
            cellForRepair.SetData(cellRepair);
            remainFreeCells.Remove(cellForRepair);
        }

        if (minorEventsCount > 0 && remainFreeCells.Count >= minorEventsCount)
        {
            var cellsForEvents = remainFreeCells.RandomElement(minorEventsCount).ToList();
            foreach (var cellContainerForEvent in cellsForEvents)
            {
                var type = GetMinorEventType();
                var cellEvent = new EventGlobalMapCell(type, Utils.GetId(), StartX + cellContainerForEvent.indX, StartZ + cellContainerForEvent.indZ, this);
                cellContainerForEvent.SetData(cellEvent);
                remainFreeCells.Remove(cellContainerForEvent);
            }
        }

        if (bigEventsCount > 0)
        {
            var cellsForEvents = remainFreeCells.RandomElement(bigEventsCount).ToList();
            foreach (var cellContainerForEvent in cellsForEvents)
            {
                var type = GetBigEventType();
                var cellEvent = new EventGlobalMapCell(type, Utils.GetId(), StartX + cellContainerForEvent.indX, StartZ + cellContainerForEvent.indZ, this);
                cellContainerForEvent.SetData(cellEvent);
                remainFreeCells.Remove(cellContainerForEvent);
            
            }
        }

        if (armiesCount > 0)
        {
            List<ArmyCreatorType> _armyCreatorTypes = new List<ArmyCreatorType>()
            {
                ArmyCreatorType.laser,
                ArmyCreatorType.destroy,
                ArmyCreatorType.mine,
                ArmyCreatorType.simple,
                ArmyCreatorType.rocket,
            };
            foreach (var armyContainer in remainFreeCells.ToList())
            {
                var config = IsDroids(_shipConfig);
                var t1 = _armyCreatorTypes.RandomElement();
                var armyCellcell = new ArmyGlobalMapCell(_power, config, Utils.GetId(), t1, StartX + armyContainer.indX, StartZ + armyContainer.indZ, this);
                armyContainer.SetData(armyCellcell);
                remainFreeCells.Remove(armyContainer);
            }
        }

//        Debug.Log($"Sector populated: {Id}");
#if UNITY_EDITOR
        foreach (var cell in _listCells)
        {
            if (cell.IsFreeToPopulate())
            {
                Debug.LogErrorFormat("Not all sectors populated sectorId:{0}.  {1}_{2}",Id, cell.indX, cell.indZ);
            }
        }
#endif

    }

    public bool ComeToSector(int visitedSectors)
    {
        if (!_isVisited)
        {
            _isVisited = true;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    var cell = Cells[i, j];
                    cell.Data.UpdatePowers(visitedSectors);
                }
            }

            return true;
        }

        return false;
    }

    private ShipConfig IsDroids(ShipConfig coreConfig)
    {
        float doirdChance = -1f;
        switch (coreConfig)
        {
            case ShipConfig.raiders:
                doirdChance = .5f;
                break;
            case ShipConfig.federation:
                doirdChance = .1f;
                break;
            case ShipConfig.mercenary:
                doirdChance = .3f;
                break;
            case ShipConfig.ocrons:
                doirdChance = .2f;
                break;
            case ShipConfig.krios:
                doirdChance = .2f;
                break;
        }

        var config = MyExtensions.IsTrue01(doirdChance) ? ShipConfig.droid : coreConfig;
        return config;
    }

    private void RandomizeBorders()
    {
        if (Size >= 4)
        {
            var last = Size - 1;
            SetNullCell(0, 0);
            SetNullCell(last, 0);
            SetNullCell(0, last);
            SetNullCell(last, last);


            for (int i = 0; i < MyExtensions.Random(0, 2); i++)
            {
                var x = MyExtensions.Random(1, last);
                SetNullCell(x, 0);
            }

            for (int i = 0; i < MyExtensions.Random(0, 2); i++)
            {
                var x = MyExtensions.Random(1, last);
                SetNullCell(0, x);
            }

            for (int i = 0; i < MyExtensions.Random(0, 2); i++)
            {
                var x = MyExtensions.Random(1, last);
                SetNullCell(x, last);
            }

            for (int i = 0; i < MyExtensions.Random(0, 2); i++)
            {
                var x = MyExtensions.Random(1, last);
                SetNullCell(x, last);
            }
        }
        else
        {
            for (int i = 0; i < MyExtensions.Random(0, 2); i++)       
            {
                var x = MyExtensions.Random(0, Size-1);
                var z = MyExtensions.Random(0, Size - 1);
                SetNullCell(x, z);
            }
        }
    }

    private void SetNullCell(int x, int z)
    {
        var c = new GlobalMapNothing(Utils.GetId(),StartX +  x,StartZ + z, this);
        var container = Cells[x, z];
        if (container.IsFreeToPopulate())
        {
            container.SetData(c);
        }
    }

//    private GlobalMapCell CreateRandomCell(int power, int Xind, int Zind)
//    {
//        var cnfg = _shipConfig;
//        List<ArmyCreatorType> _armyCreatorTypes = new List<ArmyCreatorType>()
//        {
//            ArmyCreatorType.laser,
//            ArmyCreatorType.destroy,
//            ArmyCreatorType.mine,
//            ArmyCreatorType.simple,
//            ArmyCreatorType.rocket,
//        };
//        GlobalMapCell cell;
//        var rndCell = new WDictionary<GlobalMapCellType>(new Dictionary<GlobalMapCellType, float>()
//        {
//            {GlobalMapCellType.army, 7},
//            {GlobalMapCellType.eventMap, 5},
//            {GlobalMapCellType.repair, 2},
////            { GlobalMapCellType.modif, distToStart <= 3?0: 2 },
//            {GlobalMapCellType.shop, 2},
//        });
//        var ct = rndCell.Random();
//        switch (ct)
//        {
//            case GlobalMapCellType.eventMap:
//                var type = GetEventType();
//                if (type.HasValue)
//                {
//                    cell = new EventGlobalMapCell(type.Value, Utils.GetId(), Xind, Zind);
//                }
//                else
//                {
//                    var t1 = _armyCreatorTypes.RandomElement();
////                    var power = CalcPowerOfCell(startPower, distToStart, mapSize);
//                    cell = new ArmyGlobalMapCell(power, cnfg, Utils.GetId(), t1, Xind, Zind);
//                }
//
//                break;
//            case GlobalMapCellType.shop:
//                cell = new ShopGlobalMapCell(power > 20, Utils.GetId(), Xind, Zind);
//                break;
//            case GlobalMapCellType.repair:
//                cell = new RepairStationGlobalCell(Utils.GetId(), Xind, Zind);
//                break;
//            case GlobalMapCellType.army:
//            default:
//                var t = _armyCreatorTypes.RandomElement();
//                cell = new ArmyGlobalMapCell(power, cnfg, Utils.GetId(), t, Xind, Zind);
//                break;
//        }
//        return cell;
//    }

    private bool CanAdd(GlobalMapEventType eventType)
    {
        if (_maxCount.ContainsKey(eventType))
        {
            return _eventsCount[eventType] < _maxCount[eventType];
        }
        else
        {
            return true;
        }
    }

    private GlobalMapEventType GetBigEventType()
    {
        List<GlobalMapEventType> list = new List<GlobalMapEventType>();
        list.Add(GlobalMapEventType.nothing);
        list.Add(GlobalMapEventType.prisoner);
        list.Add(GlobalMapEventType.spaceGarbage);
        list.Add(GlobalMapEventType.battleField);
        list.Add(GlobalMapEventType.retranslatorMessage);
        list.Add(GlobalMapEventType.asteroidsField);
        list.Add(GlobalMapEventType.scienceLab);
        list.Add(GlobalMapEventType.brokenNavigation);
        list.Add(GlobalMapEventType.anomaly);
        return list.RandomElement();
    }

    private GlobalMapEventType GetMinorEventType()
    {
        List<GlobalMapEventType> list = new List<GlobalMapEventType>();
        list.Add(GlobalMapEventType.trade);
        list.Add(GlobalMapEventType.teach);
        return list.RandomElement();
    }
    private GlobalMapEventType? GetEventType()
    {
        float step = 2;
        float delta = 0.3f;
//        int halfDixt = maxDist / 2;
        var d = new Dictionary<GlobalMapEventType, float>();

        if (CanAdd(GlobalMapEventType.nothing))
            d.Add(GlobalMapEventType.nothing, Mathf.Abs(1));
        step += delta;
        if (CanAdd(GlobalMapEventType.prisoner))
            d.Add(GlobalMapEventType.prisoner, Mathf.Abs(1));
        step += delta;
        if (CanAdd(GlobalMapEventType.spaceGarbage))
            d.Add(GlobalMapEventType.spaceGarbage, Mathf.Abs(1));
        step += delta;
        if (CanAdd(GlobalMapEventType.battleField))
            d.Add(GlobalMapEventType.battleField, Mathf.Abs(1));
        step += delta;
        if (CanAdd(GlobalMapEventType.retranslatorMessage))
            d.Add(GlobalMapEventType.retranslatorMessage, Mathf.Abs(1));
        step += delta;
        if (CanAdd(GlobalMapEventType.asteroidsField))
            d.Add(GlobalMapEventType.asteroidsField, Mathf.Abs(1));
        step += delta;
        if (CanAdd(GlobalMapEventType.scienceLab))
            d.Add(GlobalMapEventType.scienceLab, Mathf.Abs(1));
        step += delta;
        if (CanAdd(GlobalMapEventType.anomaly))
            d.Add(GlobalMapEventType.anomaly, Mathf.Abs(1));
        step += delta;
        if (CanAdd(GlobalMapEventType.brokenNavigation))
            d.Add(GlobalMapEventType.brokenNavigation, Mathf.Abs(1));
        step += delta;
        //        if (CanAdd(GlobalMapEventType.repairShip))
        //            d.Add(GlobalMapEventType.repairShip, Mathf.Abs(halfDixt + step - distFromStart));

        if (d.Count == 0)
        {
            return null;
        }

        WDictionary<GlobalMapEventType> rnd = new WDictionary<GlobalMapEventType>(d);
        var eventType = rnd.Random();
        if (!_eventsCount.ContainsKey(eventType))
        {
            _eventsCount.Add(eventType,0);
        }
        _eventsCount[eventType] = _eventsCount[eventType] + 1;
        return eventType;
    }


    public void MarkAsCore(int coreId)
    {
        Debug.Log($"Sector marked as core:{Id}  coreId:{coreId}");
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                var cell = Cells[i, j];
                if (!cell.IsFreeToPopulate() && !(cell is GlobalMapNothing))
                {
                    cell.SetConnectedCell(coreId);
                }
            }
        }
    }

    public void ApplyPointsTo(CellsInGalaxy cellInGalaxy)
    {
//        Debug.Log(String.Format("Start populate Sector Start:{0}_{1}",StartX,StartZ));
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                var cell = Cells[i, j];
                cellInGalaxy.SetCell(cell.Data);
            }
        }
    }

    public void CacheWays()
    {
        foreach (var cell in _listCells)
        {
            var ways = ConnectedCellsToCurrent(cell.indX, cell.indZ);
            var data = cell.Data;
#if UNITY_EDITOR

            try
            {
                data.AddWays(ways);
            }
            catch (Exception e)
            {
                Debug.LogError($"zero cell data:  Sector:{Id}  Size:{Size}. cell:{cell.indX},{cell.indZ}");
                return;
            }
#endif
            data.AddWays(ways);
        }

        CutSomeWays();
    }

    private void CutSomeWays()
    {
        foreach (var cell in _listCells)
        {
            var allWays = cell.Data.GetCurrentPosibleWays();
            if (allWays.Count > 3)
            {
                int waysToCut = allWays.Count > 5 ? 2 : 1;
                var wayToCut = allWays.RandomElement(waysToCut);
                foreach (var mapCell in wayToCut)
                {
                    cell.Data.RemoveWayTo(mapCell);
                    mapCell.RemoveWayTo(cell.Data);
                }
            }
        }
    }


    private List<GlobalMapCell> ConnectedCellsToCurrent(int XINdex, int ZINdex)
    {
        List<GlobalMapCell> list = new List<GlobalMapCell>();
        TryAddClose(XINdex - 1, ZINdex - 1, list);
        TryAddClose(XINdex - 1, ZINdex, list);
        TryAddClose(XINdex - 1, ZINdex + 1, list);
        TryAddClose(XINdex, ZINdex - 1, list);
        TryAddClose(XINdex, ZINdex + 1, list);
        TryAddClose(XINdex + 1, ZINdex - 1, list);
        TryAddClose(XINdex + 1, ZINdex, list);
        TryAddClose(XINdex + 1, ZINdex + 1, list);
        return list;
    }


    private void TryAddClose(int x, int z, List<GlobalMapCell> list)
    {
        if (x >= 0 && x < Size)
        {
            if (z >= 0 && z < Size)
            {
                var cell = Cells[x, z];
                if (cell.Data != null)
                    list.Add(cell.Data);
            }
        }
    }

}