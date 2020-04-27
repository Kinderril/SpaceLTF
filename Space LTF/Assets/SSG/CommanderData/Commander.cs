using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Commander
{
    private float rowDelta = 1.5f;
    private float lineDelta = 3;
    public Dictionary<int, ShipBase> Ships = new Dictionary<int, ShipBase>();
    // public Dictionary<ShipBase, ShipPersonalInfo> MyShipsInfos = new Dictionary<ShipBase, ShipPersonalInfo>();
    public List<ShipInventory> _destroyedShips = new List<ShipInventory>();
    private List<ShipBase> _shipsToRemove = new List<ShipBase>();
    // private Dictionary<int, CommanderShipEnemy> _enemies = new Dictionary<int, CommanderShipEnemy>();
    public ShipControlCenter MainShip;
    public CommanderCoinController CoinController;

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
    //    private Action<ShipBase> OnShipLauched;

    // private CommanderShipEnemy LastPriorityTarget;

    private bool _oneCoinContrInited;
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
    public List<TurretConnectorContainer> Connectors = new List<TurretConnectorContainer>();
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
        CoinController = new CommanderCoinController(player.Parameters.GetChargesToBattle(), 
            player.Parameters.ChargesSpeed.Level);
        //        RewardController= new CommanderRewardController(this);
        SpellController = new CommanderSpells(this);
        Priority = new CommanderPriority(this);
        //        CommanderShipBlink = new CommanderShipBlink(player.Parameters.EnginePower.Level);
    }

    public void CallReinforcments(ShipConfig config, Action<ShipBase> OnShipLauched)
    {
        var armyPower = Player.Army.GetPower();
        var armyCount = Mathf.Clamp(Player.Army.Count - 1, 1, 10);
        var powerToOneShip = armyPower / (float)armyCount;
        var armyPoints = new ArmyRemainPoints(powerToOneShip);
        var logs = new ArmyCreatorLogs();

        var dat = ArmyCreatorLibrary.GetArmy(config);
        var shipData = ArmyCreator.CreateShipByValue(armyPoints, dat, Player, logs);

        var enemyCommander = _battleController.GetCommander(BattleController.OppositeIndex(TeamIndex));
        var center = Battlefield.CellController.Data.CenterZone;
        var dirToOffset = Utils.NormalizeFast(enemyCommander.StartMyPosition - center);
        var rad = Battlefield.CellController.Data.Radius + 4;

        var pos = center + dirToOffset * rad;
        var startPos = pos;
        var dir = enemyCommander.StartMyPosition - startPos;

        var initedShip = InitShip(shipData, startPos, Utils.NormalizeFastSelf(dir));
        initedShip.Launch(OnShipLauched);
    }

    public Dictionary<int, ShipBase> InitShips(Vector3 startPosition, Vector3 enemyCenterCell, List<Vector3> positionsToClear)
    {
        _enemyCell = enemyCenterCell;
        HavePerairPlace = false;
        //        var closestCell = Battlefield.CellController.Data.FindClosestCellByType(startPosition, CellType.Free);
        StartMyPosition = startPosition;
        var dirToEnemyT = _enemyCell - startPosition;
        var distToEnemy = dirToEnemyT.magnitude;
        var dirToEnemyNorm = Utils.NormalizeFastSelf(dirToEnemyT);
        var countTurrets = _paramsOfShips.Count(x => x.Ship.ShipType == ShipType.Turret);
        var countShips = _paramsOfShips.Count - countTurrets;
        int rowIndex = 1;
        var halfShip = countShips / 2;
        var halfTurrets = 1 + countTurrets / 2;
        int indexShips = 0;
        int indexTurrets = 1;

        lineDelta = MyExtensions.Random(2, 5);
        CreateTurretConnecttors((1 + countTurrets) / 2, dirToEnemyNorm, startPosition);
        int indexPreCalc = 0;
        bool paramsIsOk = true;
        _paramsOfShips.Sort((data, pilotData) => data.Ship.ShipType == ShipType.Base
                                                 && pilotData.Ship.ShipType != ShipType.Base ? -1 : 1);
        bool haveMainShip = false;

        foreach (var paramsOfShip in _paramsOfShips)
        {
            if (paramsOfShip.Ship.ShipType == ShipType.Base)
            {
                haveMainShip = true;
                if (indexPreCalc != 0)
                {
                    paramsIsOk = false;
                }
            }
            indexPreCalc++;
        }

        if (!paramsIsOk)
        {
            Debug.LogError("Base ship not first inited big problem");
        }

        var aiPlayer = Player as PlayerAI;
        foreach (var v in _paramsOfShips)
        {
            switch (v.Ship.ShipType)
            {
                case ShipType.Light:
                    rowIndex = 1;
                    break;
                case ShipType.Middle:
                case ShipType.Heavy:
                    rowIndex = 2;
                    break;
                case ShipType.Base:
                    rowIndex = 1;
                    break;
                case ShipType.Turret:
                    rowIndex = 3;
                    break;
            }
            bool isTurret = (v.Ship.ShipType == ShipType.Turret && distToEnemy > 30);
            var index = isTurret ? indexTurrets : indexShips;
            var half = isTurret ? halfTurrets : halfShip;
            Vector3 side;
            int count = isTurret ? countTurrets : countShips;
            if (index <= half)
            {
                side = Utils.Rotate90(dirToEnemyNorm, SideTurn.left) * lineDelta * index;
            }
            else
            {
                side = Utils.Rotate90(dirToEnemyNorm, SideTurn.right) * lineDelta * (count - index);
            }
            Vector3 shipPosition;
            index++;
            if (isTurret)
            {
                shipPosition = startPosition + side + dirToEnemyNorm * (rowDelta * rowIndex + 4);
                indexTurrets = index;
            }
            else
            {
                shipPosition = startPosition + side + dirToEnemyNorm * rowDelta * rowIndex;
                indexShips = index;
            }

            var ship = InitShip(v, shipPosition, dirToEnemyNorm);
            if (haveMainShip)
            {
                if (aiPlayer != null)
                {
                    if (aiPlayer.DoBaseDefence())
                        ship.DesicionData.ChangePriority(ESideAttack.BaseDefence);
                }
            }
            positionsToClear.Add(shipPosition);
        }
        if (MainShip != null)
        {
            var weaponsIndex = 0;
            var zeroPosition = MainShip.WeaponPosition[0].transform;
            SpellController.AddMainShipBlink();
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
            MainShip.ShipParameters.ShieldParameters.Enable();
        }

        return Ships;
    }

    public SameSidePersonalInfo GetSameSide(ShipBase infoOf, ShipBase requester)
    {
        return new SameSidePersonalInfo(infoOf, requester);
    }

    private void CreateTurretConnecttors(int countTurrets, Vector3 dirToEnemyNorm, Vector3 startPosition)
    {
        var aiPlayer = Player as PlayerAI;
        Connectors.Clear();
        float turretBaseSideLenght = 5f;
        if (aiPlayer != null)
        {
            switch (aiPlayer.GetTurretBehaviour())
            {
                case ETurretBehaviour.stayAtPoint:
                    var halfTurrets = countTurrets / 2;
                    for (int i = 0; i < countTurrets; i++)
                    {
                        var element =
                            DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.TurretConnector);
                        Connectors.Add(element);
                        element.Init();
                        Vector3 side;
                        if (i < halfTurrets)
                        {
                            side = Utils.Rotate90(dirToEnemyNorm, SideTurn.left) * turretBaseSideLenght * (i + 1);
                        }
                        else
                        {
                            side = Utils.Rotate90(dirToEnemyNorm, SideTurn.right) * turretBaseSideLenght * (countTurrets - i);
                        }
                        var shipPosition = startPosition + side + dirToEnemyNorm * (rowDelta + 4);
                        element.transform.position = shipPosition;
                    }
                    break;
            }
        }
    }

    private ShipBase InitShip(StartShipPilotData v, Vector3 position, Vector3 direction)
    {
        ShipConfig config = v.Ship.ShipConfig;
        ShipBase shipPrefab;
        if (v.Ship.ShipType == ShipType.Turret)
        {
            var weapon = v.Ship.WeaponsModuls.GetNonNullActiveSlots().FirstOrDefault();
            WeaponType? type = null;
            if (weapon != null)
            {
                type = weapon.WeaponType;
            }
            shipPrefab = DataBaseController.Instance.GetShipTurret(type);
        }
        else
        {
            shipPrefab = DataBaseController.Instance.GetShip(v.Ship.ShipType, config);
        }

        //            var freeCell = closestCells[indexShips];
#if UNITY_EDITOR
        if (shipPrefab == null)
        {
            Debug.LogError("can find ship orefab by:" + v.Ship.ShipType.ToString() + "    " + v.Ship.ShipConfig.ToString());
        }
        CheckModuls(shipPrefab, v);
#endif
        var ship1 = DataBaseController.GetItem<ShipBase>(shipPrefab, position);
        ship1.Init(_teamIndex, v.Ship, new ShipBornPosition(position, direction), v.Pilot, this, ShipDeath);
        Ships.Add(ship1.Id, ship1);
        if (OnShipAdd != null)
        {
            OnShipAdd(ship1);
        }
        if (ship1.ShipParameters.StartParams.ShipType == ShipType.Base)
        {
            HavePerairPlace = true;
            MainShip = ship1 as ShipControlCenter;
            if (MainShip == null)
            {
                Debug.LogError("Crit error main ship have wrong class must me {ShipControlCenter}");
            }
            else
            {
                CommanderCoinController crollerToSet = CoinController;
                if (_oneCoinContrInited)
                {
                    crollerToSet = CoinController.Copy();
                }
                _oneCoinContrInited = true;
                MainShip.SetCoinController(crollerToSet);
            }
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

    public void AddEnemy(ShipBase enemy)
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
            s.Value.AddEnemy(enemy, true);
        }
    }

    public void SetEnemies(Dictionary<int, ShipBase> enemies)
    {
        foreach (var enemy in enemies)
        {
            // var commanderEnemy = new CommanderShipEnemy(enemy.Value.PriorityObject, enemy.Value.FakePriorityObject);
            // _enemies.Add(enemy.Key, commanderEnemy);
            AddEnemy(enemy.Value);
        }
    }

    public void LaunchAll(Action<ShipBase> OnShipLauched, Action<Commander> OnCommanderDeath)
    {
        //        this.OnShipLauched = OnShipLauched;
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
                // _enemies.Remove(shipBase.Id);
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

        if (Connectors.Count > 0)
        {
            foreach (var turretConnectorContainer in Connectors)
            {
                turretConnectorContainer.ManualUpdate();
            }
        }
    }

    // public void SetPriorityTarget(ShipBase target, bool isBait)
    // {
    //     if (LastPriorityTarget != null)
    //     {
    //         LastPriorityTarget.SetPriority(false, isBait);
    //     }
    //     LastPriorityTarget = _enemies[target.Id];
    //     LastPriorityTarget.SetPriority(true, isBait);
    // }

    public Vector3 GetWaitPosition(ShipBase ship)
    {
        if (MainShip != null)
        {
            var dir1 = (MainShip.Position - ship.Position);
            var nDir1 = Utils.NormalizeFastSelf(dir1);
            var pos1 = MainShip.Position + nDir1 * 4.5f;
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

        foreach (var turretConnector in Connectors)
        {
            GameObject.Destroy(turretConnector.gameObject);
        }
        Connectors.Clear();
        OnShipDestroy = null;
        OnShipAdd = null;
    }

    public void WinEndBattle(Commander enemyCommander)
    {
        ApplyBattleDamage();
        Player.WinBattleReward(enemyCommander);
    }

    public void ApplyBattleDamage()
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

    public TurretConnectorContainer GetClosestConnector(Vector3 pos)
    {
        TurretConnectorContainer closets = null;
        float mDist = Single.MaxValue;
        foreach (var shipBase in Connectors)
        {
            if (shipBase.Connector.CanConnect)
            {
                var trg = pos - shipBase.Position;
                var sDist = trg.sqrMagnitude;
                if (sDist < mDist)
                {
                    mDist = sDist;
                    closets = shipBase;
                }
            }
        }

        return closets;
    }
}

