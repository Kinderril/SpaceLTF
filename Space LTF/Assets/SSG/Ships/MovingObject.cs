using System;
using UnityEngine;



public abstract class MovingObject : PoolElement
{
    const float MAX_ACCELERATION = 1.5f;
    const float ACCELERATION_POWER = 2f;
    public const float BANK_MAX = 0.7f;
    public const float BANK_SPEED = 0.45f;
    public float EulerY = 0f;
    public bool EngineWork = true;
    public Vector3 LookDirection = Vector3.forward;
    public Vector3 LookLeft = Vector3.forward;
    public Vector3 LookRight = Vector3.forward;
    protected float _acceleraion = 0f;
    protected float _curSpeed = 0f;
    protected float _targetAcceleration = 0f;
    protected float _targetPercentSpeed = 0f;

    public float TargetAcceleration => _targetAcceleration;

    public float TargetPercentSpeed => _targetPercentSpeed;

    private BankingData _banking = new BankingData(Vector3.forward, 0f);
    public YMoveRotation YMoveRotation = new YMoveRotation();

    public BankingData BankingData => _banking;

    public ExternalSideForce ExternalSideForce { get; protected set; }
    public ExternalForce ExternalForce { get; protected set; }
    public EngineStop EngineStop { get; protected set; }
    //    public ExternalForce ExternalForce { get; protected set; }
    //    public EngineStop EngineStop { get; protected set; }
    //#if UNITY_EDITOR
    public DebugMovingData DebugMovingData = new DebugMovingData();
    //#endif
    public Transform RotatableObject;
    private float _curBank = 0f;
    protected virtual float BankMax => BANK_MAX;
    protected virtual float BankSpeed => BANK_SPEED;

    public event Action<MovingObject, bool> OnStop;
    public event Action<MovingObject> OnMainDispose;

    public float CurSpeed
    {
        get { return _curSpeed; }
    }

    public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public Quaternion Rotation
    {
        get { return transform.rotation; }
        set
        {
            transform.rotation = value;
            EulerY = value.eulerAngles.y;
            var y = EulerY * Mathf.Deg2Rad;
            var v = new Vector3(Mathf.Sin(y), 0, Mathf.Cos(y));
            LookDirection = v;
            LookLeft = Utils.Rotate90(LookDirection, SideTurn.left);
            LookRight = Utils.Rotate90(LookDirection, SideTurn.right);
        }
    }

    protected abstract float TurnSpeed();
    public abstract float MaxSpeed();


    public override void Init()
    {
        ExternalForce = new ExternalForce();
        ExternalSideForce = new ExternalSideForce();
        EngineStop = new EngineStop(this, null);
        base.Init();
    }

    public void Approach(ShipBase target)
    {
        Approach(target.Position);
    }

    private void Approach(Vector3 target)
    {
        var dir = target - Position;
        ApplyRotation(dir, true);
    }

    public virtual float ApplyRotation(Vector3 dir, bool exactlyPoint)
    {
        if (EngineStop.IsCrash())
        {
            return 0f;
        }
#if UNITY_EDITOR
        if (DebugParamsController.EngineOff && this is ShipBase)
        {
            return 0f;
        }
#endif
        if (!EngineWork)
        {
            return 0f;
        }
        if (Time.timeScale < 0.0000001f)
        {
            return 0f;
        }
        if (ExternalSideForce.IsActive)
        {
            Vector3 lerpRes2 = ExternalSideForce.GetLerpPercent(this);
            Rotation = Quaternion.FromToRotation(Vector3.forward, lerpRes2);
            return 1f;
        }

        var quatern = ApplyRotationXZ(dir, LookDirection, LookLeft, TurnSpeed, DebugMovingData, Position, out var steps);
        BankingData.SetNewData(dir, steps);
        Rotation = quatern;
        return 1f;
    }

    public static Quaternion ApplyRotationXZ(Vector3 targetDir, Vector3 lookDir, Vector3 lookLeft,
        Func<float> TurnSpeed, DebugMovingData debugData, Vector3 position, out float steps)
    {
        Quaternion Rotation;
        var ang = Vector3.Angle(targetDir, lookDir);
        var turnSpeed = TurnSpeed();
        var angPerFrameTurn = (turnSpeed * Time.deltaTime);
        steps = ang / angPerFrameTurn;
        if (steps <= 1f) // && exactlyPoint)
        {
#if UNITY_EDITOR
            if (debugData != null)
                debugData.AddDir(lookDir, true, lookDir, position);
#endif
            Rotation = Quaternion.FromToRotation(Vector3.forward, targetDir);
            return Rotation;
        }
        Vector3 lerpRes;
        var isLeft = Vector3.Dot(targetDir, lookLeft) > 0;
        if (isLeft)
        {
            lerpRes = Utils.RotateOnAngUp(lookDir, angPerFrameTurn);
        }
        else
        {
            lerpRes = Utils.RotateOnAngUp(lookDir, -angPerFrameTurn);
        }
#if UNITY_EDITOR
        if (debugData != null)
            debugData.AddDir(lookDir, true, lookDir, position);
#endif                 
        var qRotation = Quaternion.FromToRotation(Vector3.forward, lerpRes);
        return qRotation;
    }


