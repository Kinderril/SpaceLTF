using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class EndAnimCatcher : MonoBehaviour
{
    public FlyNumberWithDependence _info;

    public void EndUse()
    {
        _info.EndUse();
    }
}

