﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class PlayerMapData
{
    public const int TUTOR_SECTOR_SIZE = 10;
    private GlobalMapCell _currentCell;
    public GlobalMapCell LastCell = null;
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

    public void Init(StartNewGameData data)
    {
        Step = 0;
        var sectorIndex = MyExtensions.Random(10 * data.SectorSize, 100 * data.SectorSize);
        GalaxyData galaxyData;
        switch (data.GameNode)
        {
            default:
            case EGameMode.sandBox:
                galaxyData = new GalaxyData("GalaxyData " + sectorIndex.ToString());
                break;
            case EGameMode.simpleTutor:
                data.SectorSize = TUTOR_SECTOR_SIZE;
                galaxyData = new SimpleTutorialGalaxyData("SimpleTutorialGalaxyData " + sectorIndex.ToString());
                break;
            case EGameMode.safePlayer:
                galaxyData = new ExprolerGalaxyDataMap("ExprolerGalaxyData");
                break;  
            case EGameMode.champaing:
                var champaingData = data as StartNewGameChampaing;
                if (champaingData.Act == 0)
                {
                    galaxyData = new StartChampaingGalaxyDataMap("StartChampaingGalaxyData", champaingData.Act, champaingData.ShiConfigAllise);
                }
                else
                {
                    galaxyData = new MidChampaingGalaxyDataMap("StartChampaingGalaxyData", champaingData.Act, champaingData.ShiConfigAllise);
                }
                break;
            case EGameMode.advTutor:
                data.SectorSize = TUTOR_SECTOR_SIZE;
                galaxyData = new AdvTutorialGalaxyData("AdvTutorialGalaxyData " + sectorIndex.ToString());
                break;
        }

        int startPower = data.GetStartPower();

        var startCell = galaxyData.Init2(data.SectorCount, data.SectorSize, startPower, data.QuestsOnStart, 
            data.StepsBeforeDeath, data.shipConfig, data.PowerPerTurn,data.MapType);
        GalaxyData = galaxyData;
        var champaingData2 = data as StartNewGameChampaing;
        if (champaingData2 != null)
        {
            GalaxyData.GalaxyEnemiesArmyController.RemovePosibleSpecOps(champaingData2.ShiConfigAllise);
        }
        CurrentCell = startCell;
        OpenAllNear();
    }

    public int ScoutedCells(int min, int max)
    {
        var allCells = GalaxyData.GetAllContainers().Where(x => x != null && !x.Data.IsScouted && !x.Data.Completed).ToList();
        var cellsToScout = MyExtensions.Random(min, max);
        var count = Mathf.Clamp(cellsToScout, 0, allCells.Count);
        var cells = allCells.RandomElement(count);
        foreach (var cell in cells)
        {
            cell.Data.Scouted();
        }
        return count;
    }
    private void OpenAllNear()
    {
        var allConnectd = ConnectedCellsToCurrent();
        foreach (var globalMapCell in allConnectd)
        {
            if (globalMapCell != null)
                globalMapCell.Data.OpenInfo();
        }

        ScoutAllAround(CurrentCell);
    }

    public bool GoToTarget(GlobalMapCell target, GlobalMapController globalMap,bool armiesCanMove, Action<GlobalMapCell> callback)
    {
        var lastCell = CurrentCell;
        if (CanGoTo(target,true))
        {
            if (target != CurrentCell)
            {
                if (CurrentCell != null)
                {
                    CurrentCell.LeaveFromCell();
                }
                WindowManager.Instance.UiAudioSource.PlayOneShot(DataBaseController.Instance.AudioDataBase.ShipGlobalMapMove);
                globalMap.MoveToCell(target, armiesCanMove,() =>
                 {
                     callback(target);
                     GoNextAfterDialog(target);
                     target.OpenInfo();
                     target.VisitCell(this, Step);
                     UpgradeAllCellByStep();
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

    private void UpgradeAllCellByStep()
    {
        foreach (var sector in GalaxyData.AllSectors)
        {
            sector.RecalculateAllCells(Step);
        }
        GalaxyData.GalaxyEnemiesArmyController.UpdateAllPowersAdditional(Step);

    }

    private void ScoutAllAround(GlobalMapCell target)
    {
        foreach (var cell in target.GetCurrentPosibleWays())
        {
            if (MainController.Instance.MainPlayer.Parameters.Scouts.Level >= 4)
            {
                if (!cell.Data.IsScouted)
                {
                    cell.Data.Scouted();
                }
            }
//            cell.
        }
    }

    private void GoNextAfterDialog(GlobalMapCell target)
    {
        CurrentCell = target;
        OpenAllNear();
        Step++;
        GalaxyData.StepComplete(Step, CurrentCell);
        if (OnCellChanged != null)
        {
            OnCellChanged(CurrentCell);
        }

        if (OnStep != null)
        {
            OnStep();
        }
    }

    public bool CanGoTo(GlobalMapCell target,bool withActionIfCantGo)
    {       
        if (CurrentCell != null && !CurrentCell.CanGotFromIt(withActionIfCantGo))
        {
            return false;
        }
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
        var isConnected = getConnect.Contains(target.Container);
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

    public HashSet<SectorCellContainer> ConnectedCellsToCurrent()
    {
        return CurrentCell.GetCurrentPosibleWays();
    }

//    public void OpenRetranslatorInfo()
//    {
//        for (int i = 0; i < GalaxyData.SizeX; i++)
//        {
//            for (int j = 0; j < GalaxyData.SizeZ; j++)
//            {
//                var cell = GalaxyData.AllCells()[i, j];
//                var core = cell as CoreGlobalMapCell;
//                if (core != null && !core.InfoOpen)
//                {
//                    core.Scouted();
//                    return;
//                }
//            }
//        }
//
//    }

    public void Dispose()
    {
        GalaxyData.Dispose();
    }

    public void UpdateCollectedPower(Player defeatedPlayer)
    {
        var player = MainController.Instance.MainPlayer;
        var playerPower = player.Army.GetPower();
        var enemyPower = defeatedPlayer.Army.GetPower();

        float powerDelta = player.Difficulty.CalcDelta(enemyPower, playerPower);
        foreach (var currentArmy in GalaxyData.GalaxyEnemiesArmyController.GetCurrentArmies())
        {
            currentArmy.UpdateAllPowersCollected(powerDelta);
        }

        foreach (var cell in GalaxyData.GetAllContainers())
        {
            if (cell != null && cell.Data != null)
                cell.Data.UpdateCollectedPower(powerDelta);
        }

    }
}
