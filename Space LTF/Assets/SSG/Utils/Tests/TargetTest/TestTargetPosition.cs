using UnityEngine;


public class TestTargetPosition
{
    public const float Side = 0.3f;
    public const float Front = 2.4f;
    public const float Back = 1.1f;
    private SegmentPoints segmentLong;
    private SegmentPoints segmentShoot;
    private SegmentPoints segmentShort;
    public bool ShallShoot { get; private set; }
    public Vector3? PosToAim { get; private set; }

    public void TestTarget(Vector3 targerPos, Vector3 targetLookDir, Vector3 targetLookRight,
        Vector3 attackerPos, Vector3 attackerLookDir, float shootDist, float targetSpeed)
    {
        var ang = Vector3.Angle(targetLookDir, attackerLookDir);
        var offset = ang * targetSpeed;

        PosToAim = targerPos + offset * targetLookDir;
        var sideLook = targetLookRight * Side;
        segmentLong = new SegmentPoints(PosToAim.Value + targetLookDir * Front, PosToAim.Value - targetLookDir * Back);
        segmentShort = new SegmentPoints(PosToAim.Value + sideLook, PosToAim.Value - sideLook);
        segmentShoot = new SegmentPoints(attackerPos, attackerPos + attackerLookDir * shootDist);
        var cross1 = AIUtility.GetCrossPoint(segmentLong, segmentShoot);
        ShallShoot = false;
        if (cross1.HasValue)
        {
            ShallShoot = true;
        }
        else
        {
            var cross2 = AIUtility.GetCrossPoint(segmentShort, segmentShoot);
            if (cross2.HasValue)
            {
                ShallShoot = true;
            }
        }

    }

    public void DropAimPos()
    {
        PosToAim = null;
    }

    public void OnDrawGizmos()
    {
        if (segmentLong != null)
        {
            DrawSegment(segmentLong, Color.cyan);
        }
        if (segmentShort != null)
        {
            DrawSegment(segmentShort, Color.blue);
        }
        if (segmentShoot != null)
        {
            DrawSegment(segmentShoot, ShallShoot ? Color.green : Color.red);
        }

    }

    private void DrawSegment(SegmentPoints p0, Color red)
    {
        Gizmos.color = red;
        Gizmos.DrawLine(p0.A, p0.B);

    }

}

