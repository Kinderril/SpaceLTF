using System;
using UnityEngine;

public enum StartMode
{
    DebugBattle,
    Story,
}

public class MainController : Singleton<MainController>
{
    public static string VERSION = "011b.3";

    public TimerManager BattleTimerManager = new TimerManager();
    public InputManager InputManager;
    public Player MainPlayer;
    public PlayerStatistics Statistics;
    public StartMode StartMode;
    public DataBaseController DataBase;
    public NextBattleData BattleData;
    public TutorialController TutorialController;

    void Awake()
    {
        try
        {
            LogHandler.Instance.Init();
//            Debug.LogError("FIRST ERROR TEST MESSAGE");
            SteamManager.Instance.Init();

            LocalizationManager.Instance.Init();
            CamerasController.Instance.StartCheck();
            Library.Init();
            DataBase.Init();
            LibraryChecker.DoCheck();
            ShipNames.Init();
            TutorialController.Init();
            Statistics = PlayerStatistics.Load();
            WindowManager.Instance.Init();
            DataBase.DataStructPrefabs.CheckShipsWeaponsPosition();
            SteamStatsAndAchievements.Instance.RequestStats();


        }
        catch (Exception e)
        {
            Debug.LogError($"Start error {e}");
            throw;
        }
    }

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        switch (StartMode)
        {
            case StartMode.DebugBattle:
                WindowManager.Instance.OpenWindow(MainState.debugStart);
                break;
            case StartMode.Story:
                WindowManager.Instance.OpenWindow(MainState.start);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public bool TryLoadPlayer()
    {
        Player playerToLoad;
        if (Player.LoadGame(out playerToLoad))
        {
            MainPlayer = playerToLoad;
            BattleData = new NextBattleData(MainPlayer, Statistics);
            return true;
        }
        return false;
    }

    public void CreateNewPlayerAndStartGame(StartNewGameData data)
    {
        MainPlayer = new Player("Next Player", data.startParametersLevels);
        Statistics.PlayNewGame(data);
        MainPlayer.PlayNewGame(data);
        WindowManager.Instance.OpenWindow(MainState.map);
        BattleData = new NextBattleData(MainPlayer, Statistics);

    }

    public void PreBattle(Player player1, Player player2, bool isFinalBattle = false, bool canRetire = true, BattlefildEventType? battleEvent = null)
    {
        BattleData.PreBattle(player1, player2, isFinalBattle, canRetire, battleEvent);
    }

    public void LaunchBattle(Player greenSide, Player redSide)
    {
        BattleData.LaunchBattle(greenSide, redSide);
    }

    public void ReturnToMapFromCell()
    {
        WindowManager.Instance.OpenWindow(MainState.map);
    }

    void Update()
    {
        BattleTimerManager.Update();
    }


    public static void CheckVersion()
    {
#if Demo
        VERSION = VERSION + "demo";
#endif     
#if Develop 
          
        VERSION = VERSION + "dev";
#endif
    }
}

