using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Battlefield : MonoBehaviour
{
//    public List<ShipBornPosition> SideABorns = new List<ShipBornPosition>();
//    public List<ShipBornPosition> SideBBorns = new List<ShipBornPosition>();

    public BoxCollider BorderCollider;
    public Vector3 Max { get; private set; }
    public Vector3 Min { get; private set; }

    public BackgroundSpace BackgroundSpace;
    public CellController CellController;

    void Awake()
    {
        var bounds = BorderCollider.bounds;
        Max = bounds.max;
        Min = bounds.min;
        BorderCollider.enabled = false;
        Debug.DrawRay(Max,Vector3.up,Color.red,10f);
        Debug.DrawRay(Min,Vector3.up,Color.green,10f);

    }
    
    public void Dispose()
    {
        
    }
}
