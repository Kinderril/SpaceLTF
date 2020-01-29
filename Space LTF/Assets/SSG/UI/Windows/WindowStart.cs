using TMPro;
using UnityEngine;


public class WindowStart : BaseWindow
{
    public StatisticsController Statistics;
    public TextMeshProUGUI DemoField;
    public override void Init()
    {
        Statistics.Init();
        DemoField.gameObject.SetActive(true);
      var mainTxt = Namings.Tag("DemoStart");

#if Demo
        DemoField.gameObject.SetActive(true);  
      mainTxt = $"{mainTxt}{Namings.Tag("DemoStart2")}";
#else
//        DemoField.gameObject.SetActive(false);
#endif

        DemoField.text = mainTxt;
        base.Init();
    }

    public void OnClickNewGame()
    {
        StartNewGame();
    }
    public void OnClickAchievements()
    {
        WindowManager.Instance.OpenWindow(MainState.achievements);
    }
    public void OnClickSettings()
    {
        WindowManager.Instance.OpenSettingsSettings(false);
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
            WindowManager.Instance.InfoWindow.Init(null, Namings.NoSafeGame);
        }
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}

