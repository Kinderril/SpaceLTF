using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

public abstract class IAICommander
{
    protected const float CAST_PERIOD = 4f;

    protected Commander _commander;
    protected ShipControlCenter _shipControl;
    protected bool enable = false;
    protected float _createdTime;

    protected bool _startAtStart;

    protected IAICommander(ShipControlCenter shipControl, Commander commanderOwner)
    {
        _createdTime = Time.time;
        _commander = commanderOwner;
        _shipControl = shipControl;
        enable = true;
        shipControl.OnDeath += OnDeath;
    }

    protected abstract void CreateAllAiSpells();
    public abstract void ManualUpdate();

    private void OnDeath(ShipBase obj)
    {
        Dispose();
        enable = false;
    }

    public void Dispose()
    {
        if (enable && _shipControl != null)
        {
            _shipControl.OnDeath -= OnDeath;
        }
    }

    [CanBeNull]
    protected BaseAISpell Create(BaseSpellModulInv baseSpellModul)
    {
        var mainShip = _shipControl;
        var spellInGame = new SpellInGame(baseSpellModul, () => mainShip.Position, mainShip.TeamIndex, mainShip, 1,
            baseSpellModul.Name, baseSpellModul.CostTime, baseSpellModul.CostCount, baseSpellModul.SpellType,
            baseSpellModul.BulleStartParameters.distanceShoot, baseSpellModul.DescFull(), baseSpellModul.DiscCounter, 1f);


        switch (baseSpellModul.SpellType)
        {
            case SpellType.shildDamage:
                return new ShieldDamageSpellAI(baseSpellModul as ShieldOffSpell, _shipControl, spellInGame);
            case SpellType.engineLock:
                return new EngineLockSpellAI(baseSpellModul as EngineLockSpell, _shipControl, spellInGame);
            case SpellType.lineShot:
                return new LineShotSpellAI(baseSpellModul as LineShotSpell, _shipControl, spellInGame);
            case SpellType.distShot:
                return new DistShotSpellAI(baseSpellModul as DistShotSpell, _shipControl, spellInGame);
            case SpellType.mineField:
                return new MineFieldSpellAI(baseSpellModul as MineFieldSpell, _shipControl, spellInGame);
            // case SpellType.randomDamage:
            //     return new RandomDamageSpellAI(baseSpellModul as RandomDamageSpell, commander, spellInGame);
            case SpellType.artilleryPeriod:
                return new ArtilleryAI(baseSpellModul as ArtillerySpell, _shipControl, spellInGame);
            case SpellType.repairDrones:
                return new RepairDropneAI(baseSpellModul as RepairDronesSpell, _shipControl, spellInGame);
            case SpellType.rechargeShield:
                return new RechargeShieldAI(baseSpellModul as RechargeShieldSpell, _shipControl, spellInGame);
            // case SpellType.roundWave:
            //
            //     break;
            case SpellType.machineGun:
                return new MachineGunSpellAI(baseSpellModul as MachineGunSpell, _shipControl, spellInGame);
            case SpellType.throwAround:
                return new ThrowAroundAI(baseSpellModul as ThrowAroundSpell, _shipControl, spellInGame);
            case SpellType.vacuum:
                return new VacuumSpellAI(baseSpellModul as VacuumSpell, _shipControl, spellInGame);
            case SpellType.hookShot:
                return new HookShotSpellAI(baseSpellModul as HookShotSpell, _shipControl, spellInGame);
        }

        return null;
    }
}
