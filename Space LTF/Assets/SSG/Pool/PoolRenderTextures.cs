using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class PoolRenderTextures : MonoBehaviour
{
    public RenderTexture MainRenderTexture;
    public Camera Backgroundcamera;
    public List<RenderTextureWithPool> _PoolList = new List<RenderTextureWithPool>();

    public void Init()
    {
        var hh = Screen.height * MainRenderTexture.width / Screen.width;
        MainRenderTexture.height = hh;
        Backgroundcamera.depth = 10;
        if (_PoolList.Count < 7)
        {
            Debug.LogError("Wrong pool count PoolRenderTextures.   _PoolList.Count: " + _PoolList.Count);
        }
//        Debug.LogError($"Render texture {800} {hh}");
        foreach (var renderTextureWithPool in _PoolList)
        {
            renderTextureWithPool.Init(800,hh);
            renderTextureWithPool.SetFree();
            if (renderTextureWithPool.RenderTexture == null)
            {
                Debug.LogError("Wrong render texture");
            }
        }
    }

    void Update()
    {
        foreach (var renderTextureWithPool in _PoolList)
        {
            if (!renderTextureWithPool.IsFree)
            {
                Graphics.CopyTexture(MainRenderTexture, renderTextureWithPool.RenderTexture);
            }
        }
    }

    public RenderTextureWithPool GetTexture()
    {
        foreach (var renderTextureWithPool in _PoolList)
        {
            if (renderTextureWithPool.IsFree)
            {
                renderTextureWithPool.SetUsed();
                return renderTextureWithPool;
            }
        }
        return null;
    }
}

