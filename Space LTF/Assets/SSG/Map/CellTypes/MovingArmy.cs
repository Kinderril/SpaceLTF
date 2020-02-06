using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class MovingArmy
{
    public GlobalMapCell CurCell;
    public Player _player;
    private Action<MovingArmy> _destroyCallback;
    private bool _noStepNext;
    private List<IItemInv> _getRewardsItems;
    private bool _rewardsComplete = false;

    public MovingArmy(GlobalMapCell startCell, Action<MovingArmy> destroyCallback)
    {
        var humanPlayer = MainController.Instance.MainPlayer;
        _destroyCallback = destroyCallback;
        var humanPower = ArmyCreator.CalcArmyPower(humanPlayer.Army);

        CurCell = startCell;
        _player = new Player($"{ Namings.Tag("Destroyed")}:{MyExtensions.Random(3, 9999)}");
        var armyPower = humanPower * 1.5f;
        var armyData = ArmyCreatorLibrary.GetArmy(startCell.ConfigOwner);
        var army = ArmyCreator.CreateSimpleEnemyArmy(armyPower, armyData, _player);
        _player.Army.SetArmy(army);
        startCell.CurMovingArmy = this;
        BattleController.Instance.OnBattleEndCallback += OnBattleEndCallback;

        List<ItemType> items = new List<ItemType>() { ItemType.modul, ItemType.spell, ItemType.weapon };
        var rnd = items.RandomElement();
        items.Remove(rnd);
        _getRewardsItems = new List<IItemInv>();
        int deltaMin, deltaMax;

        foreach (var itemType in items)
        {
            switch (itemType)
            {
                case ItemType.weapon:
                    var weaponLvl = armyPower * 0.1f;
                    deltaMin = Mathf.Clamp((int)(weaponLvl - 1), 2, 5);
                    deltaMax = Mathf.Clamp((int)(weaponLvl + 1), 2, 6);
                    var item = Library.CreateWeapon(MyExtensions.Random(deltaMin, deltaMax));
                    item.CurrentInventory = _player.Inventory;
                    Debug.Log($"moving army weapon reward weaponLvl:{weaponLvl}  armyPower:{armyPower}");
                    _getRewardsItems.Add(item);
                    break;
                case ItemType.modul:
                    var modulLvl = armyPower * 0.06f;
                    deltaMin = Mathf.Clamp((int)(modulLvl - 1), 1, 2);
                    deltaMax = Mathf.Clamp((int)(modulLvl + 1), 1, 3);
                    var ite1m2 = Library.CreatSimpleModul(MyExtensions.Random(deltaMin, deltaMax));
                    ite1m2.CurrentInventory = _player.Inventory;
                    Debug.Log($"moving army weapon reward modulLvl:{modulLvl}  armyPower:{armyPower}");
                    _getRewardsItems.Add(ite1m2);
                    break;
                case ItemType.spell:
                    var spellLvl = armyPower * 0.08f;
                    deltaMin = Mathf.Clamp((int)(spellLvl - 1), 2, 4);
                    deltaMax = Mathf.Clamp((int)(spellLvl + 1), 2, 5);
                    var ite1m = Library.CreateSpell(MyExtensions.Random(deltaMin, deltaMax));
                    ite1m.CurrentInventory = _player.Inventory;
                    Debug.Log($"moving army weapon reward spell:{spellLvl}  armyPower:{armyPower}");
                    _getRewardsItems.Add(ite1m);
                    break;
            }
        }
    }

    public void GetRewardsItems()
    {
        RewardPlayer();
    }

    private void RewardPlayer()
    {
        var human = MainController.Instance.MainPlayer;
        var baseRep = Library.BATTLE_REPUTATION_AFTER_FIGHT * 2;
        human.ReputationData.WinBattleAgainst(_player.Army.BaseShipConfig, 2f);

        if (!_rewardsComplete)
        {
            _rewardsComplete = true;
            foreach (var rewardsItem in _getRewardsItems)
            {
                InventoryOperation.TryItemTransfered(human.Inventory, rewardsItem, b =>
                {

                });
            }
        }
    }

    private void OnBattleEndCallback(Player human, Player ai, EndBattleType win)
    {
        switch (win)
        {
            case EndBattleType.win:
                if (ai == _player)
                {
                    _destroyCallback?.Invoke(this);
                    Dispose();

                }

                break;
            case EndBattleType.lose:
                break;
            case EndBattleType.runAway:
                _noStepNext = true;
                break;
        }
    }

    public GlobalMapCell FindCellToMove(GlobalMapCell playersCell)
    {
        if (_noStepNext)
        {
            _noStepNext = false;
            return null;
        }

        if (playersCell == CurCell)
        {
            return null;
        }
        var ways = CurCell.GetCurrentPosibleWays();
        var selectedWay = ways.Where(x => !(x is GlobalMapNothing) && x.CurMovingArmy == null).ToList().RandomElement();
        return selectedWay;
    }

    public void Dispose()
    {
        CurCell.CurMovingArmy = null;
        BattleController.Instance.OnBattleEndCallback -= OnBattleEndCallback;
    }

    public string Name()
    {
        return Namings.TryFormat(Namings.Tag("MovingArmyName"), Namings.ShipConfig(_player.Army.BaseShipConfig));
    }

    public string ShortDesc()
    {
        var playersPower = MainController.Instance.MainPlayer.Army.GetPower();
        var thisPower = _player.Army.GetPower();
        var desc = PlayerArmy.ComparePowers(playersPower, thisPower);
        var status = MainController.Instance.MainPlayer.ReputationData.GetStatus(_player.Army.BaseShipConfig);
        var txt = Namings.TryFormat(Namings.Tag("MovingArmy"), _player.Name, Namings.ShipConfig(_player.Army.BaseShipConfig), Namings.Tag($"rep_{status.ToString()}"), desc);
        return txt;
    }
}

