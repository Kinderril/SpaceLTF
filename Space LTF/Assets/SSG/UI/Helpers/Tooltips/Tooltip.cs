using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class Tooltip : BaseTooltip
{
    public void Init(string info, GameObject causeTransform)
    {
        Field.text = info;
        Init(causeTransform);
    }

    public TextMeshProUGUI Field;
}
