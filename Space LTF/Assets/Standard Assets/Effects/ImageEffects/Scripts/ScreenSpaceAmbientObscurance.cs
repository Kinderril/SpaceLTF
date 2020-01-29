using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [ ExecuteInEditMode]
    [RequireComponent (typeof(Camera))]
    [AddComponentMenu ("Image Effects/Rendering/Screen Space Ambient Obscurance")]
    class ScreenSpaceAmbientObscurance : PostEffectsBase {
        [Range (0,3)]
        public float intensity = 0.5f;
        [Range (0.1f,3)]
        public float radius = 0.2f;
        [Range (0,3)]
        public int blurIterations = 1;
        [Range (0,5)]
        public float blurFilterDistance = 1.25f;
        [Range (0,1)]
        public int downsample = 0;

        public Texture2D rand = null;
        public Shader aoShader= null;

        private Material aoMaterial = null;

        #region ShaderPropertiesCache
        private static readonly int _axis = Shader.PropertyToID("_Axis");
        private static readonly int _aoTex = Shader.PropertyToID("_AOTex");
        private static readonly int _projInfo = Shader.PropertyToID("_ProjInfo");
        private static readonly int _projectionInv = Shader.PropertyToID("_ProjectionInv");
        private static readonly int _rand = Shader.PropertyToID("_Rand");
        private static readonly int _radius = Shader.PropertyToID("_Radius");
        private static readonly int _radius2 = Shader.PropertyToID("_Radius2");
        private static readonly int _intensity = Shader.PropertyToID("_Intensity");
        private static readonly int _blurFilterDistance = Shader.PropertyToID("_BlurFilterDistance");
        #endregion

        public override bool CheckResources () {
            CheckSupport (true);

            aoMaterial = CheckShaderAndCreateMaterial (aoShader, aoMaterial);

            if (!isSupported)
                ReportAutoDisable ();
            return isSupported;
        }

        void OnDisable () {
            if (aoMaterial)
                DestroyImmediate (aoMaterial);
            aoMaterial = null;
        }

        [ImageEffectOpaque]
        void OnRenderImage (RenderTexture source, RenderTexture destination) {
            if (CheckResources () == false) {
                Graphics.Blit (source, destination);
                return;
            }

            Matrix4x4 P = GetComponent<Camera>().projectionMatrix;
            var invP= P.inverse;
            Vector4 projInfo = new Vector4
                ((-2.0f / (Screen.width * P[0])),
                 (-2.0f / (Screen.height * P[5])),
                 ((1.0f - P[2]) / P[0]),
                 ((1.0f + P[6]) / P[5]));

            aoMaterial.SetVector (_projInfo, projInfo); // used for unprojection
            aoMaterial.SetMatrix (_projectionInv, invP); // only used for reference
            aoMaterial.SetTexture (_rand, rand); // not needed for DX11 :)
            aoMaterial.SetFloat (_radius, radius);
            aoMaterial.SetFloat (_radius2, radius*radius);
            aoMaterial.SetFloat (_intensity, intensity);
            aoMaterial.SetFloat (_blurFilterDistance, blurFilterDistance);

            int rtW = source.width;
            int rtH = source.height;

            RenderTexture tmpRt  = RenderTexture.GetTemporary (rtW>>downsample, rtH>>downsample);
            RenderTexture tmpRt2;

            Graphics.Blit (source, tmpRt, aoMaterial, 0);

            if (downsample > 0) {
                tmpRt2 = RenderTexture.GetTemporary (rtW, rtH);
                Graphics.Blit(tmpRt, tmpRt2, aoMaterial, 4);
                RenderTexture.ReleaseTemporary (tmpRt);
                tmpRt = tmpRt2;

                // @NOTE: it's probably worth a shot to blur in low resolution
                //  instead with a bilat-upsample afterwards ...
            }

            for (int i = 0; i < blurIterations; i++) {
                aoMaterial.SetVector(_axis, new Vector2(1.0f,0.0f));
                tmpRt2 = RenderTexture.GetTemporary (rtW, rtH);
                Graphics.Blit (tmpRt, tmpRt2, aoMaterial, 1);
                RenderTexture.ReleaseTemporary (tmpRt);

                aoMaterial.SetVector(_axis, new Vector2(0.0f,1.0f));
                tmpRt = RenderTexture.GetTemporary (rtW, rtH);
                Graphics.Blit (tmpRt2, tmpRt, aoMaterial, 1);
                RenderTexture.ReleaseTemporary (tmpRt2);
            }

            aoMaterial.SetTexture (_aoTex, tmpRt);
            Graphics.Blit (source, destination, aoMaterial, 2);

            RenderTexture.ReleaseTemporary (tmpRt);
        }
    }
}
