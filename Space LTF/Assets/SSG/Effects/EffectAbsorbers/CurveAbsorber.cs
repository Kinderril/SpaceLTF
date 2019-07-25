using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class CurveAbsorber : BaseEffectAbsorber
{
    public NcCurveAnimation uvAnimation;
    private MeshRenderer renderer;

    void Awake()
    {
        if (uvAnimation == null)
        {
            uvAnimation = GetComponent<NcCurveAnimation>();
        }
        if (renderer == null)
        {
            renderer = GetComponent<MeshRenderer>();
        }
    }

    public override void Play()
    {
        uvAnimation.gameObject.SetActive(true);
        if (renderer != null)
            renderer.enabled = true;
        uvAnimation.Play();
        base.Play();
    }

    public override void Stop()
    {
        if (uvAnimation != null && uvAnimation.gameObject != null)
            uvAnimation.gameObject.SetActive(false);
        base.Stop();
    }
}

