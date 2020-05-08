using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoSpellContainer
{
    public ShipBase Target = null;
    private float _delay = 1f;

    private float _nextCheck = 1f;

    //    protected Commander _commander;
    protected SpellInGame _spellData;
    protected ShipControlCenter _owner;
    protected BulleStartParameters _bulletStartParams;
    protected Bullet _bulletOrigin;
    protected float ShootDistSqrt;
    protected float _maxDist;

    protected TeamIndex _teamIndex;

//    protected TestTargetPosition _testTargetPosition;
    private List<AIAsteroidPredata> _allAsteroids = new List<AIAsteroidPredata>();
    private bool _isActive;
    public bool IsActive => _isActive;
    public event Action<AutoSpellContainer> OnActive;

    public AutoSpellContainer(ShipControlCenter commander, SpellInGame spellData)
    {
        _allAsteroids = BattleController.Instance.CellController.Data.Asteroids;
        //        _testTargetPosition = new TestTargetPosition();
        _spellData = spellData;
        _owner = commander;
        _teamIndex = commander.TeamIndex;
        _delay = MyExtensions.Random(1f, 4f);
        var spellRad = spellData.MaxDist; //. .r.BulleStartParameters.radiusShoot;
        _maxDist = spellRad;
        ShootDistSqrt = spellRad * spellRad;
    }

    protected void PeriodInnerUpdate()
    {
        Vector3 trg;
        if (CanCast())
            if (IsEnemyClose(out trg))
                TryUse(trg);
    }

    private bool IsEnemyClose(out Vector3 trg)
    {
        var targInfo = _owner.Enemies[Target];
        if (targInfo.Dist < _maxDist)
            foreach (var aiAsteroidPredata in _allAsteroids)
            {
            }

        trg = Vector3.zero;
        return false;

//        _testTargetPosition.TestTarget(targInfo.ShipLink.Position, targInfo.ShipLink.LookDirection,
//            targInfo.ShipLink.LookRight, mo, owner.LookDirection, _maxDist, offsetCoef);
//        WeaponInGame.IsAimedStraight(_testTargetPosition, targInfo, _owner, _modulPos(), ShootDistSqrt);
    }

    public void SetActive(bool val)
    {
        _isActive = val;
        OnActive?.Invoke(this);
    }

    protected void TryUse(Vector3 v)
    {
        if (CanCast())
        {
            Cast(v);
            _owner.CoinController.UseCoins(_spellData.CostCount, _spellData.CostPeriod);
            //            Debug.LogError($"_spell.TryCast {this}");
        }
    }

    public void Cast(Vector3 target)
    {
        _owner.Audio.PlayOneShot(DataBaseController.Instance.AudioDataBase.GetCastSpell(_spellData.SpellType));
        var startPos = _modulPos();
        var dir = Utils.NormalizeFastSelf(target - startPos);
        var distToTarget = (startPos - target).magnitude;
        if (distToTarget > _maxDist)
        {
            var maxDistPos = startPos + dir * _maxDist;
            target = maxDistPos;
        }

        _spellData.CastSpell(new BulletTarget(target), _bulletOrigin, _spellData, startPos, _bulletStartParams);
    }

    protected Vector3 _modulPos()
    {
        return _owner.Position;
    }

    public void PeriodlUpdate()
    {
        if (_nextCheck < Time.time)
        {
            _nextCheck = Time.time + MyExtensions.GreateRandom(_delay);
            PeriodInnerUpdate();
        }
    }

    protected bool CanCast()
    {
        return _owner.CoinController.CanUseCoins(_spellData.CostCount);
    }
}