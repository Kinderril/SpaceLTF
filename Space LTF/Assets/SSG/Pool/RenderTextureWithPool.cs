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

    public void Init(int w, int h)
    {
        RenderTexture = new RenderTexture(w,h,1);
    }

    public void SetFree()
    {
        IsFree = true;
    }

    public void SetUsed()
    {
        IsFree = false;
    }

    public void SetSize(int height)
    {
        Debug.LogError($"Set size {height}  {RenderTexture.name}");
        RenderTexture.height = height;
    }
}

