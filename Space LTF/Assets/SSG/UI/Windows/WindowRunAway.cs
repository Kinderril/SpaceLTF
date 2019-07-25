using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class WindowRunAway : BaseWindow
{
    public Transform LayoutMy;
    public ShipRunAwayUI ShipRunAwayUIPrefab;


    public override void Init()
    {
        var player = MainController.Instance.MainPlayer;
        foreach (var pilotData in player.Army)
        {
            var pref = DataBaseController.GetItem(ShipRunAwayUIPrefab);
            pref.gameObject.transform.SetParent(LayoutMy);
            pref.Init(pilotData.Ship,pilotData.Pilot);
        }
        base.Init();
    }

    public override void Dispose()
    {
        LayoutMy.ClearTransform();
        base.Dispose();
    }

    void OnClickToMap()
    {
        WindowManager.Instance.OpenWindow(MainState.map);
    }
}

