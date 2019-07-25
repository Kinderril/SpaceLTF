using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DebugAimingData
{
    private Vector3 ShootPos;
    private Vector3 PredictionPos;
    private Vector3 TargetPos;
    private Vector3 TargetPrediction;
    private float delta;
    private float Posible;

    public DebugAimingData(Vector3 ShootPos, Vector3 PredictionPos,
        Vector3 TargetPos, Vector3 TargetPrediction)
    {
        this.ShootPos = ShootPos;
        this.PredictionPos = PredictionPos;
        this.TargetPos = TargetPos;
        this.TargetPrediction = TargetPrediction;
    }

    public void SetAimingTimes(float delta, float posibleDelta)
    {
        this.delta = delta;
        this.Posible = posibleDelta;
    }

    public void GizmosDraw()
    {
        var dir1 = (PredictionPos - ShootPos) * 100;
        var dir2 = (TargetPrediction - TargetPos) * 100;

        var se1 = ShootPos + dir1;
        var se2 = TargetPos + dir2;
        var seg1 = new SegmentPoints(ShootPos, se1);
        var seg2 = new SegmentPoints(TargetPos, se2);
        var cross = AIUtility.GetCrossPoint(seg1, seg2);
        if (cross.HasValue)
        {

            var maxPercent = Posible * 3f;
            if (delta < maxPercent)
            {
                var perc = 1f - delta / maxPercent;
                var c = Color.Lerp(Color.green, Color.red, perc);
                DrawUtils.DrawPoint(cross.Value, c, 1f);

            }
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(ShootPos, PredictionPos);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(TargetPos, TargetPrediction);


    }

}
