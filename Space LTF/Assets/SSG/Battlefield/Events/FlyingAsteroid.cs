using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FlyingAsteroid : Asteroid
{
    private Vector3 _dir;
    private Vector3 _startPoint;
    private float MinSpeed = 1f;
    private float MaxSpeed = 3f;
    private Action<FlyingAsteroid> _callbackDeath;
    private float _sDistToDestroy;


    public  void Init(Action<FlyingAsteroid> callbackDeath, Vector3 center, Vector3 rad, float distToDestroy)
    {
        base.Init(new AIAsteroidPredata(center));
        _sDistToDestroy = distToDestroy * distToDestroy;
        _callbackDeath = callbackDeath;
        var speed = MyExtensions.Random(MinSpeed, MaxSpeed);
        _dir = rad * speed;
        transform.position = center;
        _startPoint = center;
    }

    protected override float BodyDamage => 4f;
    protected override float ShieldDamage => 3f;

    void Update()
    {
        if (Time.timeScale <= 0.001f)
        {
            return;
        }
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
        transform.position = transform.position + _dir * Time.deltaTime;
    }

    protected override void Death(bool withSound)
    {
        if (withSound)
            PlaySound();
        _callbackDeath(this);
        subDeath();
    }
}
