using UnityEngine;
using System.Collections;

public class ShakeTester : MonoBehaviour
{
    private ShipHitData _hitData;
    private float _nextShakeTime = 0;
    public ShipHitVisual ShipHitVisual;
    public float Period = 5f;
    public Easing.EaseType EaseType;

    void Awake()
    {
        _hitData = new ShipHitData();
        _hitData.Init(transform, EaseType);
    }

    void Update()
    {
        if (Time.time > _nextShakeTime)
        {
            _nextShakeTime = Time.time + Period;
            _hitData.HitTo(ShipHitVisual);
        }
        _hitData.Update();

    }
}
