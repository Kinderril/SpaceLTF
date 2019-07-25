using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;


public class FragDataUI : MonoBehaviour
{
    public TextMeshProUGUI field;

    public void Init(ShipDestroyedData data)
    {
        field.text = data.Type.ToString() + "  " + data.Config.ToString();
    }
}

