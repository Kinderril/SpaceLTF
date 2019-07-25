using JetBrains.Annotations;
using UnityEngine;

public class WaitHealAction : BaseAction
{
    private const float PERCENT_PER_TICK = 0.1f;
    private const float MAGNET_SPEED = 0.01f;
    private float _hpPercentHeal;
    private float _sdPercentHeal;
    private int _secToWait;
    private int _secRemain;
    private float _nextStep;
    private Vector3 _point;

    public WaitHealAction([NotNull] ShipBase owner,Vector3 point) 
        : base(owner, ActionType.waitHeal)
    {
        _point = point;
        //        owner.ShipParameters.MaxHealth = owner.ShipParameters.MaxHealth*0.8f;

        owner.ShipParameters.DowngradeMaxHealth(0.8f);
        var deltaHp = owner.ShipParameters.MaxHealth - owner.ShipParameters.CurHealth;
        if (deltaHp < 0)
        {
            deltaHp = 0;
        }
        _hpPercentHeal = owner.ShipParameters.MaxHealth*PERCENT_PER_TICK;
        var hpPercentNeedToHeal = deltaHp / owner.ShipParameters.MaxHealth;

        var deltaSd = owner.ShipParameters.MaxShiled - owner.ShipParameters.CurShiled;
        _sdPercentHeal = owner.ShipParameters.MaxShiled * PERCENT_PER_TICK;
        var sdPercentNeedToHeal = deltaSd / owner.ShipParameters.MaxShiled;

        var maxPecent = Mathf.Max(sdPercentNeedToHeal, hpPercentNeedToHeal);
        _secToWait = (int) (maxPecent/ PERCENT_PER_TICK) + 1;
        _secRemain = _secToWait;
        _nextStep = Time.time;


    }

    public override void ManualUpdate()
    {
        var dir = _point - _owner.Position;
        var dist = (dir).magnitude2d();
        if (dist > 1)
        {
            var norm = Utils.NormalizeFastSelf(dir);
            _owner.Position = _owner.Position + norm*MAGNET_SPEED;
        }

        if (_nextStep < Time.time)
        {
            _nextStep = 1 + _nextStep;
            _owner.ShipParameters.HealHp(_hpPercentHeal);
            _owner.ShipParameters.ShieldParameters.HealShield(_sdPercentHeal);
            _secRemain--;
        }
        LinkToPoint();
    }

    private void LinkToPoint()
    {
        _owner.SetTargetSpeed(0f);
    }

    protected override CauseAction[] GetEndCauses()
    {
        var c = new CauseAction[]
        {
            new CauseAction("out bf", () => _owner.InBattlefield),
            new CauseAction("heal comlete", () =>  _secRemain == 0),
        };
        return c;
    }
}