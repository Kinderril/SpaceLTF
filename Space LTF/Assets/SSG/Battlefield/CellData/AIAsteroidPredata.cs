using System;
using System.Collections.Generic;
using UnityEngine;

public class Force
{
    public Vector3 Dir;
    public float Power;
}

public class AIAsteroidPredata
{
    private const float friction = 0.65f;
    private const float SIZE_DELTA = 0.5f;
    private float _curFriction = friction;

    private List<Force> _forces = new List<Force>();

    private bool _shallMove => _forces.Count > 0;

    public Vector3 Position;
    public float Rad;
    public event Action OnDeath;
    public event Action<AIAsteroidPredata, Vector3> OnMove;
    public const float SHIP_SIZE_COEF = 3f;
    private float _sizeCoef = 1f;

    public AIAsteroidPredata(Vector3 ateroidPos)
    {
        this.Position = ateroidPos;
        _sizeCoef = MyExtensions.Random(1f - SIZE_DELTA, 1f + SIZE_DELTA);
        _curFriction = friction * (_sizeCoef / 2f);
        Rad = _sizeCoef * SHIP_SIZE_COEF;
    }

    public void Death()
    {
        OnMove = null;
        if (OnDeath != null)
        {
            OnDeath();
        }

    }

    public void Push(Vector3 dir, float power)
    {
        dir.y = 0f;
        var coefPower = power / _sizeCoef;
        var force = new Force()
        {
            Dir = Utils.NormalizeFastSelf(dir),
            Power = coefPower
        };
        _forces.Add(force);
    }

    public void MoveTo(Vector3 pos)
    {
        Position = pos;
        OnMove?.Invoke(this, pos);
    }

    public void UpdateFromUnity()
    {
        if (!_shallMove)
        {
            return;
        }

        var fDelta = (1f - _curFriction) * Time.deltaTime;
        var curFriection = 1f - fDelta;
        //Recalc forces
        float sum = 0f;
        foreach (var force in _forces)
        {
            var nextSpeed = force.Power * curFriection;
            force.Power = nextSpeed;
            sum += nextSpeed;
        }
        //Sum forces
        Vector3 vectorsSum = Vector3.zero;
        foreach (var force in _forces)
        {
            var percent = force.Power / sum;
            var partForce = force.Dir * percent * force.Power;
            vectorsSum = vectorsSum + partForce;
        }

        var dirToMove = vectorsSum * Time.deltaTime;
        var nextPos = Position + dirToMove;
        MoveTo(nextPos);

        _forces.RemoveAll(x => x.Power < 0.1f);

    }
}