using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[System.Serializable]
public class PlayerMapData
{
    public GlobalMapCell CurrentCell;
    public GalaxyData GalaxyData;
    public int Step = 0;
    public int VisitedSectors = 0;

    [field: NonSerialized]
    public event Action<GlobalMapCell> OnCellChanged;    
    [field: NonSerialized]
    public event Action OnStep;

    public PlayerMapData()
    {

    }

    public void Init(StartNewGameData data)
    {
        Step = 0;
        var sectorIndex = MyExtensions.Random(10 * data.SectorSize, 100 * data.SectorSize);
        var sector = new GalaxyData("Sector " + sectorIndex.ToString());
        var startCell = sector.Init2(data.SectorCount, data.SectorSize, data.BasePower, data.CoreElementsCount,data.CellsStartDeathStep);
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
    }



    public bool GoToTarget(GlobalMapCell target, GlobalMapController globalMap,Action callback)
    {
        if (CanGoTo(target))
        {
            if (target != CurrentCell)
            {
                globalMap.MoveToCell(target,() =>
                {
                    callback();
                    target.OpenInfo();
                    GoNextAfterDialog(target);
                    target.VisitCell(this);
                    target.ComeTo();
                });
            }
            else
            {
                callback();
            }

            return true;
        }

        return false;
    }

    private void GoNextAfterDialog(GlobalMapCell target)
    {
        CurrentCell = target;
        OpenAllNear();
        Step++;
        GalaxyData.StepComplete(Step, CurrentCell);
        MainController.Instance.MainPlayer.SaveGame();
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
        if (target is GlobalMapNothing)
        {
            return false;
        }
        if (target.IsDestroyed)
        {
            return false;
        }
        var getConnect = ConnectedCellsToCurrent();
        return getConnect.Contains(target);
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
}
