using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipWayDrawler : MonoBehaviour
{
    public RoadMeshCreator MeshCreator;



    public void DoDraw(List<Vector3> points)
    {
        gameObject.SetActive(true);
        MeshCreator.CreateByPoints(points);
    }

    public void Clear()
    {
        gameObject.SetActive(false);
    }
}
