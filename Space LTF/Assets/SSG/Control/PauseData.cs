using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PauseData
{
    private float _coreSpeed = 1f;
    public void Change()
    {
        if (IsPause)
        {
            Unpase(_coreSpeed);
        }
        else
        {
            Pause();
        }
    }

    private bool IsPause => Time.timeScale < 0.1f;

    public void ChangeCoreSpeed(float coreSpeed)
    {
        _coreSpeed = coreSpeed;
        if (!IsPause)
        {
            Unpase(_coreSpeed);
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Unpase(float to)
    {
        Time.timeScale = to;
    }
}

