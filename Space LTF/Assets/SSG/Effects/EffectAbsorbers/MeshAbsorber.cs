using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MeshAbsorber : BaseEffectAbsorber
{
    public GameObject obj;
    public override void Play()
    {
        obj.SetActive(true);
        base.Play();
    }

    public override void Stop()
    {
        obj.SetActive(false);

        base.Stop();
    }
}

