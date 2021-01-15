using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyGlobalMapMoverObjet : GlobalMapMoverObject
{
    public MovingArmy Owner;
    private GlobalMapController _mapController;
    public GameObject LookObject;
    public GameObject RedObject;
    public GameObject GreenObject;
    private Vector3 _cahceDirection;
    public TextMeshPro PowerField;
    private bool _isLastActive;
    private bool _canShowPower;
    private float _lastVal = -1f;

    public void Init(GlobalMapController mapController, GlobalMapCellObject startCell, MovingArmy owner)
    {
        _mapController = mapController;
        base.Init(startCell);
        Owner = owner;
        UpdateLookDirection();
        UpdateCurHideCell();
        GreenObject.SetActive(owner.IsAllies);
        RedObject.SetActive(!owner.IsAllies);
        _canShowPower = owner is SpecOpsMovingArmy;
        if (!_canShowPower)
        {
            PowerField.gameObject.SetActive(false);
        }
        PowerFieldUpdate();
    }

    public void SetAllies()
    {
        GreenObject.SetActive(true);
        RedObject.SetActive(false);
    }


    private void PowerFieldUpdate()
    {
        if (!_canShowPower)
        {
            return;
        }

        var delta = _lastVal - Owner.Power;
        var abs = Mathf.Abs(delta) > 0.5;
        if (abs)
        {
            _lastVal = Owner.Power;
            PowerField.text = _lastVal.ToString("0");
        }

    }

    public void UpdateLookDirection()
    {
        var player = MainController.Instance.MainPlayer; 
        var trg = Owner.NextCell();
        PowerFieldUpdate();
        if (trg == null)
        {
            _isLastActive = false;
        }
        else
        {
            var target = _mapController.GetCellObjectByCell(trg);
            if (player.Parameters.Scouts.Level >= 2)
            {
                if (target != null)
                {
                    _cahceDirection = target.ModifiedPosition;
                    LookObject.transform.LookAt(_cahceDirection);
                    _isLastActive = true;
                }
                else
                {
                    _isLastActive = false;
                }
            }
            else
            {
                _isLastActive = false;
            }
        }
        LookObject.SetActive(_isLastActive);
    }


    public bool AndGo(/*Action callback,*/ GlobalMapCell place, float timeToMove)
    {
        PowerFieldUpdate();
        var objPlace = _mapController.GetCellObjectByCell(place);
        if (objPlace != null)
        {
            if (timeToMove < 0.3f)
            {
                Debug.LogError($"Very fast time to move:{timeToMove}");
            }
            Debug.Log($"Id:{Owner.Id}  Moving start go to: {place.ToString()}  Owner.CurCell:{Owner.CurCell}   timeToMove:{timeToMove}");
//            if (Owner.CurCell != null)
            Owner.CurCell.CurMovingArmy.ArmyRemove(Owner);
            Owner.SetCurCell(place);
            Owner.CurCell.CurMovingArmy.ArmyCome(Owner);
            LookObject.SetActive(false);
            MoveTo(timeToMove, objPlace, () =>
            {
                UpdateLookDirection();
                 Debug.Log($"Id:{Owner.Id}   Moving army come to: {Owner.CurCell.ToString()}");
//                 callback();
             });
            return true;
        }
        else
        {
            Debug.LogError("Can't find object by cell");
            return false;
        }
    }

    void OnDrawGizmos()
    {
        if (_isLastActive)
        {
              Gizmos.color = Color.yellow;
              var up = Vector3.up * .2f;
              var from = transform.position + up;
              var to = _cahceDirection + up;
              Gizmos.DrawLine(from,to);
        }
    }

    public void UpdateCurHideCell()
    {

        var shallHide = Owner.CurCell.IsHide;
        gameObject.SetActive(!shallHide);

    }
}

