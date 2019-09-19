using UnityEngine;
using System.Collections;
using System.Linq;

public class NextBattleData
{
    private bool _isFinalBattle;
    private bool _canRetire;
    private Player MainPlayer;
    private PlayerStatistics Statistics;
    public NextBattleData(Player mainPlayer, PlayerStatistics statistics)
    {
        MainPlayer = mainPlayer;
        Statistics = statistics;
    }

    public void PreBattle(Player player1, Player player2, bool isFinalBattle = false, bool canRetire = true)
    {
        _isFinalBattle = isFinalBattle;
        _canRetire = canRetire;
        WindowManager.Instance.OpenWindow(MainState.preBattle, new Tuple<Player, Player>(player1, player2));
    }

    public void LaunchBattle(Player greenSide, Player redSide)
    {
        BattleController.Instance.LaunchGame(greenSide, redSide, _canRetire);
    }

    public void EndGameWin()
    {
        if (_isFinalBattle)
        {
            EndGame(true);
        }
        else
        {
            Statistics.AddWin();
            WindowManager.Instance.OpenWindow(MainState.endBattle);
            Statistics.EndBattle(EndBattleType.win);
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
            WindowManager.Instance.OpenWindow(MainState.loseBattle);
            Statistics.EndBattle(EndBattleType.lose);
            MainPlayer.EndGame();
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
