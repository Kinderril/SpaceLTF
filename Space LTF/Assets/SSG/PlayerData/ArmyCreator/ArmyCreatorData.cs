using JetBrains.Annotations;
using System.Collections.Generic;

public enum ArmyCreatorType
{
    simple,
    rocket,
    laser,
    mine,
    rnd,
    destroy
}

public class ArmyCreatorData
{
    // protected List<WeaponType> weapons = new List<WeaponType>();
    private List<SimpleModulType> _simlplModuls = new List<SimpleModulType>();
    private List<SimpleModulType> _eliteModuls = new List<SimpleModulType>();
    private List<SpellType> _spellModuls = new List<SpellType>();
    private List<ShipConfig> _posibleConfigs = new List<ShipConfig>();
    private List<WeaponType> _weaponType;
    private ShipConfig _ownerConfig;

    public SimpleModulType SimpleModul => _simlplModuls.RandomElement();
    public SimpleModulType EliteModul => _eliteModuls.RandomElement();
    public ShipConfig NextShipConfig => _posibleConfigs.RandomElement();
    public ShipConfig ArmyConfig => _ownerConfig;
    public WeaponType MainWeapon => _weaponType.RandomElement();


    public ArmyCreatorData Merge(ArmyCreatorData merged)
    {
        var copy = Copy();
        foreach (var mergedSpellModul in merged._spellModuls)
        {
            copy._spellModuls.Add(mergedSpellModul);
        }

        foreach (var mergedSimlplModul in merged._simlplModuls)
        {
            copy._simlplModuls.Add(mergedSimlplModul);
        }

        foreach (var mergedEliteModul in merged._eliteModuls)
        {
            copy._eliteModuls.Add(mergedEliteModul);
        }

        foreach (var weaponType in merged._weaponType)
        {
            copy._weaponType.Add(weaponType);
        }

        foreach (var cfg in merged._posibleConfigs)
        {
            copy._posibleConfigs.Add(cfg);
        }

        return copy;
    }
    public ArmyCreatorData Copy()
    {
        List<SimpleModulType> simlplModuls = new List<SimpleModulType>();
        List<SimpleModulType> eliteModuls = new List<SimpleModulType>();
        List<SpellType> spellModuls = new List<SpellType>();
        List<ShipConfig> posibleConfigs = new List<ShipConfig>();
        List<WeaponType> weaponType = new List<WeaponType>();

        foreach (var simpleModulType in _simlplModuls)
        {
            simlplModuls.Add(simpleModulType);
        }
        foreach (var simpleModulType in _eliteModuls)
        {
            eliteModuls.Add(simpleModulType);
        }
        foreach (var type in _spellModuls)
        {
            spellModuls.Add(type);
        }
        foreach (var config in _posibleConfigs)
        {
            posibleConfigs.Add(config);
        }
        foreach (var type in _weaponType)
        {
            weaponType.Add(type);
        }
        var data = new ArmyCreatorData(posibleConfigs, weaponType, simlplModuls, eliteModuls, spellModuls);
        return data;
    }

    public ArmyCreatorData(List<ShipConfig> posibleConfigs, List<WeaponType> weaponType, List<SimpleModulType> simlplModuls,
        List<SimpleModulType> eliteModuls, List<SpellType> spellModuls)
    {
        _posibleConfigs = posibleConfigs;
        _weaponType = weaponType;
        _eliteModuls = eliteModuls;
        _spellModuls = spellModuls;
        _simlplModuls = simlplModuls;
        _ownerConfig = posibleConfigs[0];
    }

    public static List<SpellType> AllSpellsStatic()
    {
        return new List<SpellType>()
        {
//            SpellType.allToBase,
            SpellType.engineLock,
            SpellType.repairDrones,
            SpellType.artilleryPeriod,
            SpellType.lineShot,
            SpellType.shildDamage,
            SpellType.throwAround,
            SpellType.mineField,
            // SpellType.roundWave,
            SpellType.machineGun,
//            SpellType.randomDamage,
            SpellType.distShot
        };
    }

    [CanBeNull]
    public SpellType? GetSpell()
    {
        if (_spellModuls.Count == 0)
        {
            return null;
        }
        var spell = _spellModuls.RandomElement();
        _spellModuls.Remove(spell);
        return spell;
    }



}

