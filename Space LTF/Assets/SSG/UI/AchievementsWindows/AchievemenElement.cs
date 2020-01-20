using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class AchievemenElement : UIElementWithTooltip
{
    private SteamStatsAndAchievements.Achievement_t _achievementT;
    public Image Icon;
    public void Init(SteamStatsAndAchievements.Achievement_t achievementT)
    {
        _achievementT = achievementT;
        Sprite spr = DataBaseController.Instance.DataStructPrefabs.GetAchiewementIcon(achievementT.m_strName);
        Icon.sprite = spr;
        Icon.color = _achievementT.m_bAchieved ? Color.white : Color.gray;
    }

    protected override string TextToTooltip()
    {
        return $"{_achievementT.ShortName}\n{_achievementT.m_strDescription}";
    }
}

