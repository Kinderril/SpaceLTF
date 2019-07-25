using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    public Transform Target;
    public Vector3 Offset;

    void Update()
    {
        var p = Target.position;
        transform.position = new Vector3(p.x + Offset.x,p.y + Offset.y, p.z + Offset.z);
    }
}
