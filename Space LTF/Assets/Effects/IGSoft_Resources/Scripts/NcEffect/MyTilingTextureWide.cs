// ----------------------------------------------------------------------------------
//
// FXMaker
// Created by ismoon - 2012 - ismoonto@gmail.com
//
// ----------------------------------------------------------------------------------

using System;
using UnityEngine;
using System.Collections;

public class MyTilingTextureWide : NcEffectBehaviour
{


    private Texture TextureOffset;
    public string OffsetTextureName;
    public string TilingTextureName;
    private bool _offsetEnable = false;
    public float OffsetX_Speed = 0;
    public float OffsetY_Speed = 0;   
    public float TilingX_Speed = 0;
    public float TilingY_Speed = 0;       
    private float OffsetX_Speed_RND = 0;
    private float OffsetY_Speed_RND = 0;
    private Vector2 _offsetVector = Vector2.zero;
    private Vector2 _tilingVector = Vector2.one;
    private Vector2 _tilingDir = Vector2.zero;
    public bool EnableRandom;
    public bool EnableFloatingTiling;
    public bool EnableRandomMouseConnection;
    public float SecPeriodEnd;
    public float LerpRndomize = 0.5f;
    private float _lastChangeRnd;
    private float _lastChangeTiling;



    private Renderer renderer;
    // Loop Function --------------------------------------------------------------------
    void Start()
    {
        renderer = GetComponent<Renderer>();

        if (renderer != null && renderer.material != null)
		{
            if (OffsetTextureName.Length > 0)
            {
                _offsetEnable = true;
//            TextureOffset = renderer.material.GetTexture(OffsetTextureName);
//                _offsetEnable = TextureOffset != null;
            }

        }
	}

	void Update()
    {

        if (EnableFloatingTiling)
        {
            EndTiling();
        }

        if (_offsetEnable)
        {
            if (EnableRandom)
            {
                if (_lastChangeRnd < Time.time)
                {
                    _lastChangeRnd = Time.time + SecPeriodEnd;

                    if (EnableRandomMouseConnection)
                    {
                        var mousePOs = Input.mousePosition;
                        var dir = mousePOs - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
                        var totlal = Mathf.Abs(OffsetX_Speed) + Mathf.Abs(OffsetY_Speed);
                        dir = Utils.NormalizeFastSelf(dir);
                        dir.y = -dir.y;
                        dir *= totlal;



                        OffsetX_Speed_RND = Mathf.Lerp(OffsetX_Speed_RND, dir.x, LerpRndomize);
                        OffsetY_Speed_RND = Mathf.Lerp(OffsetY_Speed_RND, dir.y, LerpRndomize);
//                        var v = new Vector3(OffsetX_Speed_RND, OffsetY_Speed_RND);
//                        v = Utils.NormalizeFastSelf(v);
//                        OffsetX_Speed_RND = v.x * OffsetX_Speed;
//                        OffsetY_Speed_RND = v.z * OffsetY_Speed;

//                        Debug.LogError($"dir:{dir}  OffsetX_Speed_RND:{OffsetX_Speed_RND}   OffsetY_Speed_RND:{OffsetY_Speed_RND}");

                    }
                    else
                    {
                        var newOffsetX = MyExtensions.Random(-OffsetX_Speed, OffsetX_Speed);
                        var newOffsetY = MyExtensions.Random(-OffsetY_Speed, OffsetY_Speed);

                        OffsetX_Speed_RND = Mathf.Lerp(OffsetX_Speed_RND, newOffsetX, LerpRndomize);
                        OffsetY_Speed_RND = Mathf.Lerp(OffsetY_Speed_RND, newOffsetY, LerpRndomize);
                    }

                }
                _offsetVector.x +=  OffsetX_Speed_RND;
                _offsetVector.y +=  OffsetY_Speed_RND;
            }
            else
            {
                _offsetVector.x += MyExtensions.Random(0f, OffsetX_Speed);
                _offsetVector.y += MyExtensions.Random(0f, OffsetY_Speed);
            }
            renderer.material.SetTextureOffset(OffsetTextureName, _offsetVector);
        }

	}

    private void EndTiling()
    {

        if (_lastChangeTiling < Time.time)
        {
            _lastChangeTiling = Time.time + SecPeriodEnd;
            var xx = MyExtensions.Random(-TilingX_Speed, TilingX_Speed);
            var yy = MyExtensions.Random(-TilingY_Speed, TilingY_Speed);
            var newDirt =  new Vector2(xx,yy); 
            _tilingDir = Vector2.Lerp(_tilingDir, newDirt, LerpRndomize);
//            Debug.LogError(_tilingDir);
        }

//        var prevScale = _tilingVector;

        _tilingVector.x = Mathf.Clamp(_tilingVector.x + _tilingDir.x,0.8f,1.2f);
        _tilingVector.y = Mathf.Clamp(_tilingVector.y + _tilingDir.y,0.8f,1.2f);


        renderer.material.SetTextureScale(TilingTextureName, _tilingVector);

    }

    // Control Function -----------------------------------------------------------------
	// Event Function -------------------------------------------------------------------
	public override void OnUpdateToolData()
	{
	}
}

