using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SpellModulsContainer : MonoBehaviour
{
    public Transform Layout;
    private List<SpellButton> _buttons = new List<SpellButton>();
    public Animator NotEnoughtBatteries;
    private SpellInGame _spellSelected;
    private SpellInGame _activeSpell;
    public event Action<SpellInGame> OnSelectSpell;
    public event Action<SpellInGame> OnActivateSpell;
    public event Action<SpellInGame> OnDeactivateSpell;
    // public event Action<bool> OnAutoModeSpell;
    public SpellInGame ActiveSpell => _activeSpell;
    public SpellInGame SpellSelected => _spellSelected;
    private InGameMainUI _inGameMain;
    private Commander MyCommander;
    public TextMeshProUGUI PowerCoefField;

    private CoinTempController _coinController;

    public Animator Animator;
    // private bool _isAutoMode = false;
    // public GameObject AutoActive;

    public void Init(InGameMainUI inGameMain, CommanderSpells commanderSpells,
        ShipBase mainShip, //Action<SpellInGame> buttonCallback, 
        CoinTempController coinController,AutoAICommander autoAICommander, Commander myCommander)
    {
        _coinController = coinController;
        _inGameMain = inGameMain;
        // AutoActive.gameObject.SetActive(false);
        MyCommander = myCommander;
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
                     OnSpellClicked(spell);
                 }, coinController.CoefSpeed, index);
                index++;
                _buttons.Add(b);
            }
        }
    }

    // public void OnClickAuto()
    // {
    //     AutoChange(!_isAutoMode);
    // }

    // private void AutoChange(bool to)
    // {
    //     if (to == _isAutoMode)
    //         return;
    //
    //     _isAutoMode = to;
    //     AutoActive.gameObject.SetActive(_isAutoMode);
    //     OnAutoModeSpell?.Invoke(_isAutoMode);
    // }

    void Update()
    {
        if (_spellSelected != null)
        {
            var ray = _inGameMain.GetPointByClick(Input.mousePosition);
            if (_activeSpell == null && ray.HasValue)
            {
                _spellSelected.UpdateShowCast(ray.Value);
            }
        }
    }

    private void EndCastSpell()
    {
        if (_spellSelected != null)
        {
            _spellSelected.EndShowCast();
        }
        UnselectSpell();
    }

    private void OnSpellClicked(SpellInGame spellToCast)
    {
        if (spellToCast.CanCast())
        {
            if (_spellSelected != null)
            {
                _spellSelected.EndShowCast();
            }

            _spellSelected = spellToCast;
            OnSelectSpell?.Invoke(_spellSelected);
            _spellSelected.StartShowCast();
            Debug.Log("spell select " + _spellSelected.Name);
        }
        else
        {
            CastFail();
            Debug.Log("Can't cast spell " + spellToCast.Name);
        }
    }
    public void UnselectSpell()
    {
        _spellSelected = null;
        if (OnSelectSpell != null)
        {
            OnSelectSpell(_spellSelected);
        }
    }

    public void Dispose()
    {
        // OnAutoModeSpell = null;
        if (_spellSelected != null)
        {
            _spellSelected.EndShowCast();
        }
        UnselectSpell();
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

    public bool TryCast(bool left,Vector3 pos)
    {
        var battle = BattleController.Instance;
 
        if (left)
        {
            if (!battle.PauseData.IsPause)
            {
                var ray = _inGameMain.GetPointByClick(pos);
                if (ray.HasValue)
                {
                    if (MyCommander.SpellController.TryCastspell(_spellSelected, ray.Value))
                    {
                        // Debug.LogError("spell TryCast " + _spellSelected.Name);
                        _activeSpell = _spellSelected;
                        _coinController.StartUseSpell(_activeSpell);
                        OnActivateSpell?.Invoke(_activeSpell);
                        EndCastSpell();
                        InitPowerField();
                        return true;
                    }

                    EndCastSpell();
                }

                EndCastSpell();
            }
        }
        else
        {
            EndCastSpell();
        }

        return false;
    }

    private void InitPowerField()
    {
        Animator.SetTrigger("ScaleUp");
        PowerCoefField.gameObject.SetActive(true);
        SetText(1);
        PowerCoefField.color = new Color(.5f,.5f,0f);// Color.Lerp(Color.red, Color.green, .5f); ;
    }

    public void SetText(float val)
    {
        var coef = (int)(val *100f);
        // Debug.LogError($"val:{val}  coef:{coef}");
        PowerCoefField.text =$"{Namings.Tag("Power")}:{coef}%" ;
    }

    public void EndCastActiveSpell()
    {
        // Debug.LogError($"EndCastActiveSpell....:");
        _coinController.EndCastSpell();
        _activeSpell.EndCastPeriod();
        PowerCoefField.gameObject.SetActive(false);
        Animator.SetTrigger("ScaleDown");
        _activeSpell = null;
    }

    public bool UpdateActivePeriod(Vector3 ray)
    {
        UpdatePower();
        if (_coinController.TrySpellUsage())
        {
            ActiveSpell.UpdateActivePeriod(ray);
            return true;
        }
        else
        {
            CastFail();
            return false;
        }
    }

    private void UpdatePower()
    {
        var p = ActiveSpell.PowerInc();
        var coef = Mathf.Clamp01((2f - p) * .5f);
        var color = Color.Lerp(Color.red, Color.green, coef);

        SetText(p);
        PowerCoefField.color = color;

    }
}

