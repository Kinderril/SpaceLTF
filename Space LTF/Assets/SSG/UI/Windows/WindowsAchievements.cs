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
        
        if (isInited)
        {
            bool bSuccess = SteamUserStats.RequestCurrentStats();
            Layout.ClearTransform();
            if (steam.RequestedStats)
            {
                ShowAllAchievements();
            }
            else
            {

                UnloadedSteam.gameObject.SetActive(true);
            }
        }
        else
        {

            UnloadedSteam.gameObject.SetActive(true);
        }
#if UNITY_EDITOR
        Layout.ClearTransform();
        ShowAllAchievements();
#endif
        base.Init();
    }

    private void ShowAllAchievements()
    {
        UnloadedSteam.gameObject.SetActive(false);
        var steam = SteamStatsAndAchievements.Instance;
        foreach (var achievement in steam.Achievements)
        {
            var pref = DataBaseController.GetItem(AchievemenElementPrefab);
            pref.transform.SetParent(Layout);
            pref.Init(achievement);
        }
        Layout.localPosition = Vector3.up * -10000;
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

