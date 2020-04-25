
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ArmyTutorGlobalCell : ArmyGlobalMapCell
{
    protected int _constanctPowerPower;
    protected int _stepPowerCoef = 3;
    protected int _countEnemies = 3;
    private bool _withHire;
    public ArmyTutorGlobalCell(int CountEnemies, bool withHire, int power, ShipConfig config, int id, int Xind, int Zind, SectorData sector) : base(
        power, config, id, Xind, Zind, sector)
    {
        _countEnemies = CountEnemies;
        _withHire = withHire;
        _eventType = null;
    }

    protected override MessageDialogData GetLeavedActionInner()
    {
        Complete();
        MainController.Instance.MainPlayer.MoneyData.AddMoney(50);
        if (_withHire)
        {
            return HireAction();
        }
        else
        {
            return null;
        }
    }
    private MessageDialogData HireAction()
    {
        var ans = new List<AnswerDialogData>();
        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        string msg;
        var pilot = Library.CreateDebugPilot();

        ShipType type = ShipType.Middle;
        ShipConfig cng = ShipConfig.mercenary;
        var ship = Library.CreateShip(type, cng, MainController.Instance.MainPlayer, pilot);
        var hireMsg = Namings.DialogTag("afterBattleHireOk"); //
        msg = Namings.Format(hireMsg, Namings.ShipConfig(cng), Namings.ShipType(type));
//        var itemsCount = MyExtensions.Random(1, 2);
//        for (var i = 0; i < itemsCount; i++)
//            if (ship.GetFreeWeaponSlot(out var inex))
//            {
//                var weapon = Library.CreateDamageWeapon(true);
//                ship.TryAddWeaponModul(weapon, inex);
//            }

        MainController.Instance.MainPlayer.Army.TryHireShip(new StartShipPilotData(pilot, ship));

        var dialog = new MessageDialogData(msg, ans);
        return dialog;
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
            string scoutsField;
            var scoutData = GetArmy().ScoutData.GetInfo(myPlaer.Parameters.Scouts.Level);
            if (_eventType.HasValue)
            {
                scoutsField = Namings.Format(Namings.DialogTag("armySectorEvent"), Namings.BattleEvent(_eventType.Value)); ;
            }
            else
            {
                scoutsField = "";
            }
            for (int i = 0; i < scoutData.Count; i++)
            {
                var info = scoutData[i];
                scoutsField = $"{scoutsField}\n{info}\n";
            }
            var ans = new List<AnswerDialogData>();
            bool canAttack = true;
            var battleShip = myPlaer.Army.Army.FirstOrDefault(x => x.Ship.ShipType != ShipType.Base);
            if (battleShip != null)
            {
                var weapon = battleShip.Ship.WeaponsModuls.FirstOrDefault(x => x != null);
                if (weapon == null)
                {
                      weapon = myPlaer.Inventory.Weapons.FirstOrDefault(x => x != null);
                      if (weapon == null)
                      {
                          canAttack = false;
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
//            ans.Add(new AnswerDialogData(
//                Namings.Format(Namings.DialogTag("armyRun"), scoutsField),
//                () =>
//                {
//                }, null, false, true));

            var mesData = new MessageDialogData(masinMsg, ans);
            return mesData;
        }

    }

    private void InnerTake()
    {
        Take();
    }

    protected override void CacheArmy()
    {
        var player = new PlayerAITutor(name);

        var army = new List<StartShipPilotData>();


        void CreateByType(ShipType type)
        {
            var pilot = Library.CreateDebugPilot();
            var ship = Library.CreateShip(type, ShipConfig.droid, player, pilot);
            var startData = new StartShipPilotData(pilot, ship);
            startData.Ship.SetRepairPercent(.3f);
            if (type == ShipType.Turret)
                startData.Ship.PilotParameters.MaxShieldLvl = 2;
            if (type == ShipType.Middle)
            {
                if (ship.GetFreeWeaponSlot(out var inex))
                {
                    var weapon = Library.CreateWeaponByType(WeaponType.impulse);
                    ship.TryAddWeaponModul(weapon, inex);
                }
            }

            army.Add(startData);
        }

        if (_countEnemies == 1)
        {
            CreateByType(ShipType.Turret);
        }
        else
        {

            CreateByType(ShipType.Turret);
            for (int i = 0; i < _countEnemies - 1; i++)
            {
                CreateByType(ShipType.Middle);
            }
        }

        player.Army.SetArmy(army);
        _enemyPlayer = player;
    }

    public override bool OneTimeUsed()
    {
        return false;
    }
    public override void LeaveFromCell()
    {

    }
}