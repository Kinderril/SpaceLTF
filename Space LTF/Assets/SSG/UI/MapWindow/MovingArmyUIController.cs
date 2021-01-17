using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class MovingArmyUIController : MonoBehaviour
{
    public Transform Layout;
//    public Transform LayoutFight;
    private GalaxyEnemiesArmyController _controller;
    public MovingArmyElement MovingArmyElementPrefab;
    public CellDamageElement CellDamagePrefab;
    public FightResult FightResultPrefab;
    private List<MovingArmyElement> _armyElements = new List<MovingArmyElement>();
    private List<FightResult> _fightResults = new List<FightResult>();
    private List<CellDamageElement> _cellsDamage = new List<CellDamageElement>();
    private GlobalMapController _globalMap;
    private bool _isInited = false;
    private int _movingArmies = 0;

    public void Init(GalaxyEnemiesArmyController controller, GlobalMapController globalMap)
    {
        if (_isInited)
        {
            return;
        }

        _isInited = true;
        _globalMap = globalMap;
        _controller = controller;
        _controller.OnAddMovingArmy += OnAddMovingArmy;
    }

    private void OnAddMovingArmy(MovingArmy arg1, bool arg2)
    {
        FightResaults(arg1,arg2);

        if (!arg1.IsAllies && arg1 is SpecOpsMovingArmy)
        {
            if (arg2)
            {
                _movingArmies++;
                if (_movingArmies <= 3)
                {
                    var element = DataBaseController.GetItem(MovingArmyElementPrefab);
                    element.transform.SetParent(Layout);
                    _armyElements.Add(element);
                    element.Init(_globalMap, arg1);
                }
            }
            else
            {
                var toDel = _armyElements.FirstOrDefault(x => x.Army == arg1);
                if (toDel != null)
                {
                    _armyElements.Remove(toDel);
                    GameObject.Destroy(toDel.gameObject);
                }
            }
        }


    }

    private void FightResaults(MovingArmy movingArmy, bool onAdd)
    {
        if (!onAdd)
        {
            var element = DataBaseController.GetItem(FightResultPrefab);
            element.transform.SetParent(Layout);
            element.InitToCell(_globalMap,movingArmy);
            _fightResults.Add(element);
        }
    }


    public void ClearAll()
    {
        _isInited = false;
        if (_controller != null)
            _controller.OnAddMovingArmy -= OnAddMovingArmy;
//        _fightResults.Clear();
        foreach (var movingArmyElement in _armyElements)
        {
            GameObject.Destroy(movingArmyElement.gameObject);
        }
        _armyElements.Clear();
    }

    public void DoStep()
    {
        var curStep = MainController.Instance.MainPlayer.MapData.Step;
        var fightToDel = _fightResults.Where(x => Mathf.Abs(x.StepCreated - curStep) > 1).ToList();
        foreach (var fightResult in fightToDel)
        {
            if (_fightResults != null)
            {
                _fightResults.Remove(fightResult);
                GameObject.Destroy(fightResult.gameObject);
            }
        }       
        var cellDamageElements = _cellsDamage.Where(x => Mathf.Abs(x.StepCreated - curStep) > 1).ToList();
        foreach (var cellDamageElement in cellDamageElements)
        {
            if (cellDamageElement != null)
            {
                _cellsDamage.Remove(cellDamageElement);
                GameObject.Destroy(cellDamageElement.gameObject);
            }
        }

    }

    public void CellDataChange(SectorCellContainer sectorCellContainer)
    {
        if (sectorCellContainer.Data is FreeActionGlobalMapCell)
        {
            var element = DataBaseController.GetItem(CellDamagePrefab);
            element.transform.SetParent(Layout);
            element.Init(_globalMap,sectorCellContainer);
            _cellsDamage.Add(element);
        }

    }
}

