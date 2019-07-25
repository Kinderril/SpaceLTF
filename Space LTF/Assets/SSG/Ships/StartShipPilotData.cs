using System.Collections.Generic;

[System.Serializable]
public class StartShipPilotData
{
    public IPilotParameters Pilot;
    public ShipInventory Ship;
    public float TimeToLaunch = 0f;

    public StartShipPilotData(IPilotParameters Pilot,
        ShipInventory Ship)
    {
        this.Pilot = Pilot;
        this.Ship = Ship;
    }

    public void TryRepairSelfFull()
    {
        var cost = MoneyToFullRepair();
        if (Pilot.HaveMoney(cost))
        {
            Pilot.RemoveMoney(cost);
            Ship.SetRepairPercent(1f);
        }
        else
        {
            WindowManager.Instance.NotEnoughtMoney(cost);
        }
    }

    public int MoneyToFullRepair()
    {
        var percentsToRepair = Ship.HealthPointToRepair();
        var fullPercent = 5 + (int)(Pilot.CurLevel * 0.501f); 
        return fullPercent * percentsToRepair;
    }
}