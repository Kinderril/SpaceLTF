using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class GalaxyEnemiesArmyController
{
    [field: NonSerialized]
    public event Action<MovingArmy, bool> OnAddMovingArmy;

    private const int MAX_ARMIES = 6;

    private List<MovingArmy> _armies = new List<MovingArmy>();
    private List<GlobalMapCell> _cells;
    private HashSet<int> stepsToBorn = new HashSet<int>();
    private int _lastStep = 1;
    // private int _lastAddedStep = 1;
    private int _totalBornArmies = 0;
//    private PlayerQuestData _questData;
    private float _powerPerTurn;

    private const float DELTA_STEP = 3.4f;
    private Dictionary<MovingArmy, GlobalMapCell> _nextTargets = new Dictionary<MovingArmy, GlobalMapCell>();

    [field: NonSerialized]
    private Func<GlobalMapCell, bool> _cellHaveObject = null;

    public GalaxyEnemiesArmyController(float powerPerTurn,int armiesToMove = 3)
    {
        _powerPerTurn = powerPerTurn;
        if (armiesToMove > 0)
        {
            int startStep = MyExtensions.Random(4, 8);
            // _lastAddedStep = startStep;
            stepsToBorn.Add(startStep);
        }
    }

    public List<MovingArmy> GetCurrentArmies()
    {
        return _armies;
    }

    public void SetCells(List<GlobalMapCell> cells)
    {
        _cells = cells;
    }

    public void CheckEnemiesMovingArmies(int step, GlobalMapCell curCell)
    {
        _lastStep = step;
        if (stepsToBorn.Count == 0)
        {
            return;
        }

        foreach (var i in stepsToBorn)
        {
            if (step == i)
            {
                TryBornArmy(curCell);
                stepsToBorn.Remove(i);
                break;
            }
        }
    }

    public void DebugTryBornArmy()
    {
        var data = MainController.Instance.MainPlayer.MapData;
        var allSectors = data.GalaxyData.GetAllList();

        var posibleCells = allSectors.Where(x =>
            !(x is GlobalMapNothing) && x.CurMovingArmy == null).ToList();

        if (posibleCells.Count == 0)
        {
            return;
        }

        var cell = posibleCells.RandomElement();
        if (cell != null)
        {
            BornArmyAtCell(cell);
        }
    }

    public void TryBornArmyAtCell(GlobalMapCell cell)
    {
        TryBornArmy(cell);
    }

    public void AddArmy(MovingArmy movingArmy)
    {

        if (movingArmy == null)
        {
            Debug.LogError($"Try add null army");
            return;
        }
        _armies.Add(movingArmy);
        OnAddMovingArmy?.Invoke(movingArmy, true);

        if (_lastStep > 1)
            _nextTargets = FindTargetForMovingArmies(_cellHaveObject);

    }
    public void SimpleArmyDestroyed(MovingArmy army)
    {
        DestroyArmy(army);
    }

    private void TryBornArmy(GlobalMapCell curPlayersCell)
    {
        var posibleCells = _cells.Where(x =>
            !x.Completed && x.CurMovingArmy == null && !(x is GlobalMapNothing) && Mathf.Abs(x.indX - curPlayersCell.indX) > 4 && Mathf.Abs(x.indZ - curPlayersCell.indZ) > 4);

        var posibleCell = posibleCells.FirstOrDefault();
        if (posibleCell != null)
        {
            if (_armies.Count < MAX_ARMIES)
            {
                BornArmyAtCell(posibleCell);
            }
            else
            {
                stepsToBorn.Add(_lastStep + MyExtensions.Random(4, 10));
            }
        }
    }

    public MovingArmy BornArmyAtCell(GlobalMapCell cell)
    {
        var movingArmy = new SpecOpsMovingArmy(cell, DestroySpecOpsCallback, this);
        AddArmy(movingArmy);
        _totalBornArmies++;
        var coordinates = $"{movingArmy.CurCell.indX},{movingArmy.CurCell.indZ}";
        WindowManager.Instance.InfoWindow.Init(null,
            Namings.Format(Namings.Tag("MovingArmyBorn"),
                Namings.ShipConfig(movingArmy.StartConfig), coordinates));
        return movingArmy;
    }

    private void DestroySpecOpsCallback(MovingArmy movingArmy)
    {
        AddNewStepToBorn();
        if (MyExtensions.IsTrue01(0.75f))
        {
            AddNewStepToBorn();
        }
        DestroyArmy(movingArmy);
    }

    public void DestroyArmy(MovingArmy movingArmy)
    {
        OnAddMovingArmy?.Invoke(movingArmy, false);
        _armies.Remove(movingArmy);
    }

    private void AddNewStepToBorn()
    {
        var stepToBorn = _lastStep + (int)MyExtensions.GreateRandom(_totalBornArmies * DELTA_STEP);
        stepsToBorn.Add(stepToBorn);
    }
     
    public void Dispose()
    {


    }
    public void CacheTargets(Func<GlobalMapCell, bool> cellHaveObject)
    {
        _nextTargets = FindTargetForMovingArmies(cellHaveObject);
    }

    public Dictionary<MovingArmy, GlobalMapCell> GetTargets(Func<GlobalMapCell, bool> cellHaveObject)
    {
        if (_nextTargets != null)
        {
            Dictionary<MovingArmy, GlobalMapCell> trgs = new Dictionary<MovingArmy, GlobalMapCell>();
            foreach (var globalMapCell in _nextTargets)
            {
                if (!globalMapCell.Key.Destroyed)
                {
                    trgs.Add(globalMapCell.Key,globalMapCell.Value);
                }
            }
            return trgs;
        }
        _nextTargets = FindTargetForMovingArmies(cellHaveObject);
        return _nextTargets;
    }


    public Dictionary<MovingArmy, GlobalMapCell> FindTargetForMovingArmies(Func<GlobalMapCell, bool> cellHaveObject)
    {
        _cellHaveObject = cellHaveObject;
        HashSet<GlobalMapCell> freeCells = new HashSet<GlobalMapCell>();
        HashSet<GlobalMapCell> choosedCells = new HashSet<GlobalMapCell>();
        Dictionary<MovingArmy, GlobalMapCell> _nextCell = new Dictionary<MovingArmy, GlobalMapCell>();
        if (_armies.Count == 0)
        {
            return _nextCell;
        }

//        _armies.Sort((army, movingArmy) => (army.Priority > movingArmy.Priority) ? 1 : -1);

        foreach (var globalMapCell in _cells)
        {
            if (globalMapCell != null && !(globalMapCell is GlobalMapNothing))
            {
                if (cellHaveObject == null || cellHaveObject(globalMapCell))
                {
                    if (globalMapCell.CurMovingArmy == null)
                    {
                        freeCells.Add(globalMapCell);
                    }
                    else
                    {
                        choosedCells.Add(globalMapCell);
                    }
                }
            }
        }

        foreach (var movingArmy in _armies)
        {
//            if (movingArmy.CurCell != playersCell)
//            {
                var targetCell = movingArmy.FindCellToMove(freeCells);
               
                    if (targetCell != null)
                    {
                        _nextCell.Add(movingArmy, targetCell);
                        freeCells.Remove(targetCell);
                        choosedCells.Add(targetCell);
                    }
                
//            }
        }

        return _nextCell;

    }

    public void LeaveCells(Dictionary<MovingArmy, GlobalMapCell> targets)
    {
        foreach (var globalMapCell in targets)
        {
            globalMapCell.Key.PrevCell = globalMapCell.Key.CurCell;
            globalMapCell.Key.CurCell.CurMovingArmy = null;
            globalMapCell.Key.CurCell = null;
        }
    }

    public GlobalMapCell GetNextTarget(MovingArmy movingArmy)
    {
        if (_nextTargets.TryGetValue(movingArmy, out var target))
        {
            return target;
        }
        return null;
    }

    public void UpdateAllPowersAdditional(int step)
    {
        var additionalPower = (_powerPerTurn * step);
        foreach (var movingArmy in _armies)
        {
            movingArmy.UpdateAllPowersAdditional(additionalPower);
        }
    }

}

