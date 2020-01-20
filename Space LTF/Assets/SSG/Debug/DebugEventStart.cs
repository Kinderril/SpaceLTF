using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DebugEventStart
{
    public static void AcitvateDialog(GlobalMapEventType type)
    {
        var windwoMap = WindowManager.Instance.CurrentWindow as MapWindow;
        if (windwoMap != null)
        {
            var sector = new SectorData(1, 1, 1, null, ShipConfig.federation, 1, 1,0f);
            var cellEvent = new EventGlobalMapCell(type, Utils.GetId(), 1,1, sector,20,ShipConfig.mercenary);
            windwoMap.ActivateCellDialog(cellEvent);
        }
        else
        {
            UnityEngine.Debug.LogError($"wrong cur window:{WindowManager.Instance.CurrentWindow}");
        }
    }

}

