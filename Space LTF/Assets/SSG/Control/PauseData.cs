using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PauseData
{
    public event Action OnPause;
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

    public bool IsPause { get; private set; }

    public void ChangeCoreSpeed(float coreSpeed)
    {
        _coreSpeed = coreSpeed;
        Unpase(_coreSpeed);
    }

    public void Pause()
    {
        IsPause = true;
        Time.timeScale = 0f;
        if (OnPause != null)
        {
            OnPause();
        }
    }

    public void Unpase(float to)
    {
        IsPause = false;
        Time.timeScale = to;
        if (OnPause != null)
        {
            OnPause();
        }
    }
}

