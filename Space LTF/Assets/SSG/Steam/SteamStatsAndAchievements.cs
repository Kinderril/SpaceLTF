﻿using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using Steamworks;

// This is a port of StatsAndAchievements.cpp from SpaceWar, the official Steamworks Example.
public class SteamStatsAndAchievements : Singleton<SteamStatsAndAchievements>
{
    public enum Achievement : int
    {
        ACH_WIN_1_BATTLE = 0,
        ACH_WIN_10_BATTLE = 1,
        ACH_WIN_100_BATTLES = 2,
        FULL_ARMY = 3,
        NO_CRASH = 4,       //Завершить игру без разбитых кораблей
        ALL_CELLS = 5,      //Запонить все ячейки  
        TEAM_FIVE = 6,
        WEAPON_MAX_60 = 7,
        SPELL_MAX_30 = 8,
        SHIP_LEVEL_20 = 9,
        SHIP_LEVEL_30 = 10,
        SHIP_LEVEL_40 = 11,
        SHIP_DESTROY_100 = 12,
        SHIP_DESTROY_1000 = 13,
        SHIP_DESTROY_10000 = 14,
        DAMAGE_500 = 15,
        DAMAGE_5000 = 16,
        DAMAGE_50000 = 17,
        NO_WEAPON_WIN = 18,
        COLLECT_MONEY_1000 = 19,                                            
        COLLECT_MONEY_10000 = 20,
    };

    public Achievement_t[] Achievements = new Achievement_t[] {
        new Achievement_t(Achievement.ACH_WIN_1_BATTLE, "Winner", ""),
        new Achievement_t(Achievement.ACH_WIN_10_BATTLE, "Captain", ""),
        new Achievement_t(Achievement.ACH_WIN_100_BATTLES, "Mayor", ""), 
        
        new Achievement_t(Achievement.SPELL_MAX_30, "Big guns", ""),
        new Achievement_t(Achievement.WEAPON_MAX_60, "Improver", ""),
        new Achievement_t(Achievement.TEAM_FIVE, "Team five", ""),

        new Achievement_t(Achievement.SHIP_LEVEL_20, "Friend", ""),
        new Achievement_t(Achievement.SHIP_LEVEL_30, "Boss", ""),
        new Achievement_t(Achievement.SHIP_LEVEL_40, "Master", ""),

        new Achievement_t(Achievement.SHIP_DESTROY_100, "Fighter", ""),
        new Achievement_t(Achievement.SHIP_DESTROY_1000, "Destroyer", ""),
        new Achievement_t(Achievement.SHIP_DESTROY_10000, "Annihilator", ""),  
        
        new Achievement_t(Achievement.DAMAGE_500, "Shooter", ""),
        new Achievement_t(Achievement.DAMAGE_5000, "Sniper", ""),
        new Achievement_t(Achievement.DAMAGE_50000, "Gunner", ""),     
        
        new Achievement_t(Achievement.COLLECT_MONEY_1000, "Banker", ""),
        new Achievement_t(Achievement.COLLECT_MONEY_10000, "Croesus", ""),

        new Achievement_t(Achievement.NO_WEAPON_WIN, "Unusual", ""),

        new Achievement_t(Achievement.NO_CRASH, "No crash", ""),
        new Achievement_t(Achievement.FULL_ARMY, "Full pack", ""),
        new Achievement_t(Achievement.ALL_CELLS, "Full ammo", "")
    };

    // Our GameID
    private CGameID m_GameID;

    // Did we get the stats from Steam?
    public bool RequestedStats => m_bRequestedStats;
    private bool m_bRequestedStats;
    private bool m_bStatsValid;

    // Should we store stats this frame?
    private bool m_bStoreStats;


    protected Callback<UserStatsReceived_t> m_UserStatsReceived;
    protected Callback<UserStatsStored_t> m_UserStatsStored;
    protected Callback<UserAchievementStored_t> m_UserAchievementStored;

    void OnEnable()
    {
        if (!SteamManager.Initialized)
            return;

        // Cache the GameID for use in the Callbacks
        m_GameID = new CGameID(SteamUtils.GetAppID());

        m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
        m_UserStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
        m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);

