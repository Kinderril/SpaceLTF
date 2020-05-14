using System;


[System.Serializable]
public class PlayerRepairData
{
    private PlayerArmy army;
    private PlayerParameters repairParam;

    [field: NonSerialized]
    public event Action OnSomeShipRepaired;

    private bool isInited = false;

    public PlayerRepairData()
    {

    }

    internal void Init(PlayerArmy army, PlayerMapData mapData, PlayerParameters repairParam)
    {
        if (isInited)
        {
            return;
        }

        isInited = true;
        this.army = army;
        this.repairParam = repairParam;
        mapData.OnCellChanged += OnCellChanged;
    }

    private void OnCellChanged(GlobalMapCell cell)
    {
        RepairAllShips();
    }

    public void RepairAllShips()
    {
        bool someRepaierd = false;
        foreach (var shipPilotData in army.Army)
        {
            var repaired = RepairShip(shipPilotData.Ship);
            if (repaired)
            {
                someRepaierd = true;
            }
        }
        if (someRepaierd)
        {
            if (OnSomeShipRepaired != null)
            {
                OnSomeShipRepaired();
            }
        }
    }

    private bool RepairShip(ShipInventory ship)
    {
        var lastPercent = ship.HealthPercent;
        var curPercent = lastPercent;
        if (curPercent > 0.95f)
        {
            curPercent = 1f;
        }
        else
        {
            var healPercent = repairParam.RepairPercentPerStep();
            curPercent += healPercent;
            if (curPercent > 0.95f)
            {
                curPercent = 1f;
            }
        }
        return ship.SetRepairPercent(curPercent);
    }

}

