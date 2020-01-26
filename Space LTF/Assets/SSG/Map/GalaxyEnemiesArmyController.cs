using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class GalaxyEnemiesArmyController
{
    [field: NonSerialized]
    public event Action<MovingArmy, bool> OnAddMovingArmy;


    private List<MovingArmy> _armies = new List<MovingArmy>();
    private List<GlobalMapCell> _cells;
    private HashSet<int> stepsToBorn = new HashSet<int>();
    private int _lastStep=1;
    private int _lastAddedStep=1;
    private int _totalBornArmies=0;
    private PlayerQuestData _questData;

    private const int DELTA_STEP = 9;

    public GalaxyEnemiesArmyController(List<GlobalMapCell> cells, int armiesToMove = 3)
    {
        _cells = cells;
        if (armiesToMove > 0)
        {
            int startStep = MyExtensions.Random(4, 10);
            _lastAddedStep = startStep;
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
                var posibleCell = _cells.FirstOrDefault(x =>
                    !x.Completed && !(x is GlobalMapNothing) && Mathf.Abs(x.indX - curCell.indX) > 4 && Mathf.Abs(x.indZ - curCell.indZ) > 4);

                if (posibleCell != null)
                {
                    var movingArmy = new MovingArmy(posibleCell, DestroyCallback);
                    _armies.Add(movingArmy);
                    OnAddMovingArmy?.Invoke(movingArmy, true);
                    _totalBornArmies++;
                    var coordinates = $"{movingArmy.CurCell.indX},{movingArmy.CurCell.indZ}";
                    WindowManager.Instance.InfoWindow.Init(null,
                        String.Format(Namings.Tag("MovingArmyBorn"),
                            Namings.ShipConfig(movingArmy._player.Army.BaseShipConfig), coordinates));
                }
                stepsToBorn.Remove(i);
                break;
            }
        }
    }

    private void DestroyCallback(MovingArmy movingArmy)
    {
        AddNewStepToBorn();
        OnAddMovingArmy?.Invoke(movingArmy, false);
    }

    private void AddNewStepToBorn()
    {
        var stepToBorn = _lastStep + _totalBornArmies * DELTA_STEP;
        stepsToBorn.Add(stepToBorn);

    }

    public void InitQuests(PlayerQuestData questData)
    {
        _questData = questData;
        _questData.OnElementFound += OnElementFound;
    }

    private void OnElementFound()
    {
        var stepToBorn = _lastStep + Mathf.Clamp((int)MyExtensions.GreateRandom(_totalBornArmies),2,10) ;
        stepsToBorn.Add(stepToBorn);
    }

    public void Dispose()
    {
        if (_questData != null)
        {
            _questData.OnElementFound -= OnElementFound;
        }

    }
}

