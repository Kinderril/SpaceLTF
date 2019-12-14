﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


public class ShipDesicionDataSneakyAttack : ShipDesicionDataBase
{

    private bool _timeToHide = false;
    private float _timeToChange = 0f;

    public ShipDesicionDataSneakyAttack(ShipBase owner, PilotTactic tactic)
        : base(owner, tactic)
    {

    }

    protected override ActionType DoAttackAction(ShipBase ship)
    {
        if (_owner.WeaponsController.AnyWeaponIsLoaded() && ship != null && ship.VisibilityData.Visible)
        {
            return ActionType.attack;
        }

        return HideOrWait();
    }

    protected override ActionType OptionalTask(out ShipBase ship)
    {
        if (_timeToHide)
        {
            ship = null;
            return HideOrWait();
        }
        ship = CalcBestEnemy(out var rating, _owner.Enemies).ShipLink;
        return ActionType.attackSide;
    }

    public override string GetName()
    {
        return "Sneak attack";
    }

    protected override bool HaveEnemyInDangerZone(out ShipBase ship)
    {
        if (HaveEnemyClose(out ship, 7))
        {
            return true;
        }
        ship = null;
        return false;
    }
}

