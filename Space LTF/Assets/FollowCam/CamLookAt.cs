using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class CamLookAt : MonoBehaviour
{
    public Transform trg;

    void LateUpdate()
    {
        transform.LookAt(trg);
    }
}

