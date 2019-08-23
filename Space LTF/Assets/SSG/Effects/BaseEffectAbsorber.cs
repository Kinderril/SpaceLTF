using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//public enum EffectLeavePos
//{
//    stayAtPlay,
//    followHitPointTransform
//}

public class BaseEffectAbsorber : PoolElement
{
    public virtual void Play(Transform from, Transform to)
    {
        isUsing = true;
    }

    public virtual void Play(Vector3 pos1, Vector3 pos2)
    {
        isUsing = true;
    }

    public virtual void UpdatePositions(Vector3 pos1, Vector3 pos2)
    {

    }

    public virtual void Play()
    {
        isUsing = true;
    }

    public virtual void Stop()
    {
        isUsing = false;
    }


    public IEnumerator DestroyPS(Transform oldTransform, float waitTime = 4f, string reason = "")
    {
        yield return new WaitForSeconds(waitTime);
        Stop();
        try
        {
            if (gameObject != null)
            {
                gameObject.transform.SetParent(oldTransform, false);
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localScale = _rememberredScale;
            }
        }
        catch (Exception e)
        {
            if (gameObject != null)
                Debug.LogError("destroy PS error " + gameObject.name);
            else
            {

                Debug.LogError("DestroyPS PS error STARGE ");
            }
        }
    }

    public virtual void SetColor(Color color)
    {

    }

    public virtual void StopEmmision()
    {

    }

    public virtual void StartEmmision()
    {
        
    }

    protected virtual void UpdateManual()
    {
        
    }

    void Update()
    {
        UpdateManual();
    }

    private Vector3 _rememberredScale;
    public void RemeberScale()
    {
        _rememberredScale = transform.localScale;
    }
}

