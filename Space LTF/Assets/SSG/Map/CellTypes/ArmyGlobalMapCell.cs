using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[System.Serializable]
public class ArmyGlobalMapCell : GlobalMapCell
{
    public float HIRE_CHANCE = 0.01f;
    protected ArmyCreatorType _armyType;
    protected ShipConfig _config;
    private bool canHire = false;
    private Player _player;
    private BattlefildEventType? _eventType = null;

    public BattlefildEventType? EventType => _eventType;

    public int Power
    {
        get { return _power; }
    }

    protected int _power;

    protected virtual Player GetArmy()
    {
        if (_player == null)
        {
            CacheArmy();
        }

        return _player;
    }

    private void CacheArmy()
    {
        ArmyCreatorData data = new ArmyCreatorData(_config, true);
        switch (_armyType)
        {
            case ArmyCreatorType.rocket:
                data = new ArmyCreatorRocket(_config, true);
                break;
            case ArmyCreatorType.laser:
                data = new ArmyCreatorLaser(_config, true);
                break;
            case ArmyCreatorType.mine:
                data = new ArmyCreatorAOE(_config, true);
                break;
            case ArmyCreatorType.destroy:
                data = new ArmyCreatorDestroy(_config, true);
                break;
        }

        //        var data = new ArmyCreatorRocket(ShipConfig.mercenary);
        var player = new Player(name);
//        float coreShipChance = 0;
//        switch (_config)
//        {
//            case ShipConfig.droid:
//                coreShipChance = 0f;
//                break;
//            case ShipConfig.raiders:
//                coreShipChance = 0.1f;
//                break;
//            case ShipConfig.federation:
//                coreShipChance = 0.9f;
//                break;
//            case ShipConfig.mercenary:
//                coreShipChance = 0.5f;
//                break;
//        }
//        bool withCore = MyExtensions.IsTrue01(coreShipChance);
        var army = ArmyCreator.CreateSimpleEnemyArmy(_power, data, player);
        player.Army = army;
        switch (_config)
        {
            case ShipConfig.raiders:
            case ShipConfig.mercenary:
                canHire = MyExtensions.IsTrue01(HIRE_CHANCE);
                break;
        }

        _player = player;
    }

    public override bool CanCellDestroy()
    {
        return true;
    }

    public ArmyGlobalMapCell(int power, ShipConfig config, int id, ArmyCreatorType type, int Xind, int Zind,
        SectorData secto)
        : base(id, Xind, Zind, secto)
    {
        _config = config;
        _armyType = type;
        _power = power;
        if (Xind > 5 )
//        if (true)
        {
            if (MyExtensions.IsTrue01(0.25f))
            {
                WDictionary<BattlefildEventType> chance = new WDictionary<BattlefildEventType>(
                    new Dictionary<BattlefildEventType, float>()
                    {
                        {BattlefildEventType.asteroids, 1f},
                        {BattlefildEventType.shieldsOff, 1f},
                    });
                _eventType = chance.Random();
            }
        }
//#if UNITY_EDITOR
//        _eventType = BattlefildEventType.asteroids;
//#endif

        //        Debug.LogError($"ArmyGlobalMapCell:{Xind}  {Zind}");
    }

    public override void UpdatePowers(int visitedSectors, int startPower)
    {
        var nextPower = SectorData.CalcCellPower(visitedSectors + 1, _sector.Size, startPower);
        _player = null;
        Debug.Log($"Army power sector updated prev:{_power}. next:{nextPower}");
        _power = nextPower;
    }

