using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;


public class WindowNewGame : BaseWindow
{
    // public PlayerStartParametersUI PlayerStartParametersUI;
    public ArmyTypeSelectorUI ArmyTypeSelectorUI;
    public StartGameWeaponsChooseUI StartGameWeaponsChooseUI;
    public SliderWithTextMeshPro SectorSize;
    // public SliderWithTextMeshProWithOff StartDeathTime;
    public SliderWithTextMeshPro CoresCount;
    public SliderWithTextMeshPro AdditionalPower;

    public DifficultyNewGame DifficultyNewGame;
    public SliderWithTextMeshPro SectorsCount;
    public TextMeshProUGUI DifficultyFIeld;
    private StartNewGameData gameData;
    public TextMeshProUGUI CollectedPointsField;
    public TextMeshProUGUI CollectedPointsInfo;
    public Button OpenNewOptions;

    public override void Init()
    {
        // PlayerStartParametersUI.Init();
        ArmyTypeSelectorUI.Init();
        StartGameWeaponsChooseUI.Init();
        DifficultyNewGame.Init();
        SectorSize.InitName(Namings.Tag("StartNewGameFieldSize"));
        // StartDeathTime.InitName(Namings.StartNewGameStartDeathTime);
        CoresCount.InitName(Namings.Tag("StartNewGameCoresCount"));
        AdditionalPower.InitName(Namings.Tag("AdditionalPower"));
        //        BasePower.InitName(Namings.StartNewGameBasePower);
        SectorsCount.InitName(Namings.Tag("StartNewGameSectorsCount"));
        SectorSize.InitBorders(Library.MIN_GLOBAL_SECTOR_SIZE, Library.MAX_GLOBAL_SECTOR_SIZE, true);
        // StartDeathTime.InitBorders(Library.MIN_GLOBAL_MAP_DEATHSTART, Library.MAX_GLOBAL_MAP_DEATHSTART, true);
        AdditionalPower.InitBorders(Library.MIN_GLOBAL_MAP_ADDITIONAL_POWER, Library.MAX_GLOBAL_MAP_ADDITIONAL_POWER, true);
        CoresCount.InitBorders(Library.MIN_GLOBAL_MAP_CORES, Library.MAX_GLOBAL_MAP_CORES, true);
        //        BasePower.InitBorders(Library.MIN_GLOBAL_MAP_BASE_POWER, Library.MAX_GLOBAL_MAP_BASE_POWER, true);
        SectorsCount.InitBorders(Library.MIN_GLOBAL_MAP_SECTOR_COUNT, Library.MAX_GLOBAL_MAP_SECTOR_COUNT, true);
        SectorSize.InitCallback(OnFieldChange);
        // StartDeathTime.InitCallback(OnFieldChange);
        AdditionalPower.InitCallback(OnFieldChange);
        CoresCount.InitCallback(OnFieldChange);
        SectorsCount.InitCallback(OnFieldChange);
        DifficultyNewGame.InitCallback(OnFieldChange);
        // StartDeathTime.SetValue(Library.MAX_GLOBAL_MAP_DEATHSTART + 1);
        UpdateStartData();
        CheckButtonIsOpen();
        AdditionalPower.SetValue(0);
        base.Init();
    }

    private void CheckButtonIsOpen()
    {
        var stat = MainController.Instance.Statistics;
        CollectedPointsField.text = $"{Namings.Tag("PointsToOpen")}\n{stat.CollectedPoints}/{PlayerStatistics.POINTS_TO_OPEN}";
        CollectedPointsInfo.text = Namings.Tag("RecievePoints");
        OpenNewOptions.interactable = stat.CanOpenNext();
    }

    public void OnClickOpenNew()
    {
        if (MainController.Instance.Statistics.TryOpenNext())
        {
            CheckButtonIsOpen();
            ArmyTypeSelectorUI.CheckOpens();
            StartGameWeaponsChooseUI.CheckOpens();
        }
    }

    private void OnFieldChange()
    {
        UpdateStartData();
    }

    public void OnClickStart()
    {
        // if (PlayerStartParametersUI.CheckFreePoints())
        // {
        //     PlayerStartParametersUI.OnParamClick(PlayerParameterType.repair, true);
        //     PlayerStartParametersUI.OnParamClick(PlayerParameterType.scout, true);
        // }
        //#if UNITY_EDITOR
        //        posibleStartSpells = new List<SpellType>();
        //        posibleStartSpells.Add(SpellType.engineLock);
        //
        //#endif
        UpdateStartData();
        MainController.Instance.CreateNewPlayerAndStartGame(gameData);

    }

