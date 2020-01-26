using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public delegate void CastActionSpell(BulletTarget target, Bullet origin, IWeapon weapon,
    Vector3 shootpos, BulleStartParameters bullestartparameters);

[System.Serializable]
public delegate Vector3 DistCounter(Vector3 maxDistPos,Vector3 targetDistPos);

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
    public SpellDamageData(float rad)
    {
        AOERad = rad;
        IsAOE = true;
    }

}

public class SpellInGame : IWeapon
{
    public CreateBulletDelegate CreateBulletAction;
    public WeaponInventoryAffectTarget AffectAction;

    private BulleStartParameters _bulletStartParams;

    private SpellZoneVisualCircle CircleObjectToShow;
    private SpellZoneVisualLine LineObjectToShow;


//    private int RadiusCircle;
    private Func<Vector3> _modulPos;
    private Bullet _bulletOrigin;
    private WeaponAffectionAdditionalParams _additionalParams = new WeaponAffectionAdditionalParams();
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
    public CurWeaponDamage CurrentDamage => new CurWeaponDamage(0, 0);
    public string Name { get; private set; }
    public string Desc { get; private set; }
    public SpellType SpellType { get;private set; }
//    float ShowCircleRadius { get; }
    bool ShowLine { get; }
    private float _maxDist;
    public bool ShowCircle { get;private set; }
private DistCounter _distCounter;

    public SpellInGame(ISpellToGame spellData,Func<Vector3> modulPos,
        TeamIndex teamIndex,ShipBase owner,int level,string name,int period,
        int count, SpellType spellType,float maxDist, string desc, DistCounter distCounter)
    {
        if (maxDist < 1)
        {
            Debug.LogError($"Shoot dist is vey low {spellData}  {maxDist}  name:{name}");
        }

        _distCounter = distCounter;

Desc = desc;
        _maxDist = maxDist;
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
        AffectAction = spellData.AffectAction;
        CastSpell = spellData.CastSpell;
        SubUpdateShowCast = spellData.SubUpdateShowCast;
        CanCastAtPoint = spellData.CanCastAtPoint;
        CreateBulletAction = spellData.CreateBulletAction;
        ShowCircle = _spellDamageData.IsAOE;
    }
    public bool CanCast(CommanderCoinController coinController)
    {
        var canUse = coinController.CanUseCoins(CostCount);
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
            LineObjectToShow.SetDirection(_modulPos(), _modulPos() + dir, _maxDist);
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
                CircleObjectToShow.SetSize(_spellDamageData.AOERad*4);
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
        var maxDistPos = startPos + dir * _maxDist;
        var distCal = _distCounter(maxDistPos, target);
        CastSpell(new BulletTarget(distCal), _bulletOrigin, this, startPos, _bulletStartParams);
//        CastSpell(new BulletTarget(target), _bulletOrigin, this, startPos, _bulletStartParams);
    }


    public void BulletCreateByDir(ShipBase target, Vector3 dir)
    {
        CreateBulletWithModif(dir);
    }

    protected void CreateBulletWithModif(ShipBase target)
    {
        CreateBulletAction(new BulletTarget(target), _bulletOrigin, this, _modulPos(), _bulletStartParams);
    }

    protected void CreateBulletWithModif(Vector3 target)
    {
        CreateBulletAction(new BulletTarget(target), _bulletOrigin, this, _modulPos(), _bulletStartParams);
    }

    public void DamageDoneCallback(float healthdelta, float shielddelta, ShipBase damageAppliyer)
    {
//        GlobalEventDispatcher.ShipDamage(Owner, healthdelta, shielddelta, _weaponType);
        Owner.ShipInventory.LastBattleData.AddDamage(healthdelta, shielddelta);
        if (damageAppliyer != null)
        {
#if UNITY_EDITOR
            if (damageAppliyer.IsDead == Owner)
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
            DrawUtils.DebugCircle(position,Vector3.up, Color.cyan, _spellDamageData.AOERad,3f);
#endif
            foreach (var shipBase in shipsInRad)
            {
                ApplyToShipSub(shipBase.ShipParameters, shipBase, bullet);
            }
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
        foreach (var targetDelegate in AffectAction.Additional)
        {
            targetDelegate(shipParameters, shipBase, bullet, DamageDoneCallback, _additionalParams);
        }
        AffectAction.Main(shipParameters, shipBase, bullet, DamageDoneCallback, _additionalParams);
    }


    public bool TryCast(CommanderCoinController commanderCoinController, Vector3 trg)
    {
        if (CanCast(commanderCoinController))
        {
            if (CanCastAtPoint(trg))
            {
                commanderCoinController.UseCoins(CostCount, CostPeriod);
                Cast(trg);
                return true;
            }
        }

        return false;
    }
}
