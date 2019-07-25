using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimStraightTest : MonoBehaviour
{
    public Transform ShooterPos;
    public Transform ShooterPrediction;
    public Transform TargetPos;
    public Transform TargetPrediction;

    public float BulletSpeed = 1f;
    public float TargetSpeed = 1f;

    void OnDrawGizmos()
    {

        var dataMain = AIUtility.IsAimedStraightFindCrossPoint(TargetPrediction.position, TargetPos.position,ShooterPos.position,
            ShooterPrediction.position,true);
        if (dataMain.HasValue)
        {
            bool canShoot = AIUtility.IsAimedStraightBaseOnCrossPoint(dataMain.Value, BulletSpeed, TargetSpeed, 0.1f);
            DrawUtils.DebugPoint(dataMain.Value.CrossPoint, canShoot ? Color.green : Color.red);
        }
        else
        {
            var d = AIUtility.IsAimedStraightByProjectionPoint(TargetPos.position, ShooterPos.position,
                ShooterPrediction.position,true);
            if (d)
            {
                var shooterDir = Utils.NormalizeFastSelf(ShooterPrediction.position - ShooterPos.position);
                var targetDir = Utils.NormalizeFastSelf(TargetPrediction.position - TargetPos.position);
                d = AIUtility.IsAimedStraight4(targetDir, shooterDir);
            }
            DrawUtils.DebugPoint(TargetPos.position, d ? Color.green : Color.red);
        }
    }
}
