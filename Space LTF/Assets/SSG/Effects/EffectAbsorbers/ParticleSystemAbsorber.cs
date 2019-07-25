
using UnityEngine;



public class ParticleSystemAbsorber : BaseEffectAbsorber {

    public ParticleSystem ParticleSystem;

    void Awake()
    {
        if (ParticleSystem == null)
        {
            ParticleSystem = GetComponent<ParticleSystem>();
        }
    }

    public override void Play()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        ParticleSystem.enableEmission = true;
        base.Play();
    }

    public override void Stop()
    {
        gameObject.SetActive(false);
        base.Stop();
    }

    public override void StopEmmision()
    {
        ParticleSystem.enableEmission = false;
    }

    public override void StartEmmision()
    {
        ParticleSystem.enableEmission = true;
    }
}
