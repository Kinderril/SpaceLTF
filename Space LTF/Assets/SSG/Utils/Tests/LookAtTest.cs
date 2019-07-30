using UnityEngine;
using System.Collections;

public class LookAtTest : MonoBehaviour
{
    public Transform lookTo;
    public Vector3 Up;


    void Update()
    {
          transform.LookAt(lookTo.position, Up);
    }
}
