using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnComeToDelegate(GlobalMapCell to, GlobalMapCell from);

[System.Serializable]
public class ArmyGlobalMapCell : GlobalMapCell
{
    public float HIRE_CHANCE = 0.01f;
    protected Player _enemyPlayer;
    protected BattlefildEventType? _eventType = null;
    public BattlefildEventType? EventType => _eventType;

    protected int _power;
    public virtual int Power {
        get { return _power; }
        set { _power = value; }
    }

    protected int _additionalPower;


    [field: NonSerialized]
    public event OnComeToDelegate OnComeToCell;

    protected virtual Player GetArmy()
    {
        if (_enemyPlayer == null)
        {
            CacheArmy();
        }

        return _enemyPlayer;
    }

    protected virtual void CacheArmy()
    {
        ArmyCreatorData data = ArmyCreatorLibrary.GetArmy(ConfigOwner);
        var player = new PlayerAI(name);
        var army = ArmyCreator.CreateSimpleEnemyArmy(Power, data, player);
        player.Army.SetArmy(army);
        _enemyPlayer = player;
    }

    public override bool CanCellDestroy()
    {
        return true;
    }
    public override void ComeTo(GlobalMapCell from)
    {
        OnComeToCell?.Invoke(this, from);
    }
    public ArmyGlobalMapCell(int power, ShipConfig config, int id, int Xind, int Zind,
        SectorData sector)
        : base(id, Xind, Zind, sector, config)
    {
        // _armyType = type;
        Power = power;
        if (Xind > 5)
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
    }

    public override void UpdatePowers(int visitedSectors, int startPower, int additionalPower)
    {
        _additionalPower = additionalPower;
        var nextPower = SectorData.CalcCellPower(visitedSectors, _sector.Size, startPower, _additionalPower);
        _enemyPlayer = null;
        // Debug.Log($"Army power sector updated prev:{_power}. next:{nextPower}");
        Power = nextPower;
    }

    protected override MessageDialogData GetDialog()
    {
        var myPlaer = MainController.Instance.MainPlayer;
        var status = myPlaer.ReputationData.GetStatus(ConfigOwner);
        bool isFriends = status == EReputationStatus.friend;
        string masinMsg;

        var ans = new List<AnswerDialogData>();
        var rep = MainController.Instance.MainPlayer.ReputationData.ReputationFaction[ConfigOwner];
        var scoutData = GetArmy().ScoutData.GetInfo(myPlaer.Parameters.Scouts.Level);
        string scoutsField;
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

        ans.Add(new AnswerDialogData(Namings.DialogTag("Attack"), Take));
        if (isFriends)
        {
            masinMsg = Namings.Format(Namings.DialogTag("armyFrendly"), scoutsField); ;
            ans.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("armyAskHelp"), rep), null, DimlomatyOption));
        }
        else
        {
            int buyoutCost = Power;
            var playersPower = ArmyCreator.CalcArmyPower(myPlaer.Army);

            if (myPlaer.MoneyData.HaveMoney(buyoutCost) && (ConfigOwner == ShipConfig.mercenary || ConfigOwner == ShipConfig.raiders))
            {
                ans.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("armyBuyOut"), buyoutCost), null,
                    () => BuyOutOption(buyoutCost)));
            }
            if (status == EReputationStatus.neutral)
            {
                masinMsg = Namings.Format(Namings.DialogTag("armyNeutral"), scoutsField);
            }
            else
            {
                if (playersPower < Power)
                {
                    masinMsg = Namings.Format(Namings.DialogTag("armyStronger"), scoutsField);
                }
                else
                {
                    masinMsg = Namings.Format(Namings.DialogTag("armyShallFight"), scoutsField);
                }
            }
        }


        if (isFriends || status == EReputationStatus.neutral)
        {
            ans.Add(new AnswerDialogData(Namings.Tag("leave"), null));
        }
        else
        {
            ans.Add(new AnswerDialogData(
                Namings.Format(Namings.DialogTag("armyRun"), scoutsField),
                () =>
                {
                }, null, false, true));
        }

        var mesData = new MessageDialogData(masinMsg, ans);
        return mesData;
    }

    private MessageDialogData BuyOutOption(int buyoutCost)
    {
        var player = MainController.Instance.MainPlayer;
        var ans = new List<AnswerDialogData>();
        player.MoneyData.RemoveMoney(buyoutCost);
        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        var mesData = new MessageDialogData(Namings.Format(Namings.DialogTag("armyBuyoutComplete"), buyoutCost), ans);
        return mesData;
    }

    private MessageDialogData DimlomatyOption()
    {
        var ans = new List<AnswerDialogData>();
        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        var rep = MainController.Instance.MainPlayer.ReputationData.ReputationFaction[ConfigOwner];
        bool doDip = MyExtensions.IsTrue01((float)rep / PlayerReputationData.MAX_REP);
        if (doDip)
        {
            int a = 0;
            switch (ConfigOwner)
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
                var money = AddMoney(Power / 3, Power / 2);
                helpInfo = Namings.Format("They give you some credits {0}", money);
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
                    var w = Library.CreateDamageWeapon(false);
                    itemName = Namings.Weapon(w.WeaponType);
                    canAdd = player.Inventory.GetFreeWeaponSlot(out slot);
                    if (canAdd)
                    {
                        player.Inventory.TryAddWeaponModul(w, slot);
                    }
                }

                if (canAdd)
                {
                    helpInfo = Namings.Format("They give {0}.", itemName);
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
        var army = Namings.Tag("Amry");
        if (_eventType.HasValue)
        {
            return $"{army}:{Namings.ShipConfig(ConfigOwner)}\nZone:{Namings.BattleEvent(_eventType.Value)}";
        }
        else
        {
            return $"{army}:{Namings.ShipConfig(ConfigOwner)}";
        }
    }

    public override void Take()
    {
        MainController.Instance.PreBattle(MainController.Instance.MainPlayer, GetArmy(), false, true, _eventType);
    }

    protected override MessageDialogData GetLeavedActionInner()
    {
        if (MainController.Instance.Statistics.LastBattle == EndBattleType.win)
        {
            var player = MainController.Instance.MainPlayer;
            player.ReputationData.WinBattleAgainst(ConfigOwner);
            var msg = player.AfterBattleOptions.GetDialog(player.MapData.Step, Power, ConfigOwner);
            return msg;
        }
        else
        {
            return null;
        }
    }

    public ShipConfig GetConfig()
    {
        return ConfigOwner;
    }

    public string PowerDesc()
    {
        return PlayerArmy.PowerDesc(_sector, Power, _additionalPower);
    }
}