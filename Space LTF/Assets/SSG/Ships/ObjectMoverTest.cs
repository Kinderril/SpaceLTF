using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ObjectMoverTest : MonoBehaviour
{
    public float AngSpeed = 1;

    void Start()
    {
        TEst2();
    }


    private void TEst2()
    {
        Debug.Log(EulerLerp.Lerp(350, 10, 0.25f));
        Debug.Log(EulerLerp.Lerp(10, 200, 0.25f));
        Debug.Log(EulerLerp.Lerp(190, 90, 0.25f));
        Debug.Log(EulerLerp.Lerp(190,356, 0.25f));

        Debug.Log(EulerLerp.LerpVectorByY(new Vector3(1,0,0), new Vector3(0, 0, 1), 0.25f));
        Debug.Log(EulerLerp.LerpVectorByY(new Vector3(-1,0,0), new Vector3(0, 0, 1), 0.25f));
        Debug.Log(EulerLerp.LerpVectorByY(new Vector3(-1,0,0), new Vector3(0, 0, -1), 0.25f));
        Debug.Log(EulerLerp.LerpVectorByY(new Vector3(1,0,0), new Vector3(0, 0, -1), 0.25f));
    }

    private void Test1()
    {

        var dirFrom = new Vector3(-1, 0, 0);
        var dirTo = new Vector3(0, 0, 1);
        var ang = Vector3.Angle(dirTo, dirFrom);
        var steps = ang / AngSpeed;



        var q = Quaternion.FromToRotation(Vector3.forward, dirTo);
        var q2 = Quaternion.FromToRotation(Vector3.forward, dirFrom);
        var percentOfRotate = 0.25f;

        var startEuler = q2.eulerAngles;

        Debug.Log("dirFrom:" + dirFrom + "   dirTo:" + dirTo + "   startAng:" + ang + "   " + startEuler);

        var resultRotation = Quaternion.Lerp(q2, q, percentOfRotate);

        Debug.Log(q + "   " + q2 + "   " + resultRotation);

        var resultDir = resultRotation.eulerAngles;
        var yy = resultDir.y * Mathf.Deg2Rad;
        var v = new Vector3(Mathf.Sin(yy), 0, Mathf.Cos(yy));

        var ang2 = Vector3.Angle(v, dirFrom);
        var angMustRotated = ang * percentOfRotate;
        Debug.Log("(" + v.x.ToString("0.00") + "," + v.z.ToString("0.00") + ")   angMustRotated:" + angMustRotated + "   factAngle:" + ang2 + "    resultEuler:" + resultDir);

    }

    private void RotateFromTo(Vector3 fromDir,Vector3 toDir)
    {
        var fromEuler = DirToYRotattion(fromDir);
        var toEuler = DirToYRotattion(toDir);
    }

    private float DirToYRotattion(Vector3 dir)
    {
        dir.y = 0;
        dir.Normalize();
        return Mathf.Acos(dir.z)*Mathf.Rad2Deg;
    }

   
}

