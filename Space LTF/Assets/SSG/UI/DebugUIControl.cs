using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class DebugUIControl : MonoBehaviour
{
    public List<DebugRationInfo> infos  = new List<DebugRationInfo>();
    private List<DebugRating> debugs;

    private bool enable;
    private ShipBase _selectedShip;

    public void Init()
    {
        for (int i = 0; i < 10; i++)
        {
            var d = DataBaseController.Instance.DataStructPrefabs.DebugRationInfo;
            var dd = DataBaseController.GetItem(d);
            dd.gameObject.transform.SetParent(transform);
            infos.Add(dd);
            dd.gameObject.SetActive(false);
        }
    }

    public void InitShip(ShipBase ship)
    {
        _selectedShip = ship;
        CheckEnable();
    }

    private void CheckEnable()
    {
        if (!enable)
        {
            return;
        }
        if (_selectedShip != null)
        {
            debugs = _selectedShip.Enemies.Values.Select(v => v.DebugRating).ToList();
            for (int i = 0; i < infos.Count; i++)
            {
                var info = infos[i];
                var t = i < debugs.Count;
                info.gameObject.SetActive(t);
                if (t)
                {
                    info.SetInfo(debugs[i]);
                }
            }
        }
        else
        {
            DisableAll();
        }
    }

    public void Enable(bool val)
    {
        if (enable != val)
        {
            enable = val;
            if (val)
            {
                CheckEnable();
            }
            else
            {
                DisableAll();
            }
        }
    }

    private void DisableAll()
    {
        for (int i = 0; i < infos.Count; i++)
        {
            var info = infos[i];
            info.gameObject.SetActive(false);
        }
    }
}

