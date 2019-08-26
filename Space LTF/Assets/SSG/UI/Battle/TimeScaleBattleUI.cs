using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeScaleBattleUI : MonoBehaviour
{
    public Button Pause;
    public Button SpeedButton05;
    public Button SpeedButton1;
    public Button SpeedButton2;
    private BattleController _battleController;
    public void Init(BattleController battleController)
    {
        _battleController = battleController;
        _battleController.PauseData.OnPause += OnPause;
        OnClick1();
    }

    private void OnPause()
    {
        if (Time.timeScale == 0f)
        {
            SetButtonPause();
        }

        if (Time.timeScale == 0.5f)
        {
            SetButton05();
        }
        if (Time.timeScale == 1f)
        {
            SetButton1();
        }
        if (Time.timeScale == 2f)
        {
            SetButton2();
        }
    }

    public void OnClickPause()
    {
        _battleController.PauseData.Pause();
    }

    private void SetButtonPause()
    {
        Pause.interactable = false;
        SpeedButton1.interactable = SpeedButton2.interactable = SpeedButton05.interactable = true;
    }

    private void SetButton05()
    {

        SpeedButton05.interactable = false;
        SpeedButton1.interactable = SpeedButton2.interactable = Pause.interactable = true;
    }   
    private void SetButton1()
    {

        SpeedButton1.interactable = false;
        Pause.interactable = SpeedButton2.interactable = SpeedButton05.interactable = true;
    }   
    private void SetButton2()
    {

        SpeedButton2.interactable = false;
        SpeedButton1.interactable = Pause.interactable = SpeedButton05.interactable = true;
    }

    public void OnClick1()
    {
        _battleController.PauseData.ChangeCoreSpeed(1f);
    }   
    public void OnClick2()
    {

        _battleController.PauseData.ChangeCoreSpeed(2f);
    }   
    public void OnClick05()
    {

        _battleController.PauseData.ChangeCoreSpeed(0.5f);
    }

    public void Dispose()
    {
        _battleController.PauseData.OnPause -= OnPause;
    }
   
}
