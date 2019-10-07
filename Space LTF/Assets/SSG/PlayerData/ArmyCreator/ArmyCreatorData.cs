using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum ArmyCreatorType
{
    simple,
    rocket,
    laser,
    mine,
    destroy
}

public class ArmyCreatorData
{
    protected List<WeaponType> weapons = new List<WeaponType>();
    protected List<SimpleModulType> simlplModuls = new List<SimpleModulType>(); 
    protected List<SpellType> spellModuls = new List<SpellType>();
    public ShipConfig ArmyConfig;
    private bool _isAi;

    public ArmyCreatorData(ShipConfig config,bool isAi)
    {
        _isAi = isAi;
        ArmyConfig = config;
        simlplModuls = LibraryModuls.GetNormalList();
        spellModuls =  AllSpells();
        weapons = AllWeaponModuls();
    }

    protected virtual List<WeaponType> AllWeaponModuls()
    {
        return new List<WeaponType>() { WeaponType.impulse, WeaponType.laser, WeaponType.rocket, WeaponType.casset, WeaponType.eimRocket };
    }

    
    protected virtual List<SpellType> AllSpells()
    {
        if (_isAi)
        {
            var spell = new List<SpellType>();
            spell.Add(SpellType.mineField);
            spell.Add(SpellType.lineShot);
            spell.Add(SpellType.shildDamage);
            spell.Add(SpellType.engineLock);
            spell.Add(SpellType.distShot);
            spell.Add(SpellType.artilleryPeriod);
            spell.Add(SpellType.repairDrones);
            return spell;
        }
        else
        {
            return AllSpellsStatic();
        }
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
//            SpellType.randomDamage,
            SpellType.distShot
        };
    }

    public static ArmyCreatorData GetRandom(ShipConfig config)
    {
        var all = (ArmyCreatorType[]) Enum.GetValues(typeof (ArmyCreatorType));

        ArmyCreatorData data = new ArmyCreatorData(config, true);
        switch (all.ToList().RandomElement())
        {
            case ArmyCreatorType.rocket:
                data = new ArmyCreatorRocket(config, true);
                break;
            case ArmyCreatorType.laser:
                data = new ArmyCreatorLaser(config, true);
                break;
            case ArmyCreatorType.mine:
                data = new ArmyCreatorAOE(config, true);
                break;
            case ArmyCreatorType.destroy:
                data = new ArmyCreatorDestroy(config, true);
                break;
        }
        return data;
    }

    public virtual WeaponType GetWeaponType()
    {
        return weapons.RandomElement();
    }
    public virtual SimpleModulType GetSimpleType()
    {
        return simlplModuls.RandomElement();
    }

    public virtual SpellType GetSpellType()
    {
        return spellModuls.RandomElement();
    }

    public void RemoveSimple(SimpleModulType simpleModulType)
    {
        simlplModuls.Remove(simpleModulType);
    }
    public void RemoveSpell(SpellType simpleModulType)
    {
        spellModuls.Remove(simpleModulType);
    }
}

