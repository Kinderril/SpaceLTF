using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PauseData
{
    public void Change()
    {
        if (Time.timeScale > 0.5f)
        {
            Pause();
        }
        else
        {
            Unpase();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Unpase()
    {
        Time.timeScale = 1f;
    }
}

