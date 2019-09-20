using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum DamageType
{
    Health,
    Both,
    Shield
}

public delegate void DamageDoneDelegate(float healthDelta, float shieldDelta,ShipBase attacker);
public delegate float DamageModifDelegate(float damage);
public delegate CurWeaponDamage BulletDamageModif(CurWeaponDamage damage,Bullet bullet,ShipBase target);

public class ShipParameters  : IShipAffectableParams
{
    public const float MaxHealthCoef = 3.3f;
    public const float MaxShieldCoef = 2.4f;
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

    public delegate void ParameterChange(float curent,float max,float delta,ShipBase shipOwner);

    public float MaxHealth { get; set; }
    public float MaxSpeed { get; set; }
    public float TurnSpeed { get; set; }


    public float CurHealth
    {
        get { return _curHealth;}
        private set
        {
#if UNITY_EDITOR
            if (DebugParamsController.NoDamage)
            {
                return;
            }
#endif
            _curHealth = value;
        } }
    private float _curHealth;



    public event ParameterChange OnHealthChanged;
    public BaseSpellModulInv[] Spells = new BaseSpellModulInv[0];
//    private int ShipId;
    private Action _deathCallback;
    private ShipBase _shipOwner;
    public ShieldParameters ShieldParameters;

    public IStartShipParams StartParams;

    public ShipParameters(IStartShipParams startParams,BaseSpellModulInv[] spells ,
        Action dealthCallback,int id,ShipBase shipOwner,Collider shieldCollider,IPilotParameters pilotParams)
    {
//        _shieldCollider = ;
        _shipOwner = shipOwner;
//        ShipId = id;
        Spells = spells;
        StartParams = startParams;
        MaxSpeed = ParamUpdate(startParams.MaxSpeed, pilotParams.SpeedLevel,ShipParameters.MaxSpeedCoef);
        TurnSpeed = ParamUpdate(startParams.TurnSpeed, pilotParams.TurnSpeedLevel, ShipParameters.TurnSpeedCoef);
        MaxHealth = ParamUpdate(startParams.MaxHealth, pilotParams.HealthLevel, ShipParameters.MaxHealthCoef);
        var maxShiled = ParamUpdate(startParams.MaxShiled, pilotParams.ShieldLevel, ShipParameters.MaxShieldCoef);
        CurHealth = CurHealthWIthPercent;
//        CurShiled = MaxShiled;
//        ShieldRegenPerSec = startParams.ShiledRegen;
        _deathCallback = dealthCallback;
        ShieldParameters = new ShieldParameters(shipOwner,shieldCollider, startParams.ShiledRegen, maxShiled);
    }

    public float CurHealthWIthPercent
    {
        get { return MaxHealth * StartParams.HealthPercent; }
    }

    public static float ParamUpdate(float startValue,int paramLevel,float levelCoef)
    {
        return startValue  + (paramLevel - 1) * levelCoef;
    }
    
    public void Update()
    {
        ShieldParameters.Update();
    }

//    public bool IsOnLowHealth()
//    {
//        var percentHp = CurHealth/MaxHealth;
////        var pecrcentShield = CurShiled/MaxShiled;
//        return percentHp < 0.2f;
//    }


    private void HealthAction(float delta )
    {
        if (OnHealthChanged != null)
        {
            OnHealthChanged(CurHealth, MaxHealth, delta,_shipOwner);
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

        Debug.Log(String.Format("Damage done:{0}/{1}  to:{2}. Time:{3}", 0, bodyDamage, _shipOwner.Id, Time.time).Red()); var c = CurHealth;

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

    public void Damage(float shildDamage, float bodyDamage, DamageDoneDelegate damageDoneCallback,ShipBase attacker)
    {
        bodyDamage = ModifDamage(bodyDamage, BodyModifications);
        shildDamage = ModifDamage(shildDamage, ShieldModifications);

        Debug.Log(String.Format("Damage done:{0}/{1}  to:{2}. Time:{3}",shildDamage,bodyDamage,_shipOwner.Id,Time.time).Red() );
        float healthDelta = 0f;
        float shieldDelta = 0f;
        if (!ShieldParameters.ShiledIsActive)
        {
            var c = CurHealth;
            CurHealth -= bodyDamage;
            if (CurHealth < 1f)
            {
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
            damageDoneCallback(healthDelta, shieldDelta,attacker);
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
        Damage(copy.ShieldDamage, copy.BodyDamage, callback, target);
    }

    /*     DamageByType
    public void DamageByType(float val, DamageType damageType)
    {
        float delta;

        switch (damageType)
        {
            case DamageType.Health:
                CurHealth -= val;
                if (CurHealth < 1f)
                {
                    _deathCallback();
                }

                HealthAction(-val);
                break;
            case DamageType.Both:
                if (ShieldParameters.CurShiled > val)
                {
                    ShieldParameters.CurShiled -= val;
                    ShieldParameters.ShiledAction(-val);
                    return;
                }
                delta = val;
                var deltaShield = ShieldParameters.CurShiled;
                ShieldParameters.CurShiled = 0;
                ShieldParameters.ShiledAction(-deltaShield);
                
                CurHealth -= delta;
                if (CurHealth < 1f)
                {
                    _deathCallback();
                }

                HealthAction(-delta);
                break;
            case DamageType.Shield:

                ShieldParameters.CurShiled -= val;
                if (ShieldParameters.CurShiled <=  0f)
                {
                    ShieldParameters.CurShiled = 0f;
                }
                ShieldParameters.ShiledAction(-val);
                break;
        }

        
    }
      */
    public void Dispose()
    {
        ShieldParameters.Dispose();
        OnHealthChanged = null;
    }


    public void DowngradeMaxHealth(float f)
    {
        MaxHealth = MaxHealth*f;
    }


}

