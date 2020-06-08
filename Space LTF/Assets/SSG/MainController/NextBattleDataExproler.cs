using UnityEngine;
using System.Collections;
using System.Linq;

public class NextBattleDataExproler : NextBattleData
{
    public NextBattleDataExproler(Player mainPlayer, PlayerStatistics statistics)
        : base(mainPlayer, statistics)
    {

    }

    public override void EndGame(bool win)
    {
        Statistics.EndGameAll(win, MainPlayer);
        MainController.Instance.Exproler.SetLastBalleData(win);
        var mapWindow = WindowManager.Instance.windows.FirstOrDefault(x => x.window is MapWindow);
        if (mapWindow.window != null)
        {
            var mp = mapWindow.window as MapWindow;
            mp.ClearAll();
        }
        WindowManager.Instance.OpenWindow(MainState.exprolerModeGlobalMap);

    }
    public override void MoveToWindowEndBattle()
    {                                  
        WindowManager.Instance.OpenWindow(MainState.exprolerModeGlobalMap);
    }
}
