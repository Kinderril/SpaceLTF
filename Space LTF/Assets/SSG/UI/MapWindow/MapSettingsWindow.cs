using System;
using System.Linq;
using UnityEngine;
using Toggle = UnityEngine.UI.Toggle;

public class MapSettingsWindow : MonoBehaviour
{
    public Toggle SoundToggle;
    public Toggle FXAAToggle; 
    public Toggle EngToggle;
    public Toggle RusToggle;
    private Action _closeCallback;
    public GameObject ButtonHolder;
    public WindowKeys Keys;
    public GameObject LangChanged;

    public void OnSound()
    {

        CamerasController.Instance.SetAudioMainListener(SoundToggle.isOn);
    }

    public void OnFXAA()
    {
        CamerasController.Instance.FXAASwitch();
    }

    public void Init(bool withButtons)
    {
        LangChanged.gameObject.SetActive(false);
        ButtonHolder.SetActive(withButtons);
        Keys.gameObject.SetActive(false);
        SoundToggle.isOn = (CamerasController.Instance.IsAudioEnable);
        FXAAToggle.isOn = (CamerasController.Instance.FxaaEnable);
        EngToggle.isOn = Namings.LocTag == ELocTag.English;
        RusToggle.isOn = Namings.LocTag == ELocTag.Russian;
    }

    public void Open(Action closeCallback)
    {

        _closeCallback = closeCallback;
        gameObject.SetActive(true);
    }

    public void OnClickEng()
    {
        Namings.English();
        LangChanged.gameObject.SetActive(true);
    }
    public void OnClickRus()
    {
        Namings.Rus();
        LangChanged.gameObject.SetActive(true);
    }

    public void OnClickClose()
    {
        gameObject.SetActive(false);
        _closeCallback();
    }

    public void OnClickExit()
    {
        WindowManager.Instance.ConfirmWindow.Init(OnConfigrmClick, null, Namings.Tag("MapExit"));
    }

    private void OnConfigrmClick()
    {
        WindowManager.Instance.OpenWindow(MainState.start);
        var mapWindow = WindowManager.Instance.windows.FirstOrDefault(x => x.window is MapWindow);
        if (mapWindow.window != null)
        {
            var mp = mapWindow.window as MapWindow;
            mp.ClearAll();
        }
    }
}
