using System;
using System.Linq;

[System.Serializable]
public class PlayerByStepDamage
{
    public int BaseSteps;
    public int _curRemainSteps;
    [field: NonSerialized]
    public event Action<int> OnStep;

    public bool IsEnable => _isEnable;
    private bool _isEnable;
    private Player _player;
    public PlayerByStepDamage()
    {
    }

    public void Init(int baseSteps, Player player)
    {
        _player = player;
        BaseSteps = baseSteps;
        _curRemainSteps = BaseSteps;
        _isEnable = (BaseSteps < Library.MAX_GLOBAL_MAP_DEATHSTART);
    }

    public void StepComplete(int step)
    {
        if (_isEnable)
        {
            _curRemainSteps--;
            if (_curRemainSteps == -1)
            {
                WindowManager.Instance.InfoWindow.Init(null, Namings.UnstableCore0);
            }
            if (_curRemainSteps < 0)
            {
                if (_curRemainSteps >= -3)
                {
                    BrokeShipWithRandom();
                    //                    WindowManager.Instance.InfoWindow.Init(null, Namings.UnstableCore1);
                }
                else if (_curRemainSteps >= -9)
                {
                    BrokeShipWithRandom();
                    BrokeRandomItem();
                    //                    WindowManager.Instance.InfoWindow.Init(null, Namings.UnstableCore2);

                }
                else
                {
                    WindowManager.Instance.InfoWindow.Close();
                    MainController.Instance.BattleData.EndGame(false);
                }
            }
            DoAction();
        }
    }

    private void BrokeRandomItem()
    {
        var player = MainController.Instance.MainPlayer;
        foreach (var data in player.Army.Army.Suffle())
        {
            var items = data.Ship.GetAllItems().Where(x => x != null).ToList();
            if (items.Count > 0)
            {
                var itemToDel = items.RandomElement();
                switch (itemToDel.ItemType)
                {
                    case ItemType.weapon:
                        var item = itemToDel as WeaponInv;
                        data.Ship.RemoveWeaponModul(item);
                        player.MessagesToConsole.AddMsg(String.Format(Namings.Tag("BrokenItem"), Namings.Weapon(item.WeaponType)));
                        break;
                    case ItemType.modul:
                        var item1 = itemToDel as BaseModulInv;
                        player.MessagesToConsole.AddMsg(String.Format(Namings.Tag("BrokenItem"), (item1.Name)));
                        data.Ship.RemoveSimpleModul(item1);
                        break;
                    case ItemType.spell:
                        var item2 = itemToDel as BaseSpellModulInv;
                        player.MessagesToConsole.AddMsg(String.Format(Namings.Tag("BrokenItem"), item2.Name));
                        data.Ship.RemoveSpellModul(item2);
                        break;
                }
                return;
            }
        }
    }

    private void BrokeShipWithRandom()
    {
        var player = MainController.Instance.MainPlayer;
        foreach (var data in player.Army.Army)
        {
            if (MyExtensions.IsTrueEqual())
            {
                var per = data.Ship.HealthPercent;
                data.Ship.SetRepairPercent(per * 0.5f);
            }
        }
    }
    public void Repair()
    {
        if (_isEnable)
        {
            _curRemainSteps = BaseSteps;
            DoAction();
        }
    }

    private void DoAction()
    {
        if (OnStep != null)
        {
            OnStep(_curRemainSteps);
        }
    }

}
