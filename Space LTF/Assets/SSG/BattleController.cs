using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public enum BattleState
{
    preStart,
    process,
    preEnd,
    end,
}

public enum TeamIndex
{
    red,
    green,
}

public enum EndBattleType
{
    win,
    lose,
    runAway
}

public class BattleController : Singleton<BattleController>
{
    public const float START_SAFE_RADIUS = 5;
    private List<ShipBase> SideGreen = new List<ShipBase>();
    private List<ShipBase> SideRed = new List<ShipBase>();

    public Commander GreenCommander;
    public Commander RedCommander;

    public List<AICommander> AICommander = new List<AICommander>();

    public bool CanRetire => _canRetire;
    public CellController CellController { get; private set; }
    public Battlefield Battlefield;
    private BattlefieldEventController _eventController = new BattlefieldEventController();

    public InGameMainUI InGameMainUI;
    public InputManager InputManager;
    public Light MainLight;
    public List<Color> ColorLight = new List<Color>();

    public event Action<ShipBase, bool> OnShipAdd;
    public bool CanFastEnd = false;
    public BattleState State;
    //    private float _lastTimeDelta = 1f;

    public List<Bullet> ActiveBullet = new List<Bullet>();
    public List<BulletKiller> ActiveBulletKillers = new List<BulletKiller>();
    public Transform BulletContainer;
    public Transform OneBattleContainer;
    public Transform AsteroidContainer;
    public BackgroundParticles GroundParticles;

    public PauseData PauseData = new PauseData();
    private EndBattleType LastWinner;
    private bool _canRetire;
    public BattleEndCallback OnBattleEndCallback;


    public void LaunchGame(Player greenSide, Player redSide, bool canRetire, BattlefildEventType? eventType)
    {
        AICommander.Clear();
        _canRetire = canRetire;
        ActiveBullet.Clear();
        ActiveBulletKillers.Clear();
        PreLaunchGame(greenSide, redSide, eventType);
    }

    public async void PreLaunchGame(Player greenSide, Player redSide, BattlefildEventType? eventType)
    {
        await LoadGameTask(greenSide, redSide, eventType);
    }

    private async Task LoadGameTask(Player greenSide, Player redSide, BattlefildEventType? eventType)
    {
        WindowManager.Instance.LoadingScreen.gameObject.SetActive(true);
        await Task.Yield();
        await Task.Delay(100);

        RandomizeColorAndAng();
        if (PauseData == null)
        {
            PauseData = new PauseData();
        }
        _eventController.Init(this, eventType, false);
        PauseData.Unpase(1f);
        CamerasController.Instance.StartBattle();
        CanFastEnd = false;
        State = BattleState.preStart;
        WindowManager.Instance.OpenWindow(MainState.play);
        Battlefield.BackgroundSpace.Init();

        CellController = Battlefield.CellController;

        var sumOfAllShips = redSide.Army.Count + greenSide.Army.Count;
        int coef = 0;
        if (sumOfAllShips > 4)
        {
            coef = 1;
            if (sumOfAllShips > 5)
            {
                coef = 2;
                if (sumOfAllShips > 7)
                {
                    coef = 3;
                }
            }
        }

        CellController.Init(coef);

#if UNITY_EDITOR
        var greeArmyPower = ArmyCreator.CalcArmyPower(greenSide.Army);
        var redArmyPower = ArmyCreator.CalcArmyPower(redSide.Army);
        Debug.Log("green power:" + greeArmyPower + "  red army:" + redArmyPower);

#endif

        GreenCommander = new Commander(TeamIndex.green, Battlefield, greenSide, this);
        RedCommander = new Commander(TeamIndex.red, Battlefield, redSide, this);

        var d = CellController.Data;
        GroundParticles.Init(CellController.Data.CenterZone, 10);
        var posTeam1 = d.StartPosition1;
        var posTeam2 = d.StartPosition2;

        List<Vector3> positionsToClear = new List<Vector3>();
        var shipsA = GreenCommander.InitShips(posTeam1, posTeam2, positionsToClear);
        var shipsB = RedCommander.InitShips(posTeam2, posTeam1, positionsToClear);

        foreach (var vector3 in positionsToClear)
        {
            CellController.ClearPosition(vector3);
        }

        foreach (var shipBase in shipsA)
        {
            CellController.AddShip(shipBase.Value);
        }
        foreach (var shipBase in shipsB)
        {
            CellController.AddShip(shipBase.Value);
        }

        GreenCommander.OnShipAdd += ShipAdd;
        RedCommander.OnShipAdd += ShipAdd;

        SideGreen = shipsA.Values.ToList();
        SideRed = shipsB.Values.ToList();

        GreenCommander.SetEnemies(shipsB);
        RedCommander.SetEnemies(shipsA);

        GreenCommander.OnShipDestroy += OnShipDestroy;
        RedCommander.OnShipDestroy += OnShipDestroy;

        InGameMainUI.Init(Instance);
        foreach (var redCommanderShip in RedCommander.Ships)
        {
            if (redCommanderShip.Value.ShipParameters.StartParams.ShipType == ShipType.Base)
            {
                var controlCenter = redCommanderShip.Value as ShipControlCenter;
                var aiCommander = new AICommander(controlCenter);
                AICommander.Add(aiCommander);
            }
        }
        CamerasController.Instance.GameCamera.InitBorders(CellController.Min, CellController.Max);
        InputManager.Init(InGameMainUI, GreenCommander);

        GreenCommander.LaunchAll(ShipInited, CommanderDeath);
        RedCommander.LaunchAll(ShipInited, CommanderDeathRed);

        //        Time.timeScale = 1f;
        State = BattleState.process;

        await Task.Yield();
        WindowManager.Instance.LoadingScreen.gameObject.SetActive(false);
        await Task.Yield();

        CamerasController.Instance.SetCameraTo(GreenCommander.StartMyPosition, -1);
        var ambientSource = CamerasController.Instance.GameCamera.SourceAmbient;
        ambientSource.clip = DataBaseController.Instance.AudioDataBase.AmbientsClips.RandomElement();
        ambientSource.volume = 0.2f;
        ambientSource.Play();
    }

    private void CommanderDeathRed(Commander obj)
    {
        
    }

    private void RandomizeColorAndAng()
    {
        MainLight.color = ColorLight.RandomElement();
        float d = 0.7f;
        MainLight.transform.rotation = new Quaternion(0, MyExtensions.Random(-d, d), 0, 1f);
    }

    private void ShipAdd(ShipBase obj)
    {
        CellController.AddShip(obj);
        var commanderEnemy = new CommanderShipEnemy(obj.PriorityObject, obj.FakePriorityObject);
        if (obj.Commander.TeamIndex == TeamIndex.green)
        {
            SideGreen.Add(obj);
            RedCommander.AddEnemy(obj, commanderEnemy);
            foreach (var shipBase in SideRed)
            {
                obj.AddEnemy(shipBase, true, commanderEnemy);
            }
        }
        else
        {
            SideRed.Add(obj);
            GreenCommander.AddEnemy(obj, commanderEnemy);
            foreach (var shipBase in SideGreen)
            {
                obj.AddEnemy(shipBase, true, commanderEnemy);
            }
        }
        //        ShipInited(obj);
    }

    //    private IEnumerator Loading(List<StartShipPilotData> greenSide, List<StartShipPilotData> redSide)
    //    {
    ////        WindowManager.Instance.OpenWindow(MainState.play);
    //    } 


    private void CommanderDeath(Commander commander)
    {
        BattleController.Instance.WaitEndBattle();
        //        EndPart1Battle();
    }

    private void OnShipDestroy(ShipBase ship)
    {
        if (OnShipAdd != null)
            OnShipAdd(ship, false);

        SideGreen.Remove(ship);
        SideRed.Remove(ship);

        CheckEndBattle(ship);
    }

    private void CheckEndBattle(ShipBase lastShip)
    {
        PauseData.Unpase(1f);
        IsWin();
        if (lastShip.ShipParameters.StartParams.ShipType == ShipType.Base)
        {
            if (lastShip.TeamIndex == TeamIndex.green)
            {
                LastWinner = EndBattleType.lose;
                WaitEndBattle();
                return;
            }
        }

        if (SideGreen.Count == 0 || SideRed.Count == 0)
        {
            LastWinner = (SideGreen.Count >= 1) ? EndBattleType.win : EndBattleType.lose;
            WaitEndBattle();
        }
    }

    private void IsWin()
    {
        if (SideRed.Count == 0)
        {
            WaitEndBattle();
            return;
        }
        // if (SideRed.Count == 1 && SideRed[0].ShipParameters.StartParams.ShipType == ShipType.Base)
        // {
        //     CanFastEnd = true;
        //     LastWinner = EndBattleType.win;
        //     // InGameMainUI.CanFastEnd();
        // }
    }

    private bool IsLoose()
    {
        if (SideGreen.Count == 0)
        {
            return true;
        }

        return false;
    }

