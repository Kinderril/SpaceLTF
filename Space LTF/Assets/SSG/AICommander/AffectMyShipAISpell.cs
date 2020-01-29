using UnityEngine;

public class AffectMyShipAISpell<T> : BaseAISpell where T : BaseSpellModulInv
{
    protected T _spell;
    private TeamIndex oIndex;

    public AffectMyShipAISpell(SpellInGame spellData, T spell, Commander commander)
        : base(commander, spellData)
    {
        _spell = spell;
        oIndex = commander.TeamIndex;
        _bulletOrigin = spell.GetBulletPrefab();
        _bulletStartParams = _spell.BulleStartParameters;
        var spellRad = spell.BulleStartParameters.radiusShoot;
        _maxDist = spellRad;
        ShootDistSqrt = spellRad * spellRad;
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

    protected override bool IsEnemyClose(out Vector3 trg)
    {
        var ship = BattleController.Instance.ClosestShipToPos(_commander.MainShip.Position, oIndex, out var sDist);
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
            Debug.LogError($"_spell.TryCast {this}");
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



