﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//public enum BattleRewardType
//{
//    money,
//    weapon,
//    modul,
//}



public class Commander
{
    public Dictionary<int, ShipBase> Ships = new Dictionary<int, ShipBase>();
    public List<ShipInventory> _destroyedShips = new List<ShipInventory>();
    private List<ShipBase> _shipsToRemove = new List<ShipBase>();
    private Dictionary<int, CommanderShipEnemy> _enemies = new Dictionary<int, CommanderShipEnemy>();
    public ShipBase MainShip;
    public CommanderCoinController CoinController;
    //    public CommanderRewardController RewardController;
    public CommanderSpells SpellController;
    public CommaderBattleStats BattleStats;
    public bool HavePerairPlace { get; private set; }
    private TeamIndex _teamIndex;
    public Battlefield Battlefield { get; private set; }
    //    public List<ShipBornPosition> BornPositions { get; private set; }
    public Vector3 StartMyPosition { get; private set; }
    public CommanderPriority Priority { get; private set; }
    public event Action<ShipBase> OnShipDestroy;
    public event Action<ShipBase> OnShipAdd;
    private Action<Commander> OnCommanderDeathCallback;
    private Action<Commander, ShipBase> OnShipInited;
    private Action<ShipBase> OnShipLauched;
    //    private List<StartShipPilotData> _delayedShips = new List<StartShipPilotData>();
    private int index = 0;
    private CommanderShipEnemy LastPriorityTarget;
    //    private bool isRunawayComplete = false;
    public float StartPower { get; private set; }
    private Vector3 _enemyCell;
    public Player Player { get; private set; }

    public TeamIndex TeamIndex
    {
        get { return _teamIndex; }
    }

    private List<StartShipPilotData> _paramsOfShips;
    private BattleController _battleController;
    public ShipConfig FirstShipConfig { get; private set; }

    public Commander(TeamIndex teamIndex, Battlefield battlefield, Player player, BattleController battleController)
    {
        _battleController = battleController;
        BattleStats = new CommaderBattleStats();
        StartPower = 0f;
        foreach (var startShipPilotData in player.Army.Army)
        {
            var p1 = Library.CalcPower(startShipPilotData);
            FirstShipConfig = startShipPilotData.Ship.ShipConfig;
            StartPower += p1;
        }
        Player = player;
        _paramsOfShips = player.Army.GetShipsToBattle();
        Battlefield = battlefield;
        _teamIndex = teamIndex;
        CoinController = new CommanderCoinController(player.Parameters.GetChargesToBattle(), player.Parameters.ChargesSpeed.Level);
        //        RewardController= new CommanderRewardController(this);
        SpellController = new CommanderSpells(this);
        Priority = new CommanderPriority(this);
    }

    public Dictionary<int, ShipBase> InitShips(Vector3 startPosition, Vector3 enemyCenterCell, List<Vector3> positionsToClear)
    {
        _enemyCell = enemyCenterCell;
        HavePerairPlace = false;
        //        var closestCell = Battlefield.CellController.Data.FindClosestCellByType(startPosition, CellType.Free);
        StartMyPosition = startPosition;
        var dirToEnemy = Utils.NormalizeFastSelf(_enemyCell - startPosition);
        var count = _paramsOfShips.Count;
        foreach (var v in _paramsOfShips)
        {
            int rowIndex = 1;
            float rowDelta = 3;
            float lineDelta = 3;
            var halfShip = count / 2;
            Vector3 side;
            if (index <= halfShip)
            {
                side = Utils.Rotate90(dirToEnemy, SideTurn.left) * lineDelta * index;
            }
            else
            {
                side = Utils.Rotate90(dirToEnemy, SideTurn.right) * lineDelta * (count - index);
            }
            var shipPosition = startPosition + side + dirToEnemy * rowDelta * rowIndex;

            index++;
            InitShip(v, shipPosition, dirToEnemy);
            positionsToClear.Add(shipPosition);
        }
        if (MainShip != null)
        {
            var weaponsIndex = 0;
            var zeroPosition = MainShip.WeaponPosition[0].transform;
            foreach (var baseSpellModul in MainShip.ShipParameters.Spells)
            {
                if (baseSpellModul != null)
                {
                    Transform shootPos;
                    if (MainShip.WeaponPosition.Count > weaponsIndex)
                    {
                        shootPos = MainShip.WeaponPosition[weaponsIndex].transform;
                        weaponsIndex++;
                    }
                    else
                    {
                        shootPos = zeroPosition;
                    }
                    SpellController.AddSpell(baseSpellModul, shootPos);
                }
            }
            CoinController.Init(MainShip);
            MainShip.ShipParameters.ShieldParameters.Enable();
        }

        return Ships;
    }

