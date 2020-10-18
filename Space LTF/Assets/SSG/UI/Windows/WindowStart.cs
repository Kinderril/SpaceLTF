using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WindowStart : BaseWindow
{
    public Button AchievementsButton;
    public TextMeshProUGUI DemoField;
    public GameObject TutorAdditional;
    public TutorButtonsStart TutorButtonsStart;
    public TutorButtonsStart ButtonOpenNewGame;
    public CampLoadContainer CampLoader;
    public override void Init()
    {
        DemoField.gameObject.SetActive(true);
        CamerasController.Instance.MusicControl.StartMenuAudio();
        var mainTxt = Namings.Tag("DemoStart");
        CampLoader.OnCloseClick();

        AchievementsButton.interactable = (true);
//#if UNITY_EDITOR || Develop
//        AchievementsButton.interactable = (true);
//#else                                                     
//        AchievementsButton.interactable = (false);
//
//#endif
#if Demo
        DemoField.gameObject.SetActive(true);  
      mainTxt = $"{mainTxt}{Namings.Tag("DemoStart2")}";
#else
        //        DemoField.gameObject.SetActive(false);
#endif

        DemoField.text = mainTxt;
        if (Screen.currentResolution.width < 1200 || Screen.currentResolution.height < 700)
        {
            WindowManager.Instance.InfoWindow.Init(null,Namings.Tag("resolutionTooLow"));
        }
        bool haveTutor = HaveTutor();
        TutorAdditional.gameObject.SetActive(!haveTutor);
        base.Init();
    }

    public void OnClickNewChampaing()
    {
        StartNewChampaing();
    }

    private void StartNewChampaing()
    {
        MainController.Instance.Campaing.PlayerNewGame();
    }

    public void OnClickNewGame()
    {
        StartNewSandBoxGame();
    }
    public void OnClickAchievements()
    {
        WindowManager.Instance.OpenWindow(MainState.achievements);
    }

    public void OnClickStatistics()
    {
        WindowManager.Instance.OpenWindow(MainState.statistics);
    }
    public void OnClickSettings()
    {
        WindowManager.Instance.OpenSettingsSettings(EWindowSettingsLauch.menu);
    }

    private bool HaveTutor()
    {
            return MainController.Instance.Statistics.AllTimeCollectedPoints >= PlayerStatistics.TUTORIAL_POINTS;
    }

    private void StartNewSandBoxGame()
    {
        bool haveTutor = HaveTutor();

        if (haveTutor)
        {
            StartGame();
        }
        else
        {
            WindowManager.Instance.ConfirmWindow.Init(StartGame, null, Namings.Tag("startGameNoTutor"),
                Namings.Tag("toGame"),Namings.Tag("toTutor"));
        }
        void StartGame()
        {
            WindowManager.Instance.OpenWindow(MainState.startNewGame);
        }
    }

    public void OnClickExprolerMode()
    {
        WindowManager.Instance.OpenWindow(MainState.exprolerModeStart);
    }

    public void OnClickStartTutorial()
    {
        TutorButtonsStart.Init();
//        TutorButtonsStart.gameObject.transform.position = TutorAdditional.transform.position;
    }

    public void OnClickButtonOpenNewGame()
    {
        ButtonOpenNewGame.Init();
//        TutorButtonsStart.gameObject.transform.position = TutorAdditional.transform.position;
    }

    public void OnClickLoadCamp()
    {
        CampLoader.Init();
    }

    public void OnClickLoad()
    {
        if (MainController.Instance.TryLoadPlayerSandBox())
        {
            WindowManager.Instance.OpenWindow(MainState.map);
        }
        else
        {
            WindowManager.Instance.InfoWindow.Init(null, Namings.Tag("NoSafeGame"));
        }
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}

