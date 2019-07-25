using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class GlobalMapCellConnector : MonoBehaviour
{
    public RoadMeshCreator ParticleAttractor;
    private Vector3 _from;
    private Vector3 _to;
    public bool IsDestroyed = false;

    public void Init(Vector3 from, Vector3 to)
    {
        _from = from;
        _to = to;
        gameObject.SetActive(true);
        var list = new List<Vector3>();
        list.Add(from);
        list.Add((from+to)/2f);
        list.Add(to);
        ParticleAttractor.CreateByPoints(list);
    }

    public void DestroyWay()
    {
        if (IsDestroyed)
        {
            return;
        }

        IsDestroyed = true;
        ParticleAttractor.gameObject.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_from,0.5f);
        Gizmos.DrawSphere(_to,0.5f);
    }
}

