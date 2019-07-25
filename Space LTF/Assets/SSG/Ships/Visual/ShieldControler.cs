using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ShieldControler : MonoBehaviour
{
    public MeshRenderer ShieldRender;

    public void Init(Material mat)
    {
        ShieldRender.material = mat;
    }
}

