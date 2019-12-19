using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;


public class CompleteQuestOnStartUI   : MonoBehaviour
{
    private BaseQuestOnStart _quest;
    public TextMeshProUGUI InfoField;
    public ImageWithTooltip WeaponTooltip;
    public ImageWithTooltip ModulTooltip;
    public ImageWithTooltip MoneyTooltip;
    private Action _closeCallback;
    public TextMeshProUGUI NameField;
    public void Init(BaseQuestOnStart quest,Action closeCallback)
    {
        NameField.text = quest.Name;
        _closeCallback = closeCallback;
        _quest = quest;
        gameObject.SetActive(true);
        var wSprite = DataBaseController.Instance.DataStructPrefabs.GetWeaponIcon(quest.WeaponReward.WeaponType);
        WeaponTooltip.Init(wSprite, $"{Namings.Weapon(quest.WeaponReward.WeaponType)} ({quest.WeaponReward.Level})");   
        var mSprite = DataBaseController.Instance.DataStructPrefabs.GetModulIcon(quest.ModulReward.Type);
        ModulTooltip.Init(mSprite, $"{Namings.SimpleModulName(quest.ModulReward.Type)} ({quest.ModulReward.Level})");
        MoneyTooltip.Init(DataBaseController.Instance.DataStructPrefabs.MoneyIcon, quest.MoneyCount.ToString());
        InfoField.text = Namings.Tag("QuestReward");
    }

    public void OnCloseClick()
    {
           _closeCallback();
    }

    public void OnTakeRewardWeapon()
    {
        _quest.TakeWeapon();
        _closeCallback();
    }   
    public void OnTakeRewardModul()
    {

        _quest.TakeModul();
        _closeCallback();
    }   
    public void OnTakeReward1Money()
    {

        _quest.TakeMoney();
        _closeCallback();
    }
}

