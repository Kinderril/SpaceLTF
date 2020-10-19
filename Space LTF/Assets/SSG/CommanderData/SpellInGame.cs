using System;
using UnityEngine;

public class CastSpellData
{
    public BulleStartParameters Bullestartparameters;
    public int ShootsCount = 1;

}

[System.Serializable]
public delegate void CastActionSpell(BulletTarget target, Bullet origin, IWeapon weapon,
    Vector3 shootpos, CastSpellData castData);

[System.Serializable]
public delegate Vector3 DistCounter(Vector3 maxDistPos, Vector3 targetDistPos);

[System.Serializable]
public delegate void SubUpdateShowCast(Vector3 pos, TeamIndex teamIndex, GameObject objectToShow);

[System.Serializable]
public delegate bool CanCastAtPoint(Vector3 pos);


public class SpellDamageData
{
    public float AOERad;
    public bool IsAOE;

    public SpellDamageData()
    {
        IsAOE = false;
    }

    public SpellDamageData(float rad, bool isAOE = true)
    {
        AOERad = rad;
        IsAOE = isAOE;
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

    private SpellZoneVisualCircle CircleObjectToShow;
    private SpellZoneVisualLine LineObjectToShow;


    //    private int RadiusCircle;
    private Func<Vector3> _modulPos;
    private Bullet _bulletOrigin;
    public Bullet BulletOrigin => _bulletOrigin;
//    private WeaponAffectionAdditionalParams _additionalParams = new WeaponAffectionAdditionalParams();
    private SpellDamageData _spellDamageData;

    private readonly ShipBase _owner;
    public TeamIndex TeamIndex { get; private set; }
    public Vector3 CurPosition => _modulPos();
    public ShipBase Owner => _owner;
    public int Level { get; private set; }
    public int CostCount { get; private set; }
    public int CostPeriod { get; private set; }
    public CastActionSpell CastSpell { get; private set; }
    public SubUpdateShowCast SubUpdateShowCast { get; private set; }
    public CanCastAtPoint CanCastAtPoint { get; private set; }
    public float CurOwnerSpeed => 0.001f;
    public CurWeaponDamage CurrentDamage { get; set; }


    public float AimRadius { get; set; }
    public float SetorAngle { get; set; }
    public float BulletSpeed { get; set; }
    public float ReloadSec { get; set; }
    public int ShootPerTime { get; set; } 


    public string Name { get; private set; }
    public string Desc { get; private set; }
    public SpellType SpellType { get; private set; }
    //    float ShowCircleRadius { get; }
    bool ShowLine { get; }
//    public float MaxDist => _maxDist;
//    private float _maxDist;
    public CanUseDelayedAction DelayedAction;
    public bool ShowCircle { get; private set; }
    private DistCounter _distCounter;
    private ShipControlCenter _controlShip;
    private ISpellToGame _spellData;

    public SpellInGame(ISpellToGame spellData, Func<Vector3> modulPos,
        TeamIndex teamIndex, ShipBase owner, int level, string name, int period,
        int count, SpellType spellType, string desc, 
        DistCounter distCounter, float delayPeriod,CurWeaponDamage damageAffection)
    {
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

        Debug.LogError($"AIM rad start:{AimRadius.ToString("0.00")} _ {name}");
        //        ShowCircleRadius = spellData.ShowCircle;
        ShowLine = spellData.ShowLine;
        Level = level;
        SpellType = spellType;
        _owner = owner;
        TeamIndex = teamIndex;
        _modulPos = modulPos;
        Name = name;
        CostPeriod = period;
        CostCount = count;
        _bulletOrigin = spellData.GetBulletPrefab();
        _spellDamageData = spellData.RadiusAOE();
        _bulletStartParams = spellData.BulleStartParameters;
        _affectAction = new WeaponInventoryAffectTarget(spellData.AffectAction.Main, spellData.AffectAction.TargetType);
        CastSpell = spellData.CastSpell;
        SubUpdateShowCast = spellData.SubUpdateShowCast;
        CanCastAtPoint = spellData.CanCastAtPoint;
        spellData.ResetBulletCreateAtion();
        //        _createBulletAction = spellData.CreateBulletAction;
        ///
        //        SetBulletCreateAction(_createBulletAction);
        ///
        ShallCastToTaregtAIAction = spellData.ShallCastToTaregtAIAction;
        BulletDestroyAction = spellData.BulletDestroyDelegate;
        ShowCircle = _spellDamageData.AOERad > 0;

    }

    public bool IsReady => DelayedAction.IsReady;
    public bool CanCast()
    {
        if (!DelayedAction.IsReady)
        {
            return false;
        }
        var canUse = _controlShip.CoinController.CanUseCoins(CostCount);
        if (!canUse)
        {
            return false;
        }
        return true;
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
            if (LineObjectToShow == null)
            {
                var p = DataBaseController.Instance.SpellDataBase.SpellZoneLine;
                LineObjectToShow = DataBaseController.GetItem(p);
                LineObjectToShow.transform.SetParent(BattleController.Instance.OneBattleContainer);
            }
            LineObjectToShow.gameObject.SetActive(true);
        }

        if (ShowCircle)
        {
            if (CircleObjectToShow == null)
            {
                CircleObjectToShow =
                    DataBaseController.GetItem(DataBaseController.Instance.SpellDataBase.SpellZoneCircle);
                CircleObjectToShow.transform.SetParent(BattleController.Instance.OneBattleContainer);
                CircleObjectToShow.SetSize(_spellDamageData.AOERad * 4);
            }

            CircleObjectToShow.gameObject.SetActive(true);
        }
    }


    public void EndShowCast()
    {
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
        var dir = Utils.NormalizeFastSelf(target - startPos);
        var maxDistPos = startPos + dir * AimRadius;
        var distCal = _distCounter(maxDistPos, target);
        DelayedAction.Use();
        Debug.LogError($"Cast rad start:{AimRadius.ToString("0.00")}");
        var castData = new CastSpellData()
        {
            Bullestartparameters = BulletStartParams,
            ShootsCount = ShootPerTime,
        };
        CastSpell(new BulletTarget(distCal), _bulletOrigin, this, startPos, castData);
        //        CastSpell(new BulletTarget(target), _bulletOrigin, this, startPos, _bulletStartParams);
    }


    public void BulletCreateByDir(ShipBase target, Vector3 dir)
    {
        CreateBulletWithModif(dir);
//        if (ShootPerTime > 0)
//        {
//            for (int i = 1; i < ShootPerTime; i++)
//            {
//                var timer = MainController.Instance.BattleTimerManager.MakeTimer(0.1f * i);
//                timer.OnTimer += () =>
//                {
//                    if (!Owner.IsDead)
//                        CreateBulletWithModif(dir);
//                };
//            }     
//        }
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
                _controlShip.CoinController.UseCoins(CostCount, CostPeriod);
                Cast(trg);
                return true;
            }
        }

        return false;
    }

    public WeaponInventoryAffectTarget AffectAction => _affectAction;
    public CreateBulletDelegate CreateBulletAction => _spellData.CreateBulletAction;

    public BulleStartParameters BulletStartParams =>
        new BulleStartParameters(BulletSpeed, _bulletStartParams.turnSpeed, AimRadius,AimRadius);

    public void SetBulletCreateAction(CreateBulletDelegate bulletCreate)
    {
        _spellData.SetBulletCreateAction(bulletCreate);
    }
}
