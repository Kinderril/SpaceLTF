using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class EffectController : Singleton<EffectController>
{
    private Transform effectsContainer;
    private const float WAIT_TRAIL_TIME = 4f;

    void Awake()
    {
        effectsContainer = transform;
    }

    public BaseEffectAbsorber Create(BaseEffectAbsorber ps, Transform transform,Transform from,Transform to, float delay)
    {
        var effect = DataBaseController.Instance.Pool.GetEffect(ps);
        effect.Play(from,to);
        LeaveEffect(effect, EffectController.Instance.transform, delay, transform);
        return effect;
//        effect.transform.localPosition = Vector3.zero;
    }

    public BaseEffectAbsorber Create(BaseEffectAbsorber ps, Transform transform, float delay)
    {
        var effect = DataBaseController.Instance.Pool.GetEffect(ps);
        effect.Play();
        LeaveEffect(effect, EffectController.Instance.transform, delay, transform);
        effect.transform.localPosition = Vector3.zero;
        return effect;
    }

    public BaseEffectAbsorber Create(BaseEffectAbsorber ps, Vector3 pos, float delay)
    {
        var effect = DataBaseController.Instance.Pool.GetEffect(ps);
        effect.Play();
        LeaveEffect(effect, EffectController.Instance.transform, delay, transform);
        effect.transform.position = pos;
        return effect;
    }

    public void LeaveEffect(BaseEffectAbsorber ps, Transform oldTransform,float delay = -1,Transform holder = null)
    {
        ps.RemeberScale();
        if (holder == null)
        {
            ps.transform.SetParent(effectsContainer, true);
        }
        else
        {
            ps.transform.SetParent(holder, true);
        }
        if (ps != null && ps.gameObject != null)
        {
            var d = delay > 0 ? delay : WAIT_TRAIL_TIME;
            StartCoroutine(ps.DestroyPS(oldTransform, d, "1"));
        }
    }

}