    public override MessageDialogData GetDialog()
    {
        var myPlaer = MainController.Instance.MainPlayer;
        bool isSameSide = myPlaer.ReputationData.ReputationFaction[_config] > Library.PEACE_REPUTATION;
        string masinMsg;

        var ans = new List<AnswerDialogData>();
        var rep = MainController.Instance.MainPlayer.ReputationData.ReputationFaction[_config];
        var scoutData = GetArmy().ScoutData.GetInfo(myPlaer.Parameters.Scouts.Level);
        string scoutsField;
        if (_eventType.HasValue)
        {
            scoutsField = $"Sector event:{Namings.BattleEvent(_eventType.Value)}\n";
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

        if (isSameSide && rep > 40)
        {
            masinMsg = $"This fleet looks friendly.\n {scoutsField}";
            ans.Add(new AnswerDialogData($"Ask for help. [Reputation:{rep}]", null, DimlomatyOption));
        }
        else
        {
            int buyoutCost = _power;
            var playersPower = ArmyCreator.CalcArmyPower(myPlaer.Army);

            if (playersPower < _power)
            {
                if (myPlaer.MoneyData.HaveMoney(buyoutCost))
                {
                    ans.Add(new AnswerDialogData($"Try to buy out. [Cost :{buyoutCost}]", null,
                        () => BuyOutOption(buyoutCost)));
                }

                masinMsg = $"You see enemies. They look stronger than you. Shall we fight? \n {scoutsField}";
            }
            else
            {
                masinMsg = $"You see enemies. Shall we fight? \n {scoutsField}";
            }
        }

        ans.Add(new AnswerDialogData("Fight", Take));

        if (isSameSide)
        {
            ans.Add(new AnswerDialogData("Leave.", null));
        }
        else
        {
            ans.Add(new AnswerDialogData(
                String.Format("Try Run. [Scouts:{0}]", ScoutsLevel),
                () =>
                {
                    bool doRun = MyExtensions.IsTrue01((float) ScoutsLevel / 4f);
#if UNITY_EDITOR
                    doRun = true;
#endif
                    if (doRun)
                    {
                        WindowManager.Instance.InfoWindow.Init(null, "Running away complete.");
                    }
                    else
                    {
                        WindowManager.Instance.InfoWindow.Init(Take, "Fail! Now you must fight.");
                    }
                },null,false));
        }

        var mesData = new MessageDialogData(masinMsg, ans);
        return mesData;
    }


    public string PowerDesc()
    {
        var player = MainController.Instance.MainPlayer;
        var playersPower = ArmyCreator.CalcArmyPower(player.Army);

        var isSameSecto = _sector.Id == player.MapData.CurrentCell.SectorId;
        float powerToCompare;
        if (isSameSecto)
        {
            powerToCompare = Power;
//            Debug.LogError($"power2  :{powerToCompare} ");
        }
        else
        {
            powerToCompare = SectorData.CalcCellPower(player.MapData.VisitedSectors + 1, _sector.Size,
                _sector.StartPowerGalaxy);
//            Debug.LogError($"power1  :{powerToCompare}");
        }
        var delta = playersPower / powerToCompare;
       if (delta < 0.95f)
        {
            return "Risky";
        }

        if (delta > 1.15f)
        {
            return "Easily";
        }

        return "Comparable";
    }

    private MessageDialogData BuyOutOption(int buyoutCost)
    {
        var player = MainController.Instance.MainPlayer;
        var ans = new List<AnswerDialogData>();
        player.MoneyData.RemoveMoney(buyoutCost);
        ans.Add(new AnswerDialogData("Ok"));
        var mesData = new MessageDialogData($"Buyout complete. You lose {buyoutCost} credits.", ans);
        return mesData;
    }

    private MessageDialogData DimlomatyOption()
    {
        var ans = new List<AnswerDialogData>();
        ans.Add(new AnswerDialogData("Ok"));
        var rep = MainController.Instance.MainPlayer.ReputationData.ReputationFaction[_config];
        bool doDip = MyExtensions.IsTrue01((float) rep / PlayerReputationData.MAX_REP);
        if (doDip)
        {
            int a = 0;
            switch (_config)
            {
                case ShipConfig.droid:
                case ShipConfig.raiders:
                case ShipConfig.mercenary:
                    a = 0;
                    break;
                case ShipConfig.federation:
                case ShipConfig.ocrons:
                case ShipConfig.krios:
                    a = 1;
                    break;
            }

            string helpInfo;
            if (a == 0)
            {
                var money = AddMoney(_power / 3, _power / 2);
                helpInfo = String.Format("They give you some credits {0}", money);
            }
            else
            {
                var player = MainController.Instance.MainPlayer;
//                IItemInv item;
                bool canAdd;
                string itemName;
                int slot;
                if (MyExtensions.IsTrue01(.5f))
                {
                    var m = Library.CreatSimpleModul(2);
                    itemName = Namings.SimpleModulName(m.Type);
                    canAdd = player.Inventory.GetFreeSimpleSlot(out slot);
                    if (canAdd)
                    {
                        player.Inventory.TryAddSimpleModul(m, slot);
                    }
                }
                else
                {
                    var w = Library.CreateWeapon(false);
                    itemName = Namings.Weapon(w.WeaponType);
                    canAdd = player.Inventory.GetFreeWeaponSlot(out slot);
                    if (canAdd)
                    {
                        player.Inventory.TryAddWeaponModul(w, slot);
                    }
                }

                if (canAdd)
                {
                    helpInfo = String.Format("They give {0}.", itemName);
                }
                else
                {
                    helpInfo = "But you have no free space";
                }
            }

            var pr = $"They help you as much as they can. {helpInfo}";
            var mesData = new MessageDialogData(pr, ans);
            return mesData;
        }
        else
        {
            var mesData = new MessageDialogData("They can't help you now", ans);
            return mesData;
        }
    }

    public override Color Color()
    {
        return new Color(255f / 255f, 102f / 255f, 0f / 255f);
    }

    public override bool OneTimeUsed()
    {
        return true;
    }

    public override string Desc()
    {
        return $"Amry:{Namings.ShipConfig(_config)} \n Zone:{Namings.BattleEvent(_eventType)}";
    }

    public override void Take()
    {
        MainController.Instance.PreBattle(MainController.Instance.MainPlayer, GetArmy(),false,true, _eventType);
    }

    public override MessageDialogData GetLeavedActionInner()
    {
        if (MainController.Instance.Statistics.LastBattle == EndBattleType.win)
        {
            var player = MainController.Instance.MainPlayer;
            player.ReputationData.WinBattleAgainst(_config);
            var msg = player.AfterBattleOptions.GetDialog(player.MapData.Step, Power);
            return msg;
        }
        else
        {
            return null;
        }
    }

    public ShipConfig GetConfig()
    {
        return _config;
    }
}