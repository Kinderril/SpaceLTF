using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamworks;
using TMPro;
using UnityEngine;


public class WindowsAchievements : BaseWindow
{
    public AchievemenElement AchievemenElementPrefab;
    public Transform Layout;
    public TextMeshProUGUI UnloadedSteam;

    public override void Init()
    {
        var steam = SteamStatsAndAchievements.Instance;
        var isInited = SteamManager.Initialized;
        bool bSuccess = SteamUserStats.RequestCurrentStats();
        Layout.ClearTransform();
        if (steam.RequestedStats)
        {
            UnloadedSteam.gameObject.SetActive(false);
            foreach (var achievement in steam.Achievements)
            {
                var pref = DataBaseController.GetItem(AchievemenElementPrefab);
                pref.transform.SetParent(Layout);
                pref.Init(achievement);
            }
        }
        else
        {

            UnloadedSteam.gameObject.SetActive(true);
        }
        base.Init();
    }
    public virtual void OnToStart()
    {
        WindowManager.Instance.OpenWindow(MainState.start);
    }
    public override void Dispose()
    {
        base.Dispose();
    }
}

