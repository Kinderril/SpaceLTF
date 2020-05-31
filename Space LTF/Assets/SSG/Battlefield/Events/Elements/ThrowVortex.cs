using System;
using UnityEngine;
using System.Collections;

public class ThrowVortex : ObjectElementBattleEvent
{
    public SideTurn Side;
    private float _initTime;


    protected override void Init()
    {
        Side = SideTurn.left;
        _applyShip = true;
        _applyAsteroid = true;
        _applyBullet = true;
        _applyPart = true;
        _initTime = Time.time;
    }

    protected override void ExitEvent(ShipHitCatcher ship)
    {

    }

    protected override void CheckDestroyedPartEnter(Collider other)
    {
        if (other != null && other.attachedRigidbody != null)
        {
            var dir = GetSideToPush(other.transform.position);
            other.attachedRigidbody.AddForce(dir*20,ForceMode.Impulse);
        }
    }

    protected override void BulletEnterEvent(Bullet bullet)
    {
        ApplyBullet(bullet);
    }

    private void ApplyBullet(Bullet bullet)
    {

        if (bullet.DamageType == BulletDamageType.physical && MyExtensions.IsTrue01(.7f))
        {
            var dir = GetSideToPush(bullet.Position);
            bullet.ExternalForce.Init(4, 0.4f, dir);
            var sum = Utils.NormalizeFastSelf(dir + MyExtensions.Random(1.8f, 3.2f) * bullet.LookDirection);
            if (sum.sqrMagnitude > 0.01f)
            {
                bullet.Rotation = Quaternion.FromToRotation(Vector3.forward, sum);
            }
        }
    }

    protected override void BulletExitEvent(Bullet bullet)
    {
//        ApplyBullet(bullet);
    }

    protected override void AsteroidEnterEvent(Asteroid asteroid)
    {
        var dir = GetSideToPush(asteroid.Position);
        asteroid. AiAsteroidPredata.Push(dir,20);
    }
//    protected override void AsteroidExitEvent(Asteroid asteroid)
//    {
//
//    }

    protected override void EnterEvent(ShipHitCatcher ship)
    {
        var delta = Time.time - _initTime;
        if (delta < 2f)
        {
            return;
        }
        var dir = GetSideToPush(ship.ShipBase.Position);
        ship.ShipBase.ExternalForce.Init(15,1.2f,dir);
    }

    private Vector3 GetSideToPush(Vector3 objPos)
    {

        var dir = objPos - transform.position;
        dir.y = 0;
        switch (Side)
        {
            case SideTurn.left:
                dir = Utils.Rotate90(dir, SideTurn.right);
                break;
            case SideTurn.right:
                dir = Utils.Rotate90(dir, SideTurn.left);
                break;
        }

        return dir;
    }
}
