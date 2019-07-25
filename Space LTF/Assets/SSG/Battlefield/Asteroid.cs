using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Asteroid : MonoBehaviour
{
    public BaseEffectAbsorber EffectOnHit;
    private bool _wannaRotate = false;
    private float _rotateSpeed;
    private Vector3 _rotateDir;

    void Awake()
    {
        EffectOnHit.gameObject.SetActive(false);
        _wannaRotate = true;
        _rotateSpeed = MyExtensions.Random(0.1f, 0.6f);
        var xx = MyExtensions.Random(0f, 1f);
        var yy = MyExtensions.Random(0f, 1f);
        var zz = MyExtensions.Random(0f, 1f);
        _rotateDir = new Vector3(xx,yy,zz);
    }

    void OnTriggerEnter(Collider other)
    {
        OnBulletHit(other);
    }
    
    private void OnBulletHit(Collider other)
    {
        if (other.CompareTag(TagController.OBJECT_MOVER_TAG))
        {
            var ship = other.GetComponent<ShipHitCatcher>();

            if (ship != null)
            {
                ship.ShipBase.ShipParameters.Damage(1,3,null,null);
                Death();
            }
        }
        if (other.CompareTag(TagController.BULLET_TAG))
        {
            var bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.Death();
                Death();
            }
        }
    }

    void Update()
    {
        if (_wannaRotate)
            transform.Rotate(_rotateDir, _rotateSpeed);
    }

    private void Death()
    {
        EffectOnHit.gameObject.SetActive(true);
        EffectController.Instance.LeaveEffect(EffectOnHit, transform, 5f);
        EffectOnHit.Play();
        gameObject.transform.position = new Vector3(9999, 9999, 9999);
    }
}

