using System;
using System.Collections.Generic;
using UnityEngine;


public class ShipPathController2
{
    private float DistCoef = 1.25f;

    private float RadiusToBeSafe = 1.25f;

    //    private bool _lastFrameBlocks;
    //    private SideTurn turnSideTurn = SideTurn.left;
    //    public Vector3 Target { get; private set; }
    private ShipBase _owner;
    //    private Vector3 _lastTarget;

    //    private Vector3? blockPosition = null;
    //    private Vector3? targetCorner = null;
    //    private PathDebugData _debugData;

    //#if DEBUG
    //    private SideTurn _lastSideTurn = SideTurn.left;
    //    private int _switchTimes = 0;
    //#endif

    public ShipPathController2(ShipBase owner, float distCoef)
    {
        DistCoef = distCoef;

        //        Dist = dist;
        //        _maskLayer = 1 << LayerMask.NameToLayer("Border");
        _owner = owner;
        //        _debugData = new PathDebugData();
    }

    public void Activate()
    {

        RadiusToBeSafe = _owner.CellController.Data.InsideRadius;
    }
    public Vector3 Target { get; private set; }
    private float _timeSpends = 0;
    private const float _maxTimeSpends = 0.75f;
    private Vector3 _lastDirection;

    public Vector3 GetCurentDirection(IShipData target, out bool exactlyPoint, out bool goodDir, out float speed)
    {
        var isInBlack = target.IsInBack();
        var targetDirection = GetCurentDirection(target.ShipLink.Position, out exactlyPoint, out goodDir, out speed);
        targetDirection = Utils.NormalizeFastSelf(targetDirection);
        if (_timeSpends < _maxTimeSpends && isInBlack)
        {
            _timeSpends += Time.deltaTime;
            return _lastDirection;
        }
        else
        {
            _timeSpends = 0f;
            _lastDirection = targetDirection;
            return targetDirection;
        }
    }

