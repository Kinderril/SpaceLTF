using System.Collections.Generic;
using UnityEngine;

public class AudioDataBase : MonoBehaviour
{
    public AudioClip LaserShot;
    public AudioClip RocketShot;
    public AudioClip EMIShot;
    public AudioClip ImpulseShot;
    public AudioClip CassetShot;
    public AudioClip BeamShot;
    public AudioClip DefaultShot;

    public AudioClip ShipHit1;
    public AudioClip ShipHit2;
    public AudioClip ShipHit3;

    public AudioClip EngineLight;
    public AudioClip EngineMid;
    public AudioClip EngineHeavy;
    public AudioClip EngineDefault;

    public List<AudioClip> AmbientsClips = new List<AudioClip>();
    public List<AudioClip> DeathClips = new List<AudioClip>();

    private List<AudioClip> _shipHits = new List<AudioClip>();

    public AudioClip SelectSpell;
    public AudioClip CastDefaultSpell;
    public AudioClip HealSheild;
    public AudioClip WaveStrikeShip;
    public AudioClip ChargePowerWeapons;
    public AudioClip BufffShip;
    public AudioClip EngineDamage;
    // public AudioClip WeaponDamage;
    public AudioClip ShieldDamage;
    public AudioClip FireDamage;
    public AudioClip ClickDialog;
    public AudioClip StartDialog;
    public AudioClip WindowOpen;
    public AudioClip BuySell;
    public AudioClip InventoryMove;
    public AudioClip ShipGlobalMapMove;
    public AudioClip Upgrade;
    public AudioClip ButtonClick;
    public AudioClip SliderChange;

    public void Init()
    {
        _shipHits.Add(ShipHit1);
        _shipHits.Add(ShipHit2);
        _shipHits.Add(ShipHit3);
    }

    public AudioClip GetHit()
    {
        return _shipHits.RandomElement();
    }

    public AudioClip GetDeath()
    {
        return DeathClips.RandomElement();
    }

    public AudioClip GetEngine(ShipType type)
    {
        switch (type)
        {
            case ShipType.Light:
                return EngineLight;
                break;
            case ShipType.Middle:
                return EngineMid;
                break;
            case ShipType.Heavy:
                return EngineHeavy;
                break;
        }

        return EngineDefault;

    }

    public AudioClip ShotWeapon(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.laser:
                return LaserShot;
                break;
            case WeaponType.rocket:
                return RocketShot;
                break;
            case WeaponType.impulse:
                return ImpulseShot;
                break;
            case WeaponType.casset:
                return CassetShot;
                break;
            case WeaponType.subMine:
                break;
            case WeaponType.linerShot:
                break;
            case WeaponType.staticDamageMine:
                break;
            case WeaponType.staticSystemMine:
                break;
            case WeaponType.closeStrike:
                break;
            case WeaponType.distShot:
                break;
            case WeaponType.eimRocket:
                return EMIShot;
                break;
            case WeaponType.castMine:
                break;
            case WeaponType.randomDamage:
                break;
            case WeaponType.engineLockSpell:
                break;
            case WeaponType.shieldOFfSpell:
                break;
            case WeaponType.throwAroundSpell:
                break;
            case WeaponType.nextFrame:
                break;   
            case WeaponType.nextFrameRepair:
                break;
            case WeaponType.beam:
                return BeamShot;
                break;
            case WeaponType.artilleryBullet:
                break;
        }

        return DefaultShot;
    }

    public AudioClip GetCastSpell(SpellType spellType)
    {
        return CastDefaultSpell;
    }

    public AudioClip GetDamageSpell(ShipDamageType damageType)
    {
        switch (damageType)
        {
            case ShipDamageType.engine:
                return EngineDamage;
            // case ShipDamageType.weapon:
            //     return WeaponDamage;
            case ShipDamageType.shiled:
                return ShieldDamage;
            case ShipDamageType.fire:
                return FireDamage;
        }

        return DefaultShot;
    }
}
