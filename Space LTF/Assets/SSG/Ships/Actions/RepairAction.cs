using JetBrains.Annotations;
using UnityEngine;

public class RepairAction : BaseAction
{
    private float _repairEnd;
    private bool _end = false;
    private const float  PERIOD = 10f;

    public RepairAction([NotNull] ShipBase owner) 
        : base(owner, ActionType.repairAction)
    {
        _repairEnd = Time.time + PERIOD;
        _end = false;
        if (owner.RepairEffect != null)
            owner.RepairEffect.Play();

    }
    
    public override void ManualUpdate()
    {
        // _owner.SetTargetSpeed(0f);
        if (_repairEnd < Time.time)
        {
            _owner.DamageData.RepairAll();
            _end = true;
            if (_owner.RepairEffect != null)
                _owner.RepairEffect.Stop();
        }
    }
    protected override CauseAction[] GetEndCauses()
    {
        var c = new CauseAction[]
        {
            new CauseAction("out bf", () => !_owner.InBattlefield),
            new CauseAction("end repair", () => _end),
        };
        return c;
    }
}