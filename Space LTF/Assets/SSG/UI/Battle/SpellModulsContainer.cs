using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SpellModulsContainer : MonoBehaviour
{
    public Transform Layout;
//    public Image RegenStatus;
    public TextMeshProUGUI FieldRegenStatus;
    public TextMeshProUGUI MainShipStatusField;
    private List<SpellButton> _buttons = new List<SpellButton>();
    private ShipBase _mainShip;

    public void Init(InGameMainUI inGameMain,CommanderSpells commanderSpells, ShipBase mainShip,Action<SpellInGame> buttonCallback)
    {
        _mainShip = mainShip;
        _buttons.Clear();
        SpellButton spPrefab = DataBaseController.Instance.SpellDataBase.SpellButton;
        foreach (var baseSpellModul in commanderSpells.AllSpells)
        {
            if (baseSpellModul != null)
            {
                var b = DataBaseController.GetItem(spPrefab);
                b.transform.SetParent(Layout, false);
                b.Init(inGameMain,baseSpellModul, buttonCallback);
                _buttons.Add(b);
            }
        }

        OnStateChange(_mainShip.ShipParameters.ShieldParameters.State);
        OnRegenEnableChange(_mainShip.Commander.CoinController.IsEnable);
        _mainShip.Commander.CoinController.OnRegenEnableChange += OnRegenEnableChange;
        _mainShip.ShipParameters.ShieldParameters.OnStateChange += OnStateChange;
    }

    private void OnRegenEnableChange(bool obj)
    {
        DrawRegenEnable(obj);
    }

    private void OnStateChange(ShieldChangeSt obj)
    {
        string info = "";
        switch (obj)
        {
            case ShieldChangeSt.active:
                info = "Shield is active";
                break;
            case ShieldChangeSt.restoring:
                info = "Shield restoring";
                break;
            case ShieldChangeSt.disable:
                info = "Shield disabled";
                break;
        }

        MainShipStatusField.text = info;
    }

    private void DrawRegenEnable(bool isRegenEnable)
    {
        FieldRegenStatus.text = isRegenEnable ? "Battery restoring" : "Restoring disabled";
//        RegenStatus.color = isRegenEnable ? Color.green : Color.red;
    }


    public void Dispose()
    {
        _mainShip.Commander.CoinController.OnRegenEnableChange -= OnRegenEnableChange;
        _mainShip.ShipParameters.ShieldParameters.OnStateChange -= OnStateChange;
        foreach (var spellButton in _buttons)
        {
            spellButton.Dispose();
            GameObject.Destroy(spellButton.gameObject);
        }
        _buttons.Clear();


    }

    public void TrySelectSpell(int index)
    {
        if (_buttons.Count < index)
        {
            var btn = _buttons[index];
            btn.OnClick();
        }
    }
}

