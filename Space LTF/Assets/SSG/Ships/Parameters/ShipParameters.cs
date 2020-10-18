using System;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Health,
    Both,
    Shield
}

public delegate void DamageDoneDelegate(float healthDelta, float shieldDelta, ShipBase attacker);
public delegate float DamageModifDelegate(float damage);
public delegate CurWeaponDamage BulletDamageModif(CurWeaponDamage damage, Bullet bullet, ShipBase target);

public class ShipParameters : IShipAffectableParams
{
    public const float MaxHealthCoef = 6.6f;
    public const float MaxShieldCoef = 4.8f;
    public const float MaxSpeedCoef = 0.13f;
    public const float TurnSpeedCoef = 2f;
    public List<DamageModifDelegate> BodyModifications = null;
    public List<DamageModifDelegate> ShieldModifications = null;
    public List<BulletDamageModif> BulletHitModificators = null;

    public float CurShiled
    {
        get { return ShieldParameters.CurShiled; }
    }
    public float MaxShield
    {
        get { return ShieldParameters.MaxShield; }
        set { ShieldParameters.MaxShield = value; }
    }
    public float ShieldRegenPerSec
    {
        get { return ShieldParameters.ShieldRegenPerSec; }
    }

    public delegate void ParameterChange(float curent, float max, float delta, ShipBase shipOwner);

    public float MaxHealth { get; set; }
    public float MaxSpeed { get; set; }
    public float TurnSpeed { get; set; }

    public int BodyArmor = 0;
    public int ShieldArmor = 0;

    public float CurHealth
    {
        get { return _curHealth; }
        private set
        {
#if UNITY_EDITOR
            if (DebugParamsController.NoDamage)
            {
                return;
            }
#endif
            if (_shipOwner.TeamIndex == TeamIndex.red)
            {
                MainController.Instance.Statistics.AddDamage((int)value);
            }
            _curHealth = value;
        }
    }
    private float _curHealth;



    public event ParameterChange OnHealthChanged;
    public BaseSpellModulInv[] Spells = new BaseSpellModulInv[0];

    public OnlyModulsInventory[] SubSpellModuls = new OnlyModulsInventory[0];
    //    private int ShipId;
    private Action _deathCallback;
    private ShipBase _shipOwner;
    public ShieldParameters ShieldParameters;
    public HealthRegenParameter HealthRegen { get; private set; }

    public IStartShipParams StartParams;

    public ShipParameters(IStartShipParams startParams, BaseSpellModulInv[] spells,OnlyModulsInventory[] subSpellModuls,
        Action dealthCallback, int id, ShipBase shipOwner, Collider shieldCollider, IPilotParameters pilotParams)
    {
        _shipOwner = shipOwner;
        SubSpellModuls = subSpellModuls;
        Spells = spells;
        StartParams = startParams;

        var calulatedParams = ShipParameters.CalcParams(startParams, pilotParams, new List<EParameterShip>()
        {
            EParameterShip.bodyPoints, EParameterShip.shieldPoints, EParameterShip.speed, EParameterShip.turn, EParameterShip.bodyArmor
        });
        MaxSpeed = calulatedParams[EParameterShip.speed];// ShipParameters.ParamUpdate(shipSpeedBase, _pilot.SpeedLevel, ShipParameters.MaxSpeedCoef);
        TurnSpeed = calulatedParams[EParameterShip.turn];//ShipParameters.ParamUpdate(turnSpeedBase, _pilot.TurnSpeedLevel, ShipParameters.TurnSpeedCoef);
        var maxShiled = calulatedParams[EParameterShip.shieldPoints];//ShipParameters.ParamUpdate(maxShiledBase, _pilot.ShieldLevel, ShipParameters.MaxShieldCoef);
        MaxHealth = calulatedParams[EParameterShip.bodyPoints];// ShipParameters.ParamUpdate(maxHealthBase, _pilot.HealthLevel, ShipParameters.MaxHealthCoef);


                
        CurHealth = CurHealthWIthPercent;
        _deathCallback = dealthCallback;
        HealthRegen = new HealthRegenParameter(this);
        ShieldParameters = new ShieldParameters(shipOwner, shieldCollider, startParams.ShieldRegen, maxShiled);
    }

