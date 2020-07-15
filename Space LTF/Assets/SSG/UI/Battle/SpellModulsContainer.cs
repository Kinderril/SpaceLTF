using System;
using System.Collections.Generic;
using UnityEngine;


public class SpellModulsContainer : MonoBehaviour
{
    public Transform Layout;
    private List<SpellButton> _buttons = new List<SpellButton>();
    public Animator NotEnoughtBatteries;
    private SpellInGame _spellSelected;
    public event Action<SpellInGame> OnSelectSpell;
    public event Action<bool> OnAutoModeSpell;
    public SpellInGame SpellSelected => _spellSelected;
    private InGameMainUI _inGameMain;
    private Commander MyCommander;
    private bool _isAutoMode = false;
    public GameObject AutoActive;

    public void Init(InGameMainUI inGameMain, CommanderSpells commanderSpells,
        ShipBase mainShip, //Action<SpellInGame> buttonCallback, 
        CommanderCoinController coinController,AutoAICommander autoAICommander, Commander myCommander)
    {
        _inGameMain = inGameMain;
        AutoActive.gameObject.SetActive(false);
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
                 }, coinController.CoefSpeed, index, autoAICommander.GetAutoSpell(baseSpellModul),this);
                index++;
                _buttons.Add(b);
            }
        }
    }

    public void OnClickAuto()
    {
        AutoChange(!_isAutoMode);
    }

    private void AutoChange(bool to)
    {
        if (to == _isAutoMode)
            return;

        _isAutoMode = to;
        AutoActive.gameObject.SetActive(_isAutoMode);
        OnAutoModeSpell?.Invoke(_isAutoMode);
    }

    void Update()
    {
        var ray = _inGameMain.GetPointByClick(Input.mousePosition);
        if (ray.HasValue)
        {
            if (_spellSelected != null)
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
        if (_isAutoMode || spellToCast.CanCast())
        {
            if (_spellSelected != null)
            {
                _spellSelected.EndShowCast();
            }

            _spellSelected = spellToCast;
            if (OnSelectSpell != null)
            {
                OnSelectSpell(_spellSelected);
            }
            _spellSelected.StartShowCast();
            Debug.Log("spell select " + _spellSelected.Name);
        }
        else
        {
            CastFail();
            Debug.Log("Can't cast spell " + spellToCast.Name + "   " + spellToCast.CostCount);
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
        OnAutoModeSpell = null;
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

    public void TryCast(bool left,Vector3 pos)
    {
        var battle = BattleController.Instance;
 
        if (left)
        {
            if (!battle.PauseData.IsPause)
            {
                var ray = _inGameMain.GetPointByClick(pos);
                if (ray.HasValue)
                {
                    if (_isAutoMode)
                    {
                        TeamIndex inedx;
                        if (_spellSelected.AffectAction.TargetType == TargetType.Enemy)
                        {
                            inedx = TeamIndex.red;
                        }
                        else
                        {
                            inedx = TeamIndex.green;
                        }

                        var getClosest = BattleController.Instance.ClosestShipToPos(ray.Value, inedx, out float sDIst);
                        if (getClosest != null && sDIst < 4)
                        {
                            battle.GreenAutoAICommander.ActivateAiSpell(_spellSelected, getClosest);
                        }
                        else
                        {
                            Debug.LogError($"No closest ship. dist: {Mathf.Sqrt(sDIst)}");
                        }

                        AutoChange(false);
                    }
                    else
                    {
                        if (MyCommander.SpellController.TryCastspell(_spellSelected, ray.Value))
                        {
                            Debug.Log("spell TryCast " + _spellSelected.Name);
                            EndCastSpell();
                            return;
                        }

                        EndCastSpell();
                    }
                }

                EndCastSpell();
            }
        }
        else
        {
            EndCastSpell();
        }
        

    }

    public void ManualAutoOff()
    {
        AutoChange(false);
    }
}

