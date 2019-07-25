using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum BattleRewardType
{
    money,
    weapon,
    modul,
}



public class Commander
{
    public Dictionary<int,ShipBase> Ships = new Dictionary<int, ShipBase>();
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
    public AICell StartCell { get; private set; }
    public event Action<ShipBase> OnShipDestroy;
    public event Action<ShipBase> OnShipAdd;
    private Action<Commander> OnCommanderDeathCallback;
    private Action<Commander,ShipBase> OnShipInited;
    private Action<ShipBase> OnShipLauched;
    private List<StartShipPilotData> _delayedShips = new List<StartShipPilotData>();
    private int index = 0;
    private CommanderShipEnemy LastPriorityTarget;
    public float StartPower { get; private set; }
    private AICell _enemyCell;
    private Player _player;

    public TeamIndex TeamIndex
    {
        get { return _teamIndex; }
    }

    private List<StartShipPilotData> _paramsOfShips;

    public Commander(TeamIndex teamIndex,Battlefield battlefield,Player player)
    {
        BattleStats = new CommaderBattleStats();
        StartPower = 0f;
        foreach (var startShipPilotData in player.Army)
        {
            var p1 = Library.CalcPower(startShipPilotData);
            StartPower += p1;
        }
        _player = player;
        _paramsOfShips = player.GetShipsToBattle();
        Battlefield = battlefield;
        _teamIndex = teamIndex;
        CoinController = new CommanderCoinController(player.Parameters.GetChargesToBattle(),true);
//        RewardController= new CommanderRewardController(this);
        SpellController = new CommanderSpells(this);
    }

    public Dictionary<int, ShipBase> InitShips(AICell centerCell,AICell enemyCenterCell)
    {
        _enemyCell = enemyCenterCell;
        HavePerairPlace = false;
        var closestCell = Battlefield.CellController.Data.FindClosestCellByType(centerCell, CellType.Free);
        StartCell = closestCell;
//        var closestCells = Battlefield.CellController.Data.FindClosestCellsByType(centerCell, CellType.Free);
        foreach (var v in _paramsOfShips)
        {
            if (v.Pilot.Delay < 1f || v.Ship.ShipType == ShipType.Base)
            {
                InitShip(v, closestCell, _enemyCell, ref index);
            }
            else
            {
                v.TimeToLaunch = Time.time + v.Pilot.Delay; 
                _delayedShips.Add(v);
                Debug.Log("Ship delayed for:" + v.Pilot.Delay);
            }
        }
        if (MainShip != null)
        {
            var weaponsIndex = 0;
//            SpellController..Init();
            foreach (var baseSpellModul in MainShip.ShipParameters.Spells)
            {
                if (baseSpellModul != null)
                {
                    var modulPos = MainShip.WeaponPosition[weaponsIndex].transform;
                    weaponsIndex++;
                    SpellController.AddSpell(baseSpellModul, modulPos);
                }
            }
            SpellController.AddPriorityTarget();
            CoinController.Init(MainShip);
            if (CoinController.EnableCharge)
            {
                MainShip.ShipParameters.ShieldParameters.Disable();
            }
            else
            {
                MainShip.ShipParameters.ShieldParameters.Enable();
            }
        }

        return Ships;
    }

    private ShipBase InitShip(StartShipPilotData v,AICell closestCell,AICell enemyCell,ref int index)
    {
        var shipPrefab = DataBaseController.Instance.GetShip(v.Ship.ShipType, v.Ship.ShipConfig);
        //            var freeCell = closestCells[index];
#if UNITY_EDITOR
        if (shipPrefab == null)
        {
            Debug.LogError("can find ship orefab by:" + v.Ship.ShipType.ToString() + "    "  + v.Ship.ShipConfig.ToString());
        }
        CheckModuls(shipPrefab, v);
#endif
        index++;
        var half = closestCell.Side * 0.25f;
        var xx = MyExtensions.Random(-half, half);
        var zz = MyExtensions.Random(-half, half);
        var p1 = closestCell.Center + new Vector3(xx, 0, zz);
        Vector3 position = p1;
        Vector3 direction = enemyCell.Center - position;
        ShipBase ship1;
        switch (v.Ship.ShipType)
        {
            case ShipType.Heavy:
            case ShipType.Middle:
            case ShipType.Light:
                ship1 = DataBaseController.GetItem<ShipBase>(shipPrefab);
                break;
            case ShipType.Base:
                ship1 = DataBaseController.GetItem<ShipBase>(shipPrefab);
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
        return ship1;
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
            var commanderEnemy = new CommanderShipEnemy(enemy.Value);
            _enemies.Add(enemy.Key, commanderEnemy);
            AddEnemy(enemy.Value, commanderEnemy);
        }
    }