    public float CurHealthWIthPercent
    {
        get { return MaxHealth * StartParams.HealthPercent; }
    }

    public static float ParamUpdate(float startValue, int paramLevel, float levelCoef)
    {
        levelCoef = Library.PARAMETER_LEVEL_COEF; 
        var val = startValue * (1 + (paramLevel - 1) * levelCoef);
        return Mathf.Clamp(val,1f,99999f);
    }

    public static Dictionary<EParameterShip, float> CalcParams(IStartShipParams ship, IPilotParameters pilot,List<EParameterShip> listToCalc)
    {
        Dictionary<EParameterShip, float> paramsOut = new Dictionary<EParameterShip, float>();
        foreach (var eParameterShip in listToCalc)
        {
            float coef = 1f;
            float baseParam = 1f;
            int levelPilot = 0;
            switch (eParameterShip)
            {
                case EParameterShip.speed:
                    coef = ShipParameters.MaxSpeedCoef;
                    baseParam = ship.MaxSpeed;
                    levelPilot = pilot.SpeedLevel;
                    break;
                case EParameterShip.turn:
                    coef = ShipParameters.TurnSpeedCoef;
                    baseParam = ship.TurnSpeed;
                    levelPilot = pilot.TurnSpeedLevel;
                    break;
                case EParameterShip.bodyPoints:
                    coef = ShipParameters.MaxHealthCoef;
                    baseParam = ship.MaxHealth;
                    levelPilot = pilot.HealthLevel;
                    break;
                case EParameterShip.shieldPoints:
                    coef = ShipParameters.MaxShieldCoef;
                    baseParam = ship.MaxShiled;
                    levelPilot = pilot.ShieldLevel;
                    break;
                case EParameterShip.bodyArmor:
                    baseParam = ship.BodyArmor;
                    break;
                default:

                    break;
            }

            var cofigCoef = 1f;
            switch (ship.ShipConfig)
            {
                case ShipConfig.raiders:
                    if (eParameterShip == EParameterShip.speed)
                    {
                        cofigCoef = Library.RAIDER_SPEED_COEF;
                    }

                    if (eParameterShip == EParameterShip.turn)
                    {

                        cofigCoef = Library.RAIDER_TURNSPEED_COEF;
                    }
                    break;
                case ShipConfig.federation:
                    break;
                case ShipConfig.mercenary:
                    if (eParameterShip == EParameterShip.speed)
                    {
                        cofigCoef = Library.MERC_SPEED_COEF;

                    }

                    if (eParameterShip == EParameterShip.turn)
                    {

                        cofigCoef = Library.MERC_TURNSPEED_COEF;
                    }
                    break;
                case ShipConfig.ocrons:
                    if (eParameterShip == EParameterShip.shieldPoints)
                    {
                        cofigCoef = Library.OCRONS_SHIELD_COEF;
                    }

                    if (eParameterShip == EParameterShip.bodyPoints)
                    {

                        cofigCoef = Library.OCRONS_HP_COEF;
                    }
                    break;
                case ShipConfig.krios:
                    if (eParameterShip == EParameterShip.shieldPoints)
                    {
                        cofigCoef = Library.KRIOS_SHIELD_COEF;
                    }

                    if (eParameterShip == EParameterShip.bodyPoints)
                    {

                        cofigCoef = Library.KRIOS_HP_COEF;
                    }
                    break;
            }

            baseParam = ApplySlot(ship.CocpitSlot, baseParam,eParameterShip);
            baseParam = ApplySlot(ship.EngineSlot, baseParam, eParameterShip);
            baseParam = ApplySlot(ship.WingSlot, baseParam, eParameterShip);

            float param;
            switch (eParameterShip)
            {
                case EParameterShip.speed:
                case EParameterShip.turn:
                case EParameterShip.bodyPoints:
                case EParameterShip.shieldPoints:
                    param = ParamUpdate(baseParam, levelPilot, coef); 
                    param *= cofigCoef;
                    break;
                default:
                case EParameterShip.bodyArmor:
                case EParameterShip.modulsSlots:
                case EParameterShip.weaponSlots:
                    param = baseParam;
                    break;
            }


            paramsOut.Add(eParameterShip,param);
        }

        float ApplySlot(ParameterItem item, float curVal, EParameterShip paramType)
        {
            if (item != null)
            {
                foreach (var affection in item.ParametersAffection)
                {
                    if (affection.Key == paramType)
                    {
                        curVal += affection.Value;
                    }
                }
            }

            return curVal;
        }

        return paramsOut;
    }

