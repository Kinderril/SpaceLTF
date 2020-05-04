using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponsController
{
    private List<WeaponPlace> weaponPosition;
    private List<WeaponInGame> _allWeapons = new List<WeaponInGame>(5);
    private List<WeaponInGame> _damagedWeapons = new List<WeaponInGame>(5);
    private List<WeaponInGame> _supportWeapons = new List<WeaponInGame>(5);
    private WeaponAimedType[] _weaponsToAim;
    private ShipBase _owner;
    public float MaxAttackRadius { get; private set; }
    public SupportWeaponsBuffPosibilities SupportWeaponsBuffPosibilities;


    private bool _enable = true;
    public event Action<WeaponInGame> OnWeaponShootStart;
    private WeaponsAimSectorController AimSectorController;

    public WeaponsController(List<WeaponPlace> weaponPositionInc,
        ShipBase owner, List<WeaponInv> weapons, List<BaseModulInv> moduls)
    {
        MaxAttackRadius = -1f;
        SupportWeaponsBuffPosibilities = new SupportWeaponsBuffPosibilities();
        AimSectorController = new WeaponsAimSectorController();
        Dictionary<string, WeaponAimedType> weaponsAims = new Dictionary<string, WeaponAimedType>();
        _owner = owner;
        this.weaponPosition = weaponPositionInc;
        int slotIndex = 0;
        bool haveDamageWeapons = false, haveSupportWeapons = false;

        var notNullWeapons = weapons.Where(x => x != null).ToList();
        if (notNullWeapons.Count > this.weaponPosition.Count)
        {
            var slotsToCreate = notNullWeapons.Count - weaponPosition.Count;
            for (int i = 0; i < slotsToCreate; i++)
            {
                var slotToClone = weaponPosition.RandomElement();
                var slotsClone = DataBaseController.GetItem(slotToClone);
                Vector3 posForWeapon;
                if (weaponPositionInc.Count > 1)
                {
                    var p1 = weaponPositionInc[0].transform.localPosition;
                    var p2 = weaponPositionInc[1].transform.localPosition;
                    posForWeapon = (p1 + p2) / 2f;
                }
                else
                {
                    posForWeapon = slotToClone.transform.localPosition - new Vector3(0, -.04f, 0);
                }

                slotsClone.transform.SetParent(slotToClone.transform.parent);
                slotsClone.transform.rotation = slotToClone.transform.rotation;
                slotsClone.transform.localPosition = posForWeapon;
                weaponPosition.Add(slotsClone);
            }
        }


        foreach (var weapon1 in notNullWeapons)
        {
            try
            {
                WeaponInGame weapon = weapon1.CreateForBattle();

                var slot = weaponPosition[slotIndex];
                slot.SetWeapon(weapon);
                slotIndex++;

                weapon.Init(owner);
                UpgradeWithModuls(weapon, moduls);
                _allWeapons.Add(weapon);
                if (weapon.TargetType == TargetType.Enemy)
                {
                    haveDamageWeapons = true;
                    _damagedWeapons.Add(weapon);
                }
                else
                {
                    haveSupportWeapons = true;
                    _supportWeapons.Add(weapon);
                    SupportWeaponsBuffPosibilities.AddWepon(weapon);
                }

                if (weaponsAims.TryGetValue(weapon.Name, out var data))
                {
                    data.AddWeapon(weapon);
                }
                else
                {
                    weaponsAims.Add(weapon.Name, value: new WeaponAimedType(weapon));
                }
                weapon.OnShootStart += weapon2 =>
                {
                    if (OnWeaponShootStart != null)
                    {
                        OnWeaponShootStart(weapon);
                    }
                };
                weapon.CacheAngCos();

                if (haveSupportWeapons && haveDamageWeapons)
                {
                    foreach (var weaponInGame in _allWeapons)
                    {
                        weaponInGame.IncreaseReload(Library.RELOAD_COEF_DIF_WEAPONS);
                    }
                }

                if (weapon.AimRadius >= MaxAttackRadius)
                {
                    MaxAttackRadius = weapon.AimRadius;
                }

            }
            catch (Exception e)
            {
                Debug.LogError($"Can't find slot  for weapon notNullWeapons:{notNullWeapons.Count}   weaponPosition:{weaponPosition.Count}");
            }

        }

        foreach (var weaponAimedType in weaponsAims)
        {
            weaponAimedType.Value.Cache();
        }

        _weaponsToAim = weaponsAims.Values.ToArray();
        AimSectorController.Init(_weaponsToAim, owner);

    }

    private void UpgradeWithModuls(WeaponInGame weapon, List<BaseModulInv> moduls)
    {
        foreach (var modul in moduls)
        {
            weapon.UpgradeWithModul(modul);
        }
    }

    public List<WeaponInGame> GelAllWeapons()
    {
        return _allWeapons;
    }

    public void Select(bool val)
    {
        AimSectorController.Select(val);
    }

    public void Enable(bool val)
    {
        _enable = val;
    }

    public Vector3? CheckWeaponFire(IShipData target)
    {
        if (!_enable)
        {
            return null;
        }

        Vector3? posToAim = null;
        for (int i = 0; i < _weaponsToAim.Length; i++)
        {
            var weaponsToCheck = _weaponsToAim[i];
            weaponsToCheck.TryShoot(target, _owner.LookDirection);
            if (weaponsToCheck.WeaponToAim.PosToAim.HasValue)
            {
                posToAim = weaponsToCheck.WeaponToAim.PosToAim;
            }
        }

        return posToAim;
    }

    public void Dispose()
    {
        foreach (var weapon in _allWeapons)
        {
            weapon.Dispose();
        }
    }

    private bool subAnyWeaponIsLoaded(List<WeaponInGame> list)
    {
        foreach (var weapon in list)
        {
            if (weapon.IsLoaded())
            {
                return true;
            }
        }
        return false;
    }

    public bool AnyWeaponIsLoaded()
    {
        return subAnyWeaponIsLoaded(_allWeapons);
    }
    public bool AnySupportWeaponIsLoaded()
    {
        return subAnyWeaponIsLoaded(_supportWeapons);
    }
    public bool AnyDamagedWeaponIsLoaded()
    {
        return subAnyWeaponIsLoaded(_damagedWeapons);
    }
    public bool subAnyWeaponIsLoaded(List<WeaponInGame> list, float posibleUnloadSec, out bool fullLoad)
    {

        bool loadByDelta = false;
        bool loadFull = false;
        if (list.Count == 0)
        {
            fullLoad = false;
            return false;
        }
        foreach (var weapon in list)
        {
            if (weapon.IsLoaded(posibleUnloadSec, out var fullLoadWeapon))
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
    public bool AnyDamagedWeaponIsLoaded(float posibleUnloadSec, out bool fullLoad)
    {
        return subAnyWeaponIsLoaded(_damagedWeapons, posibleUnloadSec, out fullLoad);
    }
    public bool AnySupportWeaponIsLoaded(float posibleUnloadSec, out bool fullLoad)
    {
        return subAnyWeaponIsLoaded(_supportWeapons, posibleUnloadSec, out fullLoad);
    }

    public bool AllDamageWeaponNotLoad(float posibleUnloadDelta)
    {
        return subAllWeaponNotLoad(_damagedWeapons, posibleUnloadDelta);
    }
    public bool AllSupportWeaponNotLoad(float posibleUnloadDelta)
    {
        return subAllWeaponNotLoad(_supportWeapons, posibleUnloadDelta);
    }

    public bool subAllWeaponNotLoad(List<WeaponInGame> list, float posibleUnloadDelta)
    {
        if (list.Count == 0)
        {
            return true;
        }
        foreach (var weapon in list)
        {
            if (weapon.IsLoaded(posibleUnloadDelta, out var fullLoad))
            {
                return false;
            }
        }
        return true;
    }

    public bool IsInRadius(ShipBase shipLink)
    {
        var info = _owner.Enemies[shipLink];
        foreach (var weapon in _allWeapons)
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
        foreach (var weapon in _allWeapons)
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

    // public void ChargePowerToAllWeapons()
    // {
    //     foreach (var weapon in _allWeapons)
    //     {
    //         weapon.ChargeWeaponsForNextShoot();
    //     }
    //
    // }
    // public void StrikeWave()
    // {
    //     _waveStrike.Apply();
    // }
    public void TryWeaponReload()
    {
        foreach (var weapon in _allWeapons)
        {
            weapon.ReloadNow();
        }
    }
    public void IncreaseShootsDist(float coef)
    {
        foreach (var weaponInGame in _allWeapons)
        {
            weaponInGame.AimRadius *= coef;
            weaponInGame.BulletSpeed *= coef;
        }
        AimSectorController.IncreaseShootsDist(coef);
    }
    public void UnloadAll()
    {
        foreach (var weapon in _allWeapons)
        {
            weapon.Unload();
        }
    }
    public void DrawActiveWeapons()
    {
        if (_allWeapons.Count > 0)
        {
            var weapon = _allWeapons[0];
            if (weapon != null)
            {
                weapon.GizmosDraw();
            }
        }
        foreach (var w in _allWeapons)
        {
            Gizmos.color = w.IsLoaded() ? Color.blue : new Color(.85f, .1f, .1f, 1f);
            Gizmos.DrawSphere(w.GetShootPos, 0.14f);
            DrawUtils.DrawCircle(w.GetShootPos, Vector3.up, Color.yellow, w.AimRadius);
            DrawUtils.DrawCircle(w.GetShootPos, Vector3.up, Color.green, w.AimRadius);
        }
    }

}

