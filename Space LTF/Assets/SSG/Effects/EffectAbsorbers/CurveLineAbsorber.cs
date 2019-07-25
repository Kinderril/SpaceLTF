using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class CurveLineAbsorber : BaseEffectAbsorber
{
    public RoadMeshCreator ParticleAttractor;

    public override void Play(Vector3 pos1, Vector3 pos2)
    {

        transform.position = pos1;
        ParticleAttractor.CreateByPoints(new List<Vector3>() { pos1, pos2 });
        base.Play(pos1, pos2);
    }

    public override void UpdatePositions(Vector3 pos1, Vector3 pos2)
    {
        transform.position = pos1;
        ParticleAttractor.CreateByPoints(new List<Vector3>() { pos1, pos2 });
    }

    public override void Play()
    {
        ParticleAttractor.gameObject.SetActive(true);
        base.Play();
    }

    public override void Stop()
    {
        ParticleAttractor.gameObject.SetActive(false);
        base.Stop();
    }

    public override void StopEmmision()
    {

    }

    public override void StartEmmision()
    {

    }
}

