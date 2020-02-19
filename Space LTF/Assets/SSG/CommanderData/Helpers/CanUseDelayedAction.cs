using UnityEngine;


public class CanUseDelayedAction
{
    private float _nextPosibleUse;

    public bool IsReady => _noDelay || _nextPosibleUse < Time.time;
    public float Delay { get; private set; }
    private bool _noDelay;

    public CanUseDelayedAction(float period)
    {
        Delay = period;
        _noDelay = Delay < 0f;
    }

    public void Use()
    {
        _nextPosibleUse = Time.time + Delay;
    }

    public float GetPercent()
    {
        if (_noDelay)
        {
            return 0f;
        }
        var p = (_nextPosibleUse - Time.time) / Delay;
        return p;
    }
}

