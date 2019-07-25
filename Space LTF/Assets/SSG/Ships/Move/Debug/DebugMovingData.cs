using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public struct DebugPathStep
{
    public Vector3 dir;
    public Vector3 lookDir;
    public bool isExact;
    public float time;

    public string Info()
    {
        return "dir:" + dir + "  exact:" + isExact + " " + time + "  lookDir:" + lookDir;
    }
}

public  class DebugMovingData
{
    private const int c = 10;
    DebugPathStep[] dirs = new DebugPathStep[c];
    private int lastIndex = 0;

    public DebugMovingData()
    {
        for (int i = 0; i < c; i++)
        {
            dirs[i] = new DebugPathStep();
        }
    }

    internal void AddDir(Vector3 dir, bool v,Vector3 lookDir)
    {
        var d = dirs[lastIndex];
        d.isExact = v;
        d.dir = dir;
        d.lookDir = lookDir;
        d.time = Time.time;
        lastIndex++;
        if (lastIndex >= c)
        {
            lastIndex = 0;
        }
    }

    public List<DebugPathStep> OrderedDirs()
    {
        List<DebugPathStep> list = new List<DebugPathStep>();
        for (int i = 0; i < c; i++)
        {
            var d = dirs[lastIndex];
            list.Add(d);
            lastIndex--;
            if (lastIndex < 0)
            {
                lastIndex = c - 1;
            }
        }
        return list;
    } 
}

