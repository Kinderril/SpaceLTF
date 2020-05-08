using UnityEngine;
using System.Collections;
using UnityEngine.Profiling;

public class BlurHelper : MonoBehaviour
{
    private static bool _flip = false;

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

    private static Material _blurMaterial;

    private static Material BlurMaterial
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

    private static Shader _blurShader;

    private static Material GetBlurMaterial()
    {
        if (_blurMaterial != null)
            return _blurMaterial;

        if (_blurShader == null)
        {
            _blurShader = Shader.Find("Hidden/FastBlur");
            if (_blurShader == null) Debug.LogError("Can't find 'Hidden/FastBlur'");
        }

        return _blurMaterial = new Material(_blurShader);
    }

    private const string PARAMETER = "_Parameter";
    [Range(1, 4)] public static int blurIterations = 2;
    [Range(0, 2)] public static int downsample = 1;
    [Range(0.0f, 10.0f)] public static float blurSize = 3.0f;

    public static void Modify(ref RenderTexture source, ref RenderTexture myTex)
    {
        var blurMaterial = GetBlurMaterial();

        var widthMod = 1.0f / (1.0f * (1 << downsample));

        blurMaterial.SetVector(PARAMETER, new Vector4(blurSize * widthMod, -blurSize * widthMod, 0.0f, 0.0f));
        source.filterMode = FilterMode.Bilinear;

        var rtW = source.width >> downsample;
        var rtH = source.height >> downsample;

        // downsample
        var rt = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);

        rt.filterMode = FilterMode.Bilinear;
        Graphics.Blit(source, rt, blurMaterial, 0);

        for (var i = 0; i < blurIterations; i++)
        {
            var iterationOffs = i * 1.0f;
            blurMaterial.SetVector(PARAMETER,
                new Vector4(blurSize * widthMod + iterationOffs, -blurSize * widthMod - iterationOffs, 0.0f, 0.0f));

            // vertical blur
            var rt2 = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);
            rt2.filterMode = FilterMode.Bilinear;
            Graphics.Blit(rt, rt2, blurMaterial, 1);
            RenderTexture.ReleaseTemporary(rt);
            rt = rt2;

            // horizontal blur
            rt2 = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);
            rt2.filterMode = FilterMode.Bilinear;
            Graphics.Blit(rt, rt2, blurMaterial, 2);
            RenderTexture.ReleaseTemporary(rt);
            rt = rt2;
        }

        Graphics.Blit(rt, myTex);

        RenderTexture.ReleaseTemporary(rt);
    }

    public static void MakeBlur(RenderTexture source, RenderTexture temp, Vector2 blur, float intensity = 1f)
    {
//        source = 
        Profiler.BeginSample("GraphicHelper.MakeBlur");
        InitFieldIds();
        var material = BlurMaterial;
        if (material == null)
        {
            Profiler.EndSample();
            return;
        }

        material.SetFloat(_intensityFieldID, intensity);
        material.mainTexture = source;
        material.SetVector(_blurOffsets0FieldID, new Vector2(blur.x, 0));
        DrawQuad(material, temp);

        material.mainTexture = temp;
        material.SetVector(_blurOffsets0FieldID, new Vector2(0, blur.y));
        DrawQuad(material, source);
        Profiler.EndSample();
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