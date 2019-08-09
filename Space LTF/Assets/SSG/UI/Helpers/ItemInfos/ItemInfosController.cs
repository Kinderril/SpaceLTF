using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class ItemInfosController :MonoBehaviour
{
    public WeaponBigInfoUI Weapon;
    public SpellBigInfoUI Spell;
    public ModulBigInfoUI Modul;
    

    public void Init(WeaponInv weapon,bool canChange,bool withSupport)
    {
        WindowManager.Instance.WindowMainCanvas.interactable = false;
        WindowManager.Instance.WindowSubCanvas.interactable = true;
        transform.SetAsLastSibling();
        Weapon.Init(weapon,OnSubWindowClose, canChange, withSupport);
    }
    public void Init(BaseModulInv modul, bool canChange)
    {
        WindowManager.Instance.WindowMainCanvas.interactable = false;
        WindowManager.Instance.WindowSubCanvas.interactable = true;
        transform.SetAsLastSibling();
        Modul.Init(modul, OnSubWindowClose);
    }
    public void Init(BaseSpellModulInv spell, bool canChange)
    {
        WindowManager.Instance.WindowMainCanvas.interactable = false;
        WindowManager.Instance.WindowSubCanvas.interactable = true;
        transform.SetAsLastSibling();
        Spell.Init(spell, OnSubWindowClose, canChange);
    }

    private void OnSubWindowClose()
    {
        WindowManager.Instance.WindowMainCanvas.interactable = true;
        WindowManager.Instance.WindowSubCanvas.interactable = false;
    }

    public void InitSelf()
    {
        Weapon.gameObject.SetActive(false);
        Spell.gameObject.SetActive(false);
        Modul.gameObject.SetActive(false);
    }
}

