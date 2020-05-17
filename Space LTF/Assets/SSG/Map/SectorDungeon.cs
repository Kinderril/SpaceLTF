using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class SectorDungeon : SectorData
{
    private ArmyDungeonEnterGlobalMapCell _enterCell;
    private const float EXIT_POWER_COEF = 0.3f;
    private const float OTHER_POWER_COEF = 0.15f;

    public ArmyDungeonExitGlobalMapCell ExitCell { get; private set; }
    private bool _entered;
    private bool _upSide;
    private bool _enterCreated;
    private bool _exitCreated;
    private bool _exitAndEnterConnectionCreated;
    private bool _removeOnlyOne;
    private bool _oneRemoved;
    public SectorDungeon(int startX, int startZ, int size, Dictionary<GlobalMapEventType, int> maxCountEvents,
        ShipConfig shipConfig, int index, int xIndex, float powerPerTurn, bool upSide, DeleteWayDelegeate removeWayCallback, GalaxyEnemiesArmyController enemiesArmyController)
        : base(startX, startZ, size, maxCountEvents, shipConfig, index, xIndex, powerPerTurn, removeWayCallback, enemiesArmyController)
    {
        _upSide = upSide;
        _exitCreated = false;
    }

    private void PopulateToSide()
    {
        ListCells = new HashSet<SectorCellContainer>();
        ArmyGlobalMapCell _lastCell = null;
        bool prevThisLevel = false;
        for (int i = 0; i < Size; i++)
        {
            if (i == 0 || i == Size - 1)
            {
                var rndIndex = MyExtensions.Random(0, Size - 1);
                var cell = PopulateCell(i, rndIndex);
                if (_lastCell != null && cell != null)
                {
                    _lastCell.AddWay(cell);
                    cell.AddWay(_lastCell);
//                    Debug.LogError($":Link {_lastCell.indX}.{_lastCell.indZ} <> {cell.indX}.{cell.indZ}");
                }
                _lastCell = cell;
            }
            else
            {
                bool thisLevel = MyExtensions.IsTrueEqual();
                if (prevThisLevel)
                {
                    thisLevel = false;
                }
                var rndIndex = MyExtensions.Random(0, Size - 1);
                var cell = PopulateCell(i, rndIndex);
                if (_lastCell != null && cell != null)
                {
                    _lastCell.AddWay(cell);
                    cell.AddWay(_lastCell);
//                    Debug.LogError($":Link {_lastCell.indX}.{_lastCell.indZ} <> {cell.indX}.{cell.indZ}");
                    _lastCell = cell;
                }      
                if (thisLevel)
                {
                    i--;
                }
                prevThisLevel = thisLevel;


            }

        }

        if (!_exitCreated)
        {
            Debug.LogError("Exit don't created");
        }
    }

    private ArmyGlobalMapCell TryCreateExit(SectorCellContainer cellContainer)
    {
        if (!_exitCreated)
        {
            var armyCellcell = new ArmyDungeonExitGlobalMapCell(_power, _shipConfig, Utils.GetId(),
                StartX + cellContainer.indX, StartZ + cellContainer.indZ, this, EXIT_POWER_COEF);
            _exitCreated = true;
            armyCellcell.OnComeToCell += OnComeToCellExit;
            ExitCell = armyCellcell;
            CheckConnectExitAndEnter();
            return armyCellcell;
        }
        return null;
    }
    private ArmyGlobalMapCell TryCreateEnter(SectorCellContainer cellContainer)
    {
        if (!_enterCreated)
        {
            var armyCellcell = new ArmyDungeonEnterGlobalMapCell(_power, _shipConfig, Utils.GetId(),
                StartX + cellContainer.indX, StartZ + cellContainer.indZ, this, OTHER_POWER_COEF);
            armyCellcell.OnComeToCell += OnComeToCellEnter;
            _enterCell = armyCellcell;
            _enterCreated = true;
            CheckConnectExitAndEnter();
            return armyCellcell;
        }
        return null;
    }

    private void CheckConnectExitAndEnter()
    {
        if (_exitAndEnterConnectionCreated)
        {
            return;
        }

        if (_exitCreated && _enterCreated)
        {
            _exitAndEnterConnectionCreated = true;
            ExitCell.AddWay(_enterCell);
        }

    }

    public override void MarkAsCore(int coreId, CoreGlobalMapCell coreCell)
    {
        _removeOnlyOne = true;
        base.MarkAsCore(coreId, coreCell);
        var middleCells = ListCells.Where(x => x.Data is ArmyDungeonGlobalMapCell).ToList();
        var rndCellToConnect = middleCells.RandomElement(2);
        foreach (var container in rndCellToConnect)
        {
            AddWays(container.Data, coreCell);
        }
    }

    private void OnComeToCellExit(GlobalMapCell to, GlobalMapCell from)
    {

    }
    private void OnComeToCellEnter(GlobalMapCell to, GlobalMapCell from)
    {

    }

    private void AddWays(GlobalMapCell c1, GlobalMapCell c2)
    {
        c1.AddWay(c2);
        c2.AddWay(c1);
    }
    private ArmyGlobalMapCell PopulateCell(int j, int i)
    {
        ArmyGlobalMapCell armyCellcell = null;
        var cellContainer = Cells[i, j];
        if (_upSide)
        {
            if (j == 0)
            {
                //Вход
                armyCellcell = TryCreateEnter(cellContainer);
            }
            else if (j == Size - 1)
            {
                //Выход      
                armyCellcell = TryCreateExit(cellContainer);
            }
        }

        if (!_upSide)
        {
            if (j == 0)
            {
                //Вход     
                armyCellcell = TryCreateExit(cellContainer);
            }
            else if (j == Size - 1)
            {
                //Выход      
                armyCellcell = TryCreateEnter(cellContainer);
            }
        }

        if (armyCellcell == null)
        {
            armyCellcell = new ArmyDungeonGlobalMapCell(_power, _shipConfig, Utils.GetId(),
                StartX + cellContainer.indX, StartZ + cellContainer.indZ, this, OTHER_POWER_COEF);
        }
        armyCellcell.OnComeToCell += OnComeToCell;
        cellContainer.SetData(armyCellcell);
        ListCells.Add(cellContainer);
        return armyCellcell;
    }

    private void OnComeToCell(GlobalMapCell to, GlobalMapCell @from)
    {
        if (_removeOnlyOne)
        {
            if (_oneRemoved)
            {
                return;
            }
            _oneRemoved = true;
            // to.RemoveWayTo(@from);
        }
//        else
//        {
//            if (to is ArmyDungeonEnterGlobalMapCell)
//            {
//                RemoveWayCallback(to);
//            }
//        }

    }

    public override void Populate(int startPowerGalaxy)
    {
        IsPopulated = true;
        Name = Namings.ShipConfig(_shipConfig);
        StartPowerGalaxy = startPowerGalaxy;
        _power = CalcCellPower(0, Size, startPowerGalaxy, 0);
        // RandomizeBorders();
        PopulateToSide();
    }
    public override void CacheWays()
    {

    }
}