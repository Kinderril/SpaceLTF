using UnityEngine;
using System.Collections;

public class LightUpObject : MonoBehaviour
{
    public bool IsUsing = false;
    private float _endLightUpPeriod;
    

    // Update is called once per frame
    void Update()
    {
        if (!IsUsing)
        {
            return;
        }

        if (Time.time > _endLightUpPeriod)
        {
            IsUsing = false;
            gameObject.SetActive(IsUsing);
        }

    }

    public void UseFor(float lightUpPeriod)
    {
        IsUsing = true;
        gameObject.SetActive(IsUsing);
        _endLightUpPeriod = Time.time + lightUpPeriod;
    }
}
