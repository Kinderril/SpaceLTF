using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public  class RenderTextureWithPool
{
    public RenderTexture RenderTexture;
    public bool IsFree;

    public void SetFree()
    {
        IsFree = true;
    }

    public void SetUsed()
    {
        IsFree = false;
    }
}

