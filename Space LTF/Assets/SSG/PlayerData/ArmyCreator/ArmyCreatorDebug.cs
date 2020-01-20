using System;
using System.Collections.Generic;
using UnityEngine;

public static class ArmyCreatorDebug
{
    public static void DoFight(float power)
    {
        var p = MainController.Instance.MainPlayer;

        List<ShipConfig> confis = new List<ShipConfig>()
        {
            ShipConfig.federation,
            ShipConfig.krios,
            ShipConfig.ocrons,
            ShipConfig.mercenary,
            ShipConfig.raiders,
        };
        var cfg = confis.RandomElement();
        var special = GetArmy(power, cfg);
        if (special != null)
        {
            MainController.Instance.PreBattle(p, special);
            //            BattleController.Instance.PreLaunchGame(p, special);
        }
        else
        {
            Debug.LogError($"can't crete special army to debug fight {cfg.ToString()}   power:{power}");
        }
    }


    static Player GetArmy(float power, ShipConfig _config)
    {
        List<Func<float, List<StartShipPilotData>>> posibleArmies = new List<Func<float, List<StartShipPilotData>>>();
        switch (_config)
        {
            case ShipConfig.raiders:
                posibleArmies.Add(ArmyCreatorSpecial.CreateBossBeamDistRaiders);
                posibleArmies.Add(ArmyCreatorSpecial.CreateBossLightMinesRaiders);
                break;
            case ShipConfig.federation:
                posibleArmies.Add(ArmyCreatorSpecial.CreateBossEngineLockersFederation);
                posibleArmies.Add(ArmyCreatorSpecial.CreateBossImpulseCritsFed);
                break;
            case ShipConfig.mercenary:
                posibleArmies.Add(ArmyCreatorSpecial.CreateBossCassetSprayMerc);
                posibleArmies.Add(ArmyCreatorSpecial.CreateBossManyEMIMerc);
                posibleArmies.Add(ArmyCreatorSpecial.CreateBossEMIFireMerc);
                break;
            case ShipConfig.ocrons:
                posibleArmies.Add(ArmyCreatorSpecial.CreateBossHeavyWithSpeedOcrons);
                posibleArmies.Add(ArmyCreatorSpecial.CreateBossMaxSelfDamageOcrons);
                break;
            case ShipConfig.krios:
                posibleArmies.Add(ArmyCreatorSpecial.CreateBossIgnoreShieldKrios);
                posibleArmies.Add(ArmyCreatorSpecial.CreateBossRocketTurnDistKrios);
                break;
            case ShipConfig.droid:
                break;
        }

        if (posibleArmies.Count > 0)
        {
            var rnd = posibleArmies.RandomElement();
            var army = rnd(power);
            var player = new Player("boss");
            player.Army.SetArmy(army);
            return player;
        }

        return null;
    }
}
