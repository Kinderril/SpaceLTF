using System;
using System.Collections.Generic;
using UnityEngine;

public class CastSpellData
{
    public BulleStartParameters Bullestartparameters;
    public int ShootsCount = 1;
    public int Power = 1;

}

[System.Serializable]
public delegate void EndCastDelegateSpell();

[System.Serializable]
public delegate void CastActionSpell(BulletTarget target, Bullet origin, IWeapon weapon,
    Vector3 shootpos, CastSpellData castData);

[System.Serializable]
public delegate Vector3 DistCounter(Vector3 maxDistPos, Vector3 targetDistPos);

[System.Serializable]
public delegate void SubUpdateShowCast(Vector3 pos, TeamIndex teamIndex, GameObject objectToShow);

[System.Serializable]
public delegate bool CanCastAtPoint(Vector3 pos);


[Serializable]
public class SpellDamageData
{
    public float _aoeRad;
    // public float AOERad;
    public bool IsAOE;

    public SpellDamageData()
    {
        IsAOE = false;
    }

    public SpellDamageData(float rad, bool isAOE = true)
    {
        _aoeRad = rad;
#if UNITY_EDITOR
        if (_aoeRad == 0)
        {
            Debug.LogError("");
        }
#endif
        IsAOE = isAOE;
    }

    public float AOERad
    {
        get => _aoeRad;
        set => _aoeRad = value;
    }
}

public class SpellInGame : IWeapon , IAffectable  , IAffectParameters
{
    public ShallCastToTaregtAI ShallCastToTaregtAIAction;
    public BulletDestroyDelegate BulletDestroyAction;
//    private CreateBulletDelegate _createBulletAction;
    private WeaponInventoryAffectTarget _affectAction;

    private BulleStartParameters _bulletStartParams;
    //    public BulleStartParameters BulletStartParams => _bulletStartParams;    
    public WeaponInventoryAffectTarget AffectAction => _affectAction;
    public CreateBulletDelegate CreateBulletAction => _spellData.CreateBulletAction;
    public UpdateCastDelegate UpdateCastProcess => _spellData.UpdateCast;

    public BulleStartParameters BulletStartParams =>
        new BulleStartParameters(BulletSpeed, _bulletStartParams.turnSpeed, AimRadius, AimRadius);

    private SpellZoneVisualCircle CircleActiveObj;
    private SpellZoneVisualCircle CircleObjectToShow;
    private SpellZoneVisualLine LineObjectToShow;


    //    private int RadiusCircle;
    public List<string> SupportDesc = new List<string>();
    private Func<Vector3> _modulPos;
    private Bullet _bulletOrigin;
    public Bullet BulletOrigin => _bulletOrigin;
//    private WeaponAffectionAdditionalParams _additionalParams = new WeaponAffectionAdditionalParams();
    private SpellDamageData _spellDamageData;

    private readonly ShipBase _owner;
    public TargetType TargetType => _spellData.AffectAction.TargetType;
    public TeamIndex TeamIndex { get; private set; }
    public Vector3 CurPosition => _modulPos();
    public ShipBase Owner => _owner;
    public int Level { get; private set; }
    // public int CostCount { get; private set; }
    public int CostPeriod { get; private set; }
    public CastActionSpell CastSpell { get; private set; }
    public SubUpdateShowCast SubUpdateShowCast { get; private set; }
    public CanCastAtPoint CanCastAtPoint { get; private set; }
    public float CurOwnerSpeed => 0.001f;
    public CurWeaponDamage CurrentDamage { get; set; }


    public float AimRadius { get; set; }
    public float SetorAngle { get; set; }
    public float BulletSpeed { get; set; }
    private float _reloadSec;


    public int ShootPerTime { get; set; } 


    public string Name { get; private set; }
    public string Desc { get; private set; }
    public SpellType SpellType { get; private set; }
    //    float ShowCircleRadius { get; }
    bool ShowLine { get; set; }
    //    public float MaxDist => _maxDist;
    //    private float _maxDist;
    public CanUseDelayedAction DelayedAction;
    public bool ShowCircle { get; private set; }
    public bool ShowActiveCircle { get; private set; }
    private DistCounter _distCounter;
    private ShipControlCenter _controlShip;
    private ISpellToGame _spellData;

