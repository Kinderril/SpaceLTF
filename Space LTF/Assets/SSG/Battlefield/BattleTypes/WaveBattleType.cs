
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WaveBattleType : BattleTypeEvent
{
//    private bool _isShipDead = false;
    private float _power;
//    private CommanderReinforcements _reinforcements;
    private const float WAVE_1 = 25;
//    private const float WAVE_2 = 40;
    private const float SPAWN_PERIOD_SEC = 20;
    private float _nextTimeToSpawn;
    private int _wavesCount = 1;
    private List<StartShipPilotData>[] _armiesToSpawn;
    private int _nextIndexToSpawn = 0;
    public override bool HaveActiveTime => true;


    public WaveBattleType()
        : base(Namings.Tag("nextWave"))
    {
    }

    public override void Init(BattleController battle)
    {            
        base.Init(battle);
        var ships = battle.RedCommander;
        ships.OnShipDestroy += OnDestroy;
        UpdateSpawnPeriod();
    }

    private void UpdateSpawnPeriod()
    {
        _nextTimeToSpawn = Time.time + SPAWN_PERIOD_SEC;
    }

    protected override void SubUpdate()
    {
        var remainTime = _nextTimeToSpawn - Time.time;
        if (_nextTimeToSpawn < Time.time)
        {
            SpawnWave();
        }
        OnTimeEndAction(remainTime, !IsLastWave(),GetMsg());
    }

    private void OnDestroy(ShipBase obj)
    {
        if (_battle.RedCommander.Ships.Count <= 1)
        {
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        UpdateSpawnPeriod();
        if (IsLastWave())
        {
            var wave = _armiesToSpawn[_nextIndexToSpawn];
            Spawn(wave);
            _nextIndexToSpawn++;
        }

    }

    private void Spawn(List<StartShipPilotData> wave)
    {
        foreach (var data in wave)
        {
            _battle.CallReinforcments(data, _battle.RedCommander);
        } 
    }

    public override List<StartShipPilotData> RebuildArmy(TeamIndex teamIndex, List<StartShipPilotData> paramsOfShips, Player player)
    {
        if (teamIndex == TeamIndex.green)
        {
            return paramsOfShips;
        }
        UpdateSpawnPeriod();
        var totalPower = ArmyCreator.CalcArmyPower(paramsOfShips);
//        if (totalPower > WAVE_2)
//        {
//            _wavesCount = 3;
//        }
//        else 
        if (totalPower > WAVE_1)
        {
            _wavesCount = 2;
        }
        else
        {
            _wavesCount = 1;
        }
        _armiesToSpawn = new List<StartShipPilotData>[_wavesCount];
        var shipToCopy = paramsOfShips.FirstOrDefault(x => x.Ship.ShipType != ShipType.Base && x.Ship.ShipType != ShipType.Turret);
        if (shipToCopy != null)
        {
            _power = Library.CalcPower(shipToCopy);

            var config = shipToCopy.Ship.ShipConfig;
//            var pilot = Library.CreateDebugPilot();
            var data = ArmyCreatorLibrary.GetArmy(config);
//            ArmyCreator.CreateShipWithWeapons(new ArmyRemainPoints(_power), data, _battle.RedCommander.Player, logs);
            var pointToArmy = totalPower * .6f;
            for (int i = 0; i < _wavesCount; i++)
            {
                 var army = ArmyCreator.CreateArmy(pointToArmy, ArmyCreationMode.equalize, 1, 4, data, false,
                     player);
                 _armiesToSpawn[i] = army;
            }
        }


        return paramsOfShips;
    }

    private bool IsLastWave()
    {
        return _armiesToSpawn.Length > _nextIndexToSpawn;
    }

    public override bool CanEnd()
    {
        if (!_inited)
        {
            return true;
        }

        if (IsLastWave())
        {
            return false;
        }
        return true;
    }


    public override EndBattleType WinCondition(EndBattleType prevResult)
    {
        if (prevResult == EndBattleType.win)
        {
            return EndBattleType.winFull;
        }
        return prevResult;
    }
}
