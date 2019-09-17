using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WindowStart : BaseWindow
{
    public StatisticsController Statistics;
    public override void Init()
    {
        Statistics.Init();
        base.Init();
    }

    public void OnClickNewGame()
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
//        MainController.Instance.CreateNewPlayer();
        WindowManager.Instance.OpenWindow(MainState.startNewGame);
    }

    public void OnClickLoad()
    {
        if (MainController.Instance.TryLoadPlayer())
        {
            WindowManager.Instance.OpenWindow(MainState.map);
        }
        else
        {
            WindowManager.Instance.InfoWindow.Init(null,Namings.NoSafeGame);
        }
    }
}