    public SpellInGame(ISpellToGame spellData, Func<Vector3> modulPos,
        TeamIndex teamIndex, ShipBase owner, int level, string name, int period,
         SpellType spellType, string desc, 
        DistCounter distCounter, float delayPeriod,CurWeaponDamage damageAffection)
    {
        _reloadSec = 1f;
        ShootPerTime = 1;
        _spellData = spellData;
        CurrentDamage = damageAffection;
        if (spellData.BulleStartParameters.distanceShoot < 1)
        {
            Debug.LogError($"Shoot dist is vey low {spellData}  {spellData.BulleStartParameters.distanceShoot}  name:{name}");
        }
        _controlShip = owner as ShipControlCenter;
        if (_controlShip == null)
        {
            Debug.LogError($"SpellInGame have wrong owner");
        }
        DelayedAction = new CanUseDelayedAction(delayPeriod);
        _distCounter = distCounter;

        Desc = desc;
        AimRadius = spellData.BulleStartParameters.distanceShoot;
        BulletSpeed = spellData.BulleStartParameters.bulletSpeed;

//        Debug.LogError($"AIM rad start:{AimRadius.ToString("0.00")} _ {name}");
        //        ShowCircleRadius = spellData.ShowCircle;
        ShowLine = spellData.ShowLine;
        Level = level;
        SpellType = spellType;
        _owner = owner;
        TeamIndex = teamIndex;
        _modulPos = modulPos;
        Name = name;
        CostPeriod = period;
        _bulletOrigin = spellData.GetBulletPrefab();
        _spellDamageData = spellData.RadiusAOE;
        _bulletStartParams = spellData.BulleStartParameters;
        _affectAction = new WeaponInventoryAffectTarget(spellData.AffectAction.Main, spellData.AffectAction.TargetType);
        CastSpell = spellData.CastSpell;
        SubUpdateShowCast = spellData.SubUpdateShowCast;
        CanCastAtPoint = spellData.CanCastAtPoint;
        spellData.ResetBulletCreateAtion();
        ShallCastToTaregtAIAction = spellData.ShallCastToTaregtAIAction;
        BulletDestroyAction = spellData.BulletDestroyDelegate;
        Debug.Log($"Init spell:{_spellDamageData.AOERad}   spellData:{spellData}");
        ShowCircle = _spellDamageData.AOERad > 0;
        InitActiveCircle();

    }

    public float PowerInc()
    {
        return _spellData.PowerInc();
    }

    private void InitActiveCircle()
    {
        var prefab = DataBaseController.Instance.SpellDataBase.GetVisualInfo(SpellType);
        ShowActiveCircle = true;
        if (prefab == null)
        {
            ShowActiveCircle = false;
            ShowCircle = false;
            ShowLine = false;
            return;
        }

        if (prefab.Value.RadiusAttackEffect != null)
        {
            ShowActiveCircle = true;
            CircleActiveObj = DataBaseController.GetItem(prefab.Value.RadiusAttackEffect);
            CircleActiveObj.transform.SetParent(BattleController.Instance.OneBattleContainer);
            CircleActiveObj.SetRad(_spellDamageData.AOERad);
            CircleActiveObj.gameObject.SetActive(false);
        }
        else
        {
            ShowActiveCircle = false;
        }

        if (prefab.Value.SpellZoneCircle != null)
        {
            ShowCircle = true;
            CircleObjectToShow = DataBaseController.GetItem(prefab.Value.SpellZoneCircle);
            CircleObjectToShow.transform.SetParent(BattleController.Instance.OneBattleContainer);
            CircleObjectToShow.SetRad(_spellDamageData.AOERad);
            CircleObjectToShow.gameObject.SetActive(false);
        }
        else
        {
            ShowCircle = false;
        }

        if (prefab.Value.SpellZoneLine != null)
        {
            ShowLine = true;
            LineObjectToShow = DataBaseController.GetItem(prefab.Value.SpellZoneLine);
            LineObjectToShow.transform.SetParent(BattleController.Instance.OneBattleContainer);
        }
        else
        {
            ShowLine = false;
        }

    }

