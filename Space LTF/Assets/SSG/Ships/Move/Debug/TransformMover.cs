using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMover : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;
    public float zSpeed;
    // Update is called once per frame
    void Update()
    {
        var p = transform.position;
         transform.position = new Vector3(p.x + xSpeed,p.y + ySpeed,p.z + zSpeed);
    }
}
