using System.Collections.Generic;


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
        {1, 18},
        {2, 28},
        {3, 37},
        {4, 46},
        {5, 55},
        {6, 64},
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
    public const int MICROCHIPS_COEF = 4;

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
    public const int EXP_COEF = 4;


    public const int BASE_WEAPON_MONEY_COST = 10;
    public const int LEVEL_WEAPON_MONEY_COST = 13;
    public const int SPELL_BASE_MONEY_COST = 16;
    public const int MODUL_BASE_MONEY_COST = 16;
    public const float MODUL_BASE_MONEY_COST_MID = 2f;
    public const int MODUL_LEVEL_MONEY_COST = 11;
    public const int MODUL_RARE_MONEY_COST = 26;


}

