using UnityEngine;

public class DebugRating
{
    private float dist;
    private float distDot;
    private float help;
    private int marks;
    public Vector3 MyPos;
    public ShipBase MyShip;
    public Vector3 Pos;
    private float recalcDist;
    public ShipBase Ship;
    private float totalRating;

    public DebugRating(ShipBase Ship, ShipBase MyShip, Vector3 pos, Vector3 MyPos, float dist, float distDot,
        float recalcDist,
        float help, float totalRating, int marks)
    {
        this.MyShip = MyShip;
        this.Ship = Ship;
        this.MyPos = MyPos;
        Pos = pos;
        this.distDot = distDot;
        this.dist = dist;
        this.recalcDist = recalcDist;
        this.help = help;
        this.totalRating = totalRating;
        this.marks = marks;
    }

    public string Dists()
    {
        return dist.ToString("0.0") + "  " + distDot.ToString("0.0") + "  " + recalcDist.ToString("0.00");
    }

    public string Helps()
    {
        var s = help.ToString("0.0") + "   " + marks.ToString("0");
        return s;
    }

    public string Total()
    {
        var s = totalRating.ToString("00.0");
        return s;
    }

    public void UpdateData(DebugRating debugRating)
    {
        MyShip = debugRating.MyShip;
        Ship = debugRating.Ship;
        MyPos = debugRating.MyPos;
        Pos = debugRating.Pos;
        distDot = debugRating.distDot;
        recalcDist = debugRating.recalcDist;
        help = debugRating.help;
        totalRating = debugRating.totalRating;
        dist = debugRating.dist;
        marks = debugRating.marks;
    }
}