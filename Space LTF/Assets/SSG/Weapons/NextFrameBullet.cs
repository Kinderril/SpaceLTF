using UnityEngine;
using System.Collections;

public class NextFrameBullet : Bullet
{
    public override BulletType GetType => BulletType.nextFrame;
    private int remainFrames = 3;
    public bool FindTarget = true; 

    public override void LateInit()
    {
        base.Init();
        remainFrames = 3;
        if (FindTarget && Target == null)
        {
            TeamIndex inedx = Weapon.TargetType == TargetType.Enemy
                ? BattleController.OppositeIndex(Weapon.TeamIndex)
                : Weapon.TeamIndex;
            var ship = BattleController.Instance.ClosestShipToPos(_endPos, inedx);
            Target = ship;
        }
        Debug.DrawRay(_endPos,Vector3.up*10,Color.cyan,4f);
        
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
            DrawUtils.DebugCircle(_endPos, Vector3.up, Color.yellow, 0.2f, 3f);
            Death();
        }
    }
}
