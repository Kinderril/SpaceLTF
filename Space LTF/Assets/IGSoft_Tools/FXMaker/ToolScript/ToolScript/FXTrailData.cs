using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class FXTrailData : MonoBehaviour
{
    public GameObject Go;
    [HideInInspector]
    public float Delta;
    [HideInInspector]
    public float End;
    [HideInInspector]
    public bool IsActive;

    public void Activate(float p)
    {
        Delta = p;
        End = Delta + Time.time;
        IsActive = true;
        Go.SetActive(IsActive);
    }

    public void ManualUpdate(Vector3 s, Vector3 e)
    {
        if (IsActive)
        {
            var remainTime = (End - Time.time);
            if (remainTime < 0)
            {
                Stop();
                return;
            }
            var cur = 1 - remainTime / Delta;
            Go.transform.position = Vector3.Lerp(s, e, cur);
        }
    }

    public void Stop()
    {
        IsActive = false;
        Go.SetActive(IsActive);
    }


}
