using System;
using System.Collections.Generic;
using UnityEngine;


public class SpellModulsContainer : MonoBehaviour
{
    public Transform Layout;
    //    public Image RegenStatus;
    //    public TextMeshProUGUI FieldRegenStatus;
    //    public TextMeshProUGUI MainShipStatusField;
    private List<SpellButton> _buttons = new List<SpellButton>();
    // private ShipBase _mainShip;
    public Animator NotEnoughtBatteries;

    public void Init(InGameMainUI inGameMain, CommanderSpells commanderSpells,
        ShipBase mainShip, Action<SpellInGame> buttonCallback, CommanderCoinController coinController,AutoAICommander autoAICommander)
    {
        // _mainShip = mainShip;
        _buttons.Clear();
        SpellButton spPrefab = DataBaseController.Instance.SpellDataBase.SpellButton;
        int index = 1;
        foreach (var baseSpellModul in commanderSpells.AllSpells)
        {
            if (baseSpellModul != null)
            {
                var b = DataBaseController.GetItem(spPrefab);
                b.transform.SetParent(Layout, false);
                b.Init(inGameMain, baseSpellModul, spell =>
                 {
                    //                    SpellSelected(spell);
                    buttonCallback(spell);
                 }, coinController.CoefSpeed, index, autoAICommander.GetAutoSpell(baseSpellModul));
                index++;
                _buttons.Add(b);
            }
        }

        //        OnStateChange(_mainShip.ShipParameters.ShieldParameters.State);
        //        OnRegenEnableChange(_mainShip.Commander.CoinController.IsEnable);
        //        _mainShip.Commander.CoinController.OnRegenEnableChange += OnRegenEnableChange;
        //        _mainShip.ShipParameters.ShieldParameters.OnStateChange += OnStateChange;
    }

    public void Dispose()
    {
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

    public void CastFail()
    {
        NotEnoughtBatteries.SetTrigger("Play");

    }
}

