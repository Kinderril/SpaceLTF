using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SpellButton : UIElementWithTooltip
{
    private SpellInGame _spell;
    private Action<SpellInGame> OnSpellClick;
    public Image Icon;
    public Image Selected;
    public TextMeshProUGUI TimeField;
    public TextMeshProUGUI CostField;
    public TextMeshProUGUI NameField;
    public Image ImageSlider;
    private InGameMainUI _inGameMain;
    private KeyCode _keyCode;

    public void Init(InGameMainUI inGameMain, SpellInGame spell, Action<SpellInGame> OnSpellClick, float speedCoef, int index)
    {
        _inGameMain = inGameMain;
        _inGameMain.OnSelectSpell += OnSelectSpell;
        _spell = spell;
        this.OnSpellClick = OnSpellClick;
        Selected.gameObject.SetActive(false);
        var a = DataBaseController.Instance.DataStructPrefabs.GetSpellIcon(spell.SpellType);
        CostField.text = Namings.Format("{0}", _spell.CostCount.ToString("0"));
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
    }

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
        _inGameMain.OnSelectSpell -= OnSelectSpell;
    }

    protected override string TextToTooltip()
    {
        var spellName = Namings.Format(_spell.Name);
        return $"{spellName}\n{_spell.Desc}";
    }
}

