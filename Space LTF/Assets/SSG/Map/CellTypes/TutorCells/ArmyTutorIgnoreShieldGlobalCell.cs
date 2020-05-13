
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ArmyTutorIgnoreShieldGlobalCell : ArmyTutorAbstractGlobalCell
{
    public ArmyTutorIgnoreShieldGlobalCell(int power, ShipConfig config, int id, int Xind, int Zind, SectorData sector) : base(
        power, config, id, Xind, Zind, sector,false)
    {
        _countEnemies = 1;
        _eventType = null;
    }

    protected override void CacheArmy()
    {
        var player = new  PlayerAITutorWearModuls(name);
        var army = new List<StartShipPilotData>();
        void CreateByType(ShipType type)
        {
            var pilot = Library.CreateDebugPilot();
            var ship = Library.CreateShip(type, ShipConfig.krios, player, pilot);
            var startData = new StartShipPilotData(pilot, ship);
//            startData.Ship.SetRepairPercent(.3f);
            if (type == ShipType.Middle)
            {
                startData.Ship.PilotParameters.MaxShieldLvl = 50;
                startData.Ship.RemoveItem(startData.Ship.CocpitSlot);
                startData.Ship.RemoveItem(startData.Ship.EngineSlot);
                startData.Ship.RemoveItem(startData.Ship.WingSlot);
            }

            army.Add(startData);
        }

        for (int i = 0; i < _countEnemies; i++)
        {
            CreateByType(ShipType.Middle);
        }
        player.Army.SetArmy(army);
        _enemyPlayer = player;
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
            string scoutsField = "";
            var ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData(Namings.DialogTag("Attack"), GetWeaponsAndTake));
            masinMsg = Namings.Format(Namings.DialogTag("armyTutorIgnoreShield"), scoutsField);

            var mesData = new MessageDialogData(masinMsg, ans);
            return mesData;
        }

    }

    private void GetWeaponsAndTake()
    {
        var winner = MainController.Instance.MainPlayer;
        var addExp = winner.Army.Army.FirstOrDefault(x => x.Ship.ShipType != ShipType.Base);

        var sum = 0;
        for (int i = 0; i < 8; i++)
        {
            sum += Library.PilotLvlUpCost(i + 1);
        }

        if (addExp != null)
        {
            addExp.Pilot.AddMoney(sum);
        }

        int index;
        for (int i = 0; i < 2; i++)
        {
            var laser = Library.CreateWeaponByType(WeaponType.laser);
            if (winner.Inventory.GetFreeSlot(out index, ItemType.weapon))
            {
                winner.Inventory.TryAddWeaponModul(laser, index);
            }
        }


        var anti = Library.CreatSimpleModul(SimpleModulType.WeaponShieldIgnore, 1);
        if (winner.Inventory.GetFreeSlot(out index, ItemType.modul))
        {
            winner.Inventory.TryAddSimpleModul(anti, index);
        }

        Take();
    }
}