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
    protected virtual float BodyDamage => 6f;
    protected virtual float ShieldDamage => 3f;

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
        _rotateSpeed = MyExtensions.Random(0.05f, 0.2f);
        var xx = MyExtensions.Random(0f, 1f);
        var yy = MyExtensions.Random(0f, 1f);
        var zz = MyExtensions.Random(0f, 1f);
        _rotateDir = new Vector3(xx, yy, zz);
        Source.volume = .23f;
    }
    public virtual void Init(AIAsteroidPredata aiAsteroidPredata)
    {
        _aiAsteroidPredata = aiAsteroidPredata;
        _aiAsteroidPredata.OnMove += OnMove;
        _aiAsteroidPredata.OnDeath += OnDeathLogic;
        Rad = aiAsteroidPredata.Rad / AIAsteroidPredata.SHIP_SIZE_COEF;

    }

    void OnTriggerEnter(Collider other)
    {
        //        Debug.LogError($"Asteroid DEAD {gameObject.name}. {transform.position}. {other.gameObject.name}");
        OnHit(other);
    }

    private void OnHit(Collider other)
    {
        if (other.CompareTag(TagController.OBJECT_MOVER_TAG))
        {
            var ship = other.GetComponent<ShipHitCatcher>();

            if (ship != null)
            {
                if (ship.ShipBase.Boost.BoostRam.IsActive)
                {
                    var dir = _aiAsteroidPredata.Position - ship.ShipBase.Position;
                    _aiAsteroidPredata.Push(dir, 20);
                }
                else
                {
                    ship.ShipBase.ShipParameters.Damage(ShieldDamage, BodyDamage, null, null);
                    _aiAsteroidPredata.Death();
                }

            }
        }
        if (other.CompareTag(TagController.BULLET_TAG))
        {
            var bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.Death();
                _aiAsteroidPredata.Death();
            }
        }
    }


    void Update()
    {
        if (Time.timeScale < 0.001f)
        {
            return;
        }
        RotateUpdate();
        _aiAsteroidPredata.UpdateFromUnity();
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
        _aiAsteroidPredata.OnMove -= OnMove;
        _aiAsteroidPredata.OnDeath -= OnDeathLogic;
    }

    protected void subDeath()
    {
        PlayEffect();
        InitAsteroidsPart();
        gameObject.transform.position = new Vector3(9999, 9999, 9999);
    }

    private void PlayEffect()
    {
        EffectOnHit.gameObject.SetActive(true);
        EffectController.Instance.LeaveEffect(EffectOnHit, transform, 1.5f);
        EffectOnHit.Play();
    }
    private void InitAsteroidsPart()
    {
        var pool = DataBaseController.Instance.Pool;
        int cnt = MyExtensions.Random(3, 5);
        var p = gameObject.transform.position;
        float asteroidPartOffset = .3f;
        float quaterRnd = 1f;
        for (int i = 0; i < cnt; i++)
        {
            var asteroidPart = pool.GetPartAsteroid();
            asteroidPart.Init();
            var xx = p.x + MyExtensions.Random(-asteroidPartOffset, asteroidPartOffset);
            var zz = p.z + MyExtensions.Random(-asteroidPartOffset, asteroidPartOffset);

            var xxQ = MyExtensions.Random(-quaterRnd, quaterRnd);
            var yyQ = MyExtensions.Random(-quaterRnd, quaterRnd);
            var zzQ = MyExtensions.Random(-quaterRnd, quaterRnd);
//            var wwQ = MyExtensions.Random(-quaterRnd, quaterRnd);
            asteroidPart.transform.position = new Vector3(xx, p.y, zz);
            asteroidPart.transform.rotation = new Quaternion(xxQ, yyQ, zzQ, 1f);
        }
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


    private void OnDeathLogic()
    {
        Death(true);
    }

    private void OnMove(AIAsteroidPredata data, Vector3 obj)
    {
        transform.position = obj;
    }
}