    private void InitShip(StartShipPilotData v, Vector3 position, Vector3 direction)
    {
        var shipPrefab = DataBaseController.Instance.GetShip(v.Ship.ShipType, v.Ship.ShipConfig);
        //            var freeCell = closestCells[index];
#if UNITY_EDITOR
        if (shipPrefab == null)
        {
            Debug.LogError("can find ship orefab by:" + v.Ship.ShipType.ToString() + "    " + v.Ship.ShipConfig.ToString());
        }
        CheckModuls(shipPrefab, v);
#endif
        ShipBase ship1;
        switch (v.Ship.ShipType)
        {
            case ShipType.Heavy:
            case ShipType.Middle:
            case ShipType.Light:
                ship1 = DataBaseController.GetItem<ShipBase>(shipPrefab, position);
                break;
            case ShipType.Base:
                ship1 = DataBaseController.GetItem<ShipBase>(shipPrefab, position);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        ship1.Init(_teamIndex, v.Ship, new ShipBornPosition(position, direction), v.Pilot, this, ShipDeath);
        Ships.Add(ship1.Id, ship1);
        if (OnShipAdd != null)
        {
            OnShipAdd(ship1);
        }
        if (ship1.ShipParameters.StartParams.ShipType == ShipType.Base)
        {
            HavePerairPlace = true;
            MainShip = ship1;
        }
        if (OnShipInited != null)
        {
            OnShipInited(this, ship1);
        }
    }

    private void CheckModuls(ShipBase rndShip, StartShipPilotData startShipPilotData)
    {
        if (startShipPilotData.Ship.ShipType != ShipType.Base && startShipPilotData.Ship.WeaponModulsCount != rndShip.WeaponPosition.Count)
        {
            Debug.LogError("Wrong weapons count of ShipConfig:" + startShipPilotData.Ship.ShipConfig.ToString()
                + "   ShipType:" + startShipPilotData.Ship.ShipType.ToString());
        }
    }

    public void AddEnemy(ShipBase enemy, CommanderShipEnemy commanderShipEnemy)
    {
        enemy.OnDeath += ship =>
        {
            foreach (var s in Ships)
            {
                s.Value.RemoveShip(enemy, true);
            }
        };
        foreach (var s in Ships)
        {
            s.Value.AddEnemy(enemy, true, commanderShipEnemy);
        }
    }

    public void SetEnemies(Dictionary<int, ShipBase> enemies)
    {
        foreach (var enemy in enemies)
        {
            var commanderEnemy = new CommanderShipEnemy(enemy.Value.PriorityObject, enemy.Value.FakePriorityObject);
            _enemies.Add(enemy.Key, commanderEnemy);
            AddEnemy(enemy.Value, commanderEnemy);
        }
    }

    public void LaunchAll(Action<ShipBase> OnShipLauched, Action<Commander> OnCommanderDeath)
    {
        this.OnShipLauched = OnShipLauched;
        OnCommanderDeathCallback = OnCommanderDeath;
        foreach (var shipBase in Ships)
        {
            shipBase.Value.Launch(OnShipLauched);
        }
    }

    private void ShipDeath(ShipBase ship)
    {
        _shipsToRemove.Add(ship);
        if (OnShipDestroy != null)
            OnShipDestroy(ship);
        if (ship.ShipParameters.StartParams.ShipType == ShipType.Base)
        {
            OnCommanderDeathCallback(this);
        }
    }

    //    public void ChangeShieldControlCenter()
    //    {
    //        CoinController.ChangeRegenEnable();
    //    }

    public void UpdateManual()
    {
        if (Time.time == 0f)
        {
            return;
        }

        //        SpellController.ManualUpdate();
        if (_shipsToRemove.Count > 0)
        {
            foreach (var shipBase in _shipsToRemove)
            {
                _enemies.Remove(shipBase.Id);
                shipBase.Dispose();
                Ships.Remove(shipBase.Id);
                _destroyedShips.Add(shipBase.ShipInventory);
                BattleStats.ShipDestoyed(shipBase.ShipParameters.StartParams.ShipType, shipBase.ShipParameters.StartParams.ShipConfig);
            }
            _shipsToRemove.Clear();
        }
        foreach (var shipBase in Ships)
        {
            shipBase.Value.UpdateManual();
        }
    }

    public void SetPriorityTarget(ShipBase target, bool isBait)
    {
        if (LastPriorityTarget != null)
        {
            LastPriorityTarget.SetPriority(false, isBait);
        }
        LastPriorityTarget = _enemies[target.Id];
        LastPriorityTarget.SetPriority(true, isBait);
    }

    public Vector3 GetWaitPosition(ShipBase ship)
    {
        var mainShip = MainShip;
        if (mainShip != null)
        {
            var dir1 = (mainShip.Position - ship.Position);
            var nDir1 = Utils.NormalizeFastSelf(dir1);
            var pos1 = mainShip.Position + nDir1 * 0.5f;
            return pos1;
        }
        else
        {
            return ship.Position;
        }
    }

    public ShipBase GetNextShip(ShipBase selectedShip)
    {
        if (Ships.ContainsValue(selectedShip))
        {
            return Ships.Values.First();//TODO
        }
        return Ships.Values.First();
    }

    public void Dispose()
    {
        CoinController.Dispsoe();
        foreach (var shipBase in Ships)
        {
            shipBase.Value.Dispose();
        }
        OnShipDestroy = null;
        OnShipAdd = null;
    }

    public void WinEndBattle(Commander enemyCommander)
    {
        ApplyBattleDamage();
        Player.WinBattleReward(enemyCommander);
    }

    private void ApplyBattleDamage()
    {

        var baseRepairPercent = Player.Parameters.RepairPercentPerStep();
        float percent = 0f;
        foreach (var shipBase in Ships)
        {
            var isDead = shipBase.Value.IsDead;
            if (!isDead)
            {
                percent = shipBase.Value.ShipParameters.CurHealth / shipBase.Value.ShipParameters.MaxHealth;
                if (percent < baseRepairPercent)
                {
                    percent = baseRepairPercent;
                }
                Debug.Log($"End game health remain percent:{percent}   baseRepairPercent:{baseRepairPercent}   isDead:{isDead}");
                shipBase.Value.ShipInventory.SetRepairPercent(percent);
            }
            else
            {
                Debug.LogError($"IMPOSIBLE! this ship is dead");
            }
        }
        foreach (var destroyedShip in _destroyedShips)
        {
            percent = baseRepairPercent;
            destroyedShip.SetRepairPercent(percent);
            destroyedShip.AddCriticalyDamage();
        }
        _destroyedShips.Clear();
    }

    public bool TryRechargeShield(ShipBase ship)
    {
        var c = Library.COINS_TO_CHARGE_SHIP_SHIELD;
        var delay = Library.COINS_TO_CHARGE_SHIP_SHIELD_DELAY;
        var percent = Library.CHARGE_SHIP_SHIELD_HEAL_PERCENT;
        if (CoinController.CanUseCoins(c))
        {
            CoinController.UseCoins(c, delay);
            var maxShield = ship.ShipParameters.ShieldParameters.MaxShield;
            var countToHeal = maxShield * percent;
            ship.Audio.PlayOneShot(DataBaseController.Instance.AudioDataBase.HealSheild);
            ship.ShipParameters.ShieldParameters.HealShield(countToHeal);
            return true;
        }
        return false;
    }

    public bool TryWave(ShipBase ship)
    {
        var c = Library.COINS_TO_WAVE_SHIP;
        var delay = Library.COINS_TO_WAVE_SHIP_DELAY;
        if (CoinController.CanUseCoins(c))
        {
            CoinController.UseCoins(c, delay);
            ship.Audio.PlayOneShot(DataBaseController.Instance.AudioDataBase.WaveStrikeShip);
            ship.WeaponsController.StrikeWave();
            return true;
        }
        return false;
    }

    public bool TryWeaponBuffShip(ShipBase ship)
    {
        var c = Library.COINS_TO_POWER_WEAPON_SHIP_SHIELD;
        var delay = Library.COINS_TO_POWER_WEAPON_SHIP_SHIELD_DELAY;
        if (CoinController.CanUseCoins(c))
        {
            CoinController.UseCoins(c, delay);
            ship.Audio.PlayOneShot(DataBaseController.Instance.AudioDataBase.ChargePowerWeapons);
            ship.WeaponsController.ChargePowerToAllWeapons();
            return true;
        }
        return false;
    }
    public bool TryBuffShip(ShipBase ship)
    {
        var c = Library.COINS_TO_CHARGE_SHIP_SHIELD;
        var delay = Library.COINS_TO_CHARGE_SHIP_SHIELD_DELAY;
        //        var percent = Library.CHARGE_SHIP_SHIELD_HEAL_PERCENT;
        if (CoinController.CanUseCoins(c))
        {
            CoinController.UseCoins(c, delay);
            ship.Audio.PlayOneShot(DataBaseController.Instance.AudioDataBase.BufffShip);
            ship.BuffData.Apply(15f);
            //ship.WeaponsController.TryWeaponReload();
            return true;
        }
        return false;
    }

    public void ExtraCharge()
    {
        var cointToReCharge = (int)(MainShip.ShipParameters.ShieldParameters.CurShiled / Library.SHIELD_COEF_EXTRA_CHARGE);
        if (cointToReCharge > 0)
        {
            int notCharged = CoinController.NotChargedCoins();
            if (notCharged < cointToReCharge)
            {
                cointToReCharge = notCharged;
            }

            if (cointToReCharge > 0)
            {
                var sheildToClear = cointToReCharge * Library.SHIELD_COEF_EXTRA_CHARGE;
                MainShip.ShipParameters.ShieldParameters.CurShiled =
                    MainShip.ShipParameters.ShieldParameters.CurShiled - sheildToClear;
                CoinController.RechargerCoins(sheildToClear);
            }
        }
    }

    public void ShipRunAway(ShipBase owner)
    {
        owner.ShipRunAway();
        _battleController.ShipRunAway(owner);
        var shipsREmain = Ships.Values.ToList();
        shipsREmain.Remove(owner);

        Debug.Log($"ShipRunAway complete {owner.Id}  remainCount:{shipsREmain.Count}");
        if (shipsREmain.Count == 1 && shipsREmain[0].ShipParameters.StartParams.ShipType == ShipType.Base)
        {
            BattleController.Instance.RunAway();
        }

        if (shipsREmain.Count == 1 || shipsREmain.Count == 0)
        {
            Debug.LogError($"WRONG RUN AWAY Ships.Count {Ships.Count}");
            BattleController.Instance.RunAway();
        }

    }

    public ShipBase GetClosestShip(Vector3 targetPosition, bool withBase)
    {
        ShipBase closets = null;
        float mDist = Single.MaxValue;
        foreach (var shipBase in Ships)
        {
            if (!withBase && shipBase.Value.ShipParameters.StartParams.ShipType == ShipType.Base)
            {
                continue;
            }

            var trg = targetPosition - shipBase.Value.Position;
            var sDist = trg.sqrMagnitude;
            if (sDist < mDist)
            {
                mDist = sDist;
                closets = shipBase.Value;
            }
        }

        return closets;
    }
}

