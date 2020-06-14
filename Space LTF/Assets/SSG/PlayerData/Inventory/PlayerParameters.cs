using System.Collections.Generic;

public enum PlayerParameterType
{
    scout,
    // diplomaty,
    repair,
    chargesCount,
    chargesSpeed,
    engineParameter,
}

[System.Serializable]
public class PlayerParameters
{
    //    private Player _player;
    public PlayerParameter Scouts;
    // public PlayerParameter Diplomaty;
    public PlayerParameter ChargesCount;
    public PlayerParameter ChargesSpeed;
    public PlayerParameter Repair;
    public PlayerParameter EnginePower;


    public PlayerParameters(PlayerSafe player, Dictionary<PlayerParameterType, int> level = null)
    {
        Scouts = new PlayerParameter(player) { IsBattle = false, Level = 1, Name = Namings.ParameterName(PlayerParameterType.scout), ParameterType = PlayerParameterType.scout };
        ChargesCount = new PlayerParameter(player) { IsBattle = true, Level = 1, Name = Namings.ParameterName(PlayerParameterType.chargesCount), ParameterType = PlayerParameterType.chargesCount };
        ChargesSpeed = new PlayerParameter(player) { IsBattle = true, Level = 1, Name = Namings.ParameterName(PlayerParameterType.chargesSpeed), ParameterType = PlayerParameterType.chargesSpeed };
        Repair = new PlayerParameter(player) { IsBattle = false, Level = 1, Name = Namings.ParameterName(PlayerParameterType.repair), ParameterType = PlayerParameterType.repair };
        EnginePower = new PlayerParameter(player) { IsBattle = true, Level = 1, Name = Namings.ParameterName(PlayerParameterType.engineParameter), ParameterType = PlayerParameterType.engineParameter };
        if (level != null)
        {
            foreach (var lvl in level)
            {
                switch (lvl.Key)
                {
                    case PlayerParameterType.scout:
                        Scouts.Level = lvl.Value;
                        break;
                    // case PlayerParameterType.diplomaty:
                    //     Diplomaty.Level = lvl.Value;
                    //     break;
                    case PlayerParameterType.repair:
                        Repair.Level = lvl.Value;
                        break;
                    case PlayerParameterType.chargesCount:
                        ChargesCount.Level = lvl.Value;
                        break;
                    case PlayerParameterType.chargesSpeed:
                        ChargesSpeed.Level = lvl.Value;
                        break;
                    case PlayerParameterType.engineParameter:
                        EnginePower.Level = lvl.Value;
                        break;
                }
            }
        }
    }

    public float RepairPercentPerStep()
    {
        return (1 + Repair.Level) * Library.REPAIR_PERCENT_PERSTEP_PERLEVEL;
    }



    public int GetChargesToBattle()
    {
        return ChargesCount.Level + Library.BASE_CHARGES_COUNT;
    }

    public bool ScoutsIsMax()
    {
        return !(Scouts.Level <= MoneyConsts.MAX_PASSIVE_LEVEL);
    }

    public Dictionary<PlayerParameterType, int> GetAsDictionary()
    {
        Dictionary<PlayerParameterType, int> level = new Dictionary<PlayerParameterType, int>();


        level.Add(PlayerParameterType.scout,Scouts.Level);
        level.Add(PlayerParameterType.chargesCount,ChargesCount.Level);
        level.Add(PlayerParameterType.chargesSpeed,ChargesSpeed.Level);
        level.Add(PlayerParameterType.repair,Repair.Level);
        level.Add(PlayerParameterType.engineParameter,EnginePower.Level);
        return level;

    }
}

