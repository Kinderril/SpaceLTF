﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGlobalMapMoverObjet : GlobalMapMoverObject
{
    public MovingArmy Owner;
    private GlobalMapController _mapController;
    public GameObject LookObject;
    private Vector3 _cahceDirection;
    private bool _isLastActive;

    public void Init(GlobalMapController mapController, GlobalMapCellObject startCell, MovingArmy owner)
    {
        _mapController = mapController;
        base.Init(startCell);
        Owner = owner;
        UpdateLookDirection();
    }

    public void UpdateLookDirection()
    {
        var player = MainController.Instance.MainPlayer; 
        var trg = Owner.NextCell();
        if (trg == null)
        {
            _isLastActive = false;
        }
        else
        {
            var target = _mapController.GetCellObjectByCell(trg);
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
        LookObject.SetActive(_isLastActive);
    }


    public bool AndGo(/*Action callback,*/ GlobalMapCell place, float timeToMove)
    {
        var objPlace = _mapController.GetCellObjectByCell(place);
        if (objPlace != null)
        {
            if (timeToMove < 0.3f)
            {
                Debug.LogError($"Very fast time to move:{timeToMove}");
            }
            Debug.Log($"Id:{Owner.Id}  Moving start go to: {place.ToString()}  Owner.CurCell:{Owner.CurCell}   timeToMove:{timeToMove}");
            if (Owner.CurCell != null)
                Owner.CurCell.CurMovingArmy = null;
            Owner.CurCell = place;
            Owner.CurCell.CurMovingArmy = Owner;
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
}

