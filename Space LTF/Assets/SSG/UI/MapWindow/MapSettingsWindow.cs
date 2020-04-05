using System;
using System.Linq;
using UnityEngine;
using Toggle = UnityEngine.UI.Toggle;

public class MapSettingsWindow : MonoBehaviour
{
    public Toggle SoundToggle;
    public Toggle NoMouseMoveSoundToggle;
    public Toggle FXAAToggle;
    public Toggle EngToggle;
    public Toggle RusToggle;
    private Action _closeCallback;
    public GameObject ButtonHolder;
    public WindowKeys Keys;
//    public GameObject LangChanged;
    public VideoTutorialElement BattleTutorial;
    public GameObject BattleTutorialButton;

    public void OnSound()
    {

        CamerasController.Instance.SetAudioMainListener(SoundToggle.isOn);
    }

    public void OnNoMouseMoveSound()
    {
        CamerasController.Instance.SetNoMouseMove(NoMouseMoveSoundToggle.isOn);
    }

    public void OnFXAA()
    {
        CamerasController.Instance.FXAASwitch();
    }

    public void Init(bool withButtons,bool isBattle)
    {
        BattleTutorialButton.gameObject.SetActive(isBattle);
//        LangChanged.gameObject.SetActive(false);
        ButtonHolder.SetActive(withButtons);
        Keys.gameObject.SetActive(false);
        NoMouseMoveSoundToggle.isOn = (CamerasController.Instance.IsNoMouseMove);
        SoundToggle.isOn = (CamerasController.Instance.IsAudioEnable);
        FXAAToggle.isOn = (CamerasController.Instance.FxaaEnable);
        switch (Namings.LocTag)
        {
            case ELocTag.English:
                EngToggle.isOn = true;
                RusToggle.isOn = false;
                break;
            case ELocTag.Russian:
                RusToggle.isOn = true;
                EngToggle.isOn = false;
                break;
        }
    }

    public void Open(Action closeCallback)
    {

        _closeCallback = closeCallback;
        gameObject.SetActive(true);
    }

    public void OnBattleTutorialClick()
    {
        OnClickClose();
        BattleTutorial.Open();
    }


    void ChangeConfirm(Action callback,ELocTag tag)
    {
        WindowManager.Instance.ConfirmWindow.Init(callback, null, Namings.TagByType(tag,"WantChangeLang"));
    }


    public void OnClickEng()
    {
        if (EngToggle.isOn)
        {
            ChangeConfirm(() =>
            {
                Namings.English();
                RefreshAllTags();
            },ELocTag.English);
        }

//        LangChanged.gameObject.SetActive(true);
    }
    public void OnClickRus()
    {
        if (RusToggle.isOn)
        {
            ChangeConfirm(() =>
            {
                Namings.Rus();
                RefreshAllTags();
            },ELocTag.Russian);

        }

//        LangChanged.gameObject.SetActive(true);
    }

    private void RefreshAllTags()
    {
        var tags = GameObject.FindObjectsOfType<TextMeshProLocalizer>();
        foreach (var textMeshProLocalizer in tags)
        {
            textMeshProLocalizer.Refresh();
        }
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
