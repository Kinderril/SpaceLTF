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
    public const string StencilType = "_StencilType";
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

    public List<Renderer> Renderers = new List<Renderer>();

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
    private bool _wayDestroyed = false;
    public ShipPathController2 PathController { get; private set; }
    public ShipBuffData BuffData { get; private set; }
    public ShipVisibilityData VisibilityData { get; private set; }
    public ShipPeriodDamage PeriodDamage { get; private set; }
    public CellController CellController { get; private set; }
    public ShipLocator Locator { get; private set; }
    public ShipDeathData DeathData { get; private set; }
    public ShipHitData HitData { get; private set; }
    public ShipBoost Boost { get; private set; }
    public ShipAttackersData AttackersData { get; private set; }
    public ShipDamageData DamageData { get; private set; }
    public ShipWayDrawler WayDrawler;

    public IPilotParameters PilotParameters;
//    public BaseEffectAbsorber ModulEffectDestroy;
    public BaseEffectAbsorber ShipEngineStop;
    public BaseEffectAbsorber RepairEffect;
    public BaseEffectAbsorber PeriodDamageEffect;
    public MeshTimeAbsorber RamBoostEffect;
    public BaseEffectAbsorber MoveBoostEffect;

    private BaseEffectAbsorber _lastMoveEffect;
    // public BaseEffectAbsorber WeaponCrashEffect;

    private ArrowTarget Arrow;
    public ShipParameters ShipParameters { get; private set; }

    public ShipModuls ShipModuls { get; private set; }

    //    public AimingBox AimingBox;
    public IShipData Target;
    public TeamIndex TeamIndex;
    public AICell Cell;
    public SelfCamera SelfCamera;

    public List<WeaponPlace> WeaponPosition;
    public float ExpCoef { get; protected set; }
    public IShipDesicion DesicionData { get; protected set; }
    // public int TotalExpByKill { get; protected set; }

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

    public float MaxTurnRadius => Mathf.Rad2Deg * (MaxSpeed() / TurnSpeed());

    public virtual void Init(TeamIndex teamIndex, ShipInventory shipInventory, ShipBornPosition pos,
        IPilotParameters pilotParams, Commander commander, Action<ShipBase> dealthCallback)
    {
        if (Audio != null)
        {
            Audio.volume = .28f;
        }
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
        int stencil = 3;
        switch (teamIndex)
        {
            case TeamIndex.red:
                stencil = SSAOMask.STENCI_RED;
                break;
            case TeamIndex.green:
                stencil = SSAOMask.STENCI_GREEN;
                break;
        }
        foreach (var renderer1 in Renderers)
        {
            var copy = Utils.CopyMaterials(renderer1, null, null);
            for (int i = 0; i < copy.Length; i++)
            {
                var material = copy[i];
                material.SetInt(StencilType, stencil);

            }

        }
        DeathData = new ShipDeathData(this,InitDeathParts, DeathEffects);
        transform.SetParent(BattleController.Instance.ShipsContainer);
        WayDrawler = DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.WayDrawler);
        WayDrawler.transform.SetParent(BattleController.Instance.ShipWaysContainer);
        WayDrawler.Clear();
        DamageData = new ShipDamageData(this);
        HitData = new ShipHitData();
        HitData.Init(ShipVisual.transform, Easing.EaseType.easeInOutElastic);
        EngineStop = new EngineStop(this, ShipEngineStop);
        ExternalForce = new ExternalForce(shipInventory.ShipType == ShipType.Base ? 0.3f : 1f);
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
        ShipParameters = new ShipParameters(shipInventory, shipInventory.SpellsModuls.GetAsCopyArray(),shipInventory.SpellConnectedModules,
            Death, Id, this, ShieldCollider, pilotParams);
        WeaponsController = new WeaponsController(WeaponPosition, this,
            shipInventory.WeaponsModuls.GetNonNullActiveSlots(), shipInventory.Moduls.GetNonNullActiveSlots());
        var modulsForShip = shipInventory.Moduls.GetNonNullActiveSlots();
        foreach (var subSpellModul in ShipParameters.SubSpellModuls)
        {
            if (subSpellModul != null)
            {
                foreach (var allItem in subSpellModul.GetAllItems())
                {
                    if (allItem is BaseModulInv asModul)
                    {
                        modulsForShip.Add(asModul);
                    }
                }
            }
        }

        ShipModuls = new ShipModuls(this, modulsForShip);
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
        if (RamBoostEffect != null)
            RamBoostEffect.Stop();
        if (PeriodDamageEffect != null)
            PeriodDamageEffect.Stop();
        if (MoveBoostEffect != null)
            MoveBoostEffect.Stop();
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
        var totalExpByKill = Library.GetExp(ShipParameters.StartParams.ShipType, ShipParameters.StartParams.ShipConfig);
        var maxSum = ShipParameters.MaxHealth + ShipParameters.MaxShield;
        ExpCoef = totalExpByKill / maxSum;
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
        if (MyExtensions.IsTrue01(0.3f))
        {
            float amount = 0.5f;
            float duration = 0.8f;

            switch (TeamIndex)
            {
                case TeamIndex.red:
                    amount = 0.3f;
                    duration = 0.6f;
                    break;
                case TeamIndex.green:
                    amount = 0.5f;
                    duration = 0.7f;
                    break;
            }
            CamerasController.Instance.GameCamera.MainCameraShake.Init(duration, amount);
        }

        Audio.PlayOneShot(DataBaseController.Instance.AudioDataBase.GetDeath());
        ShipInventory.LastBattleData.Destroyed = true;
        VisibilityData.Dispose();
        if (IsDead)
        {
            Debug.LogError("Can't be dead twise");
            return;
        }
        WayDrawler.Clear();
        
        DeathData.StartDeath();
