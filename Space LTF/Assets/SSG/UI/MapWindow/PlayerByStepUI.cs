using System;
using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerByStepUI : MonoBehaviour
{
    private PlayerByStepDamage _info;
    public TextMeshProUGUI Field;
    public GameObject GoodObject;
    public GameObject BadObject;
    public void Init(PlayerByStepDamage info)
    {
        this._info = info;
        if (info.IsEnable)
        {
            _info.OnStep += OnStep;
            OnStep(info._curRemainSteps);
        }
        else
        {
            gameObject.SetActive(false);
        }

    }

    private void OnStep(int obj)
    {
        Field.text = Namings.Format(Namings.Tag("StabilizaInfo"),_info._curRemainSteps);
        var isGood = _info._curRemainSteps > 0;
        GoodObject.gameObject.SetActive(isGood);
        BadObject.gameObject.SetActive(!isGood);
    }

    public void Dispose()
    {
        if (_info.IsEnable)
        {
            _info.OnStep -= OnStep;
        }
    }
}
