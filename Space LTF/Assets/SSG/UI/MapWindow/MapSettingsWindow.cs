using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Toggle = UnityEngine.UI.Toggle;

public enum EWindowSettingsLauch
{
    battle,
    map,
    menu
}

public class MapSettingsWindow : MonoBehaviour
{
    public Toggle SoundToggle;
    public Toggle NoMouseMoveSoundToggle;
    public Toggle FXAAToggle;
    public Toggle EngToggle;
    public Toggle RusToggle;
    public Toggle EspToggle;
    private Action _closeCallback;
    public WindowKeys Keys;
//    public GameObject LangChanged;
    public VideoTutorialElement BattleTutorial;
    public VideoTutorialElement MapTutorial;
    public GameObject BattleTutorialButton;
    public TMP_Dropdown ResolutionDropdown;

    public GameObject MapButtons;
    public GameObject ExitButtonMenu;
    public GameObject BattleButtons;

    private EWindowSettingsLauch _settingsLauch;



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

    public void Init(EWindowSettingsLauch settingsLauch)
    {
        _settingsLauch = settingsLauch;
        InitDropDown();
        ResolutionDropdown.value = CamerasController.Instance.CurIndexResolution;
        switch (settingsLauch)
        {
            case EWindowSettingsLauch.battle:
                BattleTutorialButton.gameObject.SetActive(true);
                MapButtons.gameObject.SetActive(false);
                ExitButtonMenu.gameObject.SetActive(false);
                BattleButtons.gameObject.SetActive(true);
                break;
            case EWindowSettingsLauch.map:
                BattleTutorialButton.gameObject.SetActive(true);
                MapButtons.gameObject.SetActive(true);
                ExitButtonMenu.gameObject.SetActive(false);
                BattleButtons.gameObject.SetActive(false);
                break;
            case EWindowSettingsLauch.menu:
                BattleTutorialButton.gameObject.SetActive(false);
                MapButtons.gameObject.SetActive(false);
                ExitButtonMenu.gameObject.SetActive(true);
                BattleButtons.gameObject.SetActive(false);
                break;
        }
        Keys.gameObject.SetActive(false);
        NoMouseMoveSoundToggle.isOn = (CamerasController.Instance.IsNoMouseMove);
        SoundToggle.isOn = (CamerasController.Instance.IsAudioEnable);
        FXAAToggle.isOn = (CamerasController.Instance.FxaaEnable);
        switch (Namings.LocTag)
        {
            case ELocTag.English:          
                EngToggle.isOn = true;
                RusToggle.isOn = false;
                EspToggle.isOn = false;
                break;
            case ELocTag.Russian:
                EngToggle.isOn = false;
                RusToggle.isOn = true;
                EspToggle.isOn = false;
                break;   
            case ELocTag.Spain:
                EngToggle.isOn = false;
                RusToggle.isOn = false;
                EspToggle.isOn = true;
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
        switch (_settingsLauch)
        {
            case EWindowSettingsLauch.battle:
                OnClickClose();
                BattleTutorial.Open();
                break;
            case EWindowSettingsLauch.map:
                OnClickClose();
                MapTutorial.Open();
                break;
            case EWindowSettingsLauch.menu:
                break;
        }
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
    public void OnClickEsp()
    {
        if (EspToggle.isOn)
        {
            ChangeConfirm(() =>
            {
                Namings.Esp();
                RefreshAllTags();
            },ELocTag.Spain);

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