    public void LaunchAll(Action<ShipBase> OnShipLauched,Action<Commander> OnCommanderDeath)
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

    public void ChangeShieldControlCenter()
    {
        CoinController.ChangeRegenEnable();
    }

    public void UpdateManual()
    {
        DelayedShips();
//        SpellController.ManualUpdate();
        if (_shipsToRemove.Count > 0)
        {
            foreach (var shipBase in _shipsToRemove)
            {
                _enemies.Remove(shipBase.Id);
                shipBase.Dispose();
                Ships.Remove(shipBase.Id);
                BattleStats.ShipDestoyed(shipBase.ShipParameters.StartParams.ShipType, shipBase.ShipParameters.StartParams.ShipConfig);
            }
            _shipsToRemove.Clear();
        }
        foreach (var shipBase in Ships)
        {
            shipBase.Value.UpdateManual();
        }
    }

    public void SetPriorityTarget(ShipBase target)
    {
        if (LastPriorityTarget != null)
        {
            LastPriorityTarget.SetPriority(false);
        }
        LastPriorityTarget = _enemies[target.Id];
        LastPriorityTarget.SetPriority(true);
    }

    public Vector3 GetWaitPosition(ShipBase ship)
    {
        var mainShip = MainShip;
        if (mainShip != null)
        {
            var dir1 = (mainShip.Position - ship.Position);
            var nDir1 = Utils.NormalizeFastSelf(dir1);
            var pos1 = mainShip.Position + nDir1*0.5f;
            return pos1;
        }
        else
        {
            return ship.Position;
        }
    }

    private void DelayedShips()
    {
        if (_delayedShips.Count == 0)
        {
            return;
        }
        foreach (var data in _delayedShips)
        {
            if (data.TimeToLaunch < Time.time)
            {
                _delayedShips.Remove(data);
                var ship = InitShip(data,StartCell , _enemyCell, ref index);
                ship.Launch(OnShipLauched);
                return;//At one frame only one
            }
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
       _player.WinBattleReward(enemyCommander);
    }

    private void ApplyBattleDamage()
    {

        foreach (var shipBase in Ships)
        {
            var baseRepairPercent = _player.Parameters.RepairPercentPerStep();
            var percent = shipBase.Value.ShipParameters.CurHealth / shipBase.Value.ShipParameters.MaxHealth;
            if (percent < baseRepairPercent)
            {
                percent = baseRepairPercent;
            }
            Debug.Log("End game health remain percent:" + percent + "   baseRepairPercent:" + baseRepairPercent);
            shipBase.Value.ShipInventory.SetRepairPercent(percent);
        }
    }

    public bool TryRecharge(ShipBase ship)
    {
        var c = Library.COINS_TO_CHARGE_SHIP_SHIELD;
        var delay = Library.COINS_TO_CHARGE_SHIP_SHIELD_DELAY;
        var percent = Library.CHARGE_SHIP_SHIELD_HEAL_PERCENT;
        if (CoinController.CanUseCoins(c))
        {
            CoinController.UseCoins(c,delay);
            var maxShield = ship.ShipParameters.ShieldParameters.MaxShiled;
            var countToHeal = maxShield*percent;
            ship.ShipParameters.ShieldParameters.HealShield(countToHeal);
            return true;
        }
        return false;
    }
    public bool TryBuffShip(ShipBase ship)
    {
        var c = Library.COINS_TO_CHARGE_SHIP_SHIELD;
        var delay = Library.COINS_TO_CHARGE_SHIP_SHIELD_DELAY;
        var percent = Library.CHARGE_SHIP_SHIELD_HEAL_PERCENT;
        if (CoinController.CanUseCoins(c))
        {
            CoinController.UseCoins(c,delay);
            ship.BuffData.Apply();
            //ship.WeaponsController.TryWeaponReload();
            return true;
        }
        return false;
    }

}

