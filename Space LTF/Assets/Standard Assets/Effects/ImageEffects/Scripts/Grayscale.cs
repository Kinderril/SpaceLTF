using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/Color Adjustments/Grayscale")]
    public class Grayscale : ImageEffectBase {
        public Texture  textureRamp;
        public float    rampOffset;
        private static readonly int _rampTex = Shader.PropertyToID("_RampTex");
        private static readonly int _rampOffset = Shader.PropertyToID("_RampOffset");

        // Called by camera to apply image effect
        void OnRenderImage (RenderTexture source, RenderTexture destination) {
            material.SetTexture(_rampTex, textureRamp);
            material.SetFloat(_rampOffset, rampOffset);
            Graphics.Blit (source, destination, material);
        }
    }
}