    public Vector3 GetCurentDirection(Vector3 vector3, out bool exactlyPoint, out bool goodDir, out float speed)
    {
#if UNITY_EDITOR
        Debug.DrawRay(_owner.Position,_owner.LookDirection,Color.yellow);
#endif
        var fromCenter = _owner.Position - _owner.CellController.Data.CenterZone;
        var distFromCnter = fromCenter.magnitude;
        var delta = RadiusToBeSafe - distFromCnter;
        var isBattleField = delta > 0;
        _owner.InBattlefield = isBattleField;
        if (!isBattleField)
        {
            //OUT OF FIELD CASE
            goodDir = false;
            exactlyPoint = false;
            speed = _owner.MaxSpeed();
            return -fromCenter;
        }

        if (delta < _owner.MaxTurnRadius)
        {
            //TODO
        }


        exactlyPoint = false;
        Target = vector3;
        Vector3 dirToTarget = vector3 - _owner.Position;
        var normalizedDir = Utils.NormalizeFastSelf(dirToTarget);
        if (!_owner.Cell.HaveAsteroids)
        {
            speed = _owner.MaxSpeed();
            exactlyPoint = true;
            goodDir = Utils.IsAngLessNormazied(normalizedDir, _owner.LookDirection, UtilsCos.COS_2_RAD);
            return normalizedDir;
        }

        var asteroids = _owner.Cell.GetAsteroidsForShip(_owner);
        //        float dist;
        //        Vector3 dirToAsteroid;


        //#if UNITY_EDITOR
        //
        //        foreach (var point in asteroids)
        //        {
        //            DrawUtils.DebugCircle(point.Position, Vector3.up, Color.white, point.Rad);
        //        }
        //#endif
        //        DrawUtils.DebugCircle(_owner.Position, Vector3.up, Color.green, _owner.CurTurnRadius);

        //        var isFront = Vector3.Dot(LookDirection, dirToTarget) > 0;
        var asteroidFindPointStart = _owner.Position - _owner.LookDirection * 3f;
        var field = GetAsteroidsInFront(_owner.LookDirection, asteroidFindPointStart, _owner.MaxTurnRadius * DistCoef,
            asteroids);
        if (field.Count == 0)
        {
            speed = _owner.MaxSpeed();
            goodDir = Utils.IsAngLessNormazied(normalizedDir, _owner.LookDirection, UtilsCos.COS_2_RAD);
#if UNITY_EDITOR
            Debug.DrawRay(_owner.Position, dirToTarget,Color.blue);
#endif
            return dirToTarget;
        }

        bool havePointBad = false;
        float maxProjectionDist = Single.MinValue;
        bool farestProjectionDiLeft = false;
        ShipAsteroidPoint controlSateroid = null;
        float blockingAsteroidDist = Single.MaxValue;
        ShipAsteroidPoint blockingAsteroid = null;

        foreach (var asteroid in field)
        {
            Debug.DrawLine(vector3, _owner.Position, Color.blue);

            var projection = AIUtility.GetProjectionPoint(asteroid.Position,
                _owner.Position + _owner.LookDirection * 100, _owner.Position);
            var projectionDist = (projection - asteroid.Position).magnitude;
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

            Debug.DrawLine(projection, asteroid.Position, affectableClose ? Color.red : Color.green);
            DrawUtils.DebugCircle(asteroid.Position, Vector3.up, Color.red, asteroid.Rad);
            if (projectionDist > maxProjectionDist)
            {
                var isLeft = Utils.FastDot(asteroid.DirToAsteroidNorm, _owner.LookLeft) > 0;
                farestProjectionDiLeft = isLeft;
                maxProjectionDist = projectionDist;
                controlSateroid = asteroid;
            }
        }

        if (controlSateroid != null)
        {
            Debug.DrawLine(controlSateroid.Position, _owner.Position, Color.white);
        }

        if (blockingAsteroid != null)
        {
            Debug.DrawLine(blockingAsteroid.Position, _owner.Position, Color.cyan);
        }

        if (havePointBad)
        {
            goodDir = false;
            if (farestProjectionDiLeft)
            {
                //            DrawUtils.DebugArrow(closestFront.transform.position, LookLeft, Color.yellow);     
                speed = _owner.MaxSpeed();
                return _owner.LookRight;
            }
            else
            {
                //            DrawUtils.DebugArrow(closestFront.transform.position, LookLeft, Color.yellow);     
                speed = _owner.MaxSpeed();
                return _owner.LookLeft;
            }
        }

        DrawUtils.DebugArrow(_owner.Position, _owner.LookDirection * _owner.MaxTurnRadius * 2, Color.yellow);
        speed = _owner.MaxSpeed();
        goodDir = Utils.IsAngLessNormazied(normalizedDir, _owner.LookDirection, UtilsCos.COS_2_RAD);
        return normalizedDir;
    }


    private List<ShipAsteroidPoint> GetAsteroidsInFront(Vector3 dirToTest, Vector3 point, float maxDist,
       List<ShipAsteroidPoint> Asteroids)
    {
        var closestAsteroids = new List<ShipAsteroidPoint>();
        var normalized = Utils.NormalizeFastSelf(dirToTest);
        for (int i = 0; i < Asteroids.Count; i++)
        {
            var asteroid = Asteroids[i];
            var dirToAsteroid = asteroid.Position - point;
            var distToAsteroid = dirToAsteroid.magnitude - asteroid.Rad;
            if (distToAsteroid < maxDist)
            {
                var dirToAsteroidNorm = Utils.NormalizeFastSelf(dirToAsteroid);
                var isInRadius = Utils.IsAngLessNormazied(normalized, dirToAsteroidNorm, UtilsCos.COS_45_RAD);
                if (isInRadius)
                {
                    asteroid.DirToAsteroidNorm = dirToAsteroidNorm;
                    asteroid.DistToShip = distToAsteroid;
                    closestAsteroids.Add(asteroid);
                }
                Debug.DrawRay(asteroid.Position, Vector3.up * 10, isInRadius ? Color.magenta : Color.yellow);
            }
        }

        return closestAsteroids;
    }


    public void OnDrawGizmosSelected()
    {
    }

    public bool Complete(Vector3 target, float checkDist = 0.5f)
    {
        var sDist = (target - _owner.Position).sqrMagnitude;
        if (sDist < checkDist)
        {
            return true;
        }

        return false;
    }
}