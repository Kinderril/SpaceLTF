using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public delegate void CastActionSpell(BulletTarget target, Bullet origin, IWeapon weapon, Vector3 shootpos, BulleStartParameters bullestartparameters);
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
    private Vector3 _modulPos;
    private Bullet _bulletOrigin;
    private WeaponAffectionAdditionalParams _additionalParams = new WeaponAffectionAdditionalParams();
    private SpellDamageData _spellDamageData;

    private readonly ShipBase _owner;
    public TeamIndex TeamIndex { get; private set; }
    public Vector3 CurPosition => _modulPos;
    public ShipBase Owner => _owner;
    public int Level { get; private set; }
    public int CostCount { get; private set; }
    public int CostPeriod { get; private set; }
    public CastActionSpell CastSpell { get; private set; }
    public float CurOwnerSpeed => 0.001f;
    public CurWeaponDamage CurrentDamage => new CurWeaponDamage(0, 0);
    public string Name { get; set; }
    public SpellType SpellType { get;private set; }

    public SpellInGame(ISpellToGame spellData,Vector3 modulPos,
        TeamIndex teamIndex,ShipBase owner,int level,string name,int period,int count, SpellType spellType)
    {
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
        CreateBulletAction = spellData.CreateBulletAction;
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
        if (_spellDamageData.IsAOE)
        {
            CircleObjectToShow.transform.position = pos;
        }

        var dir = (pos - _modulPos);
        LineObjectToShow.SetDirection(_modulPos, _modulPos + dir);
    }

    public void StartShowCast()
    {
        if (LineObjectToShow == null)
        {
            var p = DataBaseController.Instance.SpellDataBase.SpellZoneLine;
            LineObjectToShow = DataBaseController.GetItem(p);
            LineObjectToShow.transform.SetParent(BattleController.Instance.OneBattleContainer);
        }
        LineObjectToShow.gameObject.SetActive(true);
        if (_spellDamageData.IsAOE)
        {
            if (CircleObjectToShow == null)
            {
                CircleObjectToShow =
                    DataBaseController.GetItem(DataBaseController.Instance.SpellDataBase.SpellZoneCircle);
                CircleObjectToShow.transform.SetParent(BattleController.Instance.OneBattleContainer);
                CircleObjectToShow.SetSize(_spellDamageData.AOERad);
            }

            CircleObjectToShow.gameObject.SetActive(true);
        }
    }


    public void EndShowCast()
    {
        if (_spellDamageData.IsAOE)
        {
            CircleObjectToShow.gameObject.SetActive(false);
        }

        LineObjectToShow.gameObject.SetActive(false);
    }

    public void Cast(Vector3 target)
    {
        CastSpell(new BulletTarget(target), _bulletOrigin, this, _modulPos, _bulletStartParams);
    }


    public void BulletCreateByDir(ShipBase target, Vector3 dir)
    {
        CreateBulletWithModif(dir);
    }

    protected void CreateBulletWithModif(ShipBase target)
    {
        CreateBulletAction(new BulletTarget(target), _bulletOrigin, this, _modulPos, _bulletStartParams);
    }

    protected void CreateBulletWithModif(Vector3 target)
    {
        CreateBulletAction(new BulletTarget(target), _bulletOrigin, this, _modulPos, _bulletStartParams);
    }

    public void DamageDoneCallback(float healthdelta, float shielddelta, ShipBase applyied)
    {

    }

    public void BulletDestroyed(Vector3 position, Bullet bullet)
    {
        if (_spellDamageData.IsAOE)
        {
            var shipsToHitIndex = BattleController.OppositeIndex(TeamIndex);
            var shipsInRad = BattleController.Instance.GetAllShipsInRadius(position, shipsToHitIndex, _spellDamageData.AOERad);
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
            commanderCoinController.UseCoins(CostCount,CostPeriod);
            Cast(trg);
            return true;
        }

        return false;
    }
}
