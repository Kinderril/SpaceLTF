using System;
using UnityEngine;

namespace UnityStandardAssets.Water
{
    [ExecuteInEditMode]
    public class WaterBasic : MonoBehaviour
    {
        private static readonly int _waveSpeed = Shader.PropertyToID("WaveSpeed");
        private static readonly int _waveScale = Shader.PropertyToID("_WaveScale");
        private static readonly int _waveOffset = Shader.PropertyToID("_WaveOffset");

        void Update()
        {
            Renderer r = GetComponent<Renderer>();
            if (!r)
            {
                return;
            }
            Material mat = r.sharedMaterial;
            if (!mat)
            {
                return;
            }

            Vector4 waveSpeed = mat.GetVector(_waveSpeed);
            float waveScale = mat.GetFloat(_waveScale);
            float t = Time.time / 20.0f;

            Vector4 offset4 = waveSpeed * (t * waveScale);
            Vector4 offsetClamped = new Vector4(Mathf.Repeat(offset4.x, 1.0f), Mathf.Repeat(offset4.y, 1.0f),
                Mathf.Repeat(offset4.z, 1.0f), Mathf.Repeat(offset4.w, 1.0f));
            mat.SetVector(_waveOffset, offsetClamped);
        }
    }
}