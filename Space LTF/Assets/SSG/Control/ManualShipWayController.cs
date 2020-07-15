using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class ManualShipWayController
{
    private InGameMainUI _inGameMain;
    private ShipBase _selectedShip;
    private float _selectedTime;
    private float _turnRadiius;
    private float _sTurnRadiius;
    private Vector3 _lastSelected;
    private Vector3 _lastSelectedRight;
    private Vector3 _lastSelectedLeft;
    private List<Vector3> _wayPoints = new List<Vector3>();
    public event Action<Vector3> OnStart; 
    public event Action OnEnd; 
    public event Action<Vector3,bool,Vector3> OnAddPoint;
    private bool _shipInited = false;
    private bool _timeScaled = false;
    private float _prevScale;
    private PauseData _pauseData;

    public void Init(InGameMainUI inGameMain,PauseData pauseData)
    {
        _pauseData = pauseData;
        _inGameMain = inGameMain;
    }

    public bool DoMouseButtonDown(bool isSame, bool isMouseDown)
    {
        var isOverUI = EventSystem.current.IsPointerOverGameObject();
        if (isOverUI)
        {
            return false;
        }

        if (isMouseDown)
        {
            if (isSame)
            {
                if (_shipInited)
                {
                    var pos = Input.mousePosition;
                    var pt = _inGameMain.GetPointByClick(pos);
                    if (pt.HasValue)
                    {
                        TryAddPoint(pt.Value);
                        return true;
                    }
                }
            }
            else
            {
                if (!_shipInited)
                {
//                    Debug.LogError("Start draw");
                    //                Debug.LogError("Process draw");
                    var pos = Input.mousePosition;
                    var pt = _inGameMain.GetShipByPoint(pos);
                    if (pt != null && pt.TeamIndex == TeamIndex.green)
                    {
                        if (pt.ShipParameters.StartParams.ShipType == ShipType.Heavy ||
                            pt.ShipParameters.StartParams.ShipType == ShipType.Middle ||
                            pt.ShipParameters.StartParams.ShipType == ShipType.Light)
                        {
                            _selectedTime = Time.time;
                            _selectedShip = pt;
                            StartDraw();
                            _wayPoints.Clear();
                            var rad = _selectedShip.MaxTurnRadius * 1.5f;
//                            Debug.LogError($"rad:{rad}");
                            _turnRadiius = Mathf.Clamp(rad,1f,9f);
                            _sTurnRadiius = _turnRadiius * _turnRadiius;
                            SetLastPos(_selectedShip.Position, _selectedShip.LookDirection);
                            OnStart?.Invoke(_selectedShip.Position);
                            return true;
                        }
                    }
                }
            }
        }
        else
        {
            if (_shipInited)
            {
                EndDraw();
                //                Debug.LogError("End draw");
                if (_selectedShip != null)
                {
                    TryDoWay();
                    return true;
                }
            }
        }

        return false;
    }

    private void EndDraw()
    {
        if (_timeScaled)
        {
            Time.timeScale = _prevScale;
        }
        _shipInited = false;
        _timeScaled = false;
    }

    private void StartDraw()
    {
        if (!_pauseData.IsPause)
        {
            _timeScaled = true;
     _prevScale = Time.timeScale;
            Time.timeScale = 0.1f;
        }
        _shipInited = true;
    }

    private void SetLastPos(Vector3 pos,Vector3 dir)
    {
        if (dir.sqrMagnitude > 0.2f)
        {
            _lastSelected = pos;
            var norm = Utils.NormalizeFastSelf(dir);
            _lastSelectedRight = pos + Utils.Rotate90(norm, SideTurn.left) * _turnRadiius;
            _lastSelectedLeft = pos + Utils.Rotate90(norm, SideTurn.right) * _turnRadiius;
            _wayPoints.Add(_lastSelected);
            OnAddPoint?.Invoke(_lastSelected, true, dir);
        }

    }

    private void TryAddPoint(Vector3 pt)
    {
        bool shallAdd = !IsInside(pt, _lastSelected) &&
                        !IsInside(pt, _lastSelectedLeft) &&
                        !IsInside(pt, _lastSelectedRight);
        var dir = _lastSelected - pt;
        if (shallAdd)
        {
            SetLastPos(pt, dir);
        }
        else
        {
            if (dir.sqrMagnitude > 0.2f)
            {
                OnAddPoint?.Invoke(pt, false,Vector3.up);
            }
        }

    }

    private bool IsInside(Vector3 p, Vector3 test)
    {
        return (p - test).sqrMagnitude < _sTurnRadiius;
    }

    private void TryDoWay()
    {
        OnEnd?.Invoke();
        if (_wayPoints.Count > 1)
        {
            var moveByWay = new MoveByWayAction(_selectedShip, _wayPoints.ToList());
//            Debug.LogError($"moveByWay {_selectedShip.name}");
            _selectedShip.SetAction(moveByWay);
        }
    }

    public void IncomeData()
    {

    }

    public void StartPos(ShipBase ship)
    {

    }

    public void NextPoint(Vector3 pos)
    {

    }

}
