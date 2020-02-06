﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class StartWeaponToggleUI : UIElementWithTooltip
{
    public TextMeshProUGUI Field;
    public WeaponsPair WeaponsPair { get; private set; }
    private Action<StartWeaponToggleUI> oncallback;
    public Toggle Toggle;

    public void Init(WeaponsPair weaponsPair, Action<StartWeaponToggleUI> oncallback, bool interactable)
    {
        Field.text = $"{Namings.Weapon(weaponsPair.Part1)} {Namings.Tag("And")} {Namings.Weapon(weaponsPair.Part2)}";
        this.oncallback = oncallback;
        WeaponsPair = weaponsPair;
        Toggle.interactable = interactable;
    }

    public void OnClick()
    {
        oncallback(this);
    }
    protected override string TextToTooltip()
    {
        return Namings.TooltipWeapons(WeaponsPair);
    }
}

