using System;
using System.Collections.Generic;
using System.Linq;
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
            bool setLast = true;
            var nextIsDungeon = (value is ArmyDungeonExitGlobalMapCell) || (value is ArmyDungeonGlobalMapCell);
            if (nextIsDungeon)
            {
                if (LastCell is ArmyDungeonEnterGlobalMapCell)
                {
                    setLast = false;
                }
            }
            if (setLast)
            {
                LastCell = _currentCell;
            }
            _currentCell = value;
        }
    }

    public void Init(StartNewGameData data, PlayerByStepDamage stepDamage)
    {
        _stepDamage = stepDamage;
        Step = 0;
        var sectorIndex = MyExtensions.Random(10 * data.SectorSize, 100 * data.SectorSize);
        GalaxyData galaxyData;
        if (data.IsTutorial)
        {
            data.SectorSize = 10;
            galaxyData = new TutorialGalaxyData("TutorialGalaxyData " + sectorIndex.ToString());
        }
        else
        {
            galaxyData = new GalaxyData("GalaxyData " + sectorIndex.ToString());
        }
        var startCell = galaxyData.Init2(data.SectorCount, data.SectorSize, data.BasePower, data.CoreElementsCount, data.StepsBeforeDeath, data.shipConfig, data.PowerPerTurn);
        GalaxyData = galaxyData;
        CurrentCell = startCell;
        OpenAllNear();
    }

    public int ScoutedCells(int min, int max)
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

    public bool GoToTarget(GlobalMapCell target, GlobalMapController globalMap, Action<GlobalMapCell> callback)
    {
        var lastCell = CurrentCell;
        if (CanGoTo(target))
        {
            if (target != CurrentCell)
            {
                if (CurrentCell != null)
                {
                    CurrentCell.LeaveFromCell();
                }
                WindowManager.Instance.UiAudioSource.PlayOneShot(DataBaseController.Instance.AudioDataBase.ShipGlobalMapMove);
                globalMap.MoveToCell(target, () =>
                 {
                     callback(target);
                     GoNextAfterDialog(target);
                     target.OpenInfo();
                     target.VisitCell(this, Step);
                     target.ComeTo(lastCell);
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
            },Namings.TryFormat(Namings.Tag("Demo"),2,25));
            return false;
        }
#endif

        if (target == null)
        {
            Debug.LogError("Can't go to null target");
            return false;
        }
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

    public HashSet<GlobalMapCell> ConnectedCellsToCurrent()
    {
        return CurrentCell.GetCurrentPosibleWays();
    }

    public void OpenRetranslatorInfo()
    {
        for (int i = 0; i < GalaxyData.SizeX; i++)
        {
            for (int j = 0; j < GalaxyData.SizeZ; j++)
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
