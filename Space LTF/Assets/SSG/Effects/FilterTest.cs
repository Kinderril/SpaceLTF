using UnityEngine;

[ExecuteInEditMode]
public class FilterTest 
{
    public enum DownSampleMode { Off, Half, Quarter }

  
    public static Shader _shader;

    public static DownSampleMode _downSampleMode = DownSampleMode.Quarter;

    public static int _iteration =2;

    public static Material _material;
    private static Material GetBlurMaterial()
    {
        if (_material != null)
            return _material;

        if (_shader == null)
        {
            _shader = Shader.Find("Hidden/Gaussian Blur Filter");
            if (_shader == null)
                Debug.LogError("Can't find 'Hidden/FastBlur'");
        }

        return _material = new Material(_shader);
    }
    public static void Blur(RenderTexture source, RenderTexture destination)
    {
        GetBlurMaterial();

        RenderTexture rt1, rt2;

        if (_downSampleMode == DownSampleMode.Half)
        {
            rt1 = RenderTexture.GetTemporary(source.width / 2, source.height / 2);
            rt2 = RenderTexture.GetTemporary(source.width / 2, source.height / 2);
            Graphics.Blit(source, rt1);
        }
        else if (_downSampleMode == DownSampleMode.Quarter)
        {
            rt1 = RenderTexture.GetTemporary(source.width / 4, source.height / 4);
            rt2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4);
            Graphics.Blit(source, rt1, _material, 0);
        }
        else
        {
            rt1 = RenderTexture.GetTemporary(source.width, source.height);
            rt2 = RenderTexture.GetTemporary(source.width, source.height);
            Graphics.Blit(source, rt1);
        }

        for (var i = 0; i < _iteration; i++)
        {
            Graphics.Blit(rt1, rt2, _material, 1);
            Graphics.Blit(rt2, rt1, _material, 2);
        }

        Graphics.Blit(rt1, destination);

        RenderTexture.ReleaseTemporary(rt1);
        RenderTexture.ReleaseTemporary(rt2);
    }
}
