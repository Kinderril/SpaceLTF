using System;
using System.Linq;


public class TurretDesicionData : IShipDesicion
{
    private ShipTurret _shipBase;

    public TurretDesicionData(ShipTurret shipBase)
    {
        _shipBase = shipBase;
    }

    public ECommanderPriority1 CommanderPriority1 => ECommanderPriority1.Any;
    public ESideAttack SideAttack => ESideAttack.Straight;
    public EGlobalTactics GlobalTactics => EGlobalTactics.Fight;

    public ActionType CalcTask(out ShipBase ship)
    {
        if (_shipBase.Enemies.Keys.Count > 0)
        {
            ship = _shipBase.Enemies.Keys.FirstOrDefault();
            return ActionType.shootFromPlace;
        }
        else
        {
            ship = null;
            return ActionType.none;
        }
    }

    public void Dispose()
    {

    }


    public bool HaveClosestDamagedFriend(out ShipBase ship)
    {
        ship = null;
        return false;
    }

    public bool HaveEnemyInDangerZoneDefenceBase(out ShipBase ship)
    {
        ship = null;
        return false;
    }

    public void SetLastAction(ActionType actionType)
    {

    }

    public void Select(bool val)
    {

    }

    public void DrawUpdate()
    {

    }

    public string GetName()
    {
        return "Control";
    }

    public BaseAction CalcAction()
    {
        var type = CalcTask(out var ship);
        var task = new StayAttackAction(_shipBase, _shipBase.Enemies[ship]);
        return task;
        //        return new WaitEnemyTime(_shipBase, 20f);
    }


    public void ChangePriority(ESideAttack SideAttack)
    {

    }

    public void ChangePriority(EGlobalTactics globalTactics)
    {

    }

    public void ChangePriority(ECommanderPriority1 CommanderPriority1)
    {

    }

    public event Action OnChagePriority;
}

