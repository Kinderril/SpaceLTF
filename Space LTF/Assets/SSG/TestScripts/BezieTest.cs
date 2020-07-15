using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BezieTest : MonoBehaviour
{

    public Transform StartDir;
    public List<Transform> trs;

    void OnDrawGizmos()
    {
        if (StartDir == null)
        {
            return;
        }
        if (trs.Count <= 0)
        {
            return;
        }
        Vector3 curDir = StartDir.position - trs[0].position;
        List<Vector3> points = new List<Vector3>();
        foreach (var tr in trs)
        {
            points.Add(tr.position);
        }

        var bezie = BezieUtils.GetBeziePoints(curDir, points);
        foreach (var vector3 in bezie)
        {
            Gizmos.DrawSphere(vector3,0.1f);
        }
    }
}
