using System.Linq;

public delegate void BattleEndCallback(Player human, Player ai, EndBattleType win);

public class NextBattleData
{
    private bool _isFinalBattle;
    private bool _canRetire;
    private EBattlefildEventType? _battleEvent;
    private Player MainPlayer;
    private PlayerStatistics Statistics;
    public NextBattleData(Player mainPlayer, PlayerStatistics statistics)
    {
        MainPlayer = mainPlayer;
        Statistics = statistics;
    }

    public void PreBattle(Player player1, Player player2, bool isFinalBattle, bool canRetire)
    {
        _isFinalBattle = isFinalBattle;
        _canRetire = canRetire;
        _battleEvent = MainPlayer.MapData.CurrentCell.EventType;//  battleEvent;
        WindowManager.Instance.OpenWindow(MainState.preBattle, new Tuple<Player, Player>(player1, player2));
    }

    public void LaunchBattle(Player greenSide, Player redSide, EBattleType battleType)
    {
        BattleController.Instance.LaunchGame(greenSide, redSide, _canRetire, _battleEvent, battleType);
    }

    public void EndGameWin(EndBattleType winStatus)
    {
        Statistics.AddWin(MainPlayer.Army.BaseShipConfig);
        if (_isFinalBattle)
        {
            EndGame(true);
            Statistics.AddWinFinal(MainPlayer.Army.BaseShipConfig);
        }
        else
        {
            WindowManager.Instance.OpenWindow(MainState.endBattle);
            Statistics.EndBattle(winStatus);
            MainPlayer.MessagesToConsole.AddMsg("Battle won!");
        }


    }

    public void EndGameLose()
    {
        if (_isFinalBattle)
        {
            EndGame(false);
        }
        else
        {
            Statistics.EndBattle(EndBattleType.lose);
            //            MainPlayer.EndGame();
            MainController.Instance.BattleData.EndGame(false);
        }

    }

    public void EndGameRunAway()
    {
        WindowManager.Instance.OpenWindow(MainState.map);
        //        WindowManager.Instance.OpenWindow(MainState.runAwayBattle);
        Statistics.EndBattle(EndBattleType.runAway);
        //        MainPlayer.EndGame();
        MainPlayer.MessagesToConsole.AddMsg("Running away complete");
    }

    public void EndGame(bool win)
    {
        Statistics.EndGameAll(win, MainPlayer);
        WindowManager.Instance.OpenWindow(MainState.endGame);
        var mapWindow = WindowManager.Instance.windows.FirstOrDefault(x => x.window is MapWindow);
        if (mapWindow.window != null)
        {
            var mp = mapWindow.window as MapWindow;
            mp.ClearAll();
        }

    }

}
