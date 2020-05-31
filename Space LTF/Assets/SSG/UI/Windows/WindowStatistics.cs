using UnityEngine;
using System.Collections;

public class WindowStatistics : BaseWindow
{
    public StatisticsController Statistics;
    public override void Init()
    {
        Statistics.Init();
        base.Init();
    }

    public void OnCloseClick()
    {
        WindowManager.Instance.OpenWindow(MainState.start);
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
