using TMPro;
using UnityEngine;


public class MovingArmyElement : MonoBehaviour
{
    private GlobalMapController _globalMap;
    public MovingArmy Army { get; private set; }
    public TextMeshProUGUI Field;

    public void Init(GlobalMapController globalMap, MovingArmy army)
    {
        Army = army;
        _globalMap = globalMap;
        Field.text = Army.Name();

    }

    public void OnClick()
    {
        _globalMap.SetCameraToCellHome(Army.CurCell.Data);
    }

}

