using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class PathDebugObjectTest : MonoBehaviour
{
    private PathDebugData debugData = new PathDebugData();
    public Transform TargeTransform;
    private CellController _cellController;

    void Awake()
    {
        _cellController = FindObjectOfType<CellController>();
    }

    void Update()
    {
        if (_cellController == null)
        {
            return;
        }
        if (TargeTransform == null)
        {
            return;
        }
        bool nextIsSame;
        bool goodDir;
        var curCell = _cellController.FindCell(transform.position);
        var d = TargeTransform.position - transform.position;

        var dir = PathDirectionFinder.TryFindDirection(_cellController,Utils.NormalizeFastSelf(d), curCell, 
            TargeTransform.position, transform.position,  out goodDir,
            debugData);
    }

    void OnDrawGizmos()
    {
        if (debugData != null)
        {
            debugData.DrawGizmos();
        }
    }
}

