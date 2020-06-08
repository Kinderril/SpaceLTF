
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ArmyTutorAutoFightGlobalCell : ArmyTutorAbstractGlobalCell
{
    public ArmyTutorAutoFightGlobalCell(int power, ShipConfig config, int id, int Xind, int Zind, SectorData sector) : base(
        power, config, id, Xind, Zind, sector,false)
    {
        _countEnemies = 1;
        _eventType = null;
    }

    protected override void CacheArmy()
    {
        var player = new  PlayerAITutorUseAutoFight(name);
        var army = new List<StartShipPilotData>();

        void CreateByType(ShipType type)
        {
            var pilot = Library.CreateDebugPilot();
            var ship = Library.CreateShip(type, ShipConfig.ocrons, player.SafeLinks, pilot);
            var startData = new StartShipPilotData(pilot, ship);
//            startData.Ship.SetRepairPercent(.3f);
//            for (int i = 0; i < 10; i++)
//            {
//                startData.Pilot.UpgradeLevelByType(LibraryPilotUpgradeType.health, false);
//            }
            startData.Ship.RemoveItem(startData.Ship.CocpitSlot);
            startData.Ship.RemoveItem(startData.Ship.EngineSlot);
            startData.Ship.RemoveItem(startData.Ship.WingSlot);
//            if (type == ShipType.Middle)
//            {
////                startData.Ship.PilotParameters.MaxShieldLvl = 50;
//                if (ship.GetFreeWeaponSlot(out var inex))
//                {
//                    var weapon = Library.CreateWeaponByType(WeaponType.impulse);
//                    ship.TryAddWeaponModul(weapon, inex);
//                }
//            }

            army.Add(startData);
        }

        for (int i = 0; i < _countEnemies; i++)
        {
            CreateByType(ShipType.Middle);
        }
        player.Army.SetArmy(army);
        _enemyPlayer = player;
    }
}