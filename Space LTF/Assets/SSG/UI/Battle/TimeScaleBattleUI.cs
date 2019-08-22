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
        OnClick1();
    }

    public void OnClickPause()
    {
        Pause.interactable = false;
        SpeedButton1.interactable = SpeedButton2.interactable = SpeedButton05.interactable = true;
        _battleController.PauseData.Pause();
    }   
    public void OnClick1()
    {

        SpeedButton1.interactable = false;
        Pause.interactable = SpeedButton2.interactable = SpeedButton05.interactable = true;
        _battleController.ChangerCoreTimeSpeed(1f);
    }   
    public void OnClick2()
    {

        SpeedButton2.interactable = false;
        SpeedButton1.interactable = Pause.interactable = SpeedButton05.interactable = true;
        _battleController.ChangerCoreTimeSpeed(2f);
    }   
    public void OnClick05()
    {

        SpeedButton05.interactable = false;
        SpeedButton1.interactable = SpeedButton2.interactable = Pause.interactable = true;
        _battleController.ChangerCoreTimeSpeed(0.5f);
    }
   
}
