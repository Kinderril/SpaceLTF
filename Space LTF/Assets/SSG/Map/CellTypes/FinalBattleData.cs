using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Serializable]
public class FinalBattleData
{
//    [field: NonSerialized]
//    private Action<MessageDialogData, Action> _lauchDialog;
//    [field: NonSerialized]
//    private Action _complete;

    private PlayerQuestData mainQuest;
    private bool _isDimplomatyUsed = false;
    private bool _isBuyUsed = false;
    private bool _isStealUsed = false;
    private bool _lastFight = false;

    private const int MONEY_TO_BUY = 140;

    public FinalBattleData()
    {
    }

    public void Init()
    {
        mainQuest = MainController.Instance.MainPlayer.QuestData;
    }

    public MessageDialogData GetDialog()
    {
        int current = mainQuest.mainElementsFound;
        int need = mainQuest.MaxMainElements;
        //        var percents = (float)mainQuest.mainElementsFound / (float)PlayerQuestData.MaxMainElements;

        var list = new List<AnswerDialogData>();
        list.Add(new AnswerDialogData("Ok", null, FirstDialogEnds));
        var mesData = new MessageDialogData(String.Format("This is your main goal. You have {0}/{1} parts.\n Now you should acquire all others.",
            current, need), list);
        return mesData;
    }

    private MessageDialogData FirstDialogEnds()
    {
        if (mainQuest.Completed())
        {
            var list = new List<AnswerDialogData>();
            list.Add(new AnswerDialogData("Ok", () =>
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

    private void Fight()
    {
        _lastFight = true;
        var player = new Player("Final boss");
        var army = ArmyCreator.CreateArmy(90, ArmyCreationMode.equalize, 5, 7, ArmyCreatorData.GetRandom(ShipConfig.federation),
            true, player);
        player.Army = army;
        MainController.Instance.LaunchBattle(MainController.Instance.MainPlayer,player);
    }

    private void EndPart()
    {
        if (mainQuest.Completed())
        {

        }
        else
        {
            FirstDialogEnds();
        }
    }
}

