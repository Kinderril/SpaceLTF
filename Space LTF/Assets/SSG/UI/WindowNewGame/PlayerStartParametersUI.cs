using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PlayerStartParametersUI : MonoBehaviour
{
    private int _remainLevelUp = Library.START_PLAYER_FREE_PARAMETERS;
    public StartParameterUI PrefabStartParameterUI;
    public TextMeshProUGUI RemainField;
    public Transform LayoutParams;
    private Dictionary<PlayerParameterType, int> _curLevel = new Dictionary<PlayerParameterType, int>();
    private List<StartParameterUI> _allFileds = new List<StartParameterUI>();

    public void Init()
    {
        foreach (PlayerParameterType ppt in (PlayerParameterType[])Enum.GetValues(typeof(PlayerParameterType)))
        {
            _curLevel.Add(ppt, 1);
            var p = DataBaseController.GetItem(PrefabStartParameterUI);
            p.transform.SetParent(LayoutParams, false);
            p.Init(ppt, OnParamClick);
            _allFileds.Add(p);
        }
        UpdateFields();
    }

    public void OnParamClick(PlayerParameterType arg1, bool arg2)
    {
        if (arg2)
        {
            if (_remainLevelUp > 0)
            {

                _remainLevelUp--;
                _curLevel[arg1] = _curLevel[arg1] + 1;
            }
        }
        else
        {
            var cur = _curLevel[arg1];
            if (cur > 1)
            {
                _remainLevelUp++;
                _curLevel[arg1] = cur - 1;
            }
        }
        UpdateFields();
    }

    private void UpdateFields()
    {
        foreach (var parameterUi in _allFileds)
        {
            parameterUi.UpdateField(_curLevel);
        }
        RemainField.text = Namings.Tag("Remain") + ":" + _remainLevelUp;
    }

    public void Dispose()
    {
        LayoutParams.ClearTransform();
        _curLevel.Clear();
        _allFileds.Clear();
    }

    public Dictionary<PlayerParameterType, int> GetCurrentLevels()
    {
        _curLevel.Clear();
        return _curLevel;
    }

    public bool CheckFreePoints()
    {
        return _remainLevelUp > 0;

    }

    public void SetData(Dictionary<PlayerParameterType, int> ppar)
    {
        _remainLevelUp = 0;
        _curLevel = ppar;
        UpdateFields();
    }
}

