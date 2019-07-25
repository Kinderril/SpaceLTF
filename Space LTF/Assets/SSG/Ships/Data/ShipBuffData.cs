using UnityEngine;
using UnityEditor;

public class ShipBuffData : ShipData
{
    private bool IsActive = false;
    private float EndActiveTime;
    private float _deltaMaxSpeed;
    private float _deltaTurnSpeed;
    private const float PERCENT_SPEED = .4f;
    private const float BUFF_TIME = 10f;

    public ShipBuffData(ShipBase owner) : base(owner)
    {

    }

    public void ManualUpdate()
    {
        if (IsActive)
        {
            if (EndActiveTime < Time.time)
            {
                IsActive = false;
                Delete();
            }
        }
    }
    public  void Apply()
    {
        _deltaMaxSpeed = PERCENT_SPEED * _owner.ShipParameters.MaxSpeed;
        _owner.ShipParameters.MaxSpeed += _deltaMaxSpeed;

        _deltaTurnSpeed = PERCENT_SPEED * _owner.ShipParameters.TurnSpeed;
        _owner.ShipParameters.TurnSpeed += _deltaTurnSpeed;
        
        EndActiveTime = BUFF_TIME + Time.time;
        IsActive = true;
    }

    public void Delete()
    {
        _owner.ShipParameters.MaxSpeed -= _deltaMaxSpeed;
        _owner.ShipParameters.TurnSpeed -= _deltaTurnSpeed;
    }

}