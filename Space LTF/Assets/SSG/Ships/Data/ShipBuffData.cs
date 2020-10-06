using UnityEngine;

//public class ConstantBuff
//{
//    public bool IsActive;
//    public float SPEED;
//    public float TURN_SPEED;
//
//}

public class ShipBuffData : ShipData
{
    private bool _timeBuffActive = false;
    private float EndActiveTime;
    private float _deltaMaxSpeed;
    private float _deltaTurnSpeed;
    private const float PERCENT_SPEED = .4f;
    private const float BUFF_TIME = 10f;

    private float turnCoef = 1f;
    private float speedCoef = 1f;
    private float _contsntPercent = 1f;
    private bool _constActive = false;

//    private ConstantBuff _constantBuff = new ConstantBuff();

    public ShipBuffData(ShipBase owner) : base(owner)
    {

    }

    public float SpeedCoef => speedCoef;
    public float TurnCoef => turnCoef;

    public void ManualUpdate()
    {
        if (_timeBuffActive)
        {
            if (EndActiveTime < Time.time)
            {
                _timeBuffActive = false;
                RemoveTimeBuff();
            }
        }
    }
    public void Apply(float buffTime)
    {
        if (_timeBuffActive)
        {
            RemoveTimeBuff();
        }

        turnCoef = turnCoef * PERCENT_SPEED;
        speedCoef = speedCoef * PERCENT_SPEED;
        EndActiveTime = buffTime + Time.time;
        _timeBuffActive = true;


    }

    public void AddConstantSpeed(float percent)
    {
//        Debug.LogError("AddConstantSpeed");
  
        DeactivateCostBuff();
      

        _contsntPercent = percent;
        turnCoef = turnCoef * _contsntPercent;
        speedCoef = speedCoef * _contsntPercent;
        _constActive = true;

    }

    public void DeactivateCostBuff()
    {
        if (!_constActive)
        {
            return;
        }
//        Debug.LogError("DeactivateCostBuff");
        turnCoef = turnCoef / _contsntPercent;
        speedCoef = speedCoef / _contsntPercent;
        _constActive = false;
    }

    public void RemoveTimeBuff()
    {
        turnCoef = turnCoef / PERCENT_SPEED;
        speedCoef = speedCoef / PERCENT_SPEED;
        _timeBuffActive = false;
    }

}