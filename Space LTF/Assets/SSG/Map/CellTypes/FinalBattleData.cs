using System;
using System.Collections.Generic;
using System.Linq;


[Serializable]
public class FinalBattleData
{
    private PlayerQuestData mainQuest;
    private bool _isDimplomatyUsed = false;
    private bool _isBuyUsed = false;
    private bool _isStealUsed = false;
    private bool _lastFight = false;
    private int _power;

    private const int MONEY_TO_BUY = 140;

    public FinalBattleData()
    {
    }

    public void Init(int power)
    {
        _power = power;
        mainQuest = MainController.Instance.MainPlayer.QuestData;
    }

    public MessageDialogData GetDialog()
    {
        int current = mainQuest.mainElementsFound;
        int need = mainQuest.MaxMainElements;
        var isFull = current >= need;

        var list = new List<AnswerDialogData>();
        MessageDialogData mesData;
        if (isFull)
        {
            list.Add(new AnswerDialogData(Namings.Ok, null, ReadyToGo));
            mesData = new MessageDialogData(String.Format(Namings.DialogTag("finalStartReady"),
                current, need), list);
        }
        else
        {
            var player = MainController.Instance.MainPlayer;
            var armyCount = player.Army.Count;
            var delta = need - current;
            if (delta > armyCount - 1)
            {
                list.Add(new AnswerDialogData(Namings.Ok, LoseGame, null));
                mesData = new MessageDialogData(String.Format(Namings.DialogTag("finalStartNotReady"),
                    current, need), list);
            }
            else
            {
                var shipsYouCanScriface = player.Army.Army.Where(x => x.Ship.ShipType != ShipType.Base).ToList();
                foreach (var data in shipsYouCanScriface)
                {
                    StartShipPilotData shipToDel = data;
                    void Sacrifice()
                    {
                        player.Army.RemoveShip(shipToDel);
                        player.QuestData.AddElement();
                    }
                    list.Add(new AnswerDialogData(String.Format(Namings.Sacrifice, shipToDel.Ship.Name,
                        Namings.ShipType(shipToDel.Ship.ShipType), Namings.ShipConfig(shipToDel.Ship.ShipConfig)), null, () =>
                    {
                        Sacrifice();
                        return GetDialog();
                    }));
                }
                mesData = new MessageDialogData(String.Format(Namings.DialogTag("finalProcess"),
                    current, need), list);
            }
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
        list.Add(new AnswerDialogData(Namings.Fight, Fight, null));
        var mesData = new MessageDialogData(Namings.DialogTag("finalStartFight"), list);
        return mesData;
    }

    /*
    private MessageDialogData FirstDialogEnds()
    {
        if (mainQuest.Completed())
        {
            var list = new List<AnswerDialogData>();
            list.Add(new AnswerDialogData(Namings.Ok, () =>
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
        var rep = MainController.Instance.MainPlayer;
        ShipConfig config = rep.ReputationData.WorstFaction(rep.Army.BaseShipConfig);
        _lastFight = true;
        var player = new Player("Final boss");
        var army = ArmyCreator.CreateArmy(_power, ArmyCreationMode.equalize, 5, 7, ArmyCreatorData.GetRandom(config),
            true, player);
        player.Army.SetArmy(army);
        MainController.Instance.PreBattle(MainController.Instance.MainPlayer, player, true, false);
    }
    public MessageDialogData GetAfterBattleDialog()
    {
        var list = new List<AnswerDialogData>();
        list.Add(new AnswerDialogData(Namings.Ok, EndGameWin, null));
        var mesData = new MessageDialogData(Namings.DialogTag("finalEnd"), list);
        return mesData;
    }

    private void EndGameWin()
    {
        MainController.Instance.BattleData.EndGame(true);
    }
}

