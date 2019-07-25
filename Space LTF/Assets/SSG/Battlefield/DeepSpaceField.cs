using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DeepSpaceField : MonoBehaviour
{
    public Vector3 p1 { get; private set; }
    public Vector3 p2 { get; private set; }
    public Vector3 p3 { get; private set; }
    public Vector3 p4 { get; private set; }
    public List<Vector3> points { get; private set; }

    void Awake()
    {
        UpdatePoints();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag(TagController.OBJECT_MOVER_TAG))
        {
            var ship = collider.GetComponent<ShipBase>();
            if (ship != null)
            {
//                ship.AsteroidField(true);
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag(TagController.OBJECT_MOVER_TAG))
        {
            var ship = collider.GetComponent<ShipBase>();
            if (ship != null)
            {
//                ship.AsteroidField(false);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(p1,0.4f);
        Gizmos.DrawSphere(p2,0.4f);
        Gizmos.DrawSphere(p3,0.4f);
        Gizmos.DrawSphere(p4,0.4f);
    }

    public void UpdatePoints()
    {

        points = new List<Vector3>();
        var collider = GetComponent<BoxCollider>();
        var c = collider.center;
        c = c + transform.position;
        var s = collider.size   / 2;
        s.x *= transform.localScale.x;
        s.y *= transform.localScale.y;
        s.z *= transform.localScale.z;
        p1 = c + new Vector3(s.x, 0, s.z);
        p2 = c + new Vector3(-s.x, 0, s.z);
        p3 = c + new Vector3(s.x, 0, -s.z);
        p4 = c + new Vector3(-s.x, 0, -s.z);


        points.Add(p1);
        points.Add(p2);
        points.Add(p3);
        points.Add(p4);

    }
}