    private void UpdateStartData()
    {
        var posibleStartSpells = new List<SpellType>()
        {
            SpellType.lineShot,
            SpellType.engineLock,
            SpellType.shildDamage,
            SpellType.mineField,
            SpellType.throwAround,
            SpellType.distShot,
            SpellType.artilleryPeriod,
            SpellType.repairDrones,
//            SpellType.spaceWall,
        };
        List<WeaponType> posibleStartWeapons = StartGameWeaponsChooseUI.Selected.GetAsList();
        var posibleSpells = posibleStartSpells.RandomElement(2);
#if UNITY_EDITOR
        //        posibleSpells.Add(SpellType.repairDrones);
#endif
        posibleSpells.Add(SpellType.rechargeShield);
        // posibleSpells.Add(SpellType.roundWave);
        // gameData = new StartNewGameData(PlayerStartParametersUI.GetCurrentLevels(),
        gameData = new StartNewGameData(new Dictionary<PlayerParameterType, int>(),
            ArmyTypeSelectorUI.Selected, posibleStartWeapons,
            SectorSize.GetValueInt(), SectorsCount.GetValueInt(), /*StartDeathTime.GetValueInt()*/999, CoresCount.GetValueInt(),
            DifficultyNewGame.CurDifficulty, posibleSpells, GetPowerPerTurn());
        var dif = Utils.FloatToChance(gameData.CalcDifficulty());
        DifficultyFIeld.text = String.Format(Namings.StatisticDifficulty, dif);
    }

    private int GetPowerPerTurn()
    {
        return AdditionalPower.GetValueInt();
    }

    public void OnClickRandomStart()
    {
        Dictionary<PlayerParameterType, int> ppar = new Dictionary<PlayerParameterType, int>();
        foreach (PlayerParameterType ppt in (PlayerParameterType[])Enum.GetValues(typeof(PlayerParameterType)))
        {
            ppar.Add(ppt, 1);
        }
        var allKeys = ppar.Keys.ToList();

        for (int i = 0; i < Library.START_PLAYER_FREE_PARAMETERS; i++)
        {
            var key1 = allKeys.RandomElement();
            ppar[key1] = ppar[key1] + 1;
        }
        var opneShips = MainController.Instance.Statistics.OpenShipsTypes;
        var rndConfi = opneShips.Where(x => x.IsOpen).ToList().RandomElement();
        var weaponsOpne = MainController.Instance.Statistics.WeaponsPairs;
        var rndWeapon = weaponsOpne.Where(x => x.IsOpen).ToList().RandomElement();
        SetData(ppar, rndConfi.Config, rndWeapon);
        SectorSize.SetValue(MyExtensions.Random(Library.MIN_GLOBAL_SECTOR_SIZE, Library.MAX_GLOBAL_SECTOR_SIZE));
        // StartDeathTime.SetValue(MyExtensions.Random(Library.MIN_GLOBAL_MAP_DEATHSTART, Library.MAX_GLOBAL_MAP_DEATHSTART));
        AdditionalPower.SetValue(MyExtensions.Random(Library.MIN_GLOBAL_MAP_ADDITIONAL_POWER, Library.MAX_GLOBAL_MAP_ADDITIONAL_POWER));
        CoresCount.SetValue(MyExtensions.Random(Library.MIN_GLOBAL_MAP_CORES, Library.MAX_GLOBAL_MAP_CORES));
        //        BasePower.SetValue(MyExtensions.Random(Library.MIN_GLOBAL_MAP_BASE_POWER, Library.MAX_GLOBAL_MAP_BASE_POWER));
        SectorsCount.SetValue(MyExtensions.Random(Library.MIN_GLOBAL_MAP_SECTOR_COUNT, Library.MAX_GLOBAL_MAP_SECTOR_COUNT));

    }

    private void SetData(Dictionary<PlayerParameterType, int> ppar,
        ShipConfig config, WeaponsPair weaponsPair)
    {
        // PlayerStartParametersUI.SetData(ppar);
        ArmyTypeSelectorUI.SetData(config);
        StartGameWeaponsChooseUI.SetData(weaponsPair);

    }


    public override void Dispose()
    {
        ArmyTypeSelectorUI.Dispose();
        // PlayerStartParametersUI.Dispose();
        StartGameWeaponsChooseUI.Dispose();
        base.Dispose();
    }

    public void DebugOpenAll()
    {
#if UNITY_EDITOR
        StartGameWeaponsChooseUI.DebugOpenAll();
        ArmyTypeSelectorUI.DebugOpenAll();
#endif
    }
}

