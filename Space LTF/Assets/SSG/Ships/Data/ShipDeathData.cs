using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class ShipDeathData : ShipData
{
    private Action _initDeathPartsAction;
    private float _lastBulletSpeed;
    private float _endTime;
    private Vector3 _lastBulletDir;
    private Vector3 _dropVector;
    private Quaternion _rotationDelta;
    private bool _isDeathStart;
    private float rotationSpeed = 1.4f;
    private float moveSpeed = 3.6f;
    private Action _enableFire;
    public ShipDeathData(ShipBase owner,Action initDeathPartsAction,Action enableFire) : base(owner)
    {
        _initDeathPartsAction = initDeathPartsAction;
        _enableFire = enableFire;
    }

    public void StartDeath()
    {
        var period = MyExtensions.Random(1f, 3f);
        _isDeathStart = true;
        var speedVector = _lastBulletDir * _lastBulletSpeed + _owner.LookDirection * Mathf.Clamp(_owner.CurSpeed,0.01f,99f);
        var nrm = Utils.NormalizeFastSelf(speedVector);
        _dropVector = new Vector3(nrm.x,MyExtensions.Random(-1f,0f), nrm.z);
        _dropVector *= moveSpeed;
        _endTime = period + Time.time;
        var xx = MyExtensions.Random(-1f, 1f);
        var yy = MyExtensions.Random(-1f, 1f);
        var zz = MyExtensions.Random(-1f, 1f);
        var ww = MyExtensions.Random(-1f, 1f);
        _rotationDelta = new Quaternion(xx,yy,zz,ww);
        _enableFire();

    }

    public bool Update()
    {
        if (!_isDeathStart)
        {
            return false;
        }

        if (Time.time > _endTime)
        {
            ExplotionAndHide();
            _isDeathStart = false;
            return true;
        }

        var rtCoef = Time.deltaTime * rotationSpeed;
        var oldQ = _owner.transform.rotation;
        var q = new Quaternion(
            oldQ.x + _rotationDelta.x * rtCoef,
            oldQ.y + _rotationDelta.y * rtCoef,
            oldQ.z + _rotationDelta.z * rtCoef,
            oldQ.w + _rotationDelta.w * rtCoef);

        _owner.transform.rotation = q ;
        _owner.transform.position += _dropVector * Time.deltaTime;
        var coef = 0.95f * Time.deltaTime;
        _dropVector *= (1f- coef);
        return false;

    }
    

    public void LastBullet(Bullet bullet)
    {
        _lastBulletSpeed = bullet.CurSpeed;
        _lastBulletDir = bullet.LookDirection;
    }

    private void ExplotionAndHide()
    {
        _owner.gameObject.SetActive(false);
        _owner.ShipVisual.gameObject.SetActive(false);
        _initDeathPartsAction();
        EffectController.Instance.Create(DataBaseController.Instance.DataStructPrefabs.OnShipDeathEffect, _owner.transform.position, 5f);
    }
}

