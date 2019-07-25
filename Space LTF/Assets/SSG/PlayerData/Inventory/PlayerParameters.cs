using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum PlayerParameterType
{
    scout,
    diplomaty,
    repair,
    chargesCount,
    chargesSpeed,
}

[System.Serializable]
public class PlayerParameters
{
//    private Player _player;
    public PlayerParameter Scouts;
    public PlayerParameter Diplomaty;
    public PlayerParameter ChargesCount;
    public PlayerParameter ChargesSpeed;
    public PlayerParameter Repair;


    public PlayerParameters(Player player,Dictionary<PlayerParameterType,int> level = null)
    {
        Scouts = new PlayerParameter(player) {IsBattle = false, Level = 1, Name = Namings.ParameterName(PlayerParameterType.scout) };
        Diplomaty = new PlayerParameter(player) {IsBattle = false, Level = 1, Name = Namings.ParameterName(PlayerParameterType.diplomaty) };
        ChargesCount = new PlayerParameter(player) {IsBattle = true, Level = 1, Name = Namings.ParameterName(PlayerParameterType.chargesCount) };
        ChargesSpeed = new PlayerParameter(player) {IsBattle = true, Level = 1, Name = Namings.ParameterName(PlayerParameterType.chargesSpeed) };
        Repair = new PlayerParameter(player) {IsBattle = false, Level = 1, Name = Namings.ParameterName(PlayerParameterType.repair) };
        if (level != null)
        {
            foreach (var lvl in level)
            {
                switch (lvl.Key)
                {
                    case PlayerParameterType.scout:
                        Scouts.Level = lvl.Value;
                        break;
                    case PlayerParameterType.diplomaty:
                        Diplomaty.Level = lvl.Value;
                        break;
                    case PlayerParameterType.repair:
                        Repair.Level = lvl.Value;
                        break;
                    case PlayerParameterType.chargesCount:
                        ChargesCount.Level = lvl.Value;
                        break;
                    case PlayerParameterType.chargesSpeed:
                        ChargesSpeed.Level = lvl.Value;
                        break;
                }
            }
        }
    }

    public float RepairPercentPerStep()
    {
        return (1 + Repair.Level)*Library.REPAIR_PERCENT_PERSTEP_PERLEVEL;
    }

    

    public int GetChargesToBattle()
    {
        return ChargesCount.Level + 4;
    }

    public bool ScoutsIsMax()
    {
        return !(Scouts.Level <= MoneyConsts.MAX_PASSIVE_LEVEL);
    }
}

