using UnityEngine;
using UnityEngine.Rendering;

namespace UnityStandardAssets.CinematicEffects
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Cinematic/Ambient Occlusion")]
#if UNITY_5_4_OR_NEWER
    [ImageEffectAllowedInSceneView]
#endif
    public partial class AmbientOcclusion : MonoBehaviour
    {
        #region Public Properties

        /// Effect settings.
        [SerializeField]
        public Settings settings = Settings.defaultSettings;

        /// Checks if the ambient-only mode is supported under the current settings.
        public bool isAmbientOnlySupported
        {
            get { return TargetCamera.allowHDR && AOOcclusionSource == OcclusionSource.GBuffer; }
        }

        /// Checks if the G-buffer is available
        public bool isGBufferAvailable
        {
            get { return TargetCamera.actualRenderingPath == RenderingPath.DeferredShading; }
        }

        #endregion

        #region Private Properties

        // Properties referring to the current settings

        private float Intensity => settings.intensity;
        private float Radius => Mathf.Max(settings.radius, 1e-4f);
        private float CutOffDistance => Mathf.Max(settings.cutOffDistance, 1e-4f);
        private SampleCount AOSampleCount => settings.sampleCount;
        
        private int SampleCountValue
        {
            get
            {
                switch (settings.sampleCount)
                {
                    case SampleCount.Lowest: return 3;
                    case SampleCount.Low:    return 6;
                    case SampleCount.Medium: return 12;
                    case SampleCount.High:   return 20;
                }
                return Mathf.Clamp(settings.sampleCountValue, 1, 256);
            }
        }

        private OcclusionSource AOOcclusionSource
        {
            get
            {
                if (settings.occlusionSource == OcclusionSource.GBuffer && !isGBufferAvailable)
                    // An unavailable source was chosen: fallback to DepthNormalsTexture.
                    return OcclusionSource.DepthNormalsTexture;
                else
                    return settings.occlusionSource;
            }
        }

        private bool Downsampling => settings.downsampling;
        private bool AmbientOnly => settings.ambientOnly && !settings.debug && isAmbientOnlySupported;

        // Texture format used for storing AO
        private RenderTextureFormat AOTextureFormat
        {
            get
            {
                if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8))
                    return RenderTextureFormat.R8;
                else
                    return RenderTextureFormat.ARGB32;
            }
        }

        // AO shader
        private Shader AOShader
        {
            get
            {
                if (_aoShader == null)
                    _aoShader = Shader.Find("Hidden/Image Effects/Cinematic/AmbientOcclusion");
                return _aoShader;
            }
        }

        [SerializeField] Shader _aoShader;

        // Temporary aterial for the AO shader
        private Material AOMaterial
        {
            get
            {
                if (_aoMaterial == null)
                    _aoMaterial = ImageEffectHelper.CheckShaderAndCreateMaterial(AOShader);
                return _aoMaterial;
            }
        }

        Material _aoMaterial;

        // Command buffer for the AO pass
        private CommandBuffer AOCommands
        {
            get
            {
                if (_aoCommands == null)
                {
                    _aoCommands = new CommandBuffer();
                    _aoCommands.name = "AmbientOcclusion";
                }
                return _aoCommands;
            }
        }

        CommandBuffer _aoCommands;

        // Target camera
        private Camera TargetCamera => GetComponent<Camera>();

        // Property observer
        private PropertyObserver propertyObserver;

        // Reference to the quad mesh in the built-in assets
        // (used in MRT blitting)
        private Mesh QuadMesh => _quadMesh;

        [SerializeField] Mesh _quadMesh;

        #endregion

        #region ShaderPropertiesCache
        private static readonly int _blurVector = Shader.PropertyToID("_BlurVector");
        private static readonly int _occlusionTexture = Shader.PropertyToID("_OcclusionTexture");
        private static readonly int _intensity = Shader.PropertyToID("_Intensity");
        private static readonly int _radius = Shader.PropertyToID("_Radius");
        private static readonly int _cutOffDistance = Shader.PropertyToID("_CutOffDistance");
        private static readonly int _targetScale = Shader.PropertyToID("_TargetScale");
        private static readonly int _sampleCount = Shader.PropertyToID("_SampleCount");
        #endregion

        #region Effect Passes

        // Build commands for the AO pass (used in the ambient-only mode).
        void BuildAOCommands()
        {
            var cb = AOCommands;

            var tw = TargetCamera.pixelWidth;
            var th = TargetCamera.pixelHeight;
            var ts = Downsampling ? 2 : 1;
            var format = AOTextureFormat;
            var rwMode = RenderTextureReadWrite.Linear;
            var filter = FilterMode.Bilinear;

            // AO buffer
            var m = AOMaterial;
            var rtMask = Shader.PropertyToID("_OcclusionTexture");
            cb.GetTemporaryRT(rtMask, tw / ts, th / ts, 0, filter, format, rwMode);

            // AO estimation
            cb.Blit((Texture)null, rtMask, m, 2);

            // Blur buffer
            var rtBlur = Shader.PropertyToID("_OcclusionBlurTexture");

            // Primary blur filter (large kernel)
            cb.GetTemporaryRT(rtBlur, tw, th, 0, filter, format, rwMode);
            cb.SetGlobalVector("_BlurVector", Vector2.right * 2);
            cb.Blit(rtMask, rtBlur, m, 4);
            cb.ReleaseTemporaryRT(rtMask);

            cb.GetTemporaryRT(rtMask, tw, th, 0, filter, format, rwMode);
            cb.SetGlobalVector("_BlurVector", Vector2.up * 2 * ts);
            cb.Blit(rtBlur, rtMask, m, 4);
            cb.ReleaseTemporaryRT(rtBlur);

            // Secondary blur filter (small kernel)
            cb.GetTemporaryRT(rtBlur, tw, th, 0, filter, format, rwMode);
            cb.SetGlobalVector("_BlurVector", Vector2.right * ts);
            cb.Blit(rtMask, rtBlur, m, 6);
            cb.ReleaseTemporaryRT(rtMask);

            cb.GetTemporaryRT(rtMask, tw, th, 0, filter, format, rwMode);
            cb.SetGlobalVector("_BlurVector", Vector2.up * ts);
            cb.Blit(rtBlur, rtMask, m, 6);
            cb.ReleaseTemporaryRT(rtBlur);

            // Combine AO to the G-buffer.
            var mrt = new RenderTargetIdentifier[] {
                BuiltinRenderTextureType.GBuffer0,      // Albedo, Occ
                BuiltinRenderTextureType.CameraTarget   // Ambient
            };
            cb.SetRenderTarget(mrt, BuiltinRenderTextureType.CameraTarget);
            cb.SetGlobalTexture("_OcclusionTexture", rtMask);
            cb.DrawMesh(QuadMesh, Matrix4x4.identity, m, 0, 8);

            cb.ReleaseTemporaryRT(rtMask);
        }

        // Execute the AO pass immediately (used in the forward mode).
        void ExecuteAOPass(RenderTexture source, RenderTexture destination)
        {
            var tw = source.width;
            var th = source.height;
            var ts = Downsampling ? 2 : 1;
            var format = AOTextureFormat;
            var rwMode = RenderTextureReadWrite.Linear;
            var useGBuffer = settings.occlusionSource == OcclusionSource.GBuffer;

            // AO buffer
            var m = AOMaterial;
            var rtMask = RenderTexture.GetTemporary(tw / ts, th / ts, 0, format, rwMode);

            // AO estimation
            Graphics.Blit((Texture)null, rtMask, m, (int)AOOcclusionSource);

            // Primary blur filter (large kernel)
            var rtBlur = RenderTexture.GetTemporary(tw, th, 0, format, rwMode);
            m.SetVector(_blurVector, Vector2.right * 2);
            Graphics.Blit(rtMask, rtBlur, m, useGBuffer ? 4 : 3);
            RenderTexture.ReleaseTemporary(rtMask);

            rtMask = RenderTexture.GetTemporary(tw, th, 0, format, rwMode);
            m.SetVector(_blurVector, Vector2.up * 2 * ts);
            Graphics.Blit(rtBlur, rtMask, m, useGBuffer ? 4 : 3);
            RenderTexture.ReleaseTemporary(rtBlur);

            // Secondary blur filter (small kernel)
            rtBlur = RenderTexture.GetTemporary(tw, th, 0, format, rwMode);
            m.SetVector(_blurVector, Vector2.right * ts);
            Graphics.Blit(rtMask, rtBlur, m, useGBuffer ? 6 : 5);
            RenderTexture.ReleaseTemporary(rtMask);

            rtMask = RenderTexture.GetTemporary(tw, th, 0, format, rwMode);
            m.SetVector(_blurVector, Vector2.up * ts);
            Graphics.Blit(rtBlur, rtMask, m, useGBuffer ? 6 : 5);
            RenderTexture.ReleaseTemporary(rtBlur);

            // Combine AO with the source.
            m.SetTexture(_occlusionTexture, rtMask);

            if (!settings.debug)
                Graphics.Blit(source, destination, m, 7);
            else
                Graphics.Blit(source, destination, m, 9);

            RenderTexture.ReleaseTemporary(rtMask);
        }

        // Update the common material properties.
        void UpdateMaterialProperties()
        {
            var m = AOMaterial;
            m.SetFloat(_intensity, Intensity);
            m.SetFloat(_radius, Radius);
            m.SetFloat(_cutOffDistance, CutOffDistance);
            m.SetFloat(_targetScale, Downsampling ? 0.5f : 1);
            m.SetInt(_sampleCount, SampleCountValue);
        }

        #endregion

        #region MonoBehaviour Functions

        void OnEnable()
        {
            // Check if the shader is supported in the current platform.
            if (!ImageEffectHelper.IsSupported(AOShader, true, false, this))
            {
                enabled = false;
                return;
            }

            if (TargetCamera.CompareTag("MainCamera"))
                AOMaterial.EnableKeyword("SSAOMASKUSAGE");
            else
                AOMaterial.DisableKeyword("SSAOMASKUSAGE");
            
            // Register the command buffer if in the ambient-only mode.
            if (AmbientOnly)
                TargetCamera.AddCommandBuffer(CameraEvent.BeforeReflections, AOCommands);

            // Enable depth textures which the occlusion source requires.
            if (AOOcclusionSource == OcclusionSource.DepthTexture)
                TargetCamera.depthTextureMode |= DepthTextureMode.Depth;

            if (AOOcclusionSource != OcclusionSource.GBuffer)
                TargetCamera.depthTextureMode |= DepthTextureMode.DepthNormals;
        }

        void OnDisable()
        {
            // Destroy all the temporary resources.
            if (_aoMaterial != null) DestroyImmediate(_aoMaterial);
            _aoMaterial = null;

            if (_aoCommands != null)
                TargetCamera.RemoveCommandBuffer(CameraEvent.BeforeReflections, _aoCommands);
            _aoCommands = null;
        }

        void Update()
        {
            if (propertyObserver.CheckNeedsReset(settings, TargetCamera))
            {
                // Reinitialize all the resources by disabling/enabling itself.
                // This is not very efficient way but just works...
                OnDisable();
                OnEnable();

                // Build the command buffer if in the ambient-only mode.
                if (AmbientOnly)
                {
                    AOCommands.Clear();
                    BuildAOCommands();
                }

                propertyObserver.Update(settings, TargetCamera);
            }

            // Update the material properties (later used in the AO commands).
            if (AmbientOnly) UpdateMaterialProperties();
        }

        [ImageEffectOpaque]
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (AmbientOnly)
            {
                // Do nothing in the ambient-only mode.
                Graphics.Blit(source, destination);
            }
            else
            {
                // Execute the AO pass.
                UpdateMaterialProperties();
                ExecuteAOPass(source, destination);
            }
        }

        #endregion
    }
}
