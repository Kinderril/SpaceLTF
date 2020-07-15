using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WayDrawer : MonoBehaviour
{
    private ManualShipWayController _shipWayController;
    public Transform Container;
    public WayDrawPoint PointPrefab;
    public TrailRenderer LineObjectToMove;
    private List<WayDrawPoint> _points = new List<WayDrawPoint>();


    public void Init(ManualShipWayController shipWayController)
    {
        _points.Clear();
        _shipWayController = shipWayController;
        _shipWayController.OnAddPoint += OnAddPoint;
        _shipWayController.OnStart += OnStart;
        _shipWayController.OnEnd += OnEnd;
        LineObjectToMove.gameObject.SetActive(false);

    }

    private void OnEnd()
    {
        LineObjectToMove.gameObject.SetActive(false);
        LineObjectToMove.time = 0f;
        ClearAllPoints();
    }

    private void OnStart(Vector3 obj)
    {
        LineObjectToMove.gameObject.SetActive(true);
        LineObjectToMove.transform.position = obj;
        LineObjectToMove.time = 100f;
//        LineObjectToMove.re
    }

    private void OnAddPoint(Vector3 arg1, bool arg2,Vector3 dir)
    {

        LineObjectToMove.transform.position = arg1;
        if (arg2)
        {
            var point = DataBaseController.GetItem(PointPrefab);
            point.transform.SetParent(Container,false);
            point.transform.position = arg1;
            point.SetLookDir(arg1,dir);
            _points.Add(point);
        }
    }

    private void ClearAllPoints()
    {
         LineObjectToMove.Clear();
        foreach (var point in _points)
        {
            GameObject.Destroy(point.gameObject);
        }
        _points.Clear();
    }

    public void Dispose()
    {
        ClearAllPoints();
        _shipWayController.OnAddPoint -= OnAddPoint;
        _shipWayController.OnStart -= OnStart;
        _shipWayController.OnEnd -= OnEnd;
    }
}
