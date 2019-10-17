using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class FlyingAsteroid : Asteroid
{
    private Vector3 _dir;
    public float MinSpeed;
    public float MaxSpeed;

    public void Init()
    {
        var speed = MyExtensions.Random(MinSpeed, MaxSpeed);
        var xx = MyExtensions.Random(-1f, 1f);
        var zz = MyExtensions.Random(-1f, 1f);
        var v = new Vector3(xx, 00, zz);
        _dir = Utils.NormalizeFastSelf(v) * speed;
    }

    void Update()
    {
        RotateUpdate();
        MoveAsteroid();
    }

    private void MoveAsteroid()
    {
        transform.position = transform.position + _dir * Time.time;
    }
}
