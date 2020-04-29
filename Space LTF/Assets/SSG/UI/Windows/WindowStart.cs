using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WindowStart : BaseWindow
{
    public StatisticsController Statistics;
    public Button AchievementsButton;
    public TextMeshProUGUI DemoField;
    public GameObject TutorAdditional;
    public override void Init()
    {
        Statistics.Init();
        DemoField.gameObject.SetActive(true);
        CamerasController.Instance.MusicControl.StartMenuAudio();
        var mainTxt = Namings.Tag("DemoStart");

        AchievementsButton.interactable = (true);
//#if UNITY_EDITOR || Develop
//        AchievementsButton.interactable = (true);
//#else                                                     
//        AchievementsButton.interactable = (false);
//
//#endif
#if Demo
        DemoField.gameObject.SetActive(true);  
      mainTxt = $"{mainTxt}{Namings.Tag("DemoStart2")}";
#else
        //        DemoField.gameObject.SetActive(false);
#endif

        DemoField.text = mainTxt;
        if (Screen.currentResolution.width < 1200 || Screen.currentResolution.height < 700)
        {
            WindowManager.Instance.InfoWindow.Init(null,Namings.Tag("resolutionTooLow"));
        }
        bool haveTutor = HaveTutor();
        TutorAdditional.gameObject.SetActive(!haveTutor);
        base.Init();
    }

    public void OnClickNewGame()
    {
        StartNewGame();
    }
    public void OnClickAchievements()
    {
        WindowManager.Instance.OpenWindow(MainState.achievements);
    }
    public void OnClickSettings()
    {
        WindowManager.Instance.OpenSettingsSettings(EWindowSettingsLauch.menu);
    }

    private bool HaveTutor()
    {
            return MainController.Instance.Statistics.AllTimeCollectedPoints >= PlayerStatistics.TUTORIAL_POINTS;
    }

    private void StartNewGame()
    {
        bool haveTutor = HaveTutor();

        if (haveTutor)
        {
            StartGame();
        }
        else
        {
            WindowManager.Instance.ConfirmWindow.Init(StartGame, null, Namings.Tag("startGameNoTutor"),
                Namings.Tag("toGame"),Namings.Tag("toTutor"));
        }
        void StartGame()
        {
            WindowManager.Instance.OpenWindow(MainState.startNewGame);
        }
    }
    public void OnClickStartTutorial()
    {

        void StartTutor() 
        {
            var gameData = UpdateStartData();
            MainController.Instance.CreateNewPlayerAndStartGame(gameData);
        }

        WindowManager.Instance.ConfirmWindow.Init(StartTutor,null,Namings.Tag("wantStartTutor"));

    }


    private StartNewGameData UpdateStartData()
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
            SpellType.vacuum,
            SpellType.hookShot,
//            SpellType.spaceWall,
        };
        List<WeaponType> posibleStartWeapons = new List<WeaponType>();
        var posibleSpells = posibleStartSpells.RandomElement(2);
#if UNITY_EDITOR
        //        posibleSpells.Add(SpellType.repairDrones);
#endif
        posibleSpells.Add(SpellType.rechargeShield);
        // posibleSpells.Add(SpellType.roundWave);
        // gameData = new StartNewGameData(PlayerStartParametersUI.GetCurrentLevels(),
        var gameData = new StartNewGameData(new Dictionary<PlayerParameterType, int>(),
            ShipConfig.mercenary, posibleStartWeapons, 4, 1, 2, 0, 1, posibleSpells, 0, true);
        var dif = Utils.FloatToChance(gameData.CalcDifficulty());
        return gameData;
    }

    public void OnClickLoad()
    {
        if (MainController.Instance.TryLoadPlayer())
        {
            WindowManager.Instance.OpenWindow(MainState.map);
        }
        else
        {
            WindowManager.Instance.InfoWindow.Init(null, Namings.Tag("NoSafeGame"));
        }
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}

