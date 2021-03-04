﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct SpellVisualInfo
{
    public SpellType SpellType;
    public SpellZoneVisualCircle SpellZoneCircle;
    public SpellZoneVisualLine SpellZoneLine;
    public SpellZoneVisualCircle RadiusAttackEffect;
}

public class SpellDataBase : MonoBehaviour
{
    public SpellButton SpellButton;

    public BulletKiller AntiPhysicalEffect;
    public BulletKiller AntiEnergyEffect;
//    public WallCatcher WallCatcher;

    public SpellVisualInfo[] SpellVisuals;
    // public SpellZoneVisualCircle SpellZoneCircle;
    // public SpellZoneVisualLine SpellZoneLine;
    // public SpellZoneVisualCircle RadiusAttackEffect;

//    public BaseEffectAbsorber ShieldDamageEffectAOE;//Расходящийся круг 
    // public BaseEffectAbsorber InvisibleEffect;

    public BaseEffectAbsorber RoundStrikeEffect;//TMP NOT USING
    public BaseEffectAbsorber RoundStrikeEffectShip;//TMP NOT USING

    public BaseEffectAbsorber EngineLockAOE;//Расходящийся круг 

    public BaseEffectAbsorber BlinkPlaceEffect;//Спираль синяя
    public BaseEffectAbsorber BlinkTargetEffect;//Спираль обратная
    public BaseEffectAbsorber ShieldRecharge;
    public BaseEffectAbsorber ShieldOffAOE;
    public BaseEffectAbsorber ShieldHitEffect;
    public BaseEffectAbsorber GoPlaceOk;
    public BaseEffectAbsorber GoPlaceFail;
//    public BaseEffectAbsorber BattlefieldEMIEffect;
    public BaseEffectAbsorber HookShot;

    public void Init()
    {
        var pool = DataBaseController.Instance.Pool;
//        pool.RegisterEffect(Utils.GetId(), ShieldDamageEffectAOE);
        //        pool.RegisterEffect(2, ShieldDamageEffectSingle);
        //        pool.RegisterEffect(2, InvinsableEffect);
        // pool.RegisterEffect(Utils.GetId(), InvisibleEffect);

        pool.RegisterEffect(Utils.GetId(), RoundStrikeEffect);
        pool.RegisterEffect(Utils.GetId(), RoundStrikeEffectShip);
        //        pool.RegisterEffect(6, AntiPhysicalEffect);

        //        pool.RegisterEffect(7, AntiEnergyEffect);
        pool.RegisterEffect(Utils.GetId(), EngineLockAOE);
        pool.RegisterEffect(Utils.GetId(), BlinkPlaceEffect);

        pool.RegisterEffect(Utils.GetId(), BlinkTargetEffect);
        pool.RegisterEffect(Utils.GetId(), ShieldOffAOE);
        pool.RegisterEffect(Utils.GetId(), ShieldRecharge);
        pool.RegisterEffect(Utils.GetId(), ShieldHitEffect);
        pool.RegisterEffect(Utils.GetId(), GoPlaceOk);
        pool.RegisterEffect(Utils.GetId(), GoPlaceFail);
        pool.RegisterEffect(Utils.GetId(), HookShot);
    }

    public SpellVisualInfo? GetVisualInfo(SpellType spellType)
    {
        return SpellVisuals.FirstOrDefault(x => x.SpellType == spellType);
    }
}

