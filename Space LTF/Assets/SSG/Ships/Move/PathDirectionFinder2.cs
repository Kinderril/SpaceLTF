using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;



public static class PathDirectionFinder2
{
    public static Vector3 TryFindDirection(CellController cellController,Vector3 lookDirection,Vector3 lookLeft,Vector3 lookRight,AICell currentCell, 
        Vector3 targetPoint, Vector3 startPosition, float curTurnRad,
         out bool goodDir,out bool shallSlow)
    {
        var asteroids = currentCell.Asteroids;
        var dir = targetPoint - startPosition;
        var isLeft = Vector3.Dot(lookLeft, dir) > 0;
        bool isInFront = Vector3.Dot(lookDirection, dir) > 0;
        shallSlow = false;
        if (isInFront)
        {

        }
        else
        {
            Vector3 startRad;
            if (isLeft)
            {
                startRad = startPosition + lookLeft * curTurnRad;
            }
            else
            {
                startRad = startPosition + lookRight * curTurnRad;
            }

            shallSlow = false;
            var curTurnRadSqrt = curTurnRad * curTurnRad;
            foreach (var asteroid in asteroids)
            {
                var dir2 = asteroid.Position - startRad;
                var dist = dir2.sqrMagnitude;
                if (dist < curTurnRadSqrt)
                {
                    shallSlow = true;
                    break;
                }
            }
        }

        goodDir = true;
        return dir;

    }


}

