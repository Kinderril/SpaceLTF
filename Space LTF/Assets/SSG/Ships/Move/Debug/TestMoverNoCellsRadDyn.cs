using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestMoverNoCellsRadDyn : MovingObject
{
    public float MaxSpeedDebug = 4f;
    public float TURNsPEED = 4f;
    public float DistCoef = 1.34f;
    public Transform TargetPOsition;
    public DebugAsteroid[] Asteroids;

    public float CurSpeedI;

    void Awake()
    {
        Asteroids = GameObject.FindObjectsOfType<DebugAsteroid>();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        EngineUpdate();
        if (TargetPOsition == null)
        {
            return;
        }
        var v = TargetPOsition.position;
        var direction4 = GetCurentDirection1(v,out float speedRecommended);
        if (CurSpeed > speedRecommended)
        {
            SetTargetSpeed(-1f);
        }
        else
        {
            SetTargetSpeed(1f);
        }

        CurSpeedI = CurSpeed;
//        var dir = v - Position;
        ApplyRotation(direction4, true);
        ApplyMove();
    }

    private Vector3 GetCurentDirection1(Vector3 vector3, out float sppedRecommednded)
    {
        Vector3 dirToTarget = vector3 - Position;
//        float dist;
//        Vector3 dirToAsteroid;
        DrawUtils.DebugCircle(Position, Vector3.up, Color.green, CurTurnRadius);

//        var isFront = Vector3.Dot(LookDirection, dirToTarget) > 0;
        var field = GetAsteroidsInFront(LookDirection, Position,MaxTurnRadius* DistCoef);
        if (field.Count == 0)
        {
            sppedRecommednded = MaxSpeed();
            return dirToTarget;
        }
        bool havePointBad = false;
        float maxProjectionDist = Single.MinValue;
        bool farestProjectionDiLeft = false;
        DebugAsteroid controlSateroid = null;
        float blockingAsteroidDist = Single.MaxValue;
        DebugAsteroid blockingAsteroid = null;

        foreach (var asteroid in field)
        {
            Debug.DrawLine(vector3, transform.position,Color.blue);
            
            var projection = AIUtility.GetProjectionPoint(asteroid.transform.position, Position + LookDirection*100, transform.position);
            var projectionDist = (projection - asteroid.transform.position).magnitude;
            bool affectableClose = projectionDist < asteroid.Rad;
            if (affectableClose)
            {
                if (asteroid.DistToShip < blockingAsteroidDist)
                {
                    blockingAsteroidDist = asteroid.DistToShip;
                    blockingAsteroid = asteroid;
                }

                havePointBad = true;
            }
            Debug.DrawLine(projection, asteroid.transform.position, affectableClose?Color.red : Color.green);
            DrawUtils.DebugCircle(asteroid.transform.position,Vector3.up, Color.red,asteroid.Rad);
            if (projectionDist > maxProjectionDist)
            {
                var isLeft = Utils.FastDot(asteroid.DirToAsteroidNorm, LookLeft) > 0;
                farestProjectionDiLeft = isLeft;
                maxProjectionDist = projectionDist;
                controlSateroid = asteroid;
            }
        }

        if (controlSateroid != null)
        {
            Debug.DrawLine(controlSateroid.transform.position, Position, Color.white);
        }

        if (blockingAsteroid != null)
        {
            Debug.DrawLine(blockingAsteroid.transform.position, Position, Color.cyan);
        }

        if (havePointBad)
        {
            if (farestProjectionDiLeft)
            {
                //            DrawUtils.DebugArrow(closestFront.transform.position, LookLeft, Color.yellow);     
                sppedRecommednded = MaxSpeed();
                return LookRight;
            }
            else
            {

                //            DrawUtils.DebugArrow(closestFront.transform.position, LookLeft, Color.yellow);     
                sppedRecommednded = MaxSpeed();
                return LookLeft;
            }
        }

        DrawUtils.DebugArrow(transform.position, LookDirection* MaxTurnRadius*2, Color.yellow);
        sppedRecommednded = MaxSpeed();
        return dirToTarget;




    }

    private Vector3 GetCurentDirection4(Vector3 vector3, out float sppedRecommednded)
    {
        Vector3 dirToTarget = vector3 - Position;
        float dist;
        Vector3 dirToAsteroid;
        DrawUtils.DebugCircle(Position, Vector3.up, Color.green, CurTurnRadius);

        var isFront = Vector3.Dot(LookDirection, dirToTarget) > 0;

        if (isFront)
        {

            var frontpos = Position + LookDirection * CurTurnRadius;
            Debug.DrawLine(transform.position,frontpos,Color.green);
            var closestFront = ClosestsToPoint(frontpos, out dist, out dirToAsteroid);
            var isTooClose = dist< CurTurnRadius + closestFront.Rad;
            DrawUtils.DebugCircle(frontpos, Vector3.up, isTooClose?Color.red : Color.yellow, CurTurnRadius);
            if (isTooClose)    //Dange is At fron . do turn
            {
                DrawUtils.DebugCircle(closestFront.transform.position,Vector3.up, Color.red,closestFront.Rad);
                bool isLeft = Vector3.Dot(LookLeft, dirToAsteroid) > 0;
                sppedRecommednded = 0.1f;

                if (isLeft)
                {
                    DrawUtils.DebugArrow(closestFront.transform.position, LookLeft, Color.yellow);
                    return LookRight;
                }
                else
                {

                    DrawUtils.DebugArrow(closestFront.transform.position, LookLeft, Color.yellow);
                    return LookLeft;
                }

            }

            sppedRecommednded = MaxSpeed();
            return dirToTarget;
        }
        else
        {
            bool isLeft = Vector3.Dot(LookLeft, dirToTarget) > 0;
            if (isLeft)
            {

                var leftPos = vector3 + LookLeft * CurTurnRadius;
                var closestFront = ClosestsToPoint(leftPos, out dist, out dirToAsteroid);

                if (dist + closestFront.Rad < CurTurnRadius)    //Dange is At fron . do turn
                {
                    DrawUtils.DebugCircle(closestFront.transform.position, Vector3.up, Color.red, closestFront.Rad);
                    sppedRecommednded = 0.1f;
                    return dirToTarget;
                }
                sppedRecommednded = MaxSpeed();
                return dirToTarget;
            }
            else
            {
                var rightPos = vector3 + LookRight * CurTurnRadius;
                var closestFront = ClosestsToPoint(rightPos, out dist, out dirToAsteroid);

                if (dist + closestFront.Rad < CurTurnRadius)    //Dange is At fron . do turn
                {
                    DrawUtils.DebugCircle(closestFront.transform.position, Vector3.up, Color.red, closestFront.Rad);
                    sppedRecommednded = 0.1f;
                    return dirToTarget;
                }
                sppedRecommednded = MaxSpeed();
                return dirToTarget;
            }

        }
    }

    private List<DebugAsteroid> GetAsteroidsInFront(Vector3 dirToTest,Vector3 point,float maxDist)
    {
        List<DebugAsteroid> closestAsteroids = new List<DebugAsteroid>();
        var normalized = Utils.NormalizeFastSelf(dirToTest);
        for (int i = 0; i < Asteroids.Length; i++)
        {
            var asteroid = Asteroids[i];
            var dirToAsteroid = asteroid.transform.position - point;
            var distToAsteroid = dirToAsteroid.magnitude;
            if (distToAsteroid < maxDist)
            {
                var dirToAsteroidNorm = Utils.NormalizeFastSelf(dirToAsteroid);
                var isInRadius = Utils.IsAngLessNormazied(normalized, dirToAsteroidNorm, UtilsCos.COS_45_RAD);
                if (isInRadius)
                {
                    asteroid.DirToAsteroidNorm = dirToAsteroidNorm;
                    asteroid.DistToShip = distToAsteroid;
                    closestAsteroids.Add(asteroid);
                    Debug.DrawRay(asteroid.transform.position,Vector3.up*10,Color.magenta);
                }
            }
        }

        return closestAsteroids;
    }

    public float CurTurnRadius
    {
        get { return Mathf.Rad2Deg * (CurSpeed / TurnSpeed()); }
    }

    public float MaxTurnRadius => Mathf.Rad2Deg * (MaxSpeed() / TurnSpeed());

    private DebugAsteroid ClosestsToPoint(Vector3 v,out float dist,out Vector3 dir)
    {
        DebugAsteroid asteroid = null;
        var sDist = Single.MaxValue;
        Vector3 goodDir = Vector3.right;
        for (int i = 0; i < Asteroids.Length; i++)
        {
            var toTest = Asteroids[i];
            var tmpDir = toTest.transform.position - v;
            var tmpDist = tmpDir.sqrMagnitude;
            if (tmpDist < sDist)
            {
                sDist = tmpDist;
                goodDir = tmpDir;
                asteroid = toTest;
            }
        }

        dir = goodDir;
        dist = Mathf.Sqrt(sDist);
        return asteroid;
    }



    protected override float TurnSpeed()
    {
        return TURNsPEED;
    }

    public override float MaxSpeed()
    {
        return MaxSpeedDebug;
    }
}
