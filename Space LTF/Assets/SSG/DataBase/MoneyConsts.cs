using System.Collections.Generic;


public static class MoneyConsts
{
    public const int MAX_SPELLS_LEVEL = 4;
    public const int MAX_PASSIVE_LEVEL = 4;


    //Upgrade
    public static Dictionary<int, int> SpellsCostUpgrade = new Dictionary<int, int>()
    {
        {1, 31},
        {2, 49},
        {3, 71},
        {4, 83},
    };

    public static Dictionary<int, int> PassiveUpgrade = new Dictionary<int, int>()
    {
        {1, 31},
        {2, 49},
        {3, 71},
        {4, 83},
    };

    public static Dictionary<int, int> WeaponUpgrade = new Dictionary<int, int>()
    {
        {1, 22},
        {2, 30},
        {3, 39},
        {4, 49},
        {5, 58},
        {6, 69},
        {7, 73},
    };

    public static Dictionary<int, int> SpellUpgrade = new Dictionary<int, int>()
    {
        {1, 18},
        {2, 28},
        {3, 46},
        {4, 52},
        {5, 58},
        {6, 66},
        {7, 70},
    };

    public const int LOW_MICROCHIP_COEF = 1;
    public const int MICROCHIPS_COEF = 3;

    public static Dictionary<int, int> SpellMicrochipsElements = new Dictionary<int, int>()
    {
        {1, 0},
        {2, 1 * MICROCHIPS_COEF},
        {3, 2 * MICROCHIPS_COEF},
        {4, 3 * MICROCHIPS_COEF},
        {5, 4 * MICROCHIPS_COEF},
        {6, 5 * MICROCHIPS_COEF},
    };

    public const int LOW_EXP_COEF = 1;
    public const int EXP_COEF = 3;


    public const int BASE_WEAPON_MONEY_COST = 20;
    public const int LEVEL_WEAPON_MONEY_COST = 16;
    public const int SPELL_BASE_MONEY_COST = 36;
    public const int MODUL_BASE_MONEY_COST = 29;
    public const float MODUL_BASE_MONEY_COST_MID = 4f;
    public const int MODUL_LEVEL_MONEY_COST = 20;
    public const int MODUL_RARE_MONEY_COST = 26;


}

