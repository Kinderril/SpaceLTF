using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

[System.Serializable]
public class RocketInventory : WeaponInv
{
//    private bool GoodTurn;
//    private bool Longer;
//    private bool AOE;
//    private bool EngineDestroy;
//    private bool SlowEnemy;
//    private bool NextChargeBuff;

//
//    private const float EngineChance = .5f;
////    private const float ShieldChance = .5f;
//    private const float AimRadiusCoef = .5f;
//    private const float SpeedCoef = .5f;
//    private const float SlowDelay = 4.5f;
    //    public RocketInventory([NotNull] IInventory currentInventory) : base(currentInventory)
    //    {
    //
    //    }

    public RocketInventory([NotNull] WeaponInventoryParameters parameters,  int Level)
        : base(parameters, WeaponType.rocket, Level)
    {


    }

//    public override float AimRadius
//    {
//        get
//        {
//            if (Longer)
//            {
//                return base.AimRadius*(1 + AimRadiusCoef);
//            }
//            return base.AimRadius;
//        }
//    }
//    protected override float BulletSpeed
//    {
//        get
//        {
//            if (GoodTurn)
//            {
//                return base.TurnSpeed * (1+ SpeedCoef);
//            }
//            return base.TurnSpeed;
//        }
//    }

//    protected override bool A1_level1 => GoodTurn;
//
//    protected override bool B1_level1 => Longer;
//
//    protected override bool A1_level2 => AOE;
//
//    protected override bool B1_level2 => EngineDestroy;
//    protected override Dictionary<int, Dictionary<WeaponUpdageType, WeaponUpgradeData>> CreateLevelUpDependences()
//    {
//        var de = new Dictionary<int, Dictionary<WeaponUpdageType, WeaponUpgradeData>>();
//        de.Add(1, new Dictionary<WeaponUpdageType, WeaponUpgradeData>()
//        {
//            {WeaponUpdageType.a1, new WeaponUpgradeData( ()=> { GoodTurn = true; },"Lesser turn radius","Manevour") },
//            {WeaponUpdageType.b1, new WeaponUpgradeData( ()=> { Longer = true; },"Longer aim radius","Radius") },
//        });
//        de.Add(2, new Dictionary<WeaponUpdageType, WeaponUpgradeData>()
//        {
//            {WeaponUpdageType.a1, new WeaponUpgradeData( ()=> { AOE = true; },"Rocket have AOE when destroy","AOE") },
//            {WeaponUpdageType.b1, new WeaponUpgradeData( ()=> { EngineDestroy = true; },Utils.FloatToChance(EngineChance) + "% chance to destroy enfine","Demolition") },
//        });
//        de.Add(3, new Dictionary<WeaponUpdageType, WeaponUpgradeData>()
//        {
//            {WeaponUpdageType.a1, new WeaponUpgradeData( ()=> { SlowEnemy = true; },$"Slow enemy for {SlowDelay} sec"  ,"Slower") },
//            {WeaponUpdageType.b1, new WeaponUpgradeData( ()=> { NextChargeBuff = true; } ,String.Format("Every next hit have +1/+1 damage"),"Charger") },
//        });
//        return de;
//    }

//    protected override string CurrentUpgradesDesc()
//    {
//        string ss = "";
//        if (GoodTurn)
//        {
//            ss += _levelUpDependences[1][WeaponUpdageType.a1].Desc;
//        }
//
//        if (Longer)
//        {
//            ss += _levelUpDependences[1][WeaponUpdageType.b1].Desc;
//        }
//
//        ss += "\n";
//
//        if (AOE)
//        {
//            ss += _levelUpDependences[2][WeaponUpdageType.b1].Desc;
//        }
//        if (EngineDestroy)
//        {
//            ss += _levelUpDependences[2][WeaponUpdageType.b1].Desc;
//        }
//        return ss;
//    }
//    public override CurWeaponDamage CurrentDamage()
//    {
//        return new CurWeaponDamage(ShieldDamage, BodyDamage);
//    }


    public override WeaponInGame CreateForBattle()
    {
        return new RocketWeapon(this);
    }
}

