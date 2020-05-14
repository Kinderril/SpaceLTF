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
    private PlayerQuestData _questData;

    private const float DELTA_STEP = 3.4f;

    public GalaxyEnemiesArmyController(List<GlobalMapCell> cells, int armiesToMove = 3)
    {
        _cells = cells;
        if (armiesToMove > 0)
        {
            int startStep = MyExtensions.Random(4, 8);
            // _lastAddedStep = startStep;
            stepsToBorn.Add(startStep);
        }
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
        TryBornArmy(MainController.Instance.MainPlayer.MapData.CurrentCell);
    }

    public void AddArmy(GlobalMapCell cellToBorn)
    {

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
                var movingArmy = new MovingArmy(posibleCell, DestroyCallback);
                _armies.Add(movingArmy);
                OnAddMovingArmy?.Invoke(movingArmy, true);
                _totalBornArmies++;
                var coordinates = $"{movingArmy.CurCell.indX},{movingArmy.CurCell.indZ}";
                WindowManager.Instance.InfoWindow.Init(null,
                    Namings.Format(Namings.Tag("MovingArmyBorn"),
                        Namings.ShipConfig(movingArmy._player.Army.BaseShipConfig), coordinates));
            }
            else
            {
                stepsToBorn.Add(_lastStep + MyExtensions.Random(4, 10));
            }
        }
    }

    private void DestroyCallback(MovingArmy movingArmy)
    {
        AddNewStepToBorn();
        if (MyExtensions.IsTrue01(0.75f))
        {
            AddNewStepToBorn();
        }
        OnAddMovingArmy?.Invoke(movingArmy, false);
        _armies.Remove(movingArmy);
    }

    private void AddNewStepToBorn()
    {
        var stepToBorn = _lastStep + (int)MyExtensions.GreateRandom(_totalBornArmies * DELTA_STEP);
        stepsToBorn.Add(stepToBorn);
    }

    public void InitQuests(PlayerQuestData questData)
    {
        _questData = questData;
        _questData.OnElementFound += OnElementFound;
    }

    private void OnElementFound()
    {
        var stepToBorn = _lastStep + Mathf.Clamp((int)MyExtensions.GreateRandom(_totalBornArmies), 1, 4);
        stepsToBorn.Add(stepToBorn);
    }

    public void Dispose()
    {
        if (_questData != null)
        {
            _questData.OnElementFound -= OnElementFound;
        }

    }

    public Dictionary<MovingArmy, GlobalMapCell> FindTargetForMovingArmies(GlobalMapCell playersCell, Func<GlobalMapCell, bool> cellHaveObject)
    {
        HashSet<GlobalMapCell> freeCells = new HashSet<GlobalMapCell>();
        HashSet<GlobalMapCell> choosedCells = new HashSet<GlobalMapCell>();
        _armies.Sort((army, movingArmy) => (army.Priority > movingArmy.Priority)?1:-1 );
        Dictionary<MovingArmy, GlobalMapCell> _nextCell = new Dictionary<MovingArmy, GlobalMapCell>();

        foreach (var globalMapCell in _cells)
        {
            if (globalMapCell != null)
            {
                if (cellHaveObject(globalMapCell))
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
            if (movingArmy.CurCell != playersCell)
            {
                var targetCell = movingArmy.FindCellToMove(playersCell,freeCells);
               
                    if (targetCell != null)
                    {
                        _nextCell.Add(movingArmy, targetCell);
                        freeCells.Remove(targetCell);
                        choosedCells.Add(targetCell);
                    }
                
            }
        }

        foreach (var globalMapCell in _nextCell)
        {
            globalMapCell.Key.PrevCell = globalMapCell.Key.CurCell;
            globalMapCell.Key.CurCell = null;
        }

        return _nextCell;

    }

}

