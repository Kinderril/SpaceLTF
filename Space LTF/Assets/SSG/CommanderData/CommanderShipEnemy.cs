public class CommanderShipEnemy
{
    public ShipBase ShipBase;
    public bool IsPriority { get; private set; }

    public CommanderShipEnemy(ShipBase ShipBase)
    {
        this.ShipBase = ShipBase;
    }

    public void SetPriority(bool b)
    {
        IsPriority = b;
        ShipBase.PriorityObject.SetActive(b);
    }
}