using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class FlyingAsteroid : Asteroid
{
    private Vector3 _dir;
    private Vector3 _startPoint;
    public float MinSpeed;
    public float MaxSpeed;
    private Action<FlyingAsteroid> _callbackDeath;
    private float _sDistToDestroy;


    public void Init(Action<FlyingAsteroid> callbackDeath,Vector3 center,Vector3 rad,float distToDestroy)
    {
        _sDistToDestroy = distToDestroy * distToDestroy;
        _callbackDeath = callbackDeath;
        var speed = MyExtensions.Random(MinSpeed, MaxSpeed);
        _dir = rad * speed;
        transform.position = center;
        _startPoint = center;
    }

    void Update()
    {
        RotateUpdate();
        MoveAsteroid();
        CheckDist();
    }

    private void CheckDist()
    {
        var spendSDist = (transform.position - _startPoint).sqrMagnitude;
        if (spendSDist > _sDistToDestroy)
        {
            Death(false);
        }
    }

    private void MoveAsteroid()
    {
        transform.position = transform.position + _dir * Time.time;
    }

    protected override void Death(bool withSound)
    {
        if (withSound)
            PlaySound();
        _callbackDeath(this);
        subDeath();
    }
}
