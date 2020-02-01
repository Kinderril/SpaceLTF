using System;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;

public enum TurnCur
{
    none,
    left,
    right
}

public enum SideTurn
{
    left,
    right
}

public class ShipBase : MovingObject
{
    private const float PREDICTION_DIST = 25;
    private const float PREDICTION_DIST_AIM = 1.5f;
    private const float MAX_Y_DELTA = 10f;
    private const float yHeight = 2.5f;

    private static GizmoUtils.Styles s_Styles;
    public BaseAction CurAction { get; private set; }
    private SelectedElement SelectedElement { get; set; }
    public GameObject PriorityObject { get; protected set; }
    public GameObject FakePriorityObject { get; protected set; }
    public GameObject SelectedObject { get; protected set; }

    private Action<ShipBase> _dealthCallback;
    public AudioSource Audio;

    private Vector3 _predictionPos;
    private Vector3 _predictionPosAim;
    private readonly Dictionary<ShipBase, ShipPersonalInfo> Allies = new Dictionary<ShipBase, ShipPersonalInfo>();
    public Dictionary<ShipBase, ShipPersonalInfo> Enemies = new Dictionary<ShipBase, ShipPersonalInfo>();

    public Transform HitHolder;
    public int Id;
    public bool IsInited;
    public ShipVisual ShipVisual;
    public ShipPathController2 PathController { get; private set; }
    public ShipBuffData BuffData { get; private set; }
    public ShipVisibilityData VisibilityData { get; private set; }
    public ShipPeriodDamage PeriodDamage { get; private set; }
    public CellController CellController { get; private set; }
    public ShipLocator Locator { get; private set; }
    public ShipHitData HitData { get; private set; }
    public ShipBoost Boost { get; private set; }
    public ShipAttackersData AttackersData { get; private set; }
    public ShipDamageData DamageData { get; private set; }

    public IPilotParameters PilotParameters;
    public BaseEffectAbsorber ModulEffectDestroy;
    public BaseEffectAbsorber ShipEngineStop;
    public BaseEffectAbsorber RepairEffect;
    public BaseEffectAbsorber PeriodDamageEffect;

    private BaseEffectAbsorber _lastMoveEffect;
    // public BaseEffectAbsorber WeaponCrashEffect;

    private ArrowTarget Arrow;
    public ShipParameters ShipParameters { get; private set; }
    public ShipModuls ShipModuls { get; private set; }
    //    public AimingBox AimingBox;
    public ShipPersonalInfo Target;
    public TeamIndex TeamIndex;
    public AICell Cell;
    public SelfCamera SelfCamera;

    public List<WeaponPlace> WeaponPosition;
    public IShipDesicion DesicionData { get; protected set; }

    protected override float BankMax
    {
        get
        {
            if (Boost.BoostTurn.IsActive)
            {
                return base.BankMax * 1.5f;
            }
            else
            {
                return base.BankMax;
            }
        }
    }
    public bool Pause { get; set; }
    public bool InBattlefield { get; set; }
    public bool InAsteroidField { get; private set; }
    public bool IsDead { get; private set; }

    // private bool _evadeNextFrame;
    private float yMove;
    public Collider ShieldCollider;
    public ShipInventory ShipInventory;
    public WeaponsController WeaponsController { get; private set; }
    public Commander Commander { get; private set; }

    public event Action<ShipBase> OnDeath;
    public event Action<ShipBase, BaseAction> OnActionChange;
    public event Action<ShipBase, IShipDesicion> OnShipDesicionChange;
    public event Action<ShipBase> OnDispose;

    private DebugTurnData DebugTurnData;

    public float MaxTurnRadius => Mathf.Rad2Deg * (ShipParameters.MaxSpeed / ShipParameters.TurnSpeed);

