using UnityEngine;
using System.Collections;
using System.Linq;

public class NextBattleDataCapmaing : NextBattleData
{
    public NextBattleDataCapmaing(Player mainPlayer, PlayerStatistics statistics)
        : base(mainPlayer, statistics)
    {

    }

    public override void EndGame(bool win)
    {
        Statistics.EndGameAll(win, MainPlayer);
        var mapWindow = WindowManager.Instance.windows.FirstOrDefault(x => x.window is MapWindow);
        if (mapWindow.window != null)
        {
            var mp = mapWindow.window as MapWindow;
            mp.ClearAll();
        }

        if (win)
        {
            WindowManager.Instance.OpenWindow(MainState.campaingEndAct);
        }
        else
        {
            WindowManager.Instance.OpenWindow(MainState.endGame);
        }

    }
    public override void MoveToWindowEndBattle()
    {
        if (_winAct)
        {
            WindowManager.Instance.OpenWindow(MainState.campaingEndAct);
        }
        else
        {
            WindowManager.Instance.OpenWindow(MainState.start);
        }
    }
}
