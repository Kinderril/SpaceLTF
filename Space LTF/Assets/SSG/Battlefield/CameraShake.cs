using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;

    // How long the object should shake for.
    public float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    private float _shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;
    private bool isShake = false;

    void Awake()
    {
        _shakeAmount = shakeAmount;
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    public void Init(float duration,float shakeAmount1 = -1)
    {
        if (shakeAmount1 < 0)
        {
            _shakeAmount = shakeAmount;
        }
        else
        {
            _shakeAmount = shakeAmount1;
        }
        originalPos = camTransform.localPosition;
        shakeDuration = duration;
        isShake = true;
    }

    void Update()
    {
        if (isShake)
        {
            if (Time.deltaTime > 0)
            {
                if (shakeDuration > 0)
                {
                    camTransform.localPosition = originalPos + Random.insideUnitSphere * _shakeAmount;
                    shakeDuration -= Time.deltaTime * decreaseFactor;
                }
                else
                {
                    shakeDuration = 0f;
                    camTransform.localPosition = originalPos;
                    isShake = false;
                }
            }
        }
    }
}