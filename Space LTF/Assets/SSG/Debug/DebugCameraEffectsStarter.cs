using UnityEngine;
using System.Collections;

public class DebugCameraEffectsStarter : MonoBehaviour
{
    public CameraEffects Effects;
    public float PeriodBloomTest = 2f;
    void Awake()
    {
        Effects.StartBloom(PeriodBloomTest);
    }

}
