using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityStandardAssets.ImageEffects;
using Bloom = UnityEngine.Rendering.PostProcessing.Bloom;

//using UnityStandardAssets.ImageEffects;

public class CameraEffects : MonoBehaviour
{
    //    public Camera Camera;

    // public BloomOptimized Bloom;
    public Bloom Bloom;
    public ColorGrading ColorGrading;
    public PostProcessVolume ProcessVolume;
    private float _endBloomTime;
    private bool _isBloom;
    private bool _bloomRevers;
    private const float bloomMax = -0.05f;
    private float bloomStart = 1.3f;
    private float bloomPeriod = 1.3f;

    void Awake()
    {
        Bloom = ProcessVolume.profile.GetSetting<Bloom>();
        ColorGrading = ProcessVolume.profile.GetSetting<ColorGrading>();
        
//        bloomStart = Bloom.bloomThreshold;
        // bloomStart = Bloom.threshold;
    }

    public void StartBloom(float period)
    {
        if (period > 0 && Bloom != null)
        {
            _isBloom = true;
            _bloomRevers = false;
            bloomPeriod = period / 2f;
            _endBloomTime = Time.time + bloomPeriod;
        }
    }

    void Update()
    {
        if (_isBloom)
        {
            var subPer = (_endBloomTime - Time.time) / bloomPeriod;

            var per = _bloomRevers ? subPer : 1f - subPer;
            var val = Easing.EaseInBounce(bloomStart, bloomMax, per);
//            Bloom.threshold.se val;
            Bloom.threshold.value = val;
            if (subPer < 0f)
            {
                if (_bloomRevers)
                {
                    EndBloom();
                }
                else
                {
                    ReversBloom();
                }
            }
        }
    }

    private void ReversBloom()
    {
        _endBloomTime = bloomPeriod + Time.time;
        _bloomRevers = true;
    }

    private void EndBloom()
    {
        Bloom.threshold.value = bloomStart;
        _isBloom = false;
    }

    public void SetEvent(EBattlefildEventType? eventType)
    {
        if (ColorGrading != null)
        {
            if (eventType.HasValue)
            {
                switch (eventType)
                {
                    case EBattlefildEventType.fireVortex:
                        ColorGrading.temperature.value = 50f;
                        break;
                    case EBattlefildEventType.IceZone:
                        ColorGrading.temperature.value = -50f;
                        break;
                    default:
                        ColorGrading.temperature.value = 0f;
                        break;
                }
            }
            else
            {
                ColorGrading.temperature.value = 0f;
            }
        }
    }

    public void SetNormalTemperature()
    {
       if (ColorGrading != null)
       {
           ColorGrading.temperature.value = 0f;
       }
    }

}
