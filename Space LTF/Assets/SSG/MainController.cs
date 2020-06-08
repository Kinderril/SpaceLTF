using System;
using UnityEngine;

public enum StartMode
{
    DebugBattle,
    Story,
}

public class MainController : Singleton<MainController>
{
    public static string VERSION = "013b.0";

    public TimerManager BattleTimerManager = new TimerManager();
    public InputManager InputManager;
    public Player MainPlayer;
    public PlayerSlotsContainer SafeContainers;
    public PlayerStatistics Statistics;
    public ExprolerController Exproler;
    public StartMode StartMode;
    public DataBaseController DataBase;
    public NextBattleData BattleData;
    public TutorialController TutorialController;

    void Awake()
    {
        try
        {
            Exproler = new ExprolerController();
            SafeContainers = new PlayerSlotsContainer();
            SafeContainers.Init();
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
        switch (data.GameNode)
        {
            case EGameMode.sandBox:
            case EGameMode.simpleTutor:
            case EGameMode.advTutor:
            default:
                BattleData = new NextBattleData(MainPlayer, Statistics);
                break;
            case EGameMode.safePlayer:
                BattleData = new NextBattleDataExproler(MainPlayer, Statistics);
                break;
        }
    }

    public void PreBattle(Player player1, Player player2, bool isFinalBattle = false, bool canRetire = true)
    {
        BattleData.PreBattle(player1, player2, isFinalBattle, canRetire);
    }

    public void LaunchBattle(Player greenSide, Player redSide)
    {
        var predAsAi = redSide as IPlayerAIWithBattleEvent;
        EBattleType battle = EBattleType.standart;
        if (predAsAi != null)
        {
            battle = predAsAi.EBattleType;
        }
        BattleData.LaunchBattle(greenSide, redSide, battle);
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

