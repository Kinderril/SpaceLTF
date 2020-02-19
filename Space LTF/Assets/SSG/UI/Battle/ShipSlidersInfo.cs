using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ShipSlidersInfo : MonoBehaviour
{
    private ShipBase _shipBase;
    public Slider HealthSlider;
    public Slider ShieldSlider;
    public TextMeshProUGUI ShieldText;
    public TextMeshProUGUI HealthText;
    private bool withText;
    public Image backShieldSlider;

    private bool _lastState;
    //    private Color _backShieldColorBase;

    public void Init(ShipBase shipBase)
    {
        withText = ShieldText != null && HealthText != null;
        _shipBase = shipBase;
        _shipBase.DamageData.OnDamageDone += OnDamageDone;
        _shipBase.ShipParameters.OnHealthChanged += OnHealthChanged;
        _shipBase.ShipParameters.ShieldParameters.OnShildChanged += OnShildChanged;
        //        _ship.ShipParameters.DebugInfo();
        OnHealthChanged(_shipBase.ShipParameters.CurHealth, _shipBase.ShipParameters.MaxHealth, 0, _shipBase);
        OnShildChanged(_shipBase.ShipParameters.CurShiled, _shipBase.ShipParameters.MaxShield, 0, _shipBase.ShipParameters.ShieldParameters.State, _shipBase);
        _lastState = shipBase.ShipParameters.ShieldParameters.ShiledIsActive;
        //        backShieldSlider = ShieldSlider.GetComponent<Image>();
        BackSet();
    }

    private void OnDamageDone(ShipBase arg1, ShipDamageType arg2, bool arg3)
    {
        if (arg2 == ShipDamageType.shiled)
        {
            if (_shipBase.ShipParameters.ShieldParameters.ShiledIsActive != _lastState)
            {
                _lastState = _shipBase.ShipParameters.ShieldParameters.ShiledIsActive;
                BackSet();
            }
        }
    }

    private void BackSet()
    {
        backShieldSlider.gameObject.SetActive(!_shipBase.ShipParameters.ShieldParameters.ShiledIsActive);
    }


    private void OnShildChanged(float v, float max, float delta, ShieldChangeSt state,ShipBase shipOwner)
    {
        if (withText)
        {
            ShieldText.text = Namings.Format("{0}/{1}", v.ToString("0"), max.ToString("0"));
        }
        ShieldSlider.value = Mathf.Clamp01((int)v / max);
        if (_shipBase.ShipParameters.ShieldParameters.ShiledIsActive != _lastState)
        {
            _lastState = _shipBase.ShipParameters.ShieldParameters.ShiledIsActive;
            BackSet();
        }
    }

    private void OnHealthChanged(float v, float max, float delta, ShipBase shipOwner)
    {
        if (withText)
        {
            HealthText.text = Namings.Format("{0}/{1}", v.ToString("0"), max.ToString("0"));
        }
        HealthSlider.value = Mathf.Clamp01((int) v/max);
    }

    public void Dispose()
    {
        _shipBase.DamageData.OnDamageDone -= OnDamageDone;
        _shipBase.ShipParameters.OnHealthChanged -= OnHealthChanged;
        _shipBase.ShipParameters.ShieldParameters.OnShildChanged -= OnShildChanged;
    }
}

