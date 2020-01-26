using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class MovingArmyUIController : MonoBehaviour
{
    public Transform Layout;
    private GalaxyEnemiesArmyController _controller;
    public MovingArmyElement MovingArmyElementPrefab;
    private List<MovingArmyElement> _armyElements = new List<MovingArmyElement>();
    private GlobalMapController _globalMap;
    private bool _isInited = false;

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
        if (arg2)
        {
            var element = DataBaseController.GetItem(MovingArmyElementPrefab);
            element.transform.SetParent(Layout);
            _armyElements.Add(element);
            element.Init(_globalMap, arg1);
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


    public void ClearAll()
    {
        _isInited = false;
        _controller.OnAddMovingArmy -= OnAddMovingArmy;
        _armyElements.Clear();
        Layout.ClearTransform();
    }
}

