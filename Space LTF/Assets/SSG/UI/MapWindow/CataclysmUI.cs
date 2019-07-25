using UnityEngine;
using System.Collections;
using TMPro;

public class CataclysmUI : MonoBehaviour
{
    public TextMeshProUGUI Field;
    public GameObject StartedObject;
    private PlayerMapData _data;
    public void Init(PlayerMapData data)
    {
        _data = data;
        UpdateManual();
        _data.OnStep += OnStpe;
    }

    private void OnStpe()
    {
        UpdateManual();
    }

    private void UpdateManual()
    {
        var remainSteps = _data.GalaxyData.StartDeathStep - _data.Step;
        var isStarted = remainSteps <= 0;
        Field.gameObject.SetActive(!isStarted);
        StartedObject.SetActive(isStarted);
        Field.text = $"Remain days {remainSteps}";
    }

    public void Dispose()
    {
        _data.OnStep += OnStpe;
    }

}
