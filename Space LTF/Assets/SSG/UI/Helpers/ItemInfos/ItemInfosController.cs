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
    

    public void Init(WeaponInv weapon)
    {
        WindowManager.Instance.CurrentWindow.CanvasGroup.interactable = false;
        transform.SetAsLastSibling();
        Weapon.Init(weapon,OnSubWindowClose);
    }
    public void Init(BaseModulInv modul)
    {
        WindowManager.Instance.CurrentWindow.CanvasGroup.interactable = false;
        transform.SetAsLastSibling();
        Modul.Init(modul, OnSubWindowClose);
    }
    public void Init(BaseSpellModulInv spell)
    {
        WindowManager.Instance.CurrentWindow.CanvasGroup.interactable = false;
        transform.SetAsLastSibling();
        Spell.Init(spell, OnSubWindowClose);
    }

    private void OnSubWindowClose()
    {
        WindowManager.Instance.CurrentWindow.CanvasGroup.interactable = true;
    }

    public void InitSelf()
    {
        Weapon.gameObject.SetActive(false);
        Spell.gameObject.SetActive(false);
        Modul.gameObject.SetActive(false);
    }
}

