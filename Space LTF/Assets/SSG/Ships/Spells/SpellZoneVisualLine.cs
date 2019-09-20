using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SpellZoneVisualLine : MonoBehaviour
{

    public RoadMeshCreator ParticleAttractor;
    private Vector3 _from;
    private Vector3 _to;

    public void SetDirection(Vector3 from, Vector3 to,float maxDist)
    {
        _from = from;
        _to = to;

        var dir = _to - _from;
        var dist = dir.magnitude;
        if (dist > maxDist)
        {
            _to = _from + Utils.NormalizeFast(dir) * maxDist;
        }
        gameObject.SetActive(true);
        var list = new List<Vector3>();
        list.Add(_from);
        list.Add((_from + _to) / 2f);
        list.Add(_to);
        ParticleAttractor.CreateByPoints(list);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_from, 0.5f);
        Gizmos.DrawSphere(_to, 0.5f);
    }
}

