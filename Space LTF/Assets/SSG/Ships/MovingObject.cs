using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BankingData
{
    public Vector3 Dir;
    public float Steps;

    public BankingData(Vector3 dir, float steps)
    {
        Dir = dir;
        Steps = steps;
    }
}

public abstract class MovingObject : PoolElement
{
    const float MAX_ACCELERATION = 1.5f;
    const float ACCELERATION_POWER = 2f;
    const float BANK_MAX = 0.7f;
    const float BANK_SPEED = 0.9f;
    public float EulerY = 0f;
    public bool EngineWork = true;
    public Vector3 LookDirection = Vector3.forward;
    public Vector3 LookLeft = Vector3.forward;
    public Vector3 LookRight = Vector3.forward;
    protected float _acceleraion = 0f;
    protected float _curSpeed = 0f;
    protected float _targetAcceleration = 0f;
    protected float _targetPercentSpeed = 0f;

    public float TargetAcceleration
    {
        get { return _targetAcceleration; }
    }

    public float TargetPercentSpeed
    {
        get { return _targetPercentSpeed; }
    }

    private BankingData _banking = null;
    //    private bool _stopShip;
    //    private ExternalForce _externalForce;
    //    private ExternalSideForce _externalSideForce;
    //    private EngineStop _engineStop;

    public ExternalSideForce ExternalSideForce { get; protected set; }
    public ExternalForce ExternalForce { get; protected set; }
    public EngineStop EngineStop { get;protected set; }
    //    public ExternalForce ExternalForce { get; protected set; }
    //    public EngineStop EngineStop { get; protected set; }
#if UNITY_EDITOR
    public DebugMovingData DebugMovingData  = new  DebugMovingData();
#endif
    public Transform RotatableObject;
    private float _curBank = 0f;

    public event Action<MovingObject, bool> OnStop;

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
    protected abstract float MaxSpeed();


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
    
    public bool ApplyRotation(Vector3 dir, bool exactlyPoint)
    {
        if (EngineStop.IsCrash())
        {
            return false;
        }
#if UNITY_EDITOR
        if (DebugParamsController.EngineOff && this is ShipBase)
        {
            return false;
        }
#endif
        if (!EngineWork)
        {
            return false;
        }
        if (Time.deltaTime < 0.00001f)
        {
            return false;
        }
        if (ExternalSideForce.IsActive)
        {
            Vector3 lerpRes2 = ExternalSideForce.GetLerpPercent(this);
            Rotation = Quaternion.FromToRotation(Vector3.forward, lerpRes2);
            return true;
        }

        var ang = Vector3.Angle(dir, LookDirection);
        var turnSpeed = TurnSpeed();
        var angPerFrameTurn = (turnSpeed * Time.deltaTime);
        var steps = ang/angPerFrameTurn;
        if (steps <= 1f) // && exactlyPoint)
        {
#if UNITY_EDITOR
            DebugMovingData.AddDir(dir, true, LookDirection);
#endif
            Rotation = Quaternion.FromToRotation(Vector3.forward, dir);

            return true;
        }
//        var percentOfRotate = Mathf.Clamp01(1f / steps);
//        var lerpRes = EulerLerp.LerpVectorByY(LookDirection, dir, percentOfRotate);
        Vector3 lerpRes;
        var isLeft = Vector3.Dot(dir, LookLeft) > 0;
        if (isLeft)
        {
            lerpRes = Utils.RotateOnAngUp(LookDirection, angPerFrameTurn);
        }
        else
        {
            lerpRes = Utils.RotateOnAngUp(LookDirection, -angPerFrameTurn);
        }
        

        _banking = new BankingData(dir, steps);
#if UNITY_EDITOR
        DebugMovingData.AddDir(lerpRes, false, LookDirection);
#endif
        Rotation = Quaternion.FromToRotation(Vector3.forward, lerpRes);
        return false;
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

        var bSpeed = BANK_SPEED * Time.deltaTime;
        if (_banking != null)
        {
            Vector3 dir = _banking.Dir;
            float steps = _banking.Steps;
            _banking = null;
            var crossProduct = Vector3.Cross(LookDirection, dir);
            var isRight = crossProduct.y < 0;
            if (steps > 0)
            {
                if (isRight)
                {
                    _curBank += bSpeed;
                    if (_curBank >= BANK_MAX)
                    {
                        _curBank = BANK_MAX;
                    }
                }
                else
                {
                    _curBank -= bSpeed;
                    if (_curBank <= -BANK_MAX)
                    {
                        _curBank = -BANK_MAX;
                    }
                }

            }
            else
            {
                //Returning to base state
                _curBank = BankingToZero(_curBank, bSpeed);
            }
        }
        else
        {
            _curBank = BankingToZero(_curBank, bSpeed);
        }
        RotatableObject.localRotation = new Quaternion(0, 0, _curBank, 1f);
//        Debug.Log("rotation: " + _curBank + "   " + gameObject.name + "   " + RotatableObject.localRotation);

    }

    private float BankingToZero(float cur,float bSpeed)
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
            if (CurSpeed < trgSpeed && Mathf.Abs(trgSpeed-CurSpeed) > 0.03f)
            {
                _acceleraion = ACCELERATION_POWER * d;
            }
            else
            {
                _acceleraion = -ACCELERATION_POWER * d;
            }
        }

    }

    private void ApplyAccelerationOld()
    {
        float delta;
        if (_targetAcceleration > 0f)
        {

            delta = ACCELERATION_POWER;
        }
        else
        {
            delta = -ACCELERATION_POWER;
        }

//        delta = speedUp ? ACCELERATION_POWER : -ACCELERATION_POWER;
        var d = delta * Time.time;
        if (EngineStop.IsCrash())
        {
            _acceleraion = Mathf.Clamp(_acceleraion - d, -MAX_ACCELERATION / 2, MAX_ACCELERATION / 2);
        }
        else
        {
            _acceleraion = Mathf.Clamp(_acceleraion + d, -MAX_ACCELERATION, _targetAcceleration * MAX_ACCELERATION);
        }
    }

    public void SetTargetSpeed(float percent)
    {
        _targetPercentSpeed = percent;
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

    protected void ApplyMove()
    {
#if UNITY_EDITOR
        if (DebugParamsController.EngineOff && this is ShipBase)
        {
            return;
        }
#endif
        if (EngineWork)
        {
            Vector3 dir = CurSpeed * LookDirection.normalized * Time.deltaTime;
            if (ExternalForce.IsActive)
            {
                dir = dir + ExternalForce.Update();
            }
            Position = Position + dir ;
        }
    }
    
}

