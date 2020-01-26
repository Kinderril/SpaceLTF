﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[System.Serializable]
public class PlayerMapData
{
    private GlobalMapCell _currentCell;
    public GlobalMapCell LastCell = null;
    public GalaxyData GalaxyData;
    public int Step = 0;
    public int VisitedSectors = 0;
    private PlayerByStepDamage _stepDamage;

    [field: NonSerialized]
    public event Action<GlobalMapCell> OnCellChanged;    
    [field: NonSerialized]
    public event Action OnStep;

    public PlayerMapData()
    {

    }

    public GlobalMapCell CurrentCell
    {
        get => _currentCell;
        set
        {
            LastCell = _currentCell;
           _currentCell = value;
        }
    }

    public void Init(StartNewGameData data,PlayerByStepDamage stepDamage)
    {
        _stepDamage = stepDamage;
        Step = 0;
        var sectorIndex = MyExtensions.Random(10 * data.SectorSize, 100 * data.SectorSize);
        var sector = new GalaxyData("Sector " + sectorIndex.ToString());
        var startCell = sector.Init2(data.SectorCount, data.SectorSize, data.BasePower, data.CoreElementsCount,data.StepsBeforeDeath, data.shipConfig,data.PowerPerTurn);
        GalaxyData = sector;
        CurrentCell = startCell;
        OpenAllNear();
    }

    public int ScoutedCells(int min,int max)
    {
        var allCells = GalaxyData.GetAllList().Where(x => !x.IsScouted && !x.Completed).ToList();
        var cellsToScout = MyExtensions.Random(min, max);
        var count = Mathf.Clamp(cellsToScout, 0, allCells.Count);
        var cells = allCells.RandomElement(count);
        foreach (var cell in cells)
        {
            cell.Scouted();
        }
        return count;
    }
    private void OpenAllNear()
    {
        var allConnectd = ConnectedCellsToCurrent();
        foreach (var globalMapCell in allConnectd)
        {
            globalMapCell.OpenInfo();
        }

        ScoutAllAround(CurrentCell);
    }

    public bool GoToTarget(GlobalMapCell target, GlobalMapController globalMap,Action<GlobalMapCell> callback)
    {
        if (CanGoTo(target))
        {
            if (target != CurrentCell)
            {
                WindowManager.Instance.UiAudioSource.PlayOneShot(DataBaseController.Instance.AudioDataBase.ShipGlobalMapMove);
                globalMap.MoveToCell(target,() =>
                {
                    callback(target);
                    GoNextAfterDialog(target);
                    target.OpenInfo();
                    target.VisitCell(this,Step);
                    target.ComeTo();
                    ScoutAllAround(target);
                });
            }
            else
            {
                callback(target);
            }

            return true;
        }

        return false;
    }

    private void ScoutAllAround(GlobalMapCell target)
    {
        foreach (var cell in target.GetCurrentPosibleWays())
        {
            if (!cell.IsScouted && cell is ArmyGlobalMapCell)
            {
                cell.Scouted();
            }
        }
    }

    private void GoNextAfterDialog(GlobalMapCell target)
    {
        CurrentCell = target;
        OpenAllNear();
        Step++;
        GalaxyData.StepComplete(Step, CurrentCell);
        _stepDamage.StepComplete(Step);
        if (OnCellChanged != null)
        {
            OnCellChanged(CurrentCell);
        }

        if (OnStep != null)
        {
            OnStep();
        }
    }

    public bool CanGoTo(GlobalMapCell target)
    {
#if Demo
        int step = Step * 200;
        int check = 5000;
        if (VisitedSectors > 2 || step > check)
        {
            WindowManager.Instance.InfoWindow.Init(()=>
            {
                WindowManager.Instance.OpenWindow(MainState.start);
            },String.Format(Namings.Tag("Demo"),2,25));
            return false;
        }
#endif

        if (target is GlobalMapNothing)
        {
            return false;
        }
        if (target.IsDestroyed)
        {
            return false;
        }
        var getConnect = ConnectedCellsToCurrent();
        var isConnected = getConnect.Contains(target);
#if UNITY_EDITOR
        if (DebugParamsController.AnyWay)
        {
            isConnected = true;
        }
#endif
#if Develop    
        isConnected = true;
#endif
        return isConnected;
    }

    public List<GlobalMapCell> ConnectedCellsToCurrent()
    {
        return CurrentCell.GetCurrentPosibleWays();
    }

    public void OpenRetranslatorInfo()
    {
        for (int i = 0; i < GalaxyData.Size; i++)
        {
            for (int j = 0; j < GalaxyData.Size; j++)
            {
                var cell = GalaxyData.AllCells()[i, j];
                var core = cell as CoreGlobalMapCell;
                if (core != null && !core.InfoOpen)
                {
                    core.Scouted();
                    return;
                }
            }
        }

    }

    public void Dispose()
    {
        GalaxyData.Dispose();
    }
}
