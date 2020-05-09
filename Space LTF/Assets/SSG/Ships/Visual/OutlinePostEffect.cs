using UnityEngine;
using System.Collections;

public class OutlinePostEffect : MonoBehaviour
{

    public Material mat;
    public RenderTexture masktexture3;
    public RenderTexture masktexture4;
    public RenderTexture blured3;
    public RenderTexture blured4;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.CopyTexture(masktexture3, blured3);
        Graphics.CopyTexture(masktexture4, blured4);

        BlurHelper.Modify(ref masktexture3,ref blured3);
        BlurHelper.Modify(ref masktexture4,ref blured4);

//        var copy = RenderTexture.GetTemporary(destrender.descriptor);
//        BlurHelper.MakeBlur(destrender, copy, 4f);
//        RenderTexture.ReleaseTemporary(copy);

        Graphics.Blit(src, dest, mat);
    }

    void Update()
    {
//        Graphics.Blit(srcrender, destrender, mat);
    }
}