    public void Init(TeamIndex teamIndex, ShipInventory shipInventory, ShipBornPosition pos,
        IPilotParameters pilotParams, Commander commander, Action<ShipBase> dealthCallback)
    {
        //        gameObject.SetActive(false);
        SelfCamera = DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.SelfCameraPrefab);
        SelfCamera.transform.SetParent(ShipVisual.transform, false);
        SelfCamera.transform.localPosition = Vector3.zero;
        shipInventory.LastBattleData = new ShipBattleData();
        ShipInventory = shipInventory;
        CellController = commander.Battlefield.CellController;
        AttackersData = new ShipAttackersData(this);
        var selectedElementPrefab = DataBaseController.Instance.SelectedElement;
        SelectedElement = DataBaseController.GetItem<SelectedElement>(selectedElementPrefab);
        SelectedElement.Init(shipInventory.ShipType);
        SelectedElement.transform.SetParent(ShipVisual.transform, false);

        InitPriorityObjects();

        DamageData = new ShipDamageData(this);
        HitData = new ShipHitData();
        HitData.Init(ShipVisual.transform, Easing.EaseType.easeInOutElastic);
        EngineStop = new EngineStop(this, ShipEngineStop);
        ExternalForce = new ExternalForce();
        ExternalSideForce = new ExternalSideForce();
        VisibilityData = new ShipVisibilityData(this);
        //        AsteroidDamage = new ShipAsteroidDamage(this);
        PeriodDamage = new ShipPeriodDamage(this);
        Locator = new ShipLocator(this);
        Id = DataBaseController.Instance.GetNewIndex();
        PilotParameters = pilotParams;
        Commander = commander;
        TeamIndex = teamIndex;
        //        AimingBox.Init(this);
        transform.position = pos.position;
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, pos.direction);
        if (ShieldCollider == null)
        {
            Debug.LogError("can't find shield collider");
        }
        SelectedElement.gameObject.transform.position = ShieldCollider.gameObject.transform.position;
        ShipParameters = new ShipParameters(shipInventory, shipInventory.SpellsModuls,
            Death, Id, this, ShieldCollider, pilotParams);
        WeaponsController = new WeaponsController(WeaponPosition, this,
            shipInventory.WeaponsModuls, shipInventory.Moduls.SimpleModuls);
        ShipModuls = new ShipModuls(this, shipInventory.Moduls.SimpleModuls);
        ShipModuls.InitModuls();
        _dealthCallback = dealthCallback;
        DesicionDataInit();
        InitRotation();
        Select(false);
        ShipVisual.Init(this);
        if (ShipEngineStop != null)
            ShipEngineStop.Stop();
        if (RepairEffect != null)
            RepairEffect.Stop();
        if (ModulEffectDestroy != null)
            ModulEffectDestroy.Stop();
        if (PeriodDamageEffect != null)
            PeriodDamageEffect.Stop();
        // if (WeaponCrashEffect != null)
        //     WeaponCrashEffect.Stop();
        PathController = new ShipPathController2(this, 1.25f);
        BuffData = new ShipBuffData(this);
        Boost = new ShipBoost(this, ShipParameters.StartParams.BoostChargeTime, Commander.TeamIndex == TeamIndex.green);
        DamageData.Activate();
        PathController.Activate();
        if (TeamIndex == TeamIndex.green)
        {
            Arrow = DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.ArrowTargetPersonal);
            Arrow.transform.SetParent(transform);
            Arrow.transform.localPosition = Vector3.zero;
            Arrow.SetOwner(this);
        }
    }

    private void InitPriorityObjects()
    {
        var priorityTarget = DataBaseController.Instance.PriorityTarget;
        PriorityObject = DataBaseController.GetItem(priorityTarget);
        PriorityObject.transform.SetParent(ShipVisual.transform, false);
        PriorityObject.gameObject.SetActive(false);

        var selectedObject = DataBaseController.Instance.DataStructPrefabs.ShipSelectedObject;
        SelectedObject = DataBaseController.GetItem(selectedObject);
        SelectedObject.transform.SetParent(ShipVisual.transform, false);
        SelectedObject.gameObject.SetActive(false);

        var fakePriorityTarget = DataBaseController.Instance.BaitPriorityTarget;
        FakePriorityObject = DataBaseController.GetItem(fakePriorityTarget);
        FakePriorityObject.transform.SetParent(ShipVisual.transform, false);
        FakePriorityObject.gameObject.SetActive(false);
    }

    public void Select(bool val)
    {
#if UNITY_EDITOR
        if (SelectedElement == null)
        {
            Debug.LogError("WTF selected element is null");
        }
#endif
        SelectedElement.SetActive(val);
        DesicionData.Select(val);
        WeaponsController.Select(val);
    }

    protected virtual void DesicionDataInit()
    {
        SetDesision(ShipDesicionDataBase.Create(this, PilotParameters.Tactic));
    }

    public void SetDesision(IShipDesicion nData)
    {
        DesicionData = nData;
        if (OnShipDesicionChange != null)
        {
            OnShipDesicionChange(this, DesicionData);
        }
    }

    public void Launch(Action<ShipBase> OnShipLauched)
    {
        IsInited = true;
        //        gameObject.SetActive(true);
        OnShipLauched(this);
    }

    private void Death()
    {
        if (TeamIndex == TeamIndex.red && MyExtensions.IsTrueEqual())
            CamerasController.Instance.GameCamera.MainCameraShake.Init(1f);
        Audio.PlayOneShot(DataBaseController.Instance.AudioDataBase.GetDeath());
        ShipInventory.LastBattleData.Destroyed = true;
        VisibilityData.Dispose();
        if (IsDead)
        {
            Debug.LogError("Can't be dead twise");
            return;
        }

        ShipVisual.gameObject.SetActive(false);
        IsDead = true;
        _dealthCallback(this);
        if (OnDeath != null)
        {
            OnDeath(this);
        }
        EffectController.Instance.Create(DataBaseController.Instance.DataStructPrefabs.OnShipDeathEffect, transform.position, 5f);
        Dispose();
    }

    public void Dispose()
    {
        if (Arrow != null)
        {
            Arrow.Disable();
        }
        VisibilityData.Dispose();
        IsInited = false;
        if (OnDispose != null)
        {
            OnDispose(this);
        }
        SelfCamera.Dispose();
        WeaponsController.Dispose();
        DesicionData.Dispose();
        ShipParameters.Dispose();
        ShipModuls.Dispose();
        OnActionChange = null;
        //        OnAttackRewardChange = null;
        OnDeath = null;
        OnDispose = null;
        gameObject.SetActive(false);
    }

    private void EndWayCallback()
    {
        //        _moveWay = null;
    }

    private void InitRotation()
    {
        Rotation = transform.rotation;
    }

    public void UpdateManual()
    {
        if (IsDead)
        {
            return;
        }

        if (Pause || !IsInited || Time.timeScale == 0f)
        {
            return;
        }
        Boost.ManualUpdate();
        HitData.Update();
        PeriodDamage.ManualUpdate();
        //        AsteroidDamage.Update();
        UpdateAction();
        BuffData.ManualUpdate();
        DesicionData.DrawUpdate();
        VisibilityData.Update();
    }

    protected virtual void UpdateAction()
    {
        if (CurAction != null)
        {
            if (CurAction.ShallEndByTime())
            {
                CurAction.EndAction("By time");
                return;
            }

            if (CurAction != null)
            {
                CurAction.ShallEndUpdate2();
                if (CurAction == null)
                {
                    var task = DesicionData.CalcAction();
                    Debug.Log(("Task2:" + Id + "  " + task.ToString()).Yellow());
                    SetAction(task);
                }
            }
            CurAction.ManualUpdate();
        }
        else
        {
            var task = DesicionData.CalcAction();
            Debug.Log(("Task1:" + Id + "  " + task.ToString()).Yellow());
            SetAction(task);
        }

        UpdateShieldRegen();

        //        SetTargetSpeed(Boost.BoostTurn.TargetBoosSpeed);
        EngineUpdate();
        MoveByY(YMoveRotation.YMoveCoef);
        ApplyMove(Boost.LastTurnAddtionalMove, Boost.IsActive);
        Locator.ManualUpdate();
        // CheckYEnemies();
        _predictionPos = LookDirection * PREDICTION_DIST + Position;
        _predictionPosAim = LookDirection * PREDICTION_DIST_AIM + Position;
        // _backPredictionPos = -LookDirection * PREDICTION_DIST + Position;
    }

    //     private void CheckYEnemies()
    //     {
    // //        return;
    //         if (_curSpeed <= 0f)
    //         {
    //             return;
    //         }
    //
    //         bool isGreen = (TeamIndex == TeamIndex.green);
    //         if (_evadeNextFrame)
    //         {
    //             _evadeNextFrame = false;
    //             int dir = isGreen ? 1 : -1;
    //             MoveByY(dir, isGreen);
    //             return;
    //         }
    //
    //         if ((isGreen && yMove > 0) || (!isGreen && yMove < 0))
    //         {
    //             int dirInner = isGreen ? -1 : 1;
    //             MoveByY(dirInner, isGreen);
    //         }
    //         else
    //         {
    //             yMove = 0;
    //             var p = ShipVisual.transform.position;
    //             ShipVisual.transform.position = new Vector3(p.x, yMove, p.z);
    //         }
    //     }

    private void MoveByY(float resultY)
    {
#if UNITY_EDITOR
        if (DebugParamsController.EngineOff)
        {
            return;
        }
#endif
        yMove = yHeight * resultY;
        var p = ShipVisual.transform.position;
        var yy = Mathf.Clamp(yMove, 0f, MAX_Y_DELTA);
        ShipVisual.transform.position = new Vector3(p.x, yy, p.z);
    }

    protected void UpdateShieldRegen()
    {
        ShipParameters.Update();
    }


    public void MoveToDirection(Vector3 dir)
    {
        var speed = ApplyRotation(dir, false);
        SetTargetSpeed(speed);
    }

    public override float ApplyRotation(Vector3 dir, bool exactlyPoint)
    {
        if (Boost.IsActive && Boost.UseRotationByBoost)
        {
            if (Boost.BoostTurn.IsActive)
            {
                Boost.BoostTurn.ApplyRotation(dir);
                return Boost.BoostTurn.TargetBoosSpeed;
            }

            if (Boost.BoostLoop.IsActive)
            {
                return 1f;
            }

            if (Boost.BoostTwist.IsActive)
            {
                Boost.BoostTwist.ApplyRotation(dir);
                return 1f;
            }
        }
        return base.ApplyRotation(dir, exactlyPoint);
    }

    public void MoveByWay(Vector3 target)
    {
        DebugTurnData = null;
        var direction4 = PathController.GetCurentDirection(target, out var exactlyPoint, out var goodDir, out var speedRecommended);
        if (!goodDir)
        {
            var speed = ApplyRotation(direction4, exactlyPoint);
            SetTargetSpeed(speed);
        }
        else
        {
            Boost.Deactivate();
            SetTargetSpeed(speedRecommended);
        }

    }

    public void MoveByWay(ShipBase target)
    {
        DebugTurnData = null;
        var direction4 = PathController.GetCurentDirection(target, out var exactlyPoint, out var goodDir, out var speedRecommended);
        if (!goodDir)
        {
            var speed = ApplyRotation(direction4, exactlyPoint);
            SetTargetSpeed(speed);
        }
        else
        {
            Boost.Deactivate();
            SetTargetSpeed(speedRecommended);
        }
    }

    private bool GetClosestBorderPoint(Vector3 dirWanted, Vector3 target)
    {
        DebugTurnData = new DebugTurnData();

        var leftDor = Vector3.Dot(dirWanted, LookLeft) > 0;
        Vector3 offsetSied;
        if (leftDor)
        {
            offsetSied = LookLeft;
        }
        else
        {
            offsetSied = LookRight;
        }
        var offsideVector = offsetSied * MaxTurnRadius;
        var centerTurnPoint = Position + offsideVector;
        //        var centerDir = target - centerTurnPoint;
        DebugTurnData.ShipPoint = Position;
        DebugTurnData.TurnCenter = centerTurnPoint;
        DebugTurnData.TrunRaius = MaxTurnRadius;
        DebugTurnData.Target = target;
        //        var distFromCentToTarget = Vector3.Magnitude(centerTurnPoint - target);
        //        var arcCosTo = MaxTurnRadius / distFromCentToTarget;
        //        var radius = Mathf.Acos(arcCosTo);
        //        Vector3 normalizedDirToEndPoit;
        //        Vector3 test1;
        //        Vector3 test2;
        //
        //        var rotated = Utils.RotateOnAngUp(centerDir, Mathf.Rad2Deg * radius);
        //        normalizedDirToEndPoit = Utils.NormalizeFastSelf(rotated);
        //        var dirTest1 = normalizedDirToEndPoit * MaxTurnRadius;
        //        test1 = centerTurnPoint + dirTest1;
        //
        //        var rotatedOther = Utils.RotateOnAngUp(centerDir, -Mathf.Rad2Deg * radius);
        //        normalizedDirToEndPoit = Utils.NormalizeFastSelf(rotatedOther);
        //        var dirTest2 = normalizedDirToEndPoit * MaxTurnRadius;
        //        test2 = centerTurnPoint + dirTest2;

        var resuolDir = AIUtility.RotateByTraectory(Position, target, centerTurnPoint, LookDirection, MaxTurnRadius);

        //
        DebugTurnData.PointEndTurn2 = resuolDir.Wrong;
        DebugTurnData.PointEndTurn = resuolDir.Right;
        var rightVector = resuolDir.Right - centerTurnPoint;
        var middleVector = -offsideVector + rightVector;
#if UNITY_EDITOR
        var mag = Mathf.Abs(offsideVector.magnitude - rightVector.magnitude);
        if (mag > 0.001f)
        {
            Debug.LogError("Wrong middle vector");
        }
#endif
        //        if (middleVector.sqrMagnitude < 0.00001f)
        //        {
        //            return false;
        //        }

        //        return false;
        var middleVectorSize = Utils.NormalizeFastSelf(middleVector) * MaxTurnRadius;
        //        var test1 = centerTurnPoint + resuolDir.Right;
        Vector3 checkForCenter = centerTurnPoint + middleVectorSize;
        DebugTurnData.CheckForCenter = checkForCenter;
        SegmentPoints s1 = new SegmentPoints(checkForCenter, resuolDir.Right);
        SegmentPoints s2 = new SegmentPoints(checkForCenter, Position);
        return CheckOnCrossTurn(s1, s2);
    }

    private bool CheckOnCrossTurn(SegmentPoints test1, SegmentPoints test2)
    {
        bool isgood;
        isgood = HaveCroosWithBadCell(test1, Cell.Border1);
        if (isgood) { return true; }
        isgood = HaveCroosWithBadCell(test1, Cell.Border2);
        if (isgood) { return true; }
        isgood = HaveCroosWithBadCell(test1, Cell.Border3);
        if (isgood) { return true; }
        isgood = HaveCroosWithBadCell(test1, Cell.Border4);

        if (isgood) { return true; }
        isgood = HaveCroosWithBadCell(test2, Cell.Border1);
        if (isgood) { return true; }
        isgood = HaveCroosWithBadCell(test2, Cell.Border2);
        if (isgood) { return true; }
        isgood = HaveCroosWithBadCell(test2, Cell.Border3);
        if (isgood) { return true; }
        isgood = HaveCroosWithBadCell(test2, Cell.Border4);
        if (isgood) { return true; }

        return false;
    }

    private bool HaveCroosWithBadCell(SegmentPoints test1, AICellSegment border)
    {
        Vector3? cross;
        cross = AIUtility.GetCrossPoint(test1, border);
        if (cross.HasValue)
        {
            var neightCell = Cell.CellsOnSides[border];
            if (!neightCell.IsFree())
            {
                DebugTurnData.NeightCellBlocked = neightCell.Center;
                return true;
            }
        }

        return false;
    }

    private void GetMin(float nf, Vector3 nv, float oldf, Vector3 oldv, out float rf, out Vector3 rv)
    {
        if (nf < oldf)
        {
            rf = nf;
            rv = nv;
        }
        else
        {
            rf = oldf;
            rv = oldv;
        }
    }

    protected override float TurnSpeed()
    {
        return ShipParameters.TurnSpeed;
    }

    public override float MaxSpeed()
    {
        return ShipParameters.MaxSpeed;
    }

    public bool IsInFromt(ShipBase target)
    {
        return IsInFromt(target.Position);
    }

    public bool IsInFromt(Vector3 target)
    {
        //        return false;
        var dirToTrg = target - Position;
        var isAng = Utils.IsAngLessNormazied(LookDirection, Utils.NormalizeFastSelf(dirToTrg), UtilsCos.COS_90_RAD);
        return isAng;
    }


    public void AsteroidField(bool b)
    {
        InAsteroidField = b;
    }

    public void WayEnds()
    {
        //        _moveWay = null;
    }

    public void GoToPointAction(Vector3 pos, bool withEffect)
    {
        var battle = BattleController.Instance;
        var center = battle.CellController.Data.CenterZone;
        var rad = battle.CellController.Data.InsideRadius;
        var dist = (center - pos).magnitude;
        if (dist - 3 < rad)
        {
            var a = new GoToCurrentPointAction(this, pos);
            SetAction(a);
            if (withEffect)
            {
                if (_lastMoveEffect != null)
                {
                    _lastMoveEffect.Stop();
                }
                var effect = EffectController.Instance.Create(DataBaseController.Instance.SpellDataBase.GoPlaceOk, pos, 2);
                _lastMoveEffect = effect;
            }

        }
        else
        {
            if (withEffect)
            {
                if (_lastMoveEffect != null)
                {
                    _lastMoveEffect.Stop();
                }
                var effect = EffectController.Instance.Create(DataBaseController.Instance.SpellDataBase.GoPlaceFail, pos, 2);
                _lastMoveEffect = effect;
            }
        }
    }

    public void RunAwayAction()
    {
        var a = new GoToBaseAction(this, Commander.MainShip, true);
        SetAction(a);
    }

    public void AddEnemy(ShipBase enemy, bool isEnemy, CommanderShipEnemy commanderShipEnemy)
    {
        var enemyInfo = new ShipPersonalInfo(this, enemy, commanderShipEnemy);
        var dir = enemy.Position - Position;
        enemyInfo.SetParams(dir, dir.magnitude);
        if (isEnemy)
        {
            Enemies.Add(enemy, enemyInfo);
            enemy.VisibilityData.OnVisibilityChange += OnEnemyVisibilityChange;
        }
        else
        {
            Allies.Add(enemy, enemyInfo);
        }
    }

    private void OnEnemyVisibilityChange(ShipBase arg1, bool arg2)
    {
        var dir = arg1.Position - Position;
        if (dir.sqrMagnitude < 5 * 5)
        {
            if (CurAction != null)
            {
                CurAction.EndAction("Visibility change");
            }
        }
    }
    public virtual Vector3 PredictionPos()
    {
        return _predictionPos;
    }

    public Vector3 PredictionPosAim()
    {
        return _predictionPosAim;
    }

    public void SetEnemyData(Vector3 dirFromAtoB, float dist, ShipBase mover)
    {
        Enemies[mover].SetParams(dirFromAtoB, dist);
    }

    public void SetAllyData(Vector3 dirFromAtoB, float dist, ShipBase mover)
    {
        Allies[mover].SetParams(dirFromAtoB, dist);
    }

    public void GetHit(IWeapon weapon, Bullet bullet)
    {
        var hitClip = DataBaseController.Instance.AudioDataBase.GetHit();
        Audio.PlayOneShot(hitClip);
        //        Debug.LogError($"playt hit {hitClip.name}");
        if (ShipParameters.ShieldParameters.ShiledIsActive)
        {
            var effect = EffectController.Instance.Create(DataBaseController.Instance.SpellDataBase.ShieldHitEffect, transform, 3f);
            effect.transform.position = bullet.Position;
            var posToLookAt = bullet.Position - bullet.LookDirection * 3;
            Debug.DrawLine(posToLookAt, effect.transform.position, Color.red, 10);
            effect.transform.LookAt(posToLookAt, Vector3.up);
        }
        weapon.ApplyToShip(ShipParameters, this, bullet);
    }

    public void SetAction(BaseAction nextAction)
    {
        var attackAction = nextAction as AttackAction;
        if (attackAction != null)
        {
            Target = attackAction.Target;
        }

        if (CurAction != null)
        {
            DesicionData.SetLastAction(CurAction.ActionType);
        }

        CurAction = nextAction;
        if (OnActionChange != null)
        {
            OnActionChange(this, CurAction);
        }
    }

    public void EndAction()
    {
        SetAction(null);
    }

    public void RemoveShip(ShipBase p0, bool isEnemy)
    {
        Debug.Log("Ship removed:" + p0.Id + "    isENemy:" + isEnemy);
        if (isEnemy)
        {
            var info = Enemies[p0];
            Enemies.Remove(p0);
            AttackersData.Remove(info);
        }
        else
        {
            Allies.Remove(p0);
        }
    }

    protected override void DrawGizmosSelected()
    {
        if (CurAction != null)
        {
            CurAction.DrawGizmos();
        }


        GizmosSelected();
    }

    protected virtual void GizmosSelected()
    {
        if (!Application.isPlaying || !IsInited)
        {
            return;
        }

        var s = Cell.Side;
        Gizmos.DrawWireCube(Cell.Center, new Vector3(s, 0.1f, s));
        var asteroids = Cell.GetAsteroidsForShip(this);
        DrawUtils.DrawCircle(transform.position, Vector3.up, InBattlefield ? Color.green : Color.yellow, MaxTurnRadius);
        Gizmos.color = Color.white;
        switch (TeamIndex)
        {
            case TeamIndex.green:
                Gizmos.color = Color.green;
                break;
            case TeamIndex.red:
                Gizmos.color = Color.blue;
                break;
        }

        if (DebugTurnData != null)
        {
            DebugTurnData.DrawGizmos();
        }
        PathController.OnDrawGizmosSelected();
        Gizmos.DrawWireSphere(transform.position, 0.6f);
    }

    public void ShipRunAway()
    {
        _dealthCallback(this);
        if (OnDeath != null)
        {
            OnDeath(this);
        }
    }
    public void SetCell(AICell nextCell)
    {
        if (nextCell != Cell)
        {
            var inClouds = (nextCell.CellType == CellType.Clouds);
            VisibilityData.SetInClouds(inClouds);
            Cell = nextCell;
            //            InBattlefield = !nextCell.OutOfField;
            if (nextCell.CellType == CellType.DeepSpace)
            {
                ShipParameters.Damage(999, 999, null, null);
            }
        }
    }

    protected override void DrawGizmos()
    {
        if (s_Styles == null)
            s_Styles = new GizmoUtils.Styles();
        DrawOutOfBattle();
        DrawHpLable();
        DrawShieldLable();
        DrawActionLable();
    }

    private void DrawOutOfBattle()
    {

    }

    private void DrawHpLable()
    {
        var p = transform.position + Vector3.up * 0.85f;
        if (ShipParameters != null)
        {
            var s = "hp:" + ShipParameters.CurHealth.ToString("0") + "/" + ShipParameters.MaxHealth.ToString("0");
            SubDraw(s, p);
        }
    }

    private void DrawShieldLable()
    {
        if (ShipParameters != null)
        {
            var s = "s:" + ShipParameters.CurShiled.ToString("0") + "/" + ShipParameters.MaxShield.ToString("0");
            var p = transform.position + Vector3.up * 0.58f;
            SubDraw(s, p);
        }
    }

    private void DrawActionLable()
    {
        var p = transform.position + Vector3.up * 0.2f;
        var content = "";

        if (CurAction == null)
        {
            content = "no action";
        }
        else
        {
            if (CurAction is AttackAction)
            {
                content = "attack";
            }
            else if (CurAction is GoToBaseAction)
            {
                content = "goTo";
            }
            else if (CurAction is ReturnActionToBattlefield)
            {
                content = "return";
            }
            else if (CurAction is EvadeAction)
            {
                content = "evade";
            }
        }

        SubDraw(content, p);
    }

    private void SubDraw(string content, Vector3 p)
    {
        var currentCamera = Camera.current;
        var nameContent = new GUIContent(content);
        var size = s_Styles.enabledStateName.CalcSize(nameContent);
        var screenPoint = currentCamera.WorldToScreenPoint(p);
        var position = currentCamera.ScreenToWorldPoint(new Vector3(screenPoint.x - size.x * .5f, screenPoint.y, -screenPoint.z));

        // Draw enabled states name
        //        Handles.Label(position, nameContent, s_Styles.enabledStateName);
    }

}