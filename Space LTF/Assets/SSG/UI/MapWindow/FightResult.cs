using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public  class FightResult : MonoBehaviour
{
    public GameObject Win;
    public GameObject Lose;
    private SectorCellContainer _cell = null;
    private GlobalMapController _globalMap;
    public int StepCreated;
    public void InitToCell(GlobalMapController _globalMap, MovingArmy movingArmy)
    {
        StepCreated = MainController.Instance.MainPlayer.MapData.Step;
        this._globalMap = _globalMap;
        Win.SetActive(!movingArmy.IsAllies);
        Lose.SetActive(movingArmy.IsAllies);
        if (movingArmy.CurCell != null)
            _cell = movingArmy.CurCell;

    }

    public void OnClick()
    {
        if (_cell != null)
        {
            _globalMap.SetCameraToCellHome(_cell.Data);
        }
    }
}

