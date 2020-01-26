using UnityEngine;

public abstract class BaseAISpell
{
    private float _delay = 1f;
    private float _nextCheck = 1f;

    protected BaseAISpell()
    {
        _delay = MyExtensions.Random(1f, 4f);
    }

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
    protected Commander _commander;
    protected T _spell;
    protected float ShootDistSqrt;
    private TeamIndex oIndex;
    private ShipBase _owner;
    private float _maxDist;
    private BulleStartParameters _bulletStartParams;
    private Bullet _bulletOrigin;
    private SpellInGame _spellData;

    protected BaseAISpell(SpellInGame spellData, T spell, Commander commander)
    {
        _spellData = spellData;
        _spell = spell;
        _owner = commander.MainShip;
        _commander = commander;
        _bulletOrigin = spell.GetBulletPrefab();
        _bulletStartParams = _spell.BulleStartParameters;
        oIndex = BattleController.OppositeIndex(_commander.TeamIndex);
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
        Debug.LogError($"Cast spell 1 {this}");
        if (CanCast())
        {
            if (IsEnemyClose(out trg))
            {
                TryUse(trg);
            }
        }
    }

    private bool IsEnemyClose(out Vector3 trg)
    {
        var ship = BattleController.Instance.ClosestShipToPos(_commander.MainShip.Position, oIndex, out var sDist);
        Debug.LogError($"IsEnemyClose  dist {Mathf.Sqrt(sDist)} <  {Mathf.Sqrt(ShootDistSqrt)}");
        if (sDist < ShootDistSqrt)
        {
            trg = ship.Position;
            return true;
        }
        trg = Vector3.zero;
        return false;
    }
    protected bool CanCast()
    {
        return _commander.CoinController.CanUseCoins(_spell.CostCount);
    }

    protected void TryUse(Vector3 v)
    {
        if (CanCast())
        {

            Cast(v);
            _commander.CoinController.UseCoins(_spell.CostCount, _spell.CostTime);
            Debug.LogError($"_spell.TryCast {this}");
        }
    }

    private Vector3 _modulPos()
    {
        return _owner.Position;
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
        //        CastSpell(new BulletTarget(target), _bulletOrigin, this, startPos, _bulletStartParams);
    }

}

