using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AsteroidField : MonoBehaviour
{
//    public Vector3 p1 { get; private set; }
//    public Vector3 p2 { get; private set; }
//    public Vector3 p3 { get; private set; }
//    public Vector3 p4 { get; private set; }
//    public List<Vector3> points { get; private set; }ullet
    public List<Asteroid> AsteroidsPrefabs;
    public List<Asteroid> CurrentAsteroids;
    public int MinAsteroids = 4;
    public int MaxAsteroids = 8;

//    void Awake()
//    {
//        UpdatePoints();
//    }

    public void Init(float size)
    {
#if UNITY_EDITOR
        var box = GetComponent<BoxCollider>();
        box.size = size * Vector3.one;
#endif
        CurrentAsteroids = new List<Asteroid>();
        float minScale = 0.8f;
        float maxScale = 1.2f;
        float hs = size * 0.4f;
        var c = MyExtensions.Random(MinAsteroids, MaxAsteroids);
        for (int i = 0; i < c; i++)
        {
            var nAsteroid = DataBaseController.GetItem(AsteroidsPrefabs.RandomElement());
            nAsteroid.transform.SetParent(transform,false);
            var xx = MyExtensions.Random(-hs, hs);
            var zz = MyExtensions.Random(-hs, hs);
            var yy = MyExtensions.Random(-0.3f, 0.3f);
            var p = new Vector3(xx,yy,zz);
            nAsteroid.transform.localPosition = p;

            var xA = MyExtensions.Random(0f, 1f);
            var yA = MyExtensions.Random(0f, 1f);
            var zA = MyExtensions.Random(0f, 1f);
            var wA = MyExtensions.Random(0f, 1f);
            nAsteroid.transform.rotation = new Quaternion(xA,yA,zA,wA);

            var xS = MyExtensions.Random(minScale, maxScale);
            var yS = MyExtensions.Random(minScale, maxScale);
            var zS = MyExtensions.Random(minScale, maxScale);
            nAsteroid.transform.localScale = new Vector3(xS,yS,zS);

            nAsteroid.InitRad();
            CurrentAsteroids.Add(nAsteroid);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag(TagController.OBJECT_MOVER_TAG))
        {
            var ship = collider.GetComponent<ShipBase>();
            if (ship != null)
            {
                ship.AsteroidField(true);
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
                ship.AsteroidField(false);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
//        Gizmos.DrawSphere(p1,0.4f);
//        Gizmos.DrawSphere(p2,0.4f);
//        Gizmos.DrawSphere(p3,0.4f);
//        Gizmos.DrawSphere(p4,0.4f);
    }

    public void UpdatePoints()
    {

//        points = new List<Vector3>();
//        var collider = GetComponent<BoxCollider>();
//        var c = collider.center;
//        c = c + transform.position;
//        var s = collider.size   / 2;
//        s.x *= transform.localScale.x;
//        s.y *= transform.localScale.y;
//        s.z *= transform.localScale.z;
//        p1 = c + new Vector3(s.x, 0, s.z);
//        p2 = c + new Vector3(-s.x, 0, s.z);
//        p3 = c + new Vector3(s.x, 0, -s.z);
//        p4 = c + new Vector3(-s.x, 0, -s.z);


//        points.Add(p1);
//        points.Add(p2);
//        points.Add(p3);
//        points.Add(p4);

    }
}

