using System;
using UnityEngine;
using System.Collections;
using TMPro;

public enum EStartPair
{
    MidLight,
    MidHvy,
    LightHvy
}

public class ExprolerEmptyProfile : MonoBehaviour
{
    private ShipConfig _shipConfig = ShipConfig.mercenary;
    private EStartPair _startPair = EStartPair.MidHvy;
    public GameObject OpneNewInfo;
//    public GameObject ButtonContainer;

    public TextMeshProUGUI NameField;
//    private Action<ShipConfig, EStartPair> _callback;

    public void Init()
    {
        NameField.text = "";
        OpneNewInfo.gameObject.SetActive(false);
//        ButtonContainer.gameObject.SetActive(true);
        //        _callback = callback;
    }

    public void OnClickNew()
    {
        OpneNewInfo.gameObject.SetActive(true);
//        ButtonContainer.gameObject.SetActive(false);
    }

    public void OnCreateProfile()
    {
        if (MainController.Instance.SafeContainers.TryAddCreateNewProfile(_shipConfig, _startPair, NameField.text))
        {
            NameField.text = "";
            OpneNewInfo.gameObject.SetActive(false);
//            ButtonContainer.gameObject.SetActive(true);
        }
    }

    public void OnClickMidHvy()
    {
        _startPair = EStartPair.MidHvy;
    }            
    public void OnClickLightHvy()
    {
        _startPair = EStartPair.LightHvy;
    }            
    public void OnClickMidLight()
    {
        _startPair = EStartPair.MidLight;
    }

    public void OnClickMerc()
    {
        SetConfig(ShipConfig.mercenary);
    }                                 
    public void OnClickFed()
    {
        SetConfig(ShipConfig.federation);
    }                                 
    public void OnClickRaider()
    {
        SetConfig(ShipConfig.raiders);
    }                                 
    public void OnClickOcr()
    {
        SetConfig(ShipConfig.ocrons);
    }                                 
    public void OnClickKrios()
    {
        SetConfig(ShipConfig.krios);
    }

    public void SetConfig(ShipConfig config)
    {
        _shipConfig = config;
    }
}
