using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;


public class AICommander
{
    private Commander _commander;
    private bool enable = false;
    private BaseAISpell[] _spells;
    private AICommanderMainShip _mainShip;
    private float _createdTime;
    private ShipControlCenter _shipControl;
    private const float CAST_PERIOD = 4f;

    public AICommander(ShipControlCenter shipControl, Commander commanderOwner)
    {
        _createdTime = Time.time;
        _commander = commanderOwner;
        _shipControl = shipControl;
        var spellTmp = new List<BaseAISpell>();
        enable = true;
        if (enable)
        {
            shipControl.OnDeath += OnDeath;
            _mainShip = new AICommanderMainShip(_shipControl);
            foreach (var baseSpellModulInv in _shipControl.ShipInventory.SpellsModuls)
            {
                if (baseSpellModulInv != null)
                {
                    var v = Create(baseSpellModulInv);
                    if (v != null)
                    {
                        spellTmp.Add(v);
                    }
                }
            }

            _spells = spellTmp.ToArray();
            if (_spells.Length == 0)
            {
                Debug.LogError(
                    $"AI commander have no spells to use spell modules:{_shipControl.ShipInventory.SpellsModuls}");
            }
            Debug.Log("AIcommander Inited. Spells: " + _spells.Length);
        }


    }

    private void OnDeath(ShipBase obj)
    {
        Dispose();
        enable = false;
    }

    [CanBeNull]
    private BaseAISpell Create(BaseSpellModulInv baseSpellModul)
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

    public void ManualUpdate()
    {
        if (enable && Time.time - _createdTime > CAST_PERIOD)
        {
            var myArmyCount = _commander.Ships.Count;
            for (int i = 0; i < _spells.Length; i++)
            {
                var spell = _spells[i];
                spell.ManualUpdate();
                spell.PeriodlUpdate(myArmyCount);
            }

            _mainShip.Update();
        }
    }

    public void Dispose()
    {
        if (enable && _shipControl != null)
        {
            _shipControl.OnDeath -= OnDeath;
        }
    }
}