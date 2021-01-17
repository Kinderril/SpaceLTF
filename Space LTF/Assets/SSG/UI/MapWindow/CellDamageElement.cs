using TMPro;
using UnityEngine;


public class CellDamageElement : MonoBehaviour
{
    private GlobalMapController _globalMap;
    public SectorCellContainer Army { get; private set; }
    public TextMeshProUGUI Field;
    public int StepCreated;

    public void Init(GlobalMapController globalMap, SectorCellContainer army)
    {
        StepCreated = MainController.Instance.MainPlayer.MapData.Step;
        Army = army;
        _globalMap = globalMap;
        Field.text = Namings.Tag("CellDestroyed");

    }

    public void OnClick()
    {
        _globalMap.SetCameraToCellHome(Army.Data);
    }

}

