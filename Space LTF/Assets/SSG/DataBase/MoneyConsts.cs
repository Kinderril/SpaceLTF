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
        {2, 32},
        {3, 45},
        {4, 55},
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
        {1, 20},
        {2, 29},
        {3, 38},
        {4, 50},
        {5, 60},
        {6, 70},
        {7, 85},
    };

    public static Dictionary<int, int> SpellUpgrade = new Dictionary<int, int>()
    {
        {1, 18},
        {2, 28},
        {3, 39},
        {4, 45},
        {5, 50},
        {6, 60},
        {7,70},
    };
                                       

    public const int BASE_WEAPON_MONEY_COST = 10;
    public const int LEVEL_WEAPON_MONEY_COST = 13;
    public const int SPELL_BASE_MONEY_COST = 16;
    public const int MODUL_BASE_MONEY_COST = 16;
    public const float MODUL_BASE_MONEY_COST_MID = 2f;
    public const int MODUL_LEVEL_MONEY_COST = 11;
    public const int MODUL_RARE_MONEY_COST = 26;


}

