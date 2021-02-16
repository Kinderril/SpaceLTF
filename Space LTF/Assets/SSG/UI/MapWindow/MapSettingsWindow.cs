﻿using System;
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
    menu,
    exprolerGlobalMap,
    mapCampaing,
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

    public GameObject SaveLoadCampaing;
    public GameObject ReturnToExprolerGlobalMap;
    public GameObject MapButtons;
    public GameObject ExitButtonMenu;
    public GameObject BattleButtons;
    public SliderWithTextMeshPro MouseSensivity;
    public SliderWithTextMeshPro MusicLevel;
    public SliderWithTextMeshPro SoundLevel;
    private float _lastSens =1f;
    private float _lastSoundVal =1f;
    private float _lastMusicVal =1f;

    private EWindowSettingsLauch _settingsLauch;
    private bool _isDropDownInited = false;
    public SaveWindow SaveWindow;
    public ColorChangeSettingsWindow ChangeSettingsWindow;



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

    public void OnSensivityChange()
    {
        _lastSens = MouseSensivity.GetValue();
        CamerasController.Instance.SetSens(_lastSens);
    }    

    public void OnSoundLevel()
    {
        _lastSoundVal = SoundLevel.GetValue();
        CamerasController.Instance.SetSoundMixer(_lastSoundVal);
    }    
    public void OnMusicLevel()
    {
        _lastMusicVal = MusicLevel.GetValue();
        CamerasController.Instance.SetMusicMixer(_lastMusicVal);
    }  


    public void Init(EWindowSettingsLauch settingsLauch)
    {
        ChangeSettingsWindow.Init();

        MouseSensivity.InitBorders(CamerasController.MIN_CAM_MOVE_SENS,CamerasController.MAX_CAM_MOVE_SENS,false,false);
        MouseSensivity.InitCallback(OnSensivityChange);
        MouseSensivity.InitName(Namings.Tag("CamMoveSens")); 
        
        SoundLevel.InitBorders(0f,1f,false,false);
        SoundLevel.InitCallback(OnSoundLevel);
        SoundLevel.InitName(Namings.Tag("MusicLevelSens"));

        MusicLevel.InitBorders(0f,1f,false,false);
        MusicLevel.InitCallback(OnMusicLevel);
        MusicLevel.InitName(Namings.Tag("SoundLevelSens"));

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
                ReturnToExprolerGlobalMap.gameObject.SetActive(false);
                SaveLoadCampaing.gameObject.SetActive(false);
                break;
            case EWindowSettingsLauch.map:
                BattleTutorialButton.gameObject.SetActive(true);
                MapButtons.gameObject.SetActive(true);
                ExitButtonMenu.gameObject.SetActive(false);
                BattleButtons.gameObject.SetActive(false);
                ReturnToExprolerGlobalMap.gameObject.SetActive(false);
                SaveLoadCampaing.gameObject.SetActive(false);
                break;
            case EWindowSettingsLauch.menu:
                BattleTutorialButton.gameObject.SetActive(false);
                MapButtons.gameObject.SetActive(false);
                ExitButtonMenu.gameObject.SetActive(true);
                BattleButtons.gameObject.SetActive(false);
                ReturnToExprolerGlobalMap.gameObject.SetActive(false);
                SaveLoadCampaing.gameObject.SetActive(false);
                break;  
            case EWindowSettingsLauch.exprolerGlobalMap:
                BattleTutorialButton.gameObject.SetActive(false); 
                MapButtons.gameObject.SetActive(false);
                ExitButtonMenu.gameObject.SetActive(false);
                BattleButtons.gameObject.SetActive(false);
                ReturnToExprolerGlobalMap.gameObject.SetActive(true);
                SaveLoadCampaing.gameObject.SetActive(false);
                break;
            case EWindowSettingsLauch.mapCampaing:
                BattleTutorialButton.gameObject.SetActive(true);
                MapButtons.gameObject.SetActive(true);
                ExitButtonMenu.gameObject.SetActive(false);
                BattleButtons.gameObject.SetActive(false);
                ReturnToExprolerGlobalMap.gameObject.SetActive(false);
                SaveLoadCampaing.gameObject.SetActive(true);
                break;
        }

        var camController = CamerasController.Instance;

        _lastMusicVal = camController.MusicLevel;
        _lastSoundVal = camController.SoundLevel;
        _lastSens = camController.MouseSensivity;
        MouseSensivity.SetValue(_lastSens);
        MusicLevel.SetValue(_lastMusicVal);
        SoundLevel.SetValue(_lastSoundVal);
//        camController.MusicLevel = _lastMusicVal;

        Keys.gameObject.SetActive(false);
        NoMouseMoveSoundToggle.isOn = (camController.IsNoMouseMove);
        SoundToggle.isOn = (camController.IsAudioEnable);
        FXAAToggle.isOn = (camController.FxaaEnable);
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

    public void OnSaveClick()
    {
        SaveWindow.Init();
    }

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
            case EWindowSettingsLauch.exprolerGlobalMap:
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
