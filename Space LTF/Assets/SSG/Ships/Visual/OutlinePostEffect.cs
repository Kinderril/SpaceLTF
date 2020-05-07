using UnityEngine;
using System.Collections;

public class OutlinePostEffect : MonoBehaviour
{

    public Material mat;
    public RenderTexture srcrender;
    public RenderTexture destrender;

//    void OnRenderImage(RenderTexture src, RenderTexture dest)
//    {
////        Graphics.Blit(src, destrender, mat);
////        Graphics.Blit(src, dest, mat);
//    }

    void Update()
    {
        Graphics.Blit(srcrender, destrender, mat);
    }
}
