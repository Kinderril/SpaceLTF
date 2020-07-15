using UnityEngine;
using System.Collections;

public class WayDrawPoint : MonoBehaviour
{
    public Transform RotateObject;

    public void SetLookDir(Vector3 pos,Vector3 dir)
    {
        
        RotateObject.LookAt(pos + dir);
    }
}
