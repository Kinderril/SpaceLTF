using UnityEngine;
using System.Collections;

public class NextFrameBullet : Bullet
{
    public override BulletType GetType => BulletType.nextFrame;
    private int remainFrames = 3;

    public override void LateInit()
    {
        base.Init();
        remainFrames = 3;
        if (Target == null)
        {
            var inex = BattleController.OppositeIndex(Weapon.TeamIndex);
            var ship = BattleController.Instance.ClosestShipToPos(_endPos, inex);
            Target = ship;
        }
        
    }

    protected override void ManualUpdate()
    {
        UpdateLinear();
    }

    private void UpdateLinear()
    {
        transform.position = _endPos;
        remainFrames--;
        if (remainFrames <= 0)
        {
            if (Target != null)
            {
                Target.GetHit(Weapon, this);
            }
            Death();
        }
    }
}
