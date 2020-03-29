using UnityEngine;
using System.Collections;

public class NcTilingRandomizer : MonoBehaviour
{

//    public NcTilingTexture Tiling;
    public MeshRenderer Renderer;
    private float _offsetX;
    private float _offsetY;
    private Vector2 _offset;
    void Start()
    {
        Renderer.material = Utils.CopyMaterial(Renderer.material);
        _offsetX = MyExtensions.Random(-0.2f, 0.2f);
        _offsetY = MyExtensions.Random(-0.2f, 0.2f);
//        Renderer.material.offse
    }

    void Update()
    {
        var xx = _offset.x + _offsetX * Time.deltaTime;
        var yy = _offset.y + _offsetY * Time.deltaTime;
        
        _offset = new Vector2(xx,yy );
        Renderer.material.SetTextureOffset("_MainTex", _offset);
//        .material.SetTextureOffset();
    }

}
