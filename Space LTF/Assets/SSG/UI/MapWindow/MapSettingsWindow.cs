using System;
using UnityEngine;
using Toggle = UnityEngine.UI.Toggle;

public class MapSettingsWindow : MonoBehaviour
{
    public Toggle SoundToggle;
    private Action _closeCallback;
    public void OnSound()
    {
        CamerasController.Instance.MainListenerSwitch();
    }

    public void Init()
    {
        SoundToggle.isOn = (CamerasController.Instance.IsAudioEnable);
    }

    public void Open(Action closeCallback)
    {

        _closeCallback = closeCallback;
        gameObject.SetActive(true);
    }

    public void OnClickClose()
    {
        gameObject.SetActive(false);
        _closeCallback();
    }

    public void OnClickExit()
    {
        WindowManager.Instance.ConfirmWindow.Init(OnConfigrmClick,null,Namings.MapExit);
    }

    private void OnConfigrmClick()
    {
         WindowManager.Instance.OpenWindow(MainState.start);
    }
}
