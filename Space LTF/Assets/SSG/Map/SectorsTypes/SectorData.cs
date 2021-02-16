using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate void DeleteWayDelegeate(GlobalMapCell to);

[System.Serializable]
public class SectorData
{
    public int StartX;
    public int StartZ;
    public int Size { get; private set; }
    public bool IsCore { get; private set; }
    public bool IsFinal { get; private set; }
    public bool IsStart { get; private set; } = false;
    public string Name { get; protected set; }
    public int XIndex { get; private set; }
    public int Id { get; private set; }
    public bool IsPopulated { get; protected set; }
    public int StartPowerGalaxy { get; protected set; }
    public bool IsMy => _isUnderMyControl;
    protected int _power;
    private bool _isVisited;
    private bool _isUnderMyControl;
    private bool _canBeUnderMyControl;
    public SectorCellContainer[,] Cells;
    protected float _powerPerTurn;
    public HashSet<SectorCellContainer> ListCells = new HashSet<SectorCellContainer>();
    protected ShipConfig _shipConfig;
    public ShipConfig ShipConfig => _shipConfig;
    private Dictionary<GlobalMapEventType, int> _maxCount;
    protected DeleteWayDelegeate RemoveWayCallback;
    protected GalaxyEnemiesArmyController _enemiesArmyController;
    public virtual bool AnyEvent => false;

    protected bool _isHide = false;
    [field: NonSerialized]
    public event Action OnCompleteChange;
    [field: NonSerialized] public event Action<SectorData> OnBecomeMine;

    public bool IsHide => _isHide;


