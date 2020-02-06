using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameWindow : BaseWindow
{
    public Transform LayoutMy;
    public Transform LayoutEnemyShipsDestroyed;
    public Transform LayoutRewards;
    public TextMeshProUGUI ScoutsFindField;

    public MoneySlotUI MyMoneyField;
    public MoneySlotUI TotalMoneyRewardField;
    public Button GoToMapButton;

    public Image ToMy;
    public Image ToMiddle;
    public Image ToShips;


    //    private PlayerArmyUI _redArmyUi;

    public FragDataUI FragDataUIPrefab;
    public EndBattleShipPllotInfoUI EndBattleShipPllotInfoUIPrefab;


    private int _totalMoney;
    private int _myMoney;
    private float sum;
    private List<EndBattleShipPllotInfoUI> _allMyShips = new List<EndBattleShipPllotInfoUI>();
    private Dictionary<StartShipPilotData, float> percents = new Dictionary<StartShipPilotData, float>();

    public override void Init()
    {
        //        ToMiddle.enabled = (false);
        ToShips.enabled = (false);
        ToMy.enabled = (false);
        base.Init();
        GoToMapButton.interactable = false;
        var player = MainController.Instance.MainPlayer;
        MyMoneyField.Init(player.MoneyData.MoneyCount);
        float baseSum = 0;
        DrawShipsRewards();
        foreach (var shipStart in player.Army.Army)
        {
            if (shipStart.Ship.ShipType != ShipType.Base)
            {
                percents.Add(shipStart, 0f);
                var s = shipStart.Ship.LastBattleData.GetTotalExp();
                baseSum += s;
            }
        }

        sum = baseSum;
        if (sum <= 1f)
        {
            sum = 1f;
        }
        var lastReward = player.LastReward;
        _totalMoney = lastReward.Money;
        TotalMoneyRewardField.Init(_totalMoney);
        var showScotsFind = lastReward.Moduls.Count > 0 || lastReward.Weapons.Count > 0;
        ScoutsFindField.gameObject.SetActive(showScotsFind);
        ScoutsFindField.text = Namings.TryFormat("Scouts (Level{0}) find something!", player.Parameters.Scouts.Level);
        foreach (var moduls in lastReward.Moduls)
        {
            var itemSlot = InventoryOperation.GetDragableItemSlot();
            itemSlot.transform.SetParent(LayoutRewards, false);
            itemSlot.Init(moduls.CurrentInventory, false);
            itemSlot.StartItemSet(moduls);
        }

        foreach (var moduls in lastReward.Weapons)
        {
            var itemSlot = InventoryOperation.GetDragableItemSlot();
            itemSlot.transform.SetParent(LayoutRewards, false);
            itemSlot.Init(moduls.CurrentInventory, false);
            itemSlot.StartItemSet(moduls);
        }

        ClearTransform(LayoutEnemyShipsDestroyed);
        //        ClearTransform(LayoutMyShipsDestroyed);

        var enemyStats = BattleController.Instance.RedCommander.BattleStats;
        foreach (var sDestr in enemyStats.ShipsDestroyedDatas)
        {
            var d = DataBaseController.GetItem(FragDataUIPrefab);
            d.transform.SetParent(LayoutEnemyShipsDestroyed);
            d.Init(sDestr);
        }
        //        foreach (var shipPilotData in player.Army)
        //        {
        //            percents.Add(shipPilotData,0f);
        //            if (shipPilotData.Ship.FinalBattleData.Destroyed)
        //            {
        ////                shipPilotData.Ship.DamageTimes++;
        ////                if (shipPilotData.Ship.DamageTimes >= 3)
        ////                {
        ////                    shipPilotData.Ship.Destroyed = true;
        ////                }
        //            }
        //        }
        //AllToMe();
        //UpdateMoneys();
    }

    public void MiddleMoneys()
    {
        //        ToMiddle.enabled = (true);
        ToShips.enabled = (true);
        ToMy.enabled = (true);
        _myMoney = (int)(_totalMoney / 2f);
        var nextP = new Dictionary<StartShipPilotData, float>();
        foreach (var percent in percents)
        {
            float v = percent.Key.Ship.LastBattleData.GetTotalExp();
            var p = percent.Key;
            nextP.Add(p, (v / sum) * 0.5f);
        }
        reset(nextP);
        UpdateMoneys();
    }

    public void AllToMe()
    {
        //        ToMiddle.enabled = (false);
        ToShips.enabled = (false);
        ToMy.enabled = (true);
        _myMoney = _totalMoney;
        var nextP = new Dictionary<StartShipPilotData, float>();
        foreach (var percent in percents)
        {
            var p = percent.Key;
            nextP.Add(p, 0);
        }
        reset(nextP);
        UpdateMoneys();
    }


    public void AllToPilots()
    {
        //        ToMiddle.enabled = (false);
        ToShips.enabled = (true);
        ToMy.enabled = (false);
        _myMoney = 0;
        var nextP = new Dictionary<StartShipPilotData, float>();
        foreach (var percent in percents)
        {
            float v = percent.Key.Ship.LastBattleData.GetTotalExp();
            var p = percent.Key;
            nextP.Add(p, v / sum);
        }

        reset(nextP);
        UpdateMoneys();
    }

    private void reset(Dictionary<StartShipPilotData, float> source)
    {

        foreach (var p in source)
        {
            percents[p.Key] = p.Value;
        }
    }

    private void DrawShipsRewards()
    {
        var player = MainController.Instance.MainPlayer;
        foreach (var shipPilotData in player.Army.Army)
        {
            if (shipPilotData.Ship.ShipType != ShipType.Base)
            {
                var a = DataBaseController.GetItem(EndBattleShipPllotInfoUIPrefab);
                a.transform.SetParent(LayoutMy);
                a.Init(shipPilotData);
                _allMyShips.Add(a);
            }
        }
    }

    private void UpdateMoneys()
    {
        GoToMapButton.interactable = true;
        //        TotalMoneyRewardField.Init(0);
        var player = MainController.Instance.MainPlayer;
        var lastReward = player.LastReward;
        var forPilots = lastReward.Money - _myMoney;
        MyMoneyField.Init(_myMoney + player.MoneyData.MoneyCount);
        int spendedMoneys = 0;
        foreach (var infoUi in _allMyShips)
        {
            var p = percents[infoUi.StartShipPilotData];
            var moneyTo = (int)(p * forPilots);
            spendedMoneys += moneyTo;
            infoUi.SetMoneyAdd(moneyTo);
        }
        if (spendedMoneys <= forPilots)
        {
            var remain = forPilots - spendedMoneys;
            int index = 0;
            while (remain > 0)
            {
                var field = _allMyShips[index];
                spendedMoneys++;
                field.SetMoneyAdd(field.MoneyToAdd + 1);
                remain--;
                index++;
                if (index >= _allMyShips.Count)
                {
                    index = 0;
                }
            }
        }
        else
        {
            Debug.LogError("HOW??!! sum:" + sum + "  momeyForPilots:" + forPilots);
        }
        if (spendedMoneys != forPilots)
        {

            Debug.LogError("Wrong spended money! sum:" + sum);
        }
    }

    public void OnClickGoToDebugStart()//THIS is not debug now)
    {
        WindowManager.Instance.OpenWindow(MainState.map);
    }

    public override void Dispose()
    {
        ApplyMoneys();
        percents.Clear();
        _allMyShips.Clear();
        ClearTransform(LayoutMy);
        ClearTransform(LayoutEnemyShipsDestroyed);
        ClearTransform(LayoutRewards);
        base.Dispose();
    }

    private void ApplyMoneys()
    {
        var player = MainController.Instance.MainPlayer;
        player.MoneyData.AddMoney(_myMoney);
        foreach (var myShip in _allMyShips)
        {
            var money = myShip.MoneyToAdd;
            myShip.StartShipPilotData.Pilot.AddMoney(money);
            myShip.StartShipPilotData.Pilot.Stats.AddKills(myShip.StartShipPilotData.Ship.LastBattleData.Kills);
            myShip.StartShipPilotData.Pilot.Stats.AddHeathDamage(myShip.StartShipPilotData.Ship.LastBattleData.HealthDamage);
            myShip.StartShipPilotData.Pilot.Stats.AddShieldDamage(myShip.StartShipPilotData.Ship.LastBattleData.ShieldhDamage);
        }
    }

    public override void Close()
    {
        base.Close();
    }
}