    private void WaitEndBattle()
    {
        var timer = MainController.Instance.BattleTimerManager.MakeTimer(2.5f, false);
        timer.OnTimer += () => { EndPart1Battle(); };
    }

    private void ShipInited(ShipBase ship)
    {
        if (OnShipAdd != null)
            OnShipAdd(ship, true);
    }

    void Update()
    {
        if (State == BattleState.process)
        {
            UpdateDistances();
            UpdateCells();
            foreach (var aiCommander in AICommander)
            {
                aiCommander.ManualUpdate();
            }
            //Manual Ships update
            GreenCommander.UpdateManual();
            RedCommander.UpdateManual();
        }
    }

    private void UpdateCells()
    {
        foreach (var a in SideGreen)
        {
            a.SetCell(CellController.GetCell(a.Position));
        }
        foreach (var a in SideRed)
        {
            a.SetCell(CellController.GetCell(a.Position));
        }
    }

    private void UpdateDistances()
    {
        //Fast sDist recalc
        foreach (var a in SideGreen)
        {
            foreach (var b in SideRed)
            {
                var dirFromAtoB = b.Position - a.Position;
                var dist = dirFromAtoB.magnitude;
                a.SetEnemyData(dirFromAtoB, dist, b);
                b.SetEnemyData(-dirFromAtoB, dist, a);
                // if (dist < 4f)
                // {
                //     var dot = AIUtility.Vector3Dot(a.LookDirection, b.LookDirection);
                //     if (dot < 0)
                //     {
                //         var dot2 = AIUtility.Vector3Dot(a.LookDirection, dirFromAtoB);
                //         if (dot2 > 0)
                //         {
                //             a.SetEvadeEnemy();
                //             b.SetEvadeEnemy();
                //         }
                //     }
                // }
            }
        }
    }

    public ShipBase Clicked(Vector3 pos)
    {
        var tmpDist = float.MaxValue;
        ShipBase ship = null;
        foreach (var shipBase in SideGreen)
        {
            var dir = shipBase.Position - pos;
            dir.y = 0;
            var sDist = dir.sqrMagnitude;
            if (sDist < tmpDist)
            {
                tmpDist = sDist;
                ship = shipBase;
            }
        }
        foreach (var shipBase in SideRed)
        {
            var dir = shipBase.Position - pos;
            dir.y = 0;
            var sDist = dir.sqrMagnitude;
            if (sDist < tmpDist)
            {
                tmpDist = sDist;
                ship = shipBase;
            }
        }
        var dist = Mathf.Sqrt(tmpDist);
        if (dist < 3)
        {
            return ship;
        }
        return null;
    }

    public void EndPart1Battle()
    {
        Debug.Log("End battle 1:" + LastWinner.ToString());
        State = BattleState.preEnd;
        DataBaseController.Instance.Pool.Clear();
        InGameMainUI.EndBattle(LastWinner);
        foreach (var activeBulletKiller in ActiveBulletKillers.ToList())
        {
            GameObject.Destroy(activeBulletKiller.gameObject);
        }

        ActiveBulletKillers.Clear();
        _eventController.Dispose();
        Dispose();
    }

    public void EndPart2Battle()
    {
        CamerasController.Instance.GameCamera.SourceAmbient.Stop();
        Debug.Log("End battle 2 LastWinner:" + LastWinner.ToString());
        GlobalEventDispatcher.WinBattle(RedCommander.FirstShipConfig);
        if (LastWinner == EndBattleType.win)
        {
            GreenCommander.WinEndBattle(RedCommander);
        }
        foreach (var shipBase in SideGreen)
        {
            GameObject.DestroyImmediate(shipBase.gameObject);
        }
        foreach (var shipBase in SideRed)
        {
            GameObject.DestroyImmediate(shipBase.gameObject);
        }

        CellController.Dispose();
        CellController.gameObject.SetActive(false);
        Battlefield.Dispose();
        State = BattleState.end;
        MainController.Instance.BattleTimerManager.StopAll();
        CamerasController.Instance.EndGame();
        var battleData = MainController.Instance.BattleData;
        _eventController.Dispose();
        OnBattleEndCallback?.Invoke(GreenCommander.Player, RedCommander.Player, LastWinner);
        switch (LastWinner)
        {
            case EndBattleType.win:
                battleData.EndGameWin();
                break;
            case EndBattleType.lose:
                battleData.EndGameLose();
                break;
            case EndBattleType.runAway:
                battleData.EndGameRunAway();
                break;
        }
    }

