using System;
using UnityEngine;

public class GlobalMapMoverObject : MonoBehaviour
{
    public float Speed = 1f;
    private float _timeToMove;
    private float _endTimeToMove;
    private GlobalMapCellObject _curCell;
    private bool _isActive;
    private Action _callback;
    private Vector3 _startPos;
    private Vector3 _endPos;
    private Action<Vector3> _moveToPosition = null;

    public void Init(GlobalMapCellObject startCell)
    {
        if (startCell == null)
        {
            Debug.LogError("Init object at null cell");
            return;
        }
        _isActive = false;
        _curCell = startCell;
        transform.position = startCell.Container.position;
    }

    public float MoveTo(float calculatedTime, GlobalMapCellObject target, Action callback)
    {
        MoveTo(target, callback);
        _timeToMove = calculatedTime * .99f;
        _endTimeToMove = Time.time + _timeToMove;
        return _timeToMove;
    }
    public float MoveTo(GlobalMapCellObject target, Action callback, Action<Vector3> moveToPosition = null)
    {
        _moveToPosition = moveToPosition;
        _startPos = transform.position;
        _endPos = target.Container.position;
        _callback = callback;
        var dir = target.Container.position - _curCell.Container.position;
        var dist = dir.magnitude;
        var tmpTimeToMove = dist / Speed;
        _timeToMove = Mathf.Clamp(tmpTimeToMove, 1f, 4f);
        _endTimeToMove = Time.time + _timeToMove;
        _isActive = true;
        return _timeToMove;
    }

    void Update()
    {
        if (!_isActive)
        {
            return;
        }

        var p = 1 - (_endTimeToMove - Time.time) / _timeToMove;
        if (p >= 1f)
        {
            // _moveToPosition = null;
            p = 1f;
            _isActive = false;
            _callback();
        }
        else
        {
            transform.LookAt(_endPos);
        }
        var pos = Vector3.Lerp(_startPos, _endPos, p);
        transform.position = pos;
        // _moveToPosition?.Invoke(pos);
    }
}
