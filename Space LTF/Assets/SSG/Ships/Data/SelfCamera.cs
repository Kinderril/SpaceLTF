using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class SelfCamera : MonoBehaviour
{
    public Camera Camera;
    private RenderTextureWithPool _textureWithPool;
    private RawImage _rawImage;
    private bool _isInited = false;

    public void Init(RawImage rawImage)
    {
        Camera.depth = 2;
        Camera.enabled = true;
        _rawImage = rawImage;
        _textureWithPool = DataBaseController.Instance.PoolRenderTextures.GetTexture();
//        _textureWithPool.RenderTexture.height
        Camera.targetTexture = _textureWithPool.RenderTexture;
        rawImage.texture = _textureWithPool.RenderTexture;
        _isInited = true;
    }

    public void Dispose()
    {
        if (_isInited)
        {
            Camera.enabled = false;
            Camera.targetTexture = null;
            if (_rawImage != null)
                _rawImage.texture = null;
            _textureWithPool.SetFree();
            _rawImage = null;
        }
    }
}