        // These need to be reset to get the stats upon an Assembly reload in the Editor.
        m_bRequestedStats = false;
        m_bStatsValid = false;
    }

    public void CompleteAchievement(Achievement achievementType)
    {
        var findAcheiv = Achievements.FirstOrDefault(x => x.m_eAchievementID == achievementType);
        if (findAcheiv != null)
        {
            CompleteAchievement(findAcheiv);
        }
    }
    public void CompleteAchievement(Achievement_t achievementType)
    {
        if (!SteamManager.Initialized)
            return;

        if (!m_bRequestedStats)
        {
            // Is Steam Loaded? if no, can't get stats, done
            if (!SteamManager.Initialized)
            {
                m_bRequestedStats = true;
                return;
            }

            // If yes, request our stats
            bool bSuccess = SteamUserStats.RequestCurrentStats();

            // This function should only return false if we weren't logged in, and we already checked that.
            // But handle it being false again anyway, just ask again later.
            m_bRequestedStats = bSuccess;
        }

        if (!m_bStatsValid)
            return;
        UnlockAchievement(achievementType);

        //Store stats in the Steam database if necessary
        if (m_bStoreStats)
        {
            // already set any achievements in UnlockAchievement
            var playerStat = MainController.Instance.Statistics;
            SteamUserStats.SetStat("NumWins", playerStat.Wins);
            bool bSuccess = SteamUserStats.StoreStats();
            // If this failed, we never sent anything to the server, try
            // again later.
            m_bStoreStats = !bSuccess;
        }
    }

    //-----------------------------------------------------------------------------
    // Purpose: Unlock this achievement
    //-----------------------------------------------------------------------------
    private void UnlockAchievement(Achievement_t achievement)
    {
        achievement.m_bAchieved = true;

        // the icon may change once it's unlocked
        //achievement.m_iIconImage = 0;

        // mark it down
        SteamUserStats.SetAchievement(achievement.m_eAchievementID.ToString());

        // Store stats end of frame
        m_bStoreStats = true;
    }

    //-----------------------------------------------------------------------------
    // Purpose: We have stats data from Steam. It is authoritative, so update
    //			our data with those results now.
    //-----------------------------------------------------------------------------
    private void OnUserStatsReceived(UserStatsReceived_t pCallback)
    {
        if (!SteamManager.Initialized)
            return;

        // we may get callbacks for other games' stats arriving, ignore them
        if ((ulong)m_GameID == pCallback.m_nGameID)
        {
            if (EResult.k_EResultOK == pCallback.m_eResult)
            {
                Debug.Log("Received stats and achievements from Steam\n");

                m_bStatsValid = true;

                // load achievements
                foreach (Achievement_t ach in Achievements)
                {
                    bool ret = SteamUserStats.GetAchievement(ach.m_eAchievementID.ToString(), out ach.m_bAchieved);
                    if (ret)
                    {
                        ach.m_strName = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "name");
                        ach.m_strDescription = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "desc");
                    }
                    else
                    {
                        Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + ach.m_eAchievementID + "\nIs it registered in the Steam Partner site?");
                    }
                }

                // load stats 
                var playerStat = MainController.Instance.Statistics;
            }
            else
            {
                Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
            }
        }
    }

    //-----------------------------------------------------------------------------
    // Purpose: Our stats data was stored!
    //-----------------------------------------------------------------------------
    private void OnUserStatsStored(UserStatsStored_t pCallback)
    {
        // we may get callbacks for other games' stats arriving, ignore them
        if ((ulong)m_GameID == pCallback.m_nGameID)
        {
            if (EResult.k_EResultOK == pCallback.m_eResult)
            {
                Debug.Log("StoreStats - success");
            }
            else if (EResult.k_EResultInvalidParam == pCallback.m_eResult)
            {
                // One or more stats we set broke a constraint. They've been reverted,
                // and we should re-iterate the values now to keep in sync.
                Debug.Log("StoreStats - some failed to validate");
                // Fake up a callback here so that we re-load the values.
                UserStatsReceived_t callback = new UserStatsReceived_t();
                callback.m_eResult = EResult.k_EResultOK;
                callback.m_nGameID = (ulong)m_GameID;
                OnUserStatsReceived(callback);
            }
            else
            {
                Debug.Log("StoreStats - failed, " + pCallback.m_eResult);
            }
        }
    }

    //-----------------------------------------------------------------------------
    // Purpose: An achievement was stored
    //-----------------------------------------------------------------------------
    private void OnAchievementStored(UserAchievementStored_t pCallback)
    {
        // We may get callbacks for other games' stats arriving, ignore them
        if ((ulong)m_GameID == pCallback.m_nGameID)
        {
            if (0 == pCallback.m_nMaxProgress)
            {
                Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
            }
            else
            {
                Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' progress callback, (" + pCallback.m_nCurProgress + "," + pCallback.m_nMaxProgress + ")");
            }
        }
    }

    //-----------------------------------------------------------------------------
    // Purpose: Display the user's stats and achievements
    //-----------------------------------------------------------------------------
    public void Render()
    {
        if (!SteamManager.Initialized)
        {
            GUILayout.Label("Steamworks not Initialized");
            return;
        }

        var playerStat = MainController.Instance.Statistics;
        GUILayout.Label("m_ulTickCountGameStart: " + playerStat.Wins);

        GUILayout.BeginArea(new Rect(Screen.width - 300, 0, 300, 800));
        foreach (Achievement_t ach in Achievements)
        {
            GUILayout.Label(ach.m_eAchievementID.ToString());
            GUILayout.Label(ach.m_strName + " - " + ach.m_strDescription);
            GUILayout.Label("Achieved: " + ach.m_bAchieved);
            GUILayout.Space(20);
        }

        // FOR TESTING PURPOSES ONLY!
        if (GUILayout.Button("RESET STATS AND ACHIEVEMENTS"))
        {
            SteamUserStats.ResetAllStats(true);
            SteamUserStats.RequestCurrentStats();
            // OnGameStateChange(EClientGameState.k_EClientGameActive);
        }
        GUILayout.EndArea();
    }

    public class Achievement_t
    {
        public Achievement m_eAchievementID;
        public string m_strName;
        public string m_strDescription;
        public bool m_bAchieved;

        /// <summary>
        /// Creates an Achievement. You must also mirror the data provided here in https://partner.steamgames.com/apps/achievements/yourappid
        /// </summary>
        /// <param name="achievement">The "API Name Progress Stat" used to uniquely identify the achievement.</param>
        /// <param name="name">The "Display Name" that will be shown to players in game and on the Steam Community.</param>
        /// <param name="desc">The "Description" that will be shown to players in game and on the Steam Community.</param>
        public Achievement_t(Achievement achievementID, string name, string desc)
        {
            m_eAchievementID = achievementID;
            m_strName = name;
            m_strDescription = desc;
            m_bAchieved = false;
        }
    }
}