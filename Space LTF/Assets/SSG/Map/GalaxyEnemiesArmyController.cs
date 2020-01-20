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
    private List<int> stepsToBorn = new List<int>();

    public GalaxyEnemiesArmyController(List<GlobalMapCell> cells, int armiesToMove = 3)
    {
        if (armiesToMove > 0)
        {
            List<int> posibleSteps = new List<int>();
            var count = Mathf.Sqrt(cells.Count);
            for (int i = 10; i < count - 10; i++)
            {
                posibleSteps.Add(i);
            }
#if UNITY_EDITOR
            posibleSteps.Add(3);
#endif

            stepsToBorn = posibleSteps.RandomElement(armiesToMove);
            foreach (var i in stepsToBorn)
            {
                Debug.Log($"STEP TO BORN:{i}");
            }
            _cells = cells;
        }
    }

    public void CheckEnemiesMovingArmies(int step, GlobalMapCell curCell)
    {
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
                    var coordinates = $"{movingArmy.CurCell.indX},{movingArmy.CurCell.indZ}";
                    WindowManager.Instance.InfoWindow.Init(null,
                        String.Format(Namings.Tag("MovingArmyBorn"),
                            Namings.ShipConfig(movingArmy._player.Army.BaseShipConfig), coordinates));
                }
            }
        }
    }

    private void DestroyCallback(MovingArmy movingArmy)
    {
        OnAddMovingArmy?.Invoke(movingArmy, false);
    }
}

