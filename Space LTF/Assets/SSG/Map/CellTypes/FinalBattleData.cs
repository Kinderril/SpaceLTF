using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class FinalBattleData
{
//    private PlayerQuestData mainQuest;
//    private bool _isDimplomatyUsed = false;
//    private bool _isBuyUsed = false;
//    private bool _isStealUsed = false;
//    private bool _lastFight = false;
    private int _power;
    private bool _isFull;

//    private const int MONEY_TO_BUY = 140;

    public FinalBattleData()
    {

    }

    public void Init(int power)
    {
        _power = (int)(power);
//        mainQuest = MainController.Instance.MainPlayer.QuestData;
    }

    public void SetReady()
    {
        _isFull = true;
    }

    public MessageDialogData GetDialog()
    {
        var list = new List<AnswerDialogData>();
        MessageDialogData mesData;
        if (_isFull)
        {
            list.Add(new AnswerDialogData(Namings.Tag("Ok"), null, ReadyToGo));
            mesData = new MessageDialogData(Namings.Format(Namings.DialogTag("finalStartReady")), list);
        }
        else
        {
            list.Add(new AnswerDialogData(Namings.Tag("Ok"), LoseGame, null));
            mesData = new MessageDialogData(Namings.Format(Namings.DialogTag("finalStartNotReady")), list);

        }
        return mesData;
    }

    private void LoseGame()
    {
        MainController.Instance.BattleData.EndGame(false);
    }

    private MessageDialogData ReadyToGo()
    {
        var list = new List<AnswerDialogData>();
        list.Add(new AnswerDialogData(Namings.Tag("Fight"), Fight, null));
        var mesData = new MessageDialogData(Namings.DialogTag("finalStartFight"), list);
        return mesData;
    }

    /*
    private MessageDialogData FirstDialogEnds()
    {
        if (mainQuest.Completed())
        {
            var list = new List<AnswerDialogData>();
            list.Add(new AnswerDialogData(Namings.Tag("Ok"), () =>
            {
                MainController.Instance.EndGameWin();
            },null));
            var mesData = new MessageDialogData("You found all parts", list);
            return mesData;
        }
        else
        {

            bool isPointLast = mainQuest.MaxMainElements - mainQuest.mainElementsFound == 1;
            Func<MessageDialogData> onPointEnds = null;
            if (!isPointLast)
            {
                onPointEnds = FirstDialogEnds;
            }
            var list = new List<AnswerDialogData>();
            list.Add(new AnswerDialogData("Attack", Fight));
            if (_isDimplomatyUsed && MainController.Instance.MainPlayer.Parameters.Diplomaty.Level >= 3)
            {
                list.Add(new AnswerDialogData("Use diplomaty", () =>
                {
                    _isDimplomatyUsed = true;
                    MainController.Instance.MainPlayer.QuestData.AddElement();
                }, onPointEnds));
            }
            if (_isStealUsed && MainController.Instance.MainPlayer.Parameters.Scouts.Level >= 3)
            {
                list.Add(new AnswerDialogData("Steal", () =>
                {
                    _isStealUsed = true;
                    MainController.Instance.MainPlayer.QuestData.AddElement();
                }, onPointEnds));
            }
            if (_isBuyUsed && MainController.Instance.MainPlayer.MoneyData.HaveMoney(MONEY_TO_BUY))
            {

                list.Add(new AnswerDialogData("Buy", () =>
                {
                    _isBuyUsed = true;
                    MainController.Instance.MainPlayer.MoneyData.RemoveMoney(MONEY_TO_BUY);
                    MainController.Instance.MainPlayer.QuestData.AddElement();
                }, onPointEnds));
            }
            var mesData = new MessageDialogData("Choose a way to accure next.", list);
            return mesData;
        }
    }
     */
    private void Fight()
    {
//        var player = new PlayerAIMainBoss(name);
        var rep = MainController.Instance.MainPlayer.ReputationData;
        var array = rep.ReputationFaction.OrderBy(x => x.Value).ToArray();
        var conf1 = array[1].Key;
        var conf2 = array[2].Key;
        var armyType = ArmyCreatorLibrary.GetArmy(conf1, conf2);
        if (_power < 22)
        {
            armyType.MainShipCount = 1;
        }
        else
        { 
            armyType.MainShipCount = 2;
        }
        var power = Mathf.Clamp(_power, 14, 999);
        // ArmyCreator.cre
//        player.Army.SetArmy(army);
//        _player = player;


//        var rep = MainController.Instance.MainPlayer;
//        ShipConfig config = rep.ReputationData.WorstFaction(rep.Army.BaseShipConfig);
//        _lastFight = true;
        var player = new PlayerAI("Final boss");
        //        var army = ArmyCreator.CreateArmy(_power, ArmyCreationMode.equalize,
        //            5, 7, ArmyCreatorLibrary.GetArmy(config),
        //            true, player);   
        var army = ArmyCreator.CreateSimpleEnemyArmy(power, armyType, player);
        player.Army.SetArmy(army);
        MainController.Instance.PreBattle(MainController.Instance.MainPlayer, player, true, false);
    }
    public MessageDialogData GetAfterBattleDialog()
    {
        var list = new List<AnswerDialogData>();
        list.Add(new AnswerDialogData(Namings.Tag("Ok"), EndGameWin, null));
        var mesData = new MessageDialogData(Namings.DialogTag("finalEnd"), list);
        return mesData;
    }

    private void EndGameWin()
    {
        MainController.Instance.BattleData.EndGame(true);
    }
}

