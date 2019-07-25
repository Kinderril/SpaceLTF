using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;


public class StartParameterUI : MonoBehaviour
{
    public TextMeshProUGUI Field;
    private Action<PlayerParameterType, bool> callbackClick;
    private PlayerParameterType _type;

    public void Init(PlayerParameterType type,Action<PlayerParameterType,bool> callback)
    {
        _type = type;
        callbackClick = callback;
    }

    public void UpdateField(Dictionary<PlayerParameterType,int> _levels)
    {
        Field.text = Namings.ParameterName(_type) + ":" + _levels[_type];
    }

    public void OnClickUp()
    {
        callbackClick(_type, true);
    }

    public void OnClickDown()
    {
        callbackClick(_type, false);
    }
}

