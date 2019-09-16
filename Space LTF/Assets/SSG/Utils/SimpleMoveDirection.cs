using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMoveDirection : MonoBehaviour
{
    public float Ang = 90;
    public float Speed = 1;

    // Update is called once per frame
    void Update()
    {
        var rad = (transform.rotation.eulerAngles.y - Ang) * Mathf.Deg2Rad;
        var cos = Mathf.Cos(rad);
        var sin = Mathf.Sin(rad);
        Vector3 dir = Utils.NormalizeFastSelf(new Vector3(cos,0,sin));
        var dd = Speed * Time.deltaTime;

        transform.position = transform.position + dir * dd;
    }
}