    private bool? _haveRotatingObject = null;

    private bool HaveRotatingObject
    {
        get
        {
            if (_haveRotatingObject.HasValue)
            {
                return _haveRotatingObject.Value;
            }
            _haveRotatingObject = RotatableObject != null;
            return _haveRotatingObject.Value;
        }
    }


    private void Banking()
    {
#if UNITY_EDITOR
        if (DebugParamsController.EngineOff && this is ShipBase)
        {
            return;
        }
#endif  
        if (!HaveRotatingObject)
        {
            return;
        }

        var bSpeed = BankSpeed * Time.deltaTime;
        float nextBank;
        if (!_banking.ImplementedXZ)
        {
            Vector3 dir = _banking.TargetDir;
            float steps = _banking.Steps;
            _banking.CompleteXZ();
            var crossProduct = Vector3.Cross(LookDirection, dir);
            var isRight = crossProduct.y < 0;
            if (steps > 0)
            {
                if (isRight)
                {
                    nextBank = _curBank + bSpeed;
                    if (_curBank >= BankMax)
                    {
                        nextBank = BankMax;
                    }
                }
                else
                {
                    nextBank = _curBank - bSpeed;
                    if (_curBank <= -BankMax)
                    {
                        nextBank = -BankMax;
                    }
                }

            }
            else
            {
                //Returning to base state
                nextBank = BankingToZero(_curBank, bSpeed);
            }
        }
        else
        {
            nextBank = BankingToZero(_curBank, bSpeed);
        }

        _curBank = _curBank * 0.3f + nextBank * 0.7f;
        var rotationToImplement = YMoveRotation.RotateQuaternion;
        var bank = _curBank + rotationToImplement.z;
        var bankRotation = new Quaternion(rotationToImplement.x, 0, bank, rotationToImplement.w);
        RotatableObject.localRotation = bankRotation;
    }

    private float BankingToZero(float cur, float bSpeed)
    {
        if (cur > 0)
        {
            cur -= bSpeed;
            if (cur <= 0f)
            {
                cur = 0;
            }
        }
        else
        {
            cur += bSpeed;
            if (cur >= 0f)
            {
                cur = 0;
            }
        }

        return cur;
    }

    protected virtual void EngineUpdate()
    {
        ApplyAcceleration();
        Banking();
        _curSpeed = Mathf.Clamp(_curSpeed + _acceleraion * Time.deltaTime, 0f, MaxSpeed());
    }

    private void ApplyAcceleration()
    {
        var maxSpeed = MaxSpeed();
        var trgSpeed = _targetPercentSpeed * maxSpeed;
        var d = 1f;
        if (EngineStop.IsCrash())
        {
            _acceleraion = -ACCELERATION_POWER * d;
        }
        else
        {
            if (CurSpeed < trgSpeed && Mathf.Abs(trgSpeed - CurSpeed) > 0.03f)
            {
                _acceleraion = ACCELERATION_POWER * d;
            }
            else
            {
                _acceleraion = -ACCELERATION_POWER * d;
            }
        }

    }

    public void SetTargetSpeed(float percent)
    {
        _targetPercentSpeed = Mathf.Clamp(percent, -1f, 1f);
    }

    void OnDestroy()
    {
        OnDestroyAction();
    }

    protected virtual void OnDestroyAction()
    {

    }

    void OnDrawGizmosSelected()
    {
        DrawGizmosSelected();
    }

    protected virtual void DrawGizmosSelected()
    {

    }
    protected virtual void DrawGizmos()
    {

    }

    void OnDrawGizmos()
    {
        DrawGizmos();
    }

    protected void ApplyMove(Vector3 additionalMove, bool useAdditive)
    {
#if UNITY_EDITOR
        if (DebugParamsController.EngineOff && this is ShipBase)
        {
            return;
        }
#endif
        if (EngineWork)
        {
            Vector3 dir = CurSpeed * LookDirection * Time.deltaTime;
            Vector3 externalForce = ExternalForce.IsActive ? ExternalForce.Update() : Vector3.zero;
            float coef = YMoveRotation.XzMoveCoef;
            var deltaDir = useAdditive ? additionalMove : dir;
            Position = Position + (deltaDir+ externalForce) * coef;
        }
    }
    protected void ApplyMove()
    {
        ApplyMove(Vector3.zero, false);
    }

    public virtual void Dispose()
    {
        OnMainDispose?.Invoke(this);
    }
}

