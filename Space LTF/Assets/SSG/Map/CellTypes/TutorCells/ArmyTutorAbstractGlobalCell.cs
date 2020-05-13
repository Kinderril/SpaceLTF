
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public abstract class ArmyTutorAbstractGlobalCell : ArmyGlobalMapCell
{
    protected int _countEnemies = 3;
    private bool _shallCheckBattleShip;
    protected ArmyTutorAbstractGlobalCell( int power, ShipConfig config, int id, int Xind, int Zind, SectorData sector,bool shallCheckBattleShip) : base(
        power, config, id, Xind, Zind, sector)
    {
        _shallCheckBattleShip = shallCheckBattleShip;
        _eventType = null;
    }

    protected override MessageDialogData GetLeavedActionInner()
    {
        Complete();
        MainController.Instance.MainPlayer.MoneyData.AddMoney(50);
        return null;
    }

    protected override MessageDialogData GetDialog()
    {

        if (Completed)
        {
            var ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData(Namings.DialogTag("Ok")));
            var masinMsg = Namings.Format(Namings.DialogTag("sectorClear"));
            var mesData = new MessageDialogData(masinMsg, ans);
            return mesData;
        }
        else
        {
            string masinMsg;
            var myPlaer = MainController.Instance.MainPlayer;
            string scoutsField = "";
            var ans = new List<AnswerDialogData>();
            bool canAttack = true;
            if (_shallCheckBattleShip)
            {
                var battleShip = myPlaer.Army.Army.FirstOrDefault(x => x.Ship.ShipType != ShipType.Base);
                if (battleShip != null)
                {
                    var weapons = battleShip.Ship.WeaponsModuls.GetNonNullActiveSlots();
                    var weapon = weapons.FirstOrDefault();
                    if (weapon == null)
                    {
                        weapon = myPlaer.Inventory.Weapons.FirstOrDefault(x => x != null);
                        if (weapon == null)
                        {
                            canAttack = false;
                        }
                    }
                }
            }

            if (canAttack)
            {
                ans.Add(new AnswerDialogData(Namings.DialogTag("Attack"), InnerTake));
            }
            else
            {
                ans.Add(new AnswerDialogData(Namings.DialogTag("CantAttack")));
            }
            masinMsg = Namings.Format(Namings.DialogTag("armyShallFight"), scoutsField);

            var mesData = new MessageDialogData(masinMsg, ans);
            return mesData;
        }

    }

    protected void InnerTake()
    {
        Take();
    }


    public override bool OneTimeUsed()
    {
        return false;
    }
    public override void LeaveFromCell()
    {

    }
}