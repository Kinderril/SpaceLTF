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
    private readonly List<SimpleModulType> _eliteModuls = new List<SimpleModulType>();

    private int _lastShipConfig;

    private readonly List<ShipConfig> _posibleConfigs = new List<ShipConfig>();

    // protected List<WeaponType> weapons = new List<WeaponType>();
    private readonly List<SimpleModulType> _simlplModuls = new List<SimpleModulType>();
    private readonly List<SpellType> _spellModuls = new List<SpellType>();
    private readonly List<WeaponType> _weaponType;

    public int MainShipCount = 0;

    public ArmyCreatorData(List<ShipConfig> posibleConfigs, List<WeaponType> weaponType,
        List<SimpleModulType> simlplModuls,
        List<SimpleModulType> eliteModuls, List<SpellType> spellModuls)
    {
        _posibleConfigs = posibleConfigs;
        _weaponType = weaponType;
        _eliteModuls = eliteModuls;
        _spellModuls = spellModuls;
        _simlplModuls = simlplModuls;
        ArmyConfig = posibleConfigs[0];
    }

    public SimpleModulType SimpleModul => _simlplModuls.RandomElement();
    public SimpleModulType EliteModul => _eliteModuls.RandomElement();
    public ShipConfig RndShipConfig => _posibleConfigs.RandomElement();
    public ShipConfig ArmyConfig { get; }

    public WeaponType MainWeapon => _weaponType.RandomElement();

    public ShipConfig NextShipConfig()
    {
        var to = _posibleConfigs[_lastShipConfig];
        _lastShipConfig++;
        if (_lastShipConfig > _posibleConfigs.Count - 1)
            _lastShipConfig = 0;

        return to;
    }

    public ArmyCreatorData Merge(ArmyCreatorData merged)
    {
        var copy = Copy();
        foreach (var mergedSpellModul in merged._spellModuls)
            copy._spellModuls.Add(mergedSpellModul);

        foreach (var mergedSimlplModul in merged._simlplModuls)
            copy._simlplModuls.Add(mergedSimlplModul);

        foreach (var mergedEliteModul in merged._eliteModuls)
            copy._eliteModuls.Add(mergedEliteModul);

        foreach (var weaponType in merged._weaponType)
            copy._weaponType.Add(weaponType);

        foreach (var cfg in merged._posibleConfigs)
            copy._posibleConfigs.Add(cfg);

        return copy;
    }

    public ArmyCreatorData Copy()
    {
        var simlplModuls = new List<SimpleModulType>();
        var eliteModuls = new List<SimpleModulType>();
        var spellModuls = new List<SpellType>();
        var posibleConfigs = new List<ShipConfig>();
        var weaponType = new List<WeaponType>();

        foreach (var simpleModulType in _simlplModuls) simlplModuls.Add(simpleModulType);
        foreach (var simpleModulType in _eliteModuls) eliteModuls.Add(simpleModulType);
        foreach (var type in _spellModuls) spellModuls.Add(type);
        foreach (var config in _posibleConfigs) posibleConfigs.Add(config);
        foreach (var type in _weaponType) weaponType.Add(type);
        var data = new ArmyCreatorData(posibleConfigs, weaponType, simlplModuls, eliteModuls, spellModuls);
        return data;
    }

    public static List<SpellType> AllSpellsStatic()
    {
        return new List<SpellType>
        {
//            SpellType.allToBase,
            SpellType.engineLock,
            SpellType.repairDrones,
            SpellType.artilleryPeriod,
            SpellType.lineShot,
            SpellType.shildDamage,
            SpellType.throwAround,
            SpellType.mineField,
            SpellType.machineGun,
            SpellType.distShot,
            SpellType.vacuum,
            SpellType.hookShot
        };
    }

    [CanBeNull]
    public SpellType? GetSpell()
    {
        if (_spellModuls.Count == 0)
            return null;
        var spell = _spellModuls[0];
        _spellModuls.Remove(spell);
        return spell;
    }
}