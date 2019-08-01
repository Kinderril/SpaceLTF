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
    public TimerManager TimerManager = new TimerManager();
    public InputManager InputManager;
    public Player MainPlayer;
    public PlayerStatistics Statistics;
    public StartMode StartMode;
    public DataBaseController DataBase;

    void Awake()
    {
        Library.Init();
        DataBase.Init();
        ShipNames.Init();
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
            return true;
        }
        return false;
    }

    public void CreateNewPlayerAndStartGame(StartNewGameData data)
    {
        MainPlayer = new Player("Next Player", data.startParametersLevels);

        MainPlayer.PlayNewGame(data);
        WindowManager.Instance.OpenWindow(MainState.map);

    }

    public void PreBattle(Player player1, Player player2)
    {
        WindowManager.Instance.OpenWindow(MainState.preBattle, new Tuple<Player, Player>(player1, player2));
    }

    public void LaunchBattle(Player greenSide, Player redSide)
    {
        BattleController.Instance.LaunchGame(greenSide, redSide);
    }

    public void ReturnToMapFromCell()
    {
        WindowManager.Instance.OpenWindow(MainState.map);
    }

//    public void LaunchBattle(List<StartShipPilotData> greenSide, List<StartShipPilotData> redSide)
//    {
//        BattleController.Instance.LaunchGame(greenSide,redSide);
//    }

    void Update()
    {
        TimerManager.Update();
    }

    public void EndGameWin()
    {
        WindowManager.Instance.OpenWindow(MainState.endBattle);
        Statistics.EndGame(true);
        MainPlayer.MessagesToConsole.AddMsg("Battle won!");
        //        MainPlayer.EndGame();
    }

    public void EndGameLose()
    {
        WindowManager.Instance.OpenWindow(MainState.loseBattle);
        Statistics.EndGame(false);
        MainPlayer.EndGame();
    }

    public void EndGameRunAway()
    {
        WindowManager.Instance.OpenWindow(MainState.runAwayBattle);
        Statistics.EndGame(false);
//        MainPlayer.EndGame();
        MainPlayer.MessagesToConsole.AddMsg("Running away complete");
    }
}

