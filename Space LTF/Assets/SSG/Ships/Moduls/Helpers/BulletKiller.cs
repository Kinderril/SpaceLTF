using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BulletKiller : MonoBehaviour
{
//    private Vector3 _from ;
//    private float _period;
//    private float _startTime;
    private float _endTime;
    private Bullet _bulletTarget;
    public BaseEffectAbsorber HitEffect;
    public BaseEffectAbsorber ProcessEvent;
    private ShipBase _lauchShip;
    private bool _killing;

    public void Init(ShipBase from, Bullet bulletTarget, float delta)
    {
//        Debug.LogError($"Kill bullet {Time.time}");
        _lauchShip = from;
        _killing = true;
        //        _from = from.Position;
        _bulletTarget = bulletTarget;
//        _period = delta;
        _endTime = Time.time + delta;
#if UNITY_EDITOR
        if (_lauchShip == null)
        {
            Debug.LogError("_lauchShip == null)");
        }    
        if (_bulletTarget == null)
        {
            Debug.LogError("_bulletTarget == null)");
        }   
        if (ProcessEvent == null)
        {
            Debug.LogError("ProcessEvent == null)");
        }
#endif

        ProcessEvent.Play(_lauchShip.transform.position, _bulletTarget.transform.position);
    }

    void Update()
    {
        if (_killing)
        {
            if (!_bulletTarget.IsUsing)
            {
//                Debug.LogError($"Bullet dead {Time.time}");
                GameObject.Destroy(gameObject);
                _killing = false;
                return;
            }

            ProcessEvent.UpdatePositions(_lauchShip.transform.position, _bulletTarget.transform.position);
            if (_endTime < Time.time)
            {
                KillBullet();
            }
        }

//        var p = (Time.time - _startTime) /_period;
//        if (p > 1)
//        {
//            KillBullet();
//        }
//        else
//        {
//            transform.position = Vector3.Lerp(_from, _bulletTarget.transform.position, p);
//        }
    }

    private void KillBullet()
    {
        _killing = false;
//        Debug.LogError($"Kill COMPLETE {Time.time}");
        if (_bulletTarget.gameObject.activeSelf)
        {
            _bulletTarget.Death();
        }
        float delay = 3.8f;
        if (HitEffect != null)
        {
//            _hitted = true;
            HitEffect.transform.position = transform.position;
            EffectController.Instance.LeaveEffect(HitEffect, transform, delay);
            HitEffect.Play();
        }
        ProcessEvent.Stop();

        _lauchShip = null;
        GameObject.Destroy(gameObject);
//        EndUse(delay);
    }
}
