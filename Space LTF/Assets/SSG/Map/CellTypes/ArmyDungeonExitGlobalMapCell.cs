using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmyDungeonExitGlobalMapCell : ArmyGlobalMapCell
{
    //    private Player _cachedArmy = null;
    // private List<IItemInv> _getRewardsItems;
    // private bool _rewardsComplete;
    // private bool _rewardsCached;
    public override int Power => (int)(_power * Library.COEF_CORE_ARMY);
    public ArmyDungeonExitGlobalMapCell(int power, ShipConfig config, int id, int Xind, int Zind, SectorData sector)
        : base(power, config, id, Xind, Zind, sector)
    {

    }

    protected override MessageDialogData GetLeavedActionInner()
    {
        // var movingArmy = CurMovingArmy;
        var ans = new List<AnswerDialogData>()
        {
            new AnswerDialogData(Namings.DialogTag("dungeonExitFinalWin"), RewardPlayer,  null,false,false),
        };
        var mesData = new MessageDialogData(Namings.DialogTag("dungeonExitGo"), ans);
        return mesData;
    }
    private void RewardPlayer()
    {
        var human = MainController.Instance.MainPlayer;
        human.ReputationData.WinBattleAgainst(_enemyPlayer.Army.BaseShipConfig, 3f);

    }
    protected override MessageDialogData GetDialog()
    {
        var myPlaer = MainController.Instance.MainPlayer;
//        var status = myPlaer.ReputationData.GetStatus(ConfigOwner);
//        bool isFriends = status == EReputationStatus.friend;

        var ans = new List<AnswerDialogData>();
        var scoutData = GetArmy().ScoutData.GetInfo(myPlaer.Parameters.Scouts.Level);
        string scoutsField = "";
        for (int i = 0; i < scoutData.Count; i++)
        {
            var info = scoutData[i];
            scoutsField = $"{scoutsField}\n{info}\n";
        }
        var masinMsg = Namings.Format(Namings.DialogTag("dungeonExitFinalBattle"), scoutsField);
        ans.Add(new AnswerDialogData(Namings.DialogTag("Attack"), Take));
        var mesData = new MessageDialogData(masinMsg, ans);
        return mesData;
    }

    protected override Player GetArmy()
    {
        if (_enemyPlayer != null)
        {
            return _enemyPlayer;
        }

        List<Func<float, List<StartShipPilotData>>> posibleArmies = new List<Func<float, List<StartShipPilotData>>>();
        switch (ConfigOwner)
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
            // var countTurrets = Power / 12;
            // var pointsToTurrents = countTurrets * (Library.BASE_WEAPON_VALUE * 2f + Library.BASE_TURRET_VALUE);
            // var pointsToShips = Power - pointsToTurrents;           
            var pointsToTurrents = Power * 0.7f;
            // var pointsToShips = Power * 0.3f;

            var player = new PlayerAIMilitaryFinal("battleFortressExit");
            List<StartShipPilotData> turrets = ArmyCreator.CreateTurrets(pointsToTurrents, player, ConfigOwner, new ArmyCreatorLogs(), out var points);
            var onlyTurrets = ArmyCreator.CalcArmyPower(turrets);
            var pointsToShips = Power - onlyTurrets;

            var rnd = posibleArmies.RandomElement();
            var army = rnd(pointsToShips);
#if UNITY_EDITOR          
            var onlyARmyPower = ArmyCreator.CalcArmyPower(army);
#endif
#if UNITY_EDITOR
#endif
            army.AddRange(turrets);
            player.Army.SetArmy(army);
            _enemyPlayer = player;
            CacheReward();
#if UNITY_EDITOR
            var armyPower = player.Army.GetPower();
            if (armyPower > Power)
            {
                Debug.LogError($"Army is more powerfull that I want.  Target:{Power}  result:{armyPower}");
                Debug.LogError($"pointsToShips:{pointsToShips}  pointsToTurrents:{pointsToTurrents}   onlyARmyPower:{onlyARmyPower}   onlyTurrets:{onlyTurrets}");
            }
#endif

            return _enemyPlayer;
        }

        _enemyPlayer = base.GetArmy();
        CacheReward();
        return _enemyPlayer;
    }

    private void CacheReward()
    {
    }
    public override bool OneTimeUsed()
    {
        return true;
    }
}

