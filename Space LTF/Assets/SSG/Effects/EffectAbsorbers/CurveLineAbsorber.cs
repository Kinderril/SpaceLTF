using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class CurveLineAbsorber : BaseEffectAbsorber
{
    public RoadMeshCreator ParticleAttractor;
    private List<Vector3> _list = new List<Vector3>(3);
    public override void Play(Vector3 pos1, Vector3 pos2)
    {

//        transform.position = pos1;
        ParticleAttractor.CreateByPoints(new List<Vector3>() { pos1, pos2 });
        base.Play(pos1, pos2);
    }

    public override void UpdatePositions(Vector3 pos1, Vector3 pos2)
    {
        var mid = (pos1 + pos2) / 2;
        _list.Clear();
        _list.Add(pos1);
        _list.Add(mid);
        _list.Add(pos2);
        transform.position = mid;
        ParticleAttractor.CreateByPoints(_list);
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

