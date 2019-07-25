using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArrowCatch : MonoBehaviour
{
    public Rigidbody object2move;
    public float Speed = 5f;

    void Update()
    {
//#if UNITY_EDITOR
        var w = Input.GetKey(KeyCode.W);
        var s = Input.GetKey(KeyCode.S);
        var d = Input.GetKey(KeyCode.D);
        var a = Input.GetKey(KeyCode.A);
        int x = 0;
        int y = 0;
        if (w)
        {
            x = 1;
        }
        else if (s)
        {
            x = -1;
        }

        if (d)
        {
            y = 1;
        }
        else if (a)
        {
            y = -1;
        }
        var keybordDir = (new Vector3(y, 0, x))* Speed;
        object2move.velocity = keybordDir;
//        Debug.Log(object2move.velocity);
//        Debug.Log(keybordDir);
        transform.localRotation = Quaternion.identity;
        //#endif
    }
}