    public bool IsReady => DelayedAction.IsReady;

    public float ReloadSec
    {
        get { return _reloadSec; }
        set
        {
            _reloadSec = value;
            CostPeriod = (int)(CostPeriod * _reloadSec);
        }
    }

    public bool CanCast()
    {
        if (!DelayedAction.IsReady)
        {
            return false;
        }
        var canUse = _controlShip.CoinController.CanStartCast();
        if (!canUse)
        {
            return false;
        }
        return true;
    }

    public void UpdateActivePeriod(Vector3 pos)
    {
        if (ShowActiveCircle)
        {
            CircleActiveObj.transform.position = pos;
            var rad = _spellData.RadiusAOE.AOERad;
            rad = Mathf.Clamp(rad, 1, 100);
            CircleActiveObj.SetRad(rad);
            CircleActiveObj.gameObject.SetActive(true);
        }

        if (ShowLine)
        {
            if (!LineObjectToShow.gameObject.activeSelf)
            {
                LineObjectToShow.gameObject.SetActive(true);
            }
            var dir = (pos - _modulPos());
            LineObjectToShow.SetDirection(_modulPos(), _modulPos() + dir, AimRadius);
        }
        UpdateCastProcess(pos, getBulletTarget(pos), _bulletOrigin,
            this, _modulPos(), getCastSpellData());
    }

    public void UpdateShowCast(Vector3 pos)
    {
        if (ShowCircle)
        {
            CircleObjectToShow.transform.position = pos;
            if (SubUpdateShowCast != null)
            {
                SubUpdateShowCast(pos, TeamIndex, CircleObjectToShow.gameObject);
            }
        }

        if (ShowLine)
        {
            var dir = (pos - _modulPos());
            LineObjectToShow.SetDirection(_modulPos(), _modulPos() + dir, AimRadius);
        }
    }

    public void StartShowCast()
    {
        _owner.Audio.PlayOneShot(DataBaseController.Instance.AudioDataBase.SelectSpell);
        if (ShowLine)
        {
            LineObjectToShow.gameObject.SetActive(true);
        }

        if (ShowCircle)
        {
            CircleObjectToShow.SetRad(_spellDamageData.AOERad);
            CircleObjectToShow.gameObject.SetActive(true);
        }
    }


    public void EndShowCast()
    {
        if (ShowActiveCircle)
        {
            CircleActiveObj.gameObject.SetActive(false);
        }
        if (ShowCircle)
        {
            CircleObjectToShow.gameObject.SetActive(false);
        }

        if (ShowLine)
            LineObjectToShow.gameObject.SetActive(false);
    }

    public void Cast(Vector3 target)
    {
        _owner.Audio.PlayOneShot(DataBaseController.Instance.AudioDataBase.GetCastSpell(SpellType));
        var startPos = _modulPos();
        DelayedAction.Use();
        // Debug.LogError($"Cast rad start:{AimRadius.ToString("0.00")}");
 
        CastSpell(getBulletTarget(target), _bulletOrigin, this, startPos, getCastSpellData());
        CircleActiveObj.gameObject.SetActive(true);
        //        CastSpell(new BulletTarget(target), _bulletOrigin, this, startPos, _bulletStartParams);
    }

    private BulletTarget getBulletTarget(Vector3 target)
    {
        var startPos = _modulPos();
        var dir = Utils.NormalizeFastSelf(target - startPos);
        var maxDistPos = startPos + dir * AimRadius;
        var distCal = _distCounter(maxDistPos, target);
        var bulletTarget = new BulletTarget(distCal);
        return bulletTarget;
    }

    private CastSpellData getCastSpellData()
    {
        var castData = new CastSpellData()
        {
            Bullestartparameters = BulletStartParams,
            ShootsCount = ShootPerTime,
        };
        return castData;
    }                   

    public void BulletCreateByDir(ShipBase target, Vector3 dir)
    {
        CreateBulletWithModif(dir);
    }

