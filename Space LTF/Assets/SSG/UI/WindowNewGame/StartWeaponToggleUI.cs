using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class StartWeaponToggleUI : MonoBehaviour
{
    public TextMeshProUGUI Field;
    public WeaponsPair WeaponsPair { get; private set; }
    private Action<StartWeaponToggleUI> oncallback;
    public Toggle Toggle;

    public void Init(WeaponsPair weaponsPair, Action<StartWeaponToggleUI> oncallback,bool interactable)
    {
        Field.text = $"{Namings.Weapon(weaponsPair.Part1)} {Namings.And} {Namings.Weapon(weaponsPair.Part2)}";
        this.oncallback = oncallback;
        WeaponsPair = weaponsPair;
        Toggle.interactable = interactable;
    }

    public void OnClick()
    {
        oncallback(this);
    }
}

