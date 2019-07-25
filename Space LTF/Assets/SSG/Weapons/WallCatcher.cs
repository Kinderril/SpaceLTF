using UnityEngine;


public class WallCatcher : HitCatcher
{
    private float _endTime;

    public override void GetHit(IWeapon weapon,Bullet bullet)
    {
        bullet.Death();
    }

    public void Init(float rad)
    {
        _endTime = Time.time + 10f;
        transform.localScale = Vector3.one * rad;
    }

    void Update()
    {
        if (_endTime < Time.time)
        {
            GameObject.Destroy(gameObject);
        }

    }
}