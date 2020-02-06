using System;
using TMPro;
using UnityEngine;

public class CataclysmUI : MonoBehaviour
{
    public TextMeshProUGUI Field;
    //    public GameObject StartedObject;
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
        //        Field.gameObject.SetActive(!isStarted);
        if (isStarted)
        {
            Field.text = Namings.TryFormat(Namings.Tag("CataclysmProcess"), _data.GalaxyData.CellsDestroyed);
        }
        else
        {
            Field.text = Namings.TryFormat(Namings.Tag("RemainCataclysm"), remainSteps);
        }
        //        StartedObject.SetActive(isStarted);
    }

    public void Dispose()
    {
        _data.OnStep -= OnStpe;
    }

}