    public void Dispose()
    {
        BulletContainer.ClearTransform();
        InputManager.Dispose();
        OnShipAdd = null;
        GreenCommander.Dispose();
        RedCommander.Dispose();
        foreach (var aiCommander in AICommander)
        {
            aiCommander.Dispose();
        }
        AICommander.Clear();
        GroundParticles.Disable();
    }

    public static TeamIndex OppositeIndex(TeamIndex teamIndex)
    {
        switch (teamIndex)
        {
            case TeamIndex.red:
                return TeamIndex.green;
            case TeamIndex.green:
                return TeamIndex.red;
            default:
                throw new ArgumentOutOfRangeException("teamIndex", teamIndex, null);
        }
    }

    public void FastFinish()
    {
        EndPart1Battle();
    }

    public List<ShipBase> GetAllShipsInRadius(Vector3 position, TeamIndex idnex, float rad)
    {
        List<ShipBase> ships = new List<ShipBase>();
        var sRad = rad * rad;
        foreach (var ship in idnex == TeamIndex.green ? SideGreen : SideRed)
        {
            var sDist = (ship.Position - position).sqrMagnitude;
            if (sDist < sRad)
            {
                ships.Add(ship);
            }
        }
        return ships;
    }

    public void AddBullet(Bullet bullet)
    {
        if (!ActiveBullet.Contains(bullet))
        {
            //            bullet.transform.SetParent(BulletContainer,true);
            ActiveBullet.Add(bullet);
        }
    }

    public void AddBullet(BulletKiller bullet)
    {
        if (!ActiveBulletKillers.Contains(bullet))
        {
            //            bullet.transform.SetParent(BulletContainer,true);
            ActiveBulletKillers.Add(bullet);
        }
    }

    public void RemoveBullet(BulletKiller bullet)
    {
        ActiveBulletKillers.Remove(bullet);
    }

    [CanBeNull]
    public ShipBase ClosestShipToPos(Vector3 p, TeamIndex index, out float sDist)
    {
        ShipBase shipBase = null;
        sDist = Single.MaxValue;
        List<ShipBase> shipsToTest = index == TeamIndex.green ? SideGreen : SideRed;
        for (int i = 0; i < shipsToTest.Count; i++)
        {
            var b = shipsToTest[i];
            var xx = p.x - b.Position.x;
            var zz = p.z - b.Position.z;
            var d = xx * xx + zz * zz;
            if (d < sDist)
            {
                sDist = d;
                shipBase = b;
            }
        }
        return shipBase;
    }

    [CanBeNull]
    public ShipBase ClosestShipToPos(Vector3 p, TeamIndex index)
    {
        float f = 0f;
        return ClosestShipToPos(p, index, out f);
    }

    [CanBeNull]
    public Bullet ClosestBulletToPos(Vector3 p, BulletDamageType dmgType, TeamIndex index)
    {
        Bullet bullet = null;
        float dist = Single.MaxValue;
        for (int i = 0; i < ActiveBullet.Count; i++)
        {
            var b = ActiveBullet[i];
            if (b.Weapon.TeamIndex == index)
            {
                //                Debug.LogError($"Check bullet {b} IsAcive:{b.IsAcive} Id:{b.ID_Pool} {b.IsUsing} {b.DamageType} targetType:{dmgType}.");
                //                var r = (flags & b.WeaponType) != 0;
                if (b.gameObject.activeSelf && b.IsUsing && b.DamageType == dmgType)
                {
                    var xx = p.x - b.Position.x;
                    var zz = p.z - b.Position.z;
                    var d = xx * xx + zz * zz;
                    if (d < dist)
                    {
                        dist = d;
                        bullet = b;
                    }
                }
            }
        }
        //        Debug.LogError($"ClosestBulletToPos {bullet}.");
        return bullet;
    }

    public Commander GetCommander(TeamIndex enemyIndex)
    {
        if (enemyIndex == TeamIndex.green)
        {
            return GreenCommander;
        }
        return RedCommander;
    }

    public void PauseChange()
    {
        if (State == BattleState.process)
        {
            PauseData.Change();
        }
    }

    public void RunAway(HashSet<ShipBase> shipsToDamage = null)
    {
        LastWinner = EndBattleType.runAway;
        EndPart1Battle();
        if (shipsToDamage != null)
        {
            foreach (var shipBase in shipsToDamage)
            {
                shipBase.ShipInventory.SetRepairPercent(0.1f);
            }
        }

    }

    public void ShipRunAway(ShipBase owner)
    {
        SideGreen.Remove(owner);
        SideRed.Remove(owner);
    }
}

