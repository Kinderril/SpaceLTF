using UnityEngine;
using System.Collections;
using UnityEngine.Profiling;

public class BlurHelper : MonoBehaviour
{
    static bool _flip;

    private static int _colorFieldID;
    private static int _srcfactorFieldID;
    private static int _dstfactorFieldID;
    private static int _operationFieldID;
    private static int _intensityFieldID;
    private static int _blurOffsets0FieldID;
    private static int _texAFieldID;
    private static int _texBFieldID;
    private static int _tFieldID;
    private static int _maskColorFieldID;
    private static int _addColorFieldID;
    private static int _multColorFieldID;
    private static int _maskFieldID;
    private static int _offsetsFieldID;

    private static bool _isFiledIdsInitialized = false;
    private static void InitFieldIds()
    {
        if (_isFiledIdsInitialized) return;

        _colorFieldID = Shader.PropertyToID("_Color");
        _srcfactorFieldID = Shader.PropertyToID("_SrcFactor");
        _dstfactorFieldID = Shader.PropertyToID("_DstFactor");
        _operationFieldID = Shader.PropertyToID("_Operation");
        _intensityFieldID = Shader.PropertyToID("_Intensity");
        _blurOffsets0FieldID = Shader.PropertyToID("_BlurOffsets0");
        _texAFieldID = Shader.PropertyToID("_TexA");
        _texBFieldID = Shader.PropertyToID("_TexB");
        _tFieldID = Shader.PropertyToID("_T");
        _maskColorFieldID = Shader.PropertyToID("_MaskColor");
        _addColorFieldID = Shader.PropertyToID("_AddColor");
        _multColorFieldID = Shader.PropertyToID("_MultColor");
        _maskFieldID = Shader.PropertyToID("_Mask");
        _offsetsFieldID = Shader.PropertyToID("_Offsets");
        _isFiledIdsInitialized = true;
    }
    static Material _blurMaterial;
    static Material BlurMaterial
    {
        get
        {
            if (_blurMaterial == null)
                _blurMaterial = new Material(Shader.Find("Hidden/Blur"));
            return _blurMaterial;
        }
    }

    public static void MakeBlur(RenderTexture source, RenderTexture temp, float blurInPixels, float intensity = 1f)
    {
        MakeBlur(source, temp, new Vector2(blurInPixels / source.width, blurInPixels / source.height), intensity);
    }

    public static void MakeBlur(RenderTexture source, RenderTexture temp, Vector2 blur, float intensity = 1f)
    {
        UnityEngine.Profiling.Profiler.BeginSample("GraphicHelper.MakeBlur");
        InitFieldIds();
        Material material = BlurMaterial;
        if (material == null)
        {
            UnityEngine.Profiling.Profiler.EndSample();
            return;
        }
        material.SetFloat(_intensityFieldID, intensity);
        material.mainTexture = source;
        material.SetVector(_blurOffsets0FieldID, new Vector2(blur.x, 0));
        DrawQuad(material, temp);

        material.mainTexture = temp;
        material.SetVector(_blurOffsets0FieldID, new Vector2(0, blur.y));
        DrawQuad(material, source);
        UnityEngine.Profiling.Profiler.EndSample();
    }
    public static void DrawQuad(Material material, RenderTexture destination, int pass = 0)
    {
        Profiler.BeginSample("GraphicHelper.DrawQuad");
        if (destination != null) Graphics.SetRenderTarget(destination);
        material.SetPass(pass);
        GL.Begin(GL.QUADS);
        if (_flip)
        {
            GL.TexCoord2(0, 1);
            GL.Vertex3(-1, -1, 0);

            GL.TexCoord2(0, 0);
            GL.Vertex3(-1, 1, 0);

            GL.TexCoord2(1, 0);
            GL.Vertex3(1, 1, 0);

            GL.TexCoord2(1, 1);
            GL.Vertex3(1, -1, 0);
        }
        else
        {
            GL.TexCoord2(0, 0);
            GL.Vertex3(-1, -1, 0);

            GL.TexCoord2(0, 1);
            GL.Vertex3(-1, 1, 0);

            GL.TexCoord2(1, 1);
            GL.Vertex3(1, 1, 0);

            GL.TexCoord2(1, 0);
            GL.Vertex3(1, -1, 0);
        }
        GL.End();
        Profiler.EndSample();
    }
}
