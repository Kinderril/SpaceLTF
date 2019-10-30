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
    private AIAsteroidPredata _aiAsteroidPredata;
    public AudioSource Source;

    private float _rad;
    public Vector3 Position { get; set; }
    protected virtual float BodyDamage => 3f; 
    protected virtual float ShieldDamage => 1f; 

    public float Rad
    {
        get { return _rad; }
        set
        {
            transform.localScale = new Vector3(value, value, value);
            _rad = value;
        }
    }

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
//        Debug.LogError($"Asteroid DEAD {gameObject.name}. {transform.position}. {other.gameObject.name}");
        OnBulletHit(other);
    }
    
    private void OnBulletHit(Collider other)
    {
        if (other.CompareTag(TagController.OBJECT_MOVER_TAG))
        {
            var ship = other.GetComponent<ShipHitCatcher>();

            if (ship != null)
            {
                ship.ShipBase.ShipParameters.Damage(ShieldDamage, BodyDamage, null,null);
                Death(true);
            }
        }
        if (other.CompareTag(TagController.BULLET_TAG))
        {
            var bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.Death();
                Death(true);
            }
        }
    }


    void Update()
    {
        RotateUpdate();
    }

    protected void RotateUpdate()
    {
        if (_wannaRotate)
            transform.Rotate(_rotateDir, _rotateSpeed);
    }

    protected virtual void Death(bool withSound)
    {
        if (withSound)
            PlaySound();
        subDeath();
        _aiAsteroidPredata.Death();
    }

    protected void subDeath()
    {
        EffectOnHit.gameObject.SetActive(true);
        EffectController.Instance.LeaveEffect(EffectOnHit, transform, 1.5f);
        EffectOnHit.Play();
        gameObject.transform.position = new Vector3(9999, 9999, 9999);
    }

    protected void PlaySound()
    {

        if (!Source.enabled)
        {
            Source.enabled = true;
        }
        Source.Play();
    }

    public void InitRad()
    {
         var midVal = (transform.localScale.x + transform.localScale.z);
         Rad = midVal;
    }

    public void Init(AIAsteroidPredata aiAsteroidPredata)
    {
        _aiAsteroidPredata = aiAsteroidPredata;
       Rad = aiAsteroidPredata.Rad / AIAsteroidPredata.SHIP_SIZE_COEF;

    }
}

