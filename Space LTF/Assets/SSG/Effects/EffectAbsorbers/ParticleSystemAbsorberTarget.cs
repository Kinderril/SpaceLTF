
using UnityEngine;



public class ParticleSystemAbsorberTarget : BaseEffectAbsorber {

    public ParticleSystem ParticleSystem;
    public ParticleAttractorLinear AttractorLinear;

    void Awake()
    {
        if (ParticleSystem == null)
        {
            ParticleSystem = GetComponent<ParticleSystem>();
        }
    }

    public override void Play(Vector3 pos1, Vector3 pos2)
    {
        transform.position = pos1;
        AttractorLinear.SetTarget(pos2);
        ParticleSystem.enableEmission = true;
        base.Play(pos1,pos2);
    }
    public override void UpdatePositions(Vector3 pos1, Vector3 pos2)
    {
        transform.position = pos1;
        AttractorLinear.SetTarget(pos2);
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
