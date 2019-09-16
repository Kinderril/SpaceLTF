using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WeaponsController
{
    private List<WeaponPlace> weaponPosition;
    private List<WeaponInGame> _weapons = new List<WeaponInGame>(5);
    private WeaponAimedType[] _weaponsToAim;
    private ShipBase _owner;
    private float _maxAttackRadius = -1f;
    private bool _enable = true;
    public event Action<WeaponInGame> OnWeaponShootStart;
    private BaseEffectAbsorber _weaponCrashEffect;
    private WeaponsAimSectorController AimSectorController;
//    private AudioClip _maxinSootClip;

    public WeaponsController(List<WeaponPlace> weaponPosition, 
        ShipBase owner,WeaponInv[] weapons, BaseModulInv[] moduls,BaseEffectAbsorber weaponCrashEffect)
    {
        AimSectorController = new WeaponsAimSectorController();
        Dictionary<string,WeaponAimedType> weaponsAims = new Dictionary<string, WeaponAimedType>();
        _weaponCrashEffect = weaponCrashEffect;
        _owner = owner;
        this.weaponPosition = weaponPosition;
        int slotIndex = 0;
        foreach (var weapon1 in weapons)
        {
            if (weapon1 == null)
            {
                continue;
            }
            WeaponInGame weapon = weapon1.CreateForBattle();

            if (slotIndex < this.weaponPosition.Count)
            {
                var slot = weaponPosition[slotIndex];
                slot.SetWeapon(weapon);
                slotIndex++;
                weapon.Init(owner);
                UpgradeWithModuls(weapon, moduls);
                _weapons.Add(weapon);
                if (weaponsAims.TryGetValue(weapon.Name, out var data))
                {
                    data.AddWeapon(weapon);
                }
                else
                {
                    weaponsAims.Add(weapon.Name,value: new WeaponAimedType(weapon));
                }
                weapon.OnShootStart += weapon2 =>
                {
                    if (OnWeaponShootStart != null)
                    {
                        OnWeaponShootStart(weapon);
                    }
                };
                weapon.CacheAngCos();

            }
            else
            {
                Debug.LogError("ship have low weapons position " + _owner.name + " " + weaponPosition.Count);
            }
            if (_maxAttackRadius <= 0)
            {
                _maxAttackRadius = weapon.AimRadius;
            }
        }

        foreach (var weaponAimedType in weaponsAims)
        {
            weaponAimedType.Value.Cache();
        }

        _weaponsToAim = weaponsAims.Values.ToArray();
        AimSectorController.Init(_weaponsToAim,owner);

    }

    private void UpgradeWithModuls(WeaponInGame weapon, BaseModulInv[] moduls)
    {
        for (int i = 0; i < moduls.Length; i++)
        {
            var modul = moduls[i];
            weapon.UpgradeWithModul(modul);
        }
    }

    public void CrashAllWeapons(bool val)
    {
        if (_weaponCrashEffect != null)
        {
            if (val)
            {
                _weaponCrashEffect.Play();
            }
            else
            {
                _weaponCrashEffect.Stop();
            }
        }
        foreach (var weapon in _weapons)
        {
            weapon.CrashReload(val);
        }
    }
    public List<WeaponInGame> GelAllWeapons()
    {
        return _weapons;
    }

    public void Select(bool val)
    {
        AimSectorController.Select(val);
    }

    public void Enable(bool val)
    {
        _enable = val;
    }

    public void CheckWeaponFire(ShipPersonalInfo target)
    {
        if (!_enable)
        {
            return;
        }
        for (int i = 0; i < _weaponsToAim.Length; i++)
        {
            var weaponsToCheck = _weaponsToAim[i];
            weaponsToCheck.TryShoot(target, _owner.LookDirection);
        }
    }

    public void Dispose()
    {
        foreach (var weapon in _weapons)
        {
            weapon.Dispose();
        }
    }

    public bool AnyWeaponIsLoaded()
    {
        foreach (var weapon in _weapons)
        {
            if (weapon.IsLoaded())
            {
                return true;
            }
        }
        return false;
    }    
    public bool AnyWeaponIsLoaded(float delta,out bool fullLoad)
    {
        bool loadByDelta = false;
        bool loadFull = false;

        foreach (var weapon in _weapons)
        {                    
            if (weapon.IsLoaded(delta,out var fullLoadWeapon))
            {
                if (fullLoadWeapon)
                {
                    loadFull = true;
                }
                loadByDelta = true;
            }
        }

        fullLoad = loadFull;
        return loadByDelta;
    }


    public bool AllWeaponNotLoad()
    {
        foreach (var weapon in _weapons)
        {
            if (weapon.IsLoaded())
            {
                return false;
            }
        }
        return true;
    }

    public bool IsInRadius(ShipBase shipLink)
    {
        var info = _owner.Enemies[shipLink];
        foreach (var weapon in _weapons)
        {
            if (weapon.IsInRadius(info.Dist))
            {
                return true;
            }
        }
        return false;
    }

    public bool BestLoadedWeapon(out float bestLoad)
    {
        bestLoad = Single.MaxValue;
        foreach (var weapon in _weapons)
        {
            var isLoad = weapon.IsLoaded();
            if (isLoad)
            {
                bestLoad = 1f;
                return true;
            }
            var bl = weapon.PercentLoad();
            if (bl < bestLoad)
            {
                bestLoad = bl;
            }
            
        }
        return false;
    }

    public void ChargePowerToAllWeapons()
    {
        foreach (var weapon in _weapons)
        {
            weapon.ChargeWeaponsForNextShoot();
        }

    }
    public void TryWeaponReload()
    {
        foreach (var weapon in _weapons)
        {
            weapon.ReloadNow();
        }
    }
    public void DrawActiveWeapons()
    {
        if (_weapons.Count > 0)
        {
            var weapon = _weapons[0];
            if (weapon != null)
            {
                weapon.GizmosDraw();
            }
        }
        foreach (var w in _weapons)
        {
            Gizmos.color = w.IsLoaded() ? Color.blue : new Color(.85f, .1f, .1f, 1f);
            Gizmos.DrawSphere(w.GetShootPos, 0.14f);
            DrawUtils.DrawCircle(w.GetShootPos, Vector3.up, Color.yellow, w.AimRadius);
            DrawUtils.DrawCircle(w.GetShootPos, Vector3.up, Color.green, w.AimRadius);
        }
    }

}

