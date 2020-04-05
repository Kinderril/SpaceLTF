using System;
using UnityEngine;
using System.Collections;

public class ShipPursuitCameraHolder : MonoBehaviour
{
    private ShipBase _ship;
    private bool _haveShip;
    private Action DeathCallback;
    private Vector3 _prevLookAt;

    public void Init(ShipBase ship,Vector3 startPosition,Action deathCallback)
    {
        DeathCallback = deathCallback;
        transform.position = startPosition;
        _haveShip = true;
        _ship = ship;
        _prevLookAt = ship.PredictionPos();
        _ship.OnDeath += OnDispose;
        _ship.OnDispose += OnDispose;
    }

    private void OnDispose(ShipBase obj)
    {
        DeathCallback?.Invoke();
        Dispose();
    }

    public void Dispose()
    {
        if (_ship != null)
        {
            _ship.OnDeath -= OnDispose;
            _ship.OnDispose -= OnDispose;
            _ship = null;
        }

        _haveShip = false;
    }

    void Update()
    {
        if (_haveShip)
        {
            var trgPos = GetTargetPos();
            _prevLookAt = Vector3.Lerp(_prevLookAt,_ship.PredictionPos(),  .05f);
            transform.LookAt(_prevLookAt, Vector3.up);
            var posTo = Vector3.Lerp(transform.position, trgPos, .05f);
            transform.position = posTo;
        }
    }

    private Vector3 GetTargetPos()
    {
        var pos = _ship.Position - _ship.LookDirection * 5 + Vector3.up * 2;
        return pos;
    }
}
