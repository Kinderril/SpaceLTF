using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum StartMode
{
    DebugBattle,
    Story,
}

public class MainController : Singleton<MainController>
{
    public const string VERSION = "017";

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
        Library.Init();
        DataBase.Init();
        ShipNames.Init();
        TutorialController.Init();
        Statistics = PlayerStatistics.Load();
        WindowManager.Instance.Init();
        DataBase.DataStructPrefabs.CheckShipsWeaponsPosition();
        //        BattleController.Instance.LaunchBattle();
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
            BattleData = new NextBattleData(MainPlayer,Statistics);
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

    public void PreBattle(Player player1, Player player2,bool isFinalBattle = false, bool canRetire = true, BattlefildEventType? battleEvent = null)
    {
        BattleData.PreBattle(player1,player2,isFinalBattle, canRetire,battleEvent);
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




}

