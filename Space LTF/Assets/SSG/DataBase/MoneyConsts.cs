using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class MoneyConsts
{
    public const int MAX_SPELLS_LEVEL = 4;
    public const int MAX_PASSIVE_LEVEL = 4;


    //Upgrade
    public static Dictionary<int, int> SpellsCostUpgrade = new Dictionary<int, int>()
    {
        {1, 16},
        {2, 27},
        {3, 35},
        {4, 40},
    };

    public static Dictionary<int, int> PassiveUpgrade = new Dictionary<int, int>()
    {
        {1, 15},
        {2, 23},
        {3, 40},
        {4, 50},
    };

    public static Dictionary<int, int> WeaponUpgrade = new Dictionary<int, int>()
    {
        {1, 14},
        {2, 24},
        {3, 28},
        {4, 40},
        {5, 50},
        {6, 60},
        {7, 75},
    };

    public static Dictionary<int, int> SpellUpgrade = new Dictionary<int, int>()
    {
        {1, 13},
        {2, 24},
        {3, 28},
        {4, 32},
        {5, 36},
        {6, 40},
        {7, 45},
    };
                                       

    public const int BASE_WEAPON_MONEY_COST = 10;
    public const int LEVEL_WEAPON_MONEY_COST = 13;
    public const int SPELL_BASE_MONEY_COST = 16;
    public const int MODUL_BASE_MONEY_COST = 16;
    public const float MODUL_BASE_MONEY_COST_MID = 2f;
    public const int MODUL_LEVEL_MONEY_COST = 11;
    public const int MODUL_RARE_MONEY_COST = 26;


}

