using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[System.Serializable]
public class PlayerParameterDiplomacy : PlayerParameter
{

    public override int Level
    {
        get { return base.Level + (int)((_player.ReputationData.Reputation - 50) * Library.REPURARTION_TO_DIPLOMATY_COEF); }
    }


    public PlayerParameterDiplomacy(Player player)
        : base(player)
    {

    }
}

