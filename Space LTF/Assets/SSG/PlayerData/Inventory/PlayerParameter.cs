using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[System.Serializable]
public class PlayerParameter
{
    public string Name;
    public bool IsBattle;
    public virtual int Level { get; set; }
    protected Player _player;

    [field: NonSerialized]
    public event Action<PlayerParameter> OnUpgrade;

    public PlayerParameter(Player player)
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
        if (CanUpgrade())
        {
            int cost = UpgradeCost();
            WindowManager.Instance.ConfirmWindow.Init(() =>
            {

                _player.MoneyData.RemoveMoney(cost);
                Level++;
                if (OnUpgrade != null)
                {
                    OnUpgrade(this);
                }
            },null,String.Format(Namings.UpgParams,Name, cost));
        }
        else
        {
            WindowManager.Instance.InfoWindow.Init(null,Namings.cantUpgrade);
        }
    }

    private bool CanUpgradePassive(int lvl)
    {
        if (lvl >= MoneyConsts.MAX_PASSIVE_LEVEL)
        {
            return false;
        }
        var cost = MoneyConsts.PassiveUpgrade[lvl];
        var haveMoney = _player.MoneyData.HaveMoney(cost);
        return haveMoney;
    }

    private bool CanUpgradeSpells(int lvl)
    {
        if (lvl >= MoneyConsts.MAX_SPELLS_LEVEL)
        {
            return false;
        }
        var cost = MoneyConsts.SpellsCostUpgrade[lvl];
        var haveMoney = _player.MoneyData.HaveMoney(cost);
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

