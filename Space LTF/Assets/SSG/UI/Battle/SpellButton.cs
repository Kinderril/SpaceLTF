using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SpellButton : UIElementWithTooltip, IPointerClickHandler
{
    private SpellInGame _spell;
    private Action<SpellInGame> OnSpellClick;
    public Image Icon;
    public Image Selected;
    public TextMeshProUGUI TimeField;
    // public TextMeshProUGUI CostField;
    public TextMeshProUGUI NameField;
    public Image ImageSlider;
    private InGameMainUI _inGameMain;
    private KeyCode _keyCode;
    // private AutoSpellContainer _autoSpell;
    // public GameObject AutoKey;
    // private SpellModulsContainer     _modulsContainer;
    // private bool _canAuto = true;
    // public GameObject AutoCastObject;
    private string _cachedTooltip;

    public void Init(InGameMainUI inGameMain, SpellInGame spell, 
        Action<SpellInGame> OnSpellClick, float speedCoef, int index)
    {
        // _modulsContainer = modulsContainer;
        // _modulsContainer.OnAutoModeSpell += OnAutoModeSpell;
        // AutoKey.SetActive(false);
        // AutoCastObject.SetActive(false);
        // _autoSpell = autoSpell;
        // _autoSpell.OnActive += OnAutoSpellActive;
        _inGameMain = inGameMain;
        _inGameMain.SpellModulsContainer.OnSelectSpell += OnSelectSpell;
        _spell = spell;
        this.OnSpellClick = OnSpellClick;
        Selected.gameObject.SetActive(false);
        var a = DataBaseController.Instance.DataStructPrefabs.GetSpellIcon(spell.SpellType);
        TimeField.text = $"{(_spell.CostPeriod * speedCoef).ToString("0")}";
        NameField.text = _spell.Name;
        Icon.sprite = a; switch (index)
        {
            case 1:
                _keyCode = KeyCode.Alpha1;
                break;
            case 2:
                _keyCode = KeyCode.Alpha2;
                break;
            case 3:
                _keyCode = KeyCode.Alpha3;
                break;
            case 4:
                _keyCode = KeyCode.Alpha4;
                break;
            case 5:
                _keyCode = KeyCode.Alpha5;
                break;             
        }

        CacheTooltipData();
        //        Debug.LogError($"Init spc brn :{_spell.Name}   TargetType:{_spell.AffectAction.TargetType}");
        //        _canAuto = _spell.AffectAction.TargetType == TargetType.Enemy;
    }

    private void CacheTooltipData()
    {
        var spellName = Namings.Format(_spell.Name);
        var info = $"{spellName}\n{_spell.Desc}";
        foreach (var sDesc in _spell.SupportDesc)
        {
            info = $"{info}\n{sDesc}";
        }
        _cachedTooltip = info;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnClick();
        }
        // else if (eventData.button == PointerEventData.InputButton.Right)
        // {
        //     OnRightClick();
        // }
    }

    // private void OnRightClick()
    // {
    //     if (_autoSpell.IsActive)
    //     {
    //         _autoSpell.SetActive(false,null);
    //     }
    //
    // }

    // private void OnAutoModeSpell(bool obj)
    // {
    //     AutoCastObject.SetActive(obj);
    // }

    // private void OnAutoSpellActive(AutoSpellContainer obj)
    // {
    //         AutoKey.SetActive(obj.IsActive);
    // }

    private void OnSelectSpell(SpellInGame obj)
    {
        var isMySpell = obj == _spell;
        Selected.gameObject.SetActive(isMySpell);
    }

    public void OnClick()
    {
        OnSpellClick(_spell);
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(_keyCode))
        {
            OnSpellClick(_spell);
        }

        if (!_spell.IsReady)
        {
            if (!ImageSlider.gameObject.activeSelf)
                ImageSlider.gameObject.SetActive(true);

            ImageSlider.fillAmount = _spell.DelayedAction.GetPercent();
        }
        else
        {
            if (ImageSlider.gameObject.activeSelf)
                ImageSlider.gameObject.SetActive(false);
        }
    }

    public void Dispose()
    {
        // _modulsContainer.OnAutoModeSpell -= OnAutoModeSpell;
        _inGameMain.SpellModulsContainer.OnSelectSpell -= OnSelectSpell;
    }

    protected override string TextToTooltip()
    {
        return _cachedTooltip;
    }

}