    protected void CreateBulletWithModif(ShipBase target)
    {
        CreateBulletAction(new BulletTarget(target), _bulletOrigin, this, _modulPos(), BulletStartParams );
    }

    protected void CreateBulletWithModif(Vector3 target)
    {
        CreateBulletAction(new BulletTarget(target), _bulletOrigin, this, _modulPos(), BulletStartParams);
    }

    public void DamageDoneCallback(float healthdelta, float shielddelta, ShipBase damageAppliyer)
    {
        //        GlobalEventDispatcher.ShipDamage(Owner, healthdelta, shielddelta, _weaponType);
        var coef = damageAppliyer != null ? damageAppliyer.ExpCoef : 0f;
        Owner.ShipInventory.LastBattleData.AddDamage(healthdelta, shielddelta, coef);
        if (damageAppliyer != null)
        {
#if UNITY_EDITOR
            if (damageAppliyer.Id == Owner.Id)
                Debug.LogError(
                    $"Strange things. I wanna kill my self??? {Owner.Id}_{Owner.name}  side:{Owner.TeamIndex}  spell:{Name}");
#endif
            if (damageAppliyer.IsDead)
            {
                GlobalEventDispatcher.ShipDeath(damageAppliyer, Owner);
                Owner.ShipInventory.LastBattleData.AddKill();
            }
        }
    }

    public void BulletDestroyed(Vector3 position, Bullet bullet)
    {
        if (_spellDamageData.IsAOE)
        {
            var shipsToHitIndex = BattleController.OppositeIndex(TeamIndex);
            var shipsInRad = BattleController.Instance.GetAllShipsInRadius(position, shipsToHitIndex, _spellDamageData.AOERad);
#if UNITY_EDITOR
            DrawUtils.DebugCircle(position, Vector3.up, Color.cyan, _spellDamageData.AOERad, 3f);
#endif
            foreach (var shipBase in shipsInRad)
            {
                ApplyToShipSub(shipBase.ShipParameters, shipBase, bullet);
            }
        }

        if (BulletDestroyAction != null)
        {
            var cell = BattleController.Instance.CellController.GetCell(bullet.Position);
            BulletDestroyAction(bullet, this, cell);
        }
    }

    public void ApplyToShip(ShipParameters shipParameters, ShipBase shipBase, Bullet bullet)
    {
        if (!_spellDamageData.IsAOE)
        {
            ApplyToShipSub(shipParameters, shipBase, bullet);
        }
    }

    public void ApplyToShipSub(ShipParameters shipParameters, ShipBase shipBase, Bullet bullet)
    {
        var addInfo = new WeaponAffectionAdditionalParams()
        {
            AimRadius = AimRadius,
            BulletSpeed = BulletSpeed,
            ReloadSec = ReloadSec,
            SetorAngle = SetorAngle,
            ShootPerTime = ShootPerTime,
            CurrentDamage = CurrentDamage
        };

        foreach (var targetDelegate in AffectAction.Additional)
        {
            targetDelegate(shipParameters, shipBase, bullet, DamageDoneCallback, addInfo);
        }
        AffectAction.Main(shipParameters, shipBase, bullet, DamageDoneCallback, addInfo);
    }


    public bool TryCast(Vector3 trg)
    {
        if (CanCast())
        {
            if (CanCastAtPoint(trg))
            {
                // _controlShip.CoinController.UseCoins(CostCount, CostPeriod);
                Cast(trg);
                return true;
            }
        }

        return false;
    }


    public void SetBulletCreateAction(CreateBulletDelegate bulletCreate)
    {
        _spellData.SetBulletCreateAction(bulletCreate);
    }

    public void Dispose()
    {
        _spellData.DisposeAfterBattle();
        _spellData = null;
    }

    public void AddInfoForTooltip(string descSupport)
    {
        SupportDesc.Add(descSupport);
    }

    public void EndCastPeriod()
    {
        _spellData.EndCastPeriod();
        if (ShowCircle)
            CircleObjectToShow.gameObject.SetActive(false);
        if (ShowActiveCircle)
            CircleActiveObj.gameObject.SetActive(false);
        if (ShowLine)
            LineObjectToShow.gameObject.SetActive(false);
    }
}
