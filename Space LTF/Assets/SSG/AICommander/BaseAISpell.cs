using UnityEngine;

public abstract class BaseAISpell
{
    private float _delay = 1f;
    private float _nextCheck = 1f;
    protected Commander _commander;
    protected SpellInGame _spellData;
    protected ShipBase _owner;
    protected BulleStartParameters _bulletStartParams;
    protected Bullet _bulletOrigin;
    protected float ShootDistSqrt;
    protected float _maxDist;

    protected BaseAISpell(Commander commander, SpellInGame spellData)
    {
        _spellData = spellData;
        _owner = commander.MainShip;
        _commander = commander;
        _delay = MyExtensions.Random(1f, 4f);
        _owner = commander.MainShip;
    }
    protected bool CanCast()
    {
        return _commander.CoinController.CanUseCoins(_spellData.CostCount);
    }

    protected Vector3 _modulPos()
    {
        return _owner.Position;
    }
    protected abstract bool IsEnemyClose(out Vector3 trg);
    public virtual void Init()
    {

    }

    public virtual void ManualUpdate()
    {
    }

    public void PeriodlUpdate()
    {
        if (_nextCheck < Time.time)
        {
            _nextCheck = Time.time + MyExtensions.GreateRandom(_delay);
            PeriodInnerUpdate();
        }
    }

    protected virtual void PeriodInnerUpdate()
    {

    }

    public virtual void Dispose()
    {

    }
}

public abstract class BaseAISpell<T> : BaseAISpell where T : BaseSpellModulInv
{
    protected T _spell;
    protected TeamIndex oIndex;

    protected BaseAISpell(SpellInGame spellData, T spell, Commander commander)
        : base(commander, spellData)
    {
        oIndex = BattleController.OppositeIndex(_commander.TeamIndex);
        _spell = spell;
        _bulletOrigin = spell.GetBulletPrefab();
        _bulletStartParams = _spell.BulleStartParameters;
        var spellRad = spell.BulleStartParameters.radiusShoot;
        _maxDist = spellRad;
        ShootDistSqrt = spellRad * spellRad;
#if UNITY_EDITOR
        if (ShootDistSqrt < 0.001f)
        {
            Debug.LogError($"{this} Spell have BAD Shoot radius");
        }
        var canUseSpell = _commander.CoinController.CanUseCoins(_spell.CostCount);
        Debug.Log($"AI spell controller init: {spell.GetType()}  spell.AimRadius:{spell.AimRadius}   canUseSpell:{canUseSpell}");
        if (!canUseSpell)
        {
            Debug.LogError("AI can't use spell cause havn't enought coins");
        }
#endif

    }
    protected override void PeriodInnerUpdate()
    {
        Vector3 trg;
//        Debug.LogError($"Cast spell 1 {this}");
        if (CanCast())
        {
            if (IsEnemyClose(out trg))
            {
                TryUse(trg);
            }
        }
    }

    protected override bool IsEnemyClose(out Vector3 trg)
    {
        var ship = BattleController.Instance.ClosestShipToPos(_commander.MainShip.Position, oIndex, out var sDist);
//        Debug.LogError($"IsEnemyClose  dist {Mathf.Sqrt(sDist)} <  {Mathf.Sqrt(ShootDistSqrt)}");
        if (sDist < ShootDistSqrt)
        {
            trg = ship.Position;
            return true;
        }
        trg = Vector3.zero;
        return false;
    }

    protected void TryUse(Vector3 v)
    {
        if (CanCast())
        {
            Cast(v);
            _commander.CoinController.UseCoins(_spell.CostCount, _spell.CostTime);
//            Debug.LogError($"_spell.TryCast {this}");
        }
    }

    public void Cast(Vector3 target)
    {
        _owner.Audio.PlayOneShot(DataBaseController.Instance.AudioDataBase.GetCastSpell(_spell.SpellType));
        var startPos = _modulPos();
        var dir = Utils.NormalizeFastSelf(target - startPos);
        var distToTarget = (startPos - target).magnitude;
        if (distToTarget > _maxDist)
        {
            var maxDistPos = startPos + dir * _maxDist;
            target = maxDistPos;
        }
        _spell.CastSpell(new BulletTarget(target), _bulletOrigin, _spellData, startPos, _bulletStartParams);
    }

}

