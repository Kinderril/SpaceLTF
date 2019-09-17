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
    public TimerManager BattleTimerManager = new TimerManager();
    public InputManager InputManager;
    public Player MainPlayer;
    public PlayerStatistics Statistics;
    public StartMode StartMode;
    public DataBaseController DataBase;
    private bool _isFinalBattle;

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

    public void PreBattle(Player player1, Player player2,bool isFinalBattle = false)
    {
        _isFinalBattle = isFinalBattle;
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
        BattleTimerManager.Update();
    }

    public void EndGameWin()
    {
        if (_isFinalBattle)
        {
            EndGame(true);
        }
        else
        {
            WindowManager.Instance.OpenWindow(MainState.endBattle);
            Statistics.EndGame(EndBattleType.win);
            MainPlayer.MessagesToConsole.AddMsg("Battle won!");
        }

        //        MainPlayer.EndGame();
    }

    public void EndGameLose()
    {
        if (_isFinalBattle)
        {
            EndGame(false);
        }
        else
        {
            WindowManager.Instance.OpenWindow(MainState.loseBattle);
            Statistics.EndGame(EndBattleType.lose);
            MainPlayer.EndGame();
        }

    }

    public void EndGameRunAway()
    {
        WindowManager.Instance.OpenWindow(MainState.map);
//        WindowManager.Instance.OpenWindow(MainState.runAwayBattle);
        Statistics.EndGame(EndBattleType.runAway);
//        MainPlayer.EndGame();
        MainPlayer.MessagesToConsole.AddMsg("Running away complete");
    }

    public void EndGame(bool win)
    {
        Statistics.EndGameAll(win,MainPlayer);
        WindowManager.Instance.OpenWindow(MainState.endGame);
        var mapWindow = WindowManager.Instance.windows.FirstOrDefault(x => x.window is MapWindow) ;
        if (mapWindow.window  != null)
        {
            var mp = mapWindow.window as MapWindow;
            mp.GlobalMap.ClearAll();
        }

    }
}

