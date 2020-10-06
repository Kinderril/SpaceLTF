using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class SectorFinalBattle : SectorData
{
    private List<ShipConfig> configs;
    public SectorFinalBattle(int startX, int startZ, int size, Dictionary<GlobalMapEventType, int> maxCountEvents, 
        ShipConfig shipConfig, int xIndex, float powerPerTurn, 
        DeleteWayDelegeate removeWayCallback, GalaxyEnemiesArmyController enemiesArmyController) 
        : base(startX, startZ, size, maxCountEvents, shipConfig, xIndex, powerPerTurn, 
            removeWayCallback, enemiesArmyController)
    {

    }

    public override void Populate(int startPowerGalaxy)
    {
        configs = new List<ShipConfig>()
        {
            ShipConfig.federation,
            ShipConfig.ocrons,
            ShipConfig.mercenary,
            ShipConfig.krios,
            ShipConfig.raiders,
        };

        var playerConfig = MainController.Instance.MainPlayer.ReputationData.Allies;
        if (playerConfig.HasValue)
        {
            configs.Remove(playerConfig.Value);
        }
        IsPopulated = true;
        Name = Namings.ShipConfig(ShipConfig.droid);
        StartPowerGalaxy = startPowerGalaxy;
        _power = startPowerGalaxy;
        PopulateToSide2();
    }

    private void PopulateToSide2()
    {
        _isHide = true;
        ListCells = new HashSet<SectorCellContainer>();
        FreeActionGlobalMapCell _lastCell = null;
        bool prevThisLevel = false;
        int id = 1;
        for (int i = 0; i < Size; i++)
        {
            if (i == 0 || i == Size - 1)
            {
                var rndIndex = MyExtensions.Random(0, Size - 1);
                var cell = PopulateCell(i, rndIndex,id);
                if (_lastCell != null && cell != null)
                {
                    id++;
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
                var cell = PopulateCell(i, rndIndex, id);
         
                if (_lastCell != null && cell != null)
                {
                    id++;
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
    }

    private FreeActionGlobalMapCell PopulateCell(int j, int i,int id)
    {
//        if (id > 5)
//        {
//            return null;
//        }
        FreeActionGlobalMapCell armyCellcell = null;
        var cellContainer = Cells[i, j];
        armyCellcell = new FreeActionGlobalMapCell(StartPowerGalaxy, configs.RandomElement(),
            id, StartX + cellContainer.indX, StartZ + cellContainer.indZ, this, _enemiesArmyController, _powerPerTurn,false);
//        armyCellcell.OnComeToCell += OnComeToCell;
        cellContainer.SetData(armyCellcell);
        ListCells.Add(cellContainer);
        armyCellcell.Hide();
        return armyCellcell;
    }


    public override void CacheWays()
    {

    }
}
