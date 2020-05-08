using UnityEngine;
using System.Collections;

public class OutlinePostEffect : MonoBehaviour
{

    public Material mat;
    public RenderTexture srcrender;
    public RenderTexture destrender;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.CopyTexture(srcrender, destrender);

        BlurHelper.Modify(ref srcrender,ref destrender);

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
