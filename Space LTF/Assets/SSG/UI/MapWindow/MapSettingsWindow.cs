using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    public GameObject ExitButtonMenu;
    public WindowKeys Keys;
//    public GameObject LangChanged;
    public VideoTutorialElement BattleTutorial;
    public GameObject BattleTutorialButton;
    public TMP_Dropdown ResolutionDropdown;



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
        InitDropDown();
        ResolutionDropdown.value = CamerasController.Instance.CurIndexResolution;
        BattleTutorialButton.gameObject.SetActive(isBattle);
//        LangChanged.gameObject.SetActive(false);
        ButtonHolder.SetActive(withButtons);
        ExitButtonMenu.SetActive(!withButtons);
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

    private bool _isDropDownInited = false;
    private void InitDropDown()
    {
        if (_isDropDownInited)
        {
            return;
        }

        _isDropDownInited = true;

        var lisOptions = new List<TMP_Dropdown.OptionData>();
        foreach (var resolutionData in CamerasController.Instance._resolutionDatas)
        {
            lisOptions.Add(new TMP_Dropdown.OptionData(resolutionData.Name));

        }
        ResolutionDropdown.AddOptions(lisOptions);
    }

    public void OnResolutionChange()
    {
        CamerasController.Instance.ChangeResolutionToIndex(ResolutionDropdown.value);
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
