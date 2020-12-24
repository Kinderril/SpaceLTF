using System;


[System.Serializable]
public class PlayerParameter
{
    public string Name;
    public bool IsBattle;
    public virtual int Level { get; set; }
    public virtual PlayerParameterType ParameterType { get; set; }
    protected PlayerSafe _player;

    [field: NonSerialized]
    public event Action<PlayerParameter> OnUpgrade;

    public PlayerParameter(PlayerSafe player)
    {
        _player = player;
    }

    public bool CanUpgrade()
    {
        if (IsBattle)
        {
            return CanUpgradeSpells(Level);
        }
        else
        {
            return CanUpgradePassive(Level);
        }
    }

    public void TryUpgrade()
    {
        int cost = UpgradeCost();
        if (CanUpgrade())
        {
            WindowManager.Instance.ConfirmWindow.Init(() =>
            {

                _player.RemoveMoney(cost);
                Level++;
                if (OnUpgrade != null)
                {
                    OnUpgrade(this);
                }
            }, null, Namings.Format(Namings.Tag("UpgParams"), Name, cost));
        }
        else
        {
            WindowManager.Instance.InfoWindow.Init(null, $"{Namings.Tag("cantUpgrade")}\n{Namings.Format(Namings.Tag("NotEnoughtMoneyLong"), cost)}" );
        }
    }

    private bool CanUpgradePassive(int lvl)
    {
        if (lvl >= MoneyConsts.MAX_PASSIVE_LEVEL)
        {
            return false;
        }
        var cost = MoneyConsts.PassiveUpgrade[lvl];
        var haveMoney = _player.HaveMoney(cost);
        return haveMoney;
    }

    private bool CanUpgradeSpells(int lvl)
    {
        if (lvl >= MoneyConsts.MAX_SPELLS_LEVEL)
        {
            return false;
        }
        var cost = MoneyConsts.SpellsCostUpgrade[lvl];
        var haveMoney = _player.HaveMoney(cost);
        return haveMoney;
    }

    public int UpgradeCost()
    {
        int cost;
        if (IsBattle)
        {
            cost = MoneyConsts.SpellsCostUpgrade[Level];
        }
        else
        {
            cost = MoneyConsts.PassiveUpgrade[Level];
        }
        return cost;
    }

    public bool IsMaxLevel()
    {
        if (IsBattle)
        {
            if (Level >= MoneyConsts.MAX_SPELLS_LEVEL)
            {
                return true;
            }
        }
        else
        {
            if (Level >= MoneyConsts.MAX_PASSIVE_LEVEL)
            {
                return true;
            }
        }
        return false;

    }
}