    public void Update()
    {
        ShieldParameters.Update();
        HealthRegen.Update();
    }

    private void HealthAction(float delta)
    {
        if (OnHealthChanged != null)
        {
            OnHealthChanged(CurHealth, MaxHealth, delta, _shipOwner);
        }
    }

    public void HealHp(float v)
    {
        var c = CurHealth + v;
        var d = MaxHealth - CurHealth;
        if (d <= 0)
        {
            return;
        }
        if (c > MaxHealth)
        {
            c = MaxHealth;
        }
        var delta = c - CurHealth;
        CurHealth = c;
        HealthAction(delta);
    }



    public void DamageIgnoreShield(float bodyDamage, DamageDoneDelegate damageDoneCallback)
    {
        bodyDamage = ModifDamage(bodyDamage, BodyModifications);

        Debug.Log(Namings.Format("Damage done:{0}/{1}  to:{2}. Time:{3}", 0, bodyDamage, _shipOwner.Id, Time.time).Red()); var c = CurHealth;

        float healthDelta = 0f;
        CurHealth -= bodyDamage;
        if (CurHealth < 1f)
        {
            _deathCallback();
        }
        healthDelta = Mathf.Abs(CurHealth - c);
        HealthAction(-healthDelta);
        if (damageDoneCallback != null)
        {
            damageDoneCallback(healthDelta, 0, null);
        }
    }

    public void Damage(float shildDamage, float bodyDamage, DamageDoneDelegate damageDoneCallback, ShipBase attacker)
    {
        bodyDamage = ModifDamage(bodyDamage, BodyModifications);
        shildDamage = ModifDamage(shildDamage, ShieldModifications);

#if UNITY_EDITOR
        Debug.Log(Namings.Format("Damage done:{0}/{1}  to:{2}. Time:{3}", shildDamage, bodyDamage, _shipOwner.Id, Time.time).Red());

#endif
        float healthDelta = 0f;
        float shieldDelta = 0f;
        if (!ShieldParameters.ShiledIsActive)
        {
            var c = CurHealth;
            CurHealth -= bodyDamage;
            if (CurHealth < 1f)
            {
                if (_shipOwner.TeamIndex == TeamIndex.red)
                {
                    MainController.Instance.Statistics.AddShipsDestroyed();
                }
                _deathCallback();
            }
            healthDelta = Mathf.Abs(CurHealth - c);
            HealthAction(-healthDelta);
        }

        var c1 = ShieldParameters.CurShiled;
        ShieldParameters.CurShiled -= shildDamage;
        if (ShieldParameters.CurShiled <= 0f)
        {
            ShieldParameters.CurShiled = 0f;
        }
        shieldDelta = Mathf.Abs(ShieldParameters.CurShiled - c1);
        ShieldParameters.ShiledAction(-shieldDelta);
        if (damageDoneCallback != null)
        {
            damageDoneCallback(healthDelta, shieldDelta, attacker);
        }
    }

    private float ModifDamage(float dmg, List<DamageModifDelegate> modificators)
    {
        if (modificators != null)
        {
            foreach (var func in modificators)
            {
                dmg = func(dmg);
            }
        }

        return dmg;
    }
    public void DamageByWeaponBullet(Bullet bullet, CurWeaponDamage currentDamage, DamageDoneDelegate callback, ShipBase target)
    {
        var copy = currentDamage.Copy();
        if (BulletHitModificators != null)
        {
            foreach (var hitModificator in BulletHitModificators)
            {
                copy = hitModificator(copy, bullet, target);
            }
        }

        var ShieldDamage = currentDamage.ShieldDamage - ShieldArmor;
        var BodyDamage = currentDamage.BodyDamage - BodyArmor;
        Damage(ShieldDamage, BodyDamage, callback, target);
    }

    public void Dispose()
    {
        ShieldParameters.Dispose();
        OnHealthChanged = null;
    }
    public void DowngradeMaxHealth(float f)
    {
        MaxHealth = MaxHealth * f;
    }


}

