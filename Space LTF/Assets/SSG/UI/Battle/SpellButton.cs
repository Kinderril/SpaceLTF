using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SpellButton : MonoBehaviour
{
    private SpellInGame _spell;
    private Action<SpellInGame> OnSpellClick;
    public Image Icon;
    public Image Selected;
    public TextMeshProUGUI TimeField;
    public TextMeshProUGUI CostField;
    public TextMeshProUGUI NameField;
    private InGameMainUI _inGameMain;

    public void Init(InGameMainUI inGameMain, SpellInGame spell,Action<SpellInGame> OnSpellClick,float speedCoef)
    {
        _inGameMain = inGameMain;
        _inGameMain.OnSelectSpell += OnSelectSpell;
        _spell = spell;
        this.OnSpellClick = OnSpellClick;
        Selected.gameObject.SetActive(false);
        var a = DataBaseController.Instance.DataStructPrefabs.GetSpellIcon(spell.SpellType);
        CostField.text = String.Format("{0}", _spell.CostCount.ToString("0"));
        TimeField.text = $"{(_spell.CostPeriod * speedCoef).ToString("0")}";
        NameField.text = _spell.Name;
        Icon.sprite = a;
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

    public void Dispose()
    {
        _inGameMain.OnSelectSpell -= OnSelectSpell;
    }
}