//        ShipVisual.gameObject.SetActive(false);
//        InitDeathParts();
        IsDead = true;
        _dealthCallback(this);
        OnDeath?.Invoke(this);
//        EffectController.Instance.Create(DataBaseController.Instance.DataStructPrefabs.OnShipDeathEffect, transform.position, 5f);
        Dispose();
    }


    private void InitDeathParts()
    {
        var pool = DataBaseController.Instance.Pool;
        int cnt = MyExtensions.Random(2, 5);
        var p = gameObject.transform.position;
        float partOffset = .3f;
        float velocity  = .6f;
        float quaterRnd = 1f;
        for (int i = 0; i < cnt; i++)
        {
            var partShip = pool.GetPartShip();
            partShip.Init();

            var xxV = MyExtensions.Random(-velocity, velocity);
            var zzV = MyExtensions.Random(-velocity, velocity);

            partShip.Rigidbody.velocity = new Vector3(xxV,0,zzV);

           var xx = p.x + MyExtensions.Random(-partOffset, partOffset);
            var zz = p.z + MyExtensions.Random(-partOffset, partOffset);
            partShip.transform.position = new Vector3(xx, p.y, zz);

            var xxQ = MyExtensions.Random(-quaterRnd, quaterRnd);
            var yyQ = MyExtensions.Random(-quaterRnd, quaterRnd);
            var zzQ = MyExtensions.Random(-quaterRnd, quaterRnd);
//            var wwQ = MyExtensions.Random(-quaterRnd, quaterRnd);
            partShip.transform.position = new Vector3(xx, p.y, zz);
            partShip.transform.rotation = new Quaternion(xxQ, yyQ, zzQ, 1f);
        }
    }

    public override void Dispose()
    {
        if (Arrow != null)
        {
            Arrow.Disable();
        }
        VisibilityData.Dispose();
        IsInited = false;
        base.Dispose();
        OnDispose?.Invoke(this);

        if (!_wayDestroyed)
        {
            _wayDestroyed = true;
            GameObject.Destroy(WayDrawler.gameObject);
        }
//        WayDrawler.Clear();
        SelfCamera.Dispose();
        WeaponsController.Dispose();
        DesicionData.Dispose();
        ShipParameters.Dispose();
        ShipModuls.Dispose();
        OnActionChange = null;
        //        OnAttackRewardChange = null;
        OnDeath = null;
        OnDispose = null;
//        gameObject.SetActive(false);
    }

    private void EndWayCallback()
    {
        //        _moveWay = null;
    }

    private void InitRotation()
    {
        Rotation = transform.rotation;
    }

    public bool UpdateDeath()
    {
        return DeathData.Update();
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
//                    Debug.Log(("Task2:" + Id + "  " + task.ToString()).Yellow());
                    SetAction(task);
                }
            }
            CurAction.ManualUpdate();
        }
        else
        {
            var task = DesicionData.CalcAction();
//            Debug.Log(("Task1:" + Id + "  " + task.ToString()).Yellow());
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
                return Boost.BoostTurn.CurSpeed;
            }

            if (Boost.BoostTwist.IsActive)
            {
                Boost.BoostTwist.ApplyRotation(dir);
                return Boost.BoostTwist.CurSpeed;
            }

            if (Boost.BoostLoop.IsActive)
            {
                return 1f;
            }
            if (Boost.BoostRam.IsActive)
            {
                return 1f;
            }
        }
        return base.ApplyRotation(dir, exactlyPoint);
    }

    public void MoveByWay(Vector3 target)
    {
        DebugTurnData = null;
        var direction4 = PathController.GetCurentDirection(target, out var exactlyPoint, out var goodDir, out var speedRecommended);
        if (!goodDir || (Boost.IsActive && Boost.UseRotationByBoost))
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

    private Vector3 _lastDirection;
    public void MoveByWay(IShipData target)
    {
        DebugTurnData = null;
        var direction4 = PathController.GetCurentDirection(target, out var exactlyPoint, out var goodDir, out var speedRecommended);
        if (!goodDir)
        {
            var dirToMove = Vector3.Lerp(_lastDirection, direction4, .5f);
            var speed = ApplyRotation(dirToMove, exactlyPoint);
            _lastDirection = dirToMove;
            SetTargetSpeed(speed);
        }
        else
        {
            Boost.Deactivate();
            SetTargetSpeed(speedRecommended);
        }
    }


    protected override float TurnSpeed()
    {
        return ShipParameters.TurnSpeed * BuffData.TurnCoef;
    }

    public override float MaxSpeed()
    {
        return ShipParameters.MaxSpeed * BuffData.SpeedCoef;
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

    public void AddEnemy(ShipBase enemy, bool isEnemy)
    {
        var enemyInfo = new ShipPersonalInfo(this, enemy);
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

    private void DeathEffects()
    {
        if (ShipEngineStop != null)
        {
            ShipEngineStop.Play();
        }

        if (PeriodDamageEffect != null)
        {
            PeriodDamageEffect.Play();
        }
    }

    public void SetEnemyData(Vector3 dirFromAtoB, float dist, ShipBase mover)
    {
        Enemies[mover].SetParams(dirFromAtoB, dist);
    }

    public void SetAllyData(Vector3 dirFromAtoB, float dist, ShipBase mover)
    {
        Allies[mover].SetParams(dirFromAtoB, dist);
    }

    private float _nextPosibleHitClip;
    public void GetHit(IWeapon weapon, Bullet bullet)
    {
        if (_nextPosibleHitClip < Time.time)
        {
            _nextPosibleHitClip = Time.time + 0.1f;
            var hitClip = DataBaseController.Instance.AudioDataBase.GetHit();
            Audio.PlayOneShot(hitClip);
        }
        //        Debug.LogError($"playt hit {hitClip.name}");
        if (ShipParameters.ShieldParameters.ShiledIsActive)
        {
            if (bullet.AffectTypeHit == BulletAffectType.damage)
            {
                var effect = EffectController.Instance.Create(DataBaseController.Instance.SpellDataBase.ShieldHitEffect,
                    transform, 3f);
                effect.transform.position = bullet.Position;
                var posToLookAt = bullet.Position - bullet.LookDirection * 3;
                Debug.DrawLine(posToLookAt, effect.transform.position, Color.red, 10);
                effect.transform.LookAt(posToLookAt, Vector3.up);
            }
        }
        DeathData.LastBullet(bullet);
        weapon.ApplyToShip(ShipParameters, this, bullet);
    }
    public bool HaveClosestDamagedFriend(out ShipBase ship)
    {
        return DesicionData.HaveClosestDamagedFriend(out ship);
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
//        var asteroids = Cell.GetAsteroidsForShip(this);
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