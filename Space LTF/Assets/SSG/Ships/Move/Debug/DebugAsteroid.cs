using UnityEngine;
using System.Collections;

public class DebugAsteroid : MonoBehaviour
{
    public float Rad;
    public Vector3 DirToAsteroidNorm { get; set; }
    public float DistToShip { get; set; }

    void Awake()
    {
        Rad = transform.localScale.x/2f;
    }
}
