using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SpellDataBase : MonoBehaviour
{
    public SpellButton SpellButton;

    public BulletKiller AntiPhysicalEffect;
    public BulletKiller AntiEnergyEffect;
    public WallCatcher WallCatcher;

    public SpellZoneVisualCircle SpellZoneCircle;
    public SpellZoneVisualLine SpellZoneLine;
    public SpellZoneVisualCircle RadiusAttackEffect;

    public BaseEffectAbsorber ShieldDamageEffectAOE;//Расходящийся круг 
    public BaseEffectAbsorber InvisibleEffect;

    public BaseEffectAbsorber RoundStrikeEffect;//TMP NOT USING
    public BaseEffectAbsorber RoundStrikeEffectShip;//TMP NOT USING
    
    public BaseEffectAbsorber EngineLockAOE;//Расходящийся круг 

    public BaseEffectAbsorber BlinkPlaceEffect;//Спираль синяя
    public BaseEffectAbsorber BlinkTargetEffect;//Спираль обратная
    public BaseEffectAbsorber ShieldOffAOE;
    public BaseEffectAbsorber ShieldHitEffect;

    public void Init()
    {
        var pool = DataBaseController.Instance.Pool;
        pool.RegisterEffect(Utils.GetId(), ShieldDamageEffectAOE);
//        pool.RegisterEffect(2, ShieldDamageEffectSingle);
//        pool.RegisterEffect(2, InvinsableEffect);
        pool.RegisterEffect(Utils.GetId(), InvisibleEffect);

        pool.RegisterEffect(Utils.GetId(), RoundStrikeEffect);
        pool.RegisterEffect(Utils.GetId(), RoundStrikeEffectShip);
//        pool.RegisterEffect(6, AntiPhysicalEffect);

//        pool.RegisterEffect(7, AntiEnergyEffect);
        pool.RegisterEffect(Utils.GetId(), EngineLockAOE);
        pool.RegisterEffect(Utils.GetId(), BlinkPlaceEffect);

        pool.RegisterEffect(Utils.GetId(), BlinkTargetEffect);
        pool.RegisterEffect(Utils.GetId(), ShieldOffAOE);
        pool.RegisterEffect(Utils.GetId(), ShieldHitEffect);
    }
    
}

