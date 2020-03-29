using System.Collections.Generic;
using UnityEngine;


public class GlobalMapCellConnector : MonoBehaviour
{
    public RoadMeshCreator ParticleAttractor;
    private Vector3 _from;
    private Vector3 _to;
    public bool IsDestroyed = false;
    public int FromId { get; private set; }
    public int ToId { get; private set; }

    public void Init(GlobalMapCellObject from, GlobalMapCellObject to)
    {
        FromId = from.Cell.Id;
        ToId = to.Cell.Id;
        _from = from.ModifiedPosition;
        _to = to.ModifiedPosition;
        gameObject.SetActive(true);
        var list = new List<Vector3>();
        var p1 = _from;
        var p3 = _to;//(_from + _to) * .5f;
        var p2 = (p3+ p1) * .5f;
        list.Add(p1);
        list.Add(p2);
        list.Add(p3);
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
        Gizmos.DrawSphere(_from, 0.5f);
        Gizmos.DrawSphere(_to, 0.5f);
    }
}