    public SectorData(int startX, int startZ, int size, Dictionary<GlobalMapEventType, int> maxCountEvents,
         ShipConfig shipConfig,  int xIndex, float powerPerTurn, 
         DeleteWayDelegeate removeWayCallback, GalaxyEnemiesArmyController enemiesArmyController,bool canBeUnderMyControl = false)
    {
        _canBeUnderMyControl = canBeUnderMyControl && shipConfig != ShipConfig.droid;
        _enemiesArmyController = enemiesArmyController;
        RemoveWayCallback = removeWayCallback;
        XIndex = xIndex;
        _powerPerTurn = powerPerTurn;
        Id = Guid.NewGuid().GetHashCode();
        Cells = new SectorCellContainer[size, size];
        _shipConfig = shipConfig;
        _maxCount = maxCountEvents;
        StartX = startX;
        StartZ = startZ;
        Size = size;
        Debug.Log(Namings.Format("Sub Sector X:{0} Z:{1}.     Congif:{2}   Size:{3}", startX, startZ, shipConfig.ToString(), Size));
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                var cell = new SectorCellContainer(i, j);
                Cells[i, j] = cell;
                ListCells.Add(cell);
            }
        }
    }

    public void UnHide()
    {
//        Debug.LogError("sector data unhide");
        _isHide = false;
        foreach (var sectorCellContainer in ListCells)
        {
            if (sectorCellContainer.Data != null)
            {
                sectorCellContainer.Data.Unhide();
            }
        }
    }

    public void ChangeSectorOwner(ShipConfig shipConfig)
    {
        _shipConfig = shipConfig;
    }

    public void MarkAsStart()
    {
        IsStart = true;
    }

    protected void AddWays(GlobalMapCell c1, GlobalMapCell c2)
    {
        c1.AddWay(c2.Container);
        c2.AddWay(c1.Container);
    }

    public void SetCell(GlobalMapCell cell, int subSectotId)
    {
        var x = cell.indX - StartX;
        var z = cell.indZ - StartZ;
        //        cell.SectorId = subSectotId;
#if UNITY_EDITOR
        try
        {
            Cells[x, z].SetData(cell);
        }
        catch (Exception e)
        {
            Debug.LogErrorFormat("can't put cell {0}  {1}.  {2}   {3}.", x, z, cell.indX, cell.indZ);
        }


#endif
        var oldData = Cells[x, z];
//        if (oldData.Data != null)
//        {
//            Debug.LogError($"error cell: {oldData.indX} {oldData.indZ}  d:{oldData.Data.indX} {oldData.Data.indZ}");
//            ListCells.Remove(oldData);
//        }
        oldData.SetData(cell);
        if (cell is EndGlobalCell)
        {
            IsFinal = true;
        }
    }

    protected virtual void PrePopulate()
    {

    }

    public virtual void Populate(int startPowerGalaxy)
    {
        IsPopulated = true;
        StartPowerGalaxy = startPowerGalaxy;
        _power = startPowerGalaxy;
        RandomizeBorders();
        PrePopulate();
        var remainFreeCells = ListCells.Where(x => x.IsFreeToPopulate()).ToList();
        SectorDataEventsCounts counts = GetCountOfEvents(remainFreeCells.Count,Size);

        if (counts.shopsCount > 0)
        {
            var cellsForShop = remainFreeCells.RandomElement(counts.shopsCount);
            foreach (var container in cellsForShop)
            {
                var shopCell = new ShopGlobalMapCell(_power, Utils.GetId(), StartX + container.indX,
                    StartZ + container.indZ, this, _shipConfig);
                container.SetData(shopCell);
                remainFreeCells.Remove(container);
            }
        }

        if (counts.repairCount == 1 && remainFreeCells.Count >= 1)
        {
            var cellForRepair = remainFreeCells.RandomElement();
            var cellRepair = new RepairStationGlobalCell(Utils.GetId(), StartX + cellForRepair.indX, StartZ + cellForRepair.indZ, this, _shipConfig);
            cellForRepair.SetData(cellRepair);
            remainFreeCells.Remove(cellForRepair);
        }

        if (counts.minorEventsCount > 0 && remainFreeCells.Count >= counts.minorEventsCount)
        {
            var cellsForEvents = remainFreeCells.RandomElement(counts.minorEventsCount).ToList();
            foreach (var cellContainerForEvent in cellsForEvents)
            {
                var type = GetMinorEventType();
                var cellEvent = new EventGlobalMapCell(type, Utils.GetId(), StartX + cellContainerForEvent.indX, StartZ + cellContainerForEvent.indZ, this, _power, _shipConfig);
                cellContainerForEvent.SetData(cellEvent);
                remainFreeCells.Remove(cellContainerForEvent);
            }
        }

        if (counts.bigEventsCount > 0)
        {
            var cellsForEvents = remainFreeCells.RandomElement(counts.bigEventsCount).ToList();
            foreach (var cellContainerForEvent in cellsForEvents)
            {
                var type = GetBigEventType();
                var cellEvent = new EventGlobalMapCell(type, Utils.GetId(), StartX + cellContainerForEvent.indX, StartZ + cellContainerForEvent.indZ, this, _power, _shipConfig);
                cellContainerForEvent.SetData(cellEvent);
                remainFreeCells.Remove(cellContainerForEvent);

            }
        }

        if (remainFreeCells.Count > 1 && _canBeUnderMyControl)
        {
            var rndRemainCell = remainFreeCells.RandomElement();
            if (rndRemainCell != null)
            {

                remainFreeCells.Remove(rndRemainCell);
                var bornCenterGlobalCell = new ArmyBornCenterGlobalCell(Utils.GetId(), StartX + rndRemainCell.indX,
                    StartZ + rndRemainCell.indZ, this, _shipConfig);
                rndRemainCell.SetData(bornCenterGlobalCell);
            }
        }

        foreach (var armyContainer in remainFreeCells.ToList())
        {
            var config = IsDroids(_shipConfig);
            var armyCellcell = new FreeActionGlobalMapCell(_power, config, 
                Utils.GetId(), StartX + armyContainer.indX, StartZ + armyContainer.indZ, 
                this,_enemiesArmyController, _powerPerTurn);
            armyContainer.SetData(armyCellcell);
            remainFreeCells.Remove(armyContainer);
        }

        //        Debug.Log($"Sector populated: {Id}");
#if UNITY_EDITOR
        foreach (var cell in ListCells)
        {
            if (cell.IsFreeToPopulate())
            {
                Debug.LogErrorFormat("Not all sectors populated sectorId:{0}.  {1}_{2}", Id, cell.indX, cell.indZ);
            }
        }
#endif
        Name = Namings.ShipConfig(_shipConfig);
    }


    protected virtual SectorDataEventsCounts GetCountOfEvents(int remainCells, int sectorSize)
    {
        return SectorDataEventsCounts.Standart(remainCells, sectorSize);
    }

    public bool ComeToSector()
    {
        CheckIsAllArmiesDestryoed();
        OnCompleteChange?.Invoke();
        if (!_isVisited)
        {
            _isVisited = true;
//            RecalculateAllCells(visitedSectors, step);
            return true;
        }

        return false;
    }

    private void CheckIsAllArmiesDestryoed()
    {
        if (!_canBeUnderMyControl)
        {
             return;
        }
        bool isAllArmiesDestyoyed = true;
        foreach (var sectorCellContainer in ListCells)
        {
            if (sectorCellContainer.Data.CurMovingArmy.HaveArmy())
            {
                isAllArmiesDestyoyed = false;
            }
        }

        if (isAllArmiesDestyoyed && ShipConfig != ShipConfig.droid)
        {
            var sectorCells = ListCells.Where(x => !(x.Data is GlobalMapNothing)).ToList();
            var completedCount = sectorCells.Count(x => x.Data.Completed);
            var totalCount = sectorCells.Count;
            var allCompleted = completedCount >= totalCount;
            if (!_isUnderMyControl && allCompleted)
            {
                _isUnderMyControl = true;
                OnBecomeMine?.Invoke(this);
            }
        }

    }

    protected StartGlobalCell CreateStartCell(bool campCell = false, int act = 0,ShipConfig campConfig = ShipConfig.mercenary)
    {

        var zzStart = 0;
        for (int i = 0; i < Size; i++)
        {
            for (int j = zzStart; j < Size + zzStart; j++)
            {
                var testCell = Cells[i, j];
                if (testCell.Data is GlobalMapNothing)
                {
                    continue;
                }
                var indZ = StartZ + j;
                var indX = StartX + i;
                StartGlobalCell startCEll;
                if (campCell)
                {
                    startCEll = new CampaingStartGlobalCell(999999, indX, indZ, this, campConfig, act);
                }
                else
                {
                    startCEll = new StartGlobalCell(999999, indX, indZ, this, ShipConfig);
                }
                var cellContainer = Cells[i, j];
                cellContainer.SetData(startCEll);
                ListCells.Add(cellContainer);
                return startCEll;
            }
        }

        return null;
    }

    public void RecalculateAllCells(int step)
    {

        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                var cell = Cells[i, j];
                if (cell.Data != null)
                {
                    cell.Data.UpdateAdditionalPower((int)(_powerPerTurn * step));
                }
            }
        }
    }

    protected virtual ShipConfig IsDroids(ShipConfig coreConfig)
    {
        if (IsStart)
        {
            return ShipConfig.droid;
        }
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

    protected virtual void RandomizeBorders()
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
                var x = MyExtensions.Random(0, Size - 1);
                var z = MyExtensions.Random(0, Size - 1);
                SetNullCell(x, z);
            }
        }
    }

    private void SetNullCell(int x, int z)
    {
        var c = new GlobalMapNothing(Utils.GetId(), StartX + x, StartZ + z, this, ShipConfig);
        var container = Cells[x, z];
        if (container.IsFreeToPopulate())
        {
            container.SetData(c);
        }
    }

    private GlobalMapEventType GetBigEventType()
    {
        List<GlobalMapEventType> list = new List<GlobalMapEventType>();
        list.Add(GlobalMapEventType.nothing);
        list.Add(GlobalMapEventType.prisoner);
        list.Add(GlobalMapEventType.spaceGarbage);
        list.Add(GlobalMapEventType.battleField);
        list.Add(GlobalMapEventType.creditStorage);
        list.Add(GlobalMapEventType.asteroidsField);
        list.Add(GlobalMapEventType.scienceLab);
        list.Add(GlobalMapEventType.brokenNavigation);
        list.Add(GlobalMapEventType.anomaly);
        list.Add(GlobalMapEventType.mercHideout);
        list.Add(GlobalMapEventType.excavation);
        list.Add(GlobalMapEventType.secretDeal);
        return list.RandomElement();
    }

    private GlobalMapEventType GetMinorEventType()
    {
        List<GlobalMapEventType> list = new List<GlobalMapEventType>();
        list.Add(GlobalMapEventType.trade);
        list.Add(GlobalMapEventType.teach);
        list.Add(GlobalMapEventType.change);
        return list.RandomElement();
    }

    public void MarkAsVisited()
    {
        _isVisited = true;
    }
          
    public void ApplyPointsTo(CellsInGalaxy cellInGalaxy)
    {
//        Debug.Log($"Apply points to cells");
        int countPopulated = 0;
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                var cell = Cells[i, j];
                if (cell.Data != null)
                {
                    countPopulated++;
//                    Debug.LogError($"Set cell: {cell.indX}  {cell.indZ}");
                    cellInGalaxy.SetCell(cell.Data);
                }
            }
        }
        Debug.Log(Namings.Format("End populate Sector Start:{0}_{1}  countPopulated:{2}", StartX, StartZ, countPopulated));
    }

    public virtual void CacheWays()
    {
        foreach (var cell in ListCells)
        {
            if (cell.Data is GlobalMapNothing)
            {
                continue;
            } 
            var ways = ConnectedCellsToCurrent(cell.indX, cell.indZ);
            ways.RemoveAll(x => x is GlobalMapNothing);
            var data = cell.Data;
            foreach (var globalMapCell in ways)
            {
                if (globalMapCell != null)
                {
                    data.AddWay(globalMapCell.Container);
                }
            }
//
//#if UNITY_EDITOR
//
//            try
//            {
//                data.AddWays(ways);
//            }
//            catch (Exception e)
//            {
//                Debug.LogError($"zero cell data:  Sector:{Id}  Size:{Size}. cell:{cell.indX},{cell.indZ}");
//                return;
//            }
//#endif
//            data.AddWays(ways);
        }

        CutSomeWays();
    }

    private void CutSomeWays()
    {
        foreach (var cell in ListCells)
        {
            var allWays = cell.Data.GetCurrentPosibleWays();
            if (allWays.Count > 3)
            {
                int waysToCut = allWays.Count > 5 ? 2 : 1;
                var wayToCut = allWays.ToList().RandomElement(waysToCut);
                foreach (var mapCell in wayToCut)
                {
                    cell.Data.RemoveWayTo(mapCell);
                    mapCell.RemoveWayTo(cell);
                }
            }
        }
    }


    protected List<GlobalMapCell> ConnectedCellsToCurrent(int XINdex, int ZINdex)
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

    protected List<GlobalMapCell> connectedCellsToCurrentOnlyZUpMapCells(int XINdex, int ZINdex)
    {
        List<GlobalMapCell> list = new List<GlobalMapCell>();
        TryAddClose(XINdex - 1, ZINdex + 1, list);
        TryAddClose(XINdex, ZINdex + 1, list);
        TryAddClose(XINdex + 1, ZINdex + 1, list);
        return list;
    }
    protected List<GlobalMapCell> connectedCellsToCurrentOnlyZDownMapCells(int XINdex, int ZINdex)
    {
        List<GlobalMapCell> list = new List<GlobalMapCell>();
        TryAddClose(XINdex - 1, ZINdex - 1, list);
        TryAddClose(XINdex, ZINdex - 1, list);
        TryAddClose(XINdex + 1, ZINdex - 1, list);
        return list;
    }


    private void TryAddClose(int x, int z, List<GlobalMapCell> list)
    {
        if (x >= 0 && x < Size)
        {
            if (z >= 0 && z < Size)
            {
                var cell = Cells[x, z];
                if (cell.Data != null && !(cell.Data is GlobalMapNothing))
                    list.Add(cell.Data);
            }
        }
    }

    public virtual void BornArmies()
    {
        foreach (var cell in ListCells)
        {
            var born = cell.Data as FreeActionGlobalMapCell;
            if (born != null)
            {
                born.BornArmy();
            }
        }
    }

    public string IndexInfo()
    {
        return $"X:{StartX},Z:{StartZ}  Config:{ShipConfig.ToString()}";
    }

    public bool CanConcuqer()
    {
        return _canBeUnderMyControl;
    }
}