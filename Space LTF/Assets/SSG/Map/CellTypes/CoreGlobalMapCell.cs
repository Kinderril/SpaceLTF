using System;
using System.Collections.Generic;
using UnityEngine;

public enum GlobalCellType
{
    simple,
    army,
    talk,
}

[Serializable]
public class CoreGlobalMapCell : ArmyGlobalMapCell
{
    public GlobalCellType CellType;
    private bool _afterFightActivated = false;
    public bool HaveInfo = false;
    public bool Taken = false;
    [field: NonSerialized]
    public event Action<GlobalMapCell> OnTaken;


    public CoreGlobalMapCell(int power, int id, int intX, int intZ) : base( power,ShipConfig.mercenary, id, ArmyCreatorType.destroy, intX, intZ)
    {
        _power = power;
        WDictionary<GlobalCellType> chances = new WDictionary<GlobalCellType>(new Dictionary<GlobalCellType, float>()
        {
            { GlobalCellType.simple, 2},
            { GlobalCellType.army, 4},
            { GlobalCellType.talk, 5},
        });
        WDictionary<ArmyCreatorType> armyTypes = new WDictionary<ArmyCreatorType>(new Dictionary<ArmyCreatorType, float>()
        {
            { ArmyCreatorType.simple, 2},
            { ArmyCreatorType.laser, 4},
            { ArmyCreatorType.destroy, 2},
            { ArmyCreatorType.rocket, 2},
        });
        _armyType = armyTypes.Random();
        CellType = chances.Random();
    }

    public override void OpenInfo()
    {
        base.OpenInfo();
        HaveInfo = true;
    }

    public override string Desc()
    {
        if (Taken)
        {
            return "Retranslaitor (Taken)";
        }
        return "Retranslaitor";
    }

    public override void Take()
    {

    }

    public override MessageDialogData GetDialog()
    {
        switch (CellType)
        {
            case GlobalCellType.army:
                return GetArmyDialog();
            case GlobalCellType.talk:
                return GetTalkDialog();
        }
        return GetSimlpleDialog();
    }


    private MessageDialogData GetArmyDialog()
    {
        List<AnswerDialogData> answerDialog = new List<AnswerDialogData>();
        answerDialog.Add(new AnswerDialogData("Attack.", Fight));
        var mesData = new MessageDialogData("Target is under protection.", answerDialog);
        return mesData;
    }

    private MessageDialogData GetSimlpleDialog()
    {
        List<AnswerDialogData> answerDialog = new List<AnswerDialogData>();
        answerDialog.Add(new AnswerDialogData("Attack.", Fight));
        var mesData = new MessageDialogData("Target is under protection.", answerDialog);
        return mesData;
    }

    private int MoneyToBuy()
    {
        return _power*2;
    }

    private MessageDialogData GetTalkDialog()
    {
        var player = MainController.Instance.MainPlayer;
        List<AnswerDialogData> answerDialog = new List<AnswerDialogData>();
        answerDialog.Add(new AnswerDialogData("Attack.", Fight));
        if (player.MoneyData.HaveMoney(MoneyToBuy()))
            answerDialog.Add(new AnswerDialogData(String.Format("Buy for {0} credits.", MoneyToBuy()), Buy));
        if (player.Parameters.Diplomaty.Level >= 3)
            answerDialog.Add(new AnswerDialogData(String.Format("Use diplomaty."), Diplomaty));
//        if (player.Parameters.Scouts.Level >= 1)
        answerDialog.Add(new AnswerDialogData(String.Format("Steal."), Steal));
        var mesData = new MessageDialogData("Some other fleet already have your target.", answerDialog);
        return mesData;
    }

    private void Buy()
    {
        MainController.Instance.MainPlayer.MoneyData.RemoveMoney(MoneyToBuy());
        WindowManager.Instance.InfoWindow.Init(null, "Element was purchased");
        SetTake();
        MainController.Instance.MainPlayer.QuestData.AddElement();
    }

    private void Diplomaty()
    {
        WindowManager.Instance.InfoWindow.Init(null, "Element is yours");
        SetTake();
        MainController.Instance.MainPlayer.QuestData.AddElement();
    }

    private void SetTake()
    {
        Taken = true;
        if (OnTaken != null)
        {
            OnTaken(this);
        }
    }

    private void Steal()
    {
        WDictionary<bool> wd = new WDictionary<bool>(new Dictionary<bool, float>()
        {
            { true,MainController.Instance.MainPlayer.Parameters.Scouts.Level},
            { false,2},
        });
        if (wd.Random())
        {

            WindowManager.Instance.InfoWindow.Init(null, "Element is yours");
            SetTake();
            MainController.Instance.MainPlayer.QuestData.AddElement();
        }
        else
        {
            WindowManager.Instance.InfoWindow.Init(Fight, "Fail. Now you should fight");

        }
    }

    protected override MessageDialogData GetLeavedActionInner()
    {
        if (_afterFightActivated)
        {
            List<AnswerDialogData> answerDialog = new List<AnswerDialogData>();
            answerDialog.Add(new AnswerDialogData("Ok.", null, null));
            var mesData = new MessageDialogData("This part is yours now.", answerDialog);
            return mesData;
        }
        else
        {
         return base.GetLeavedActionInner();
        }
    }

    private void Fight()
    {
        _afterFightActivated = true;
        MainController.Instance.PreBattle(MainController.Instance.MainPlayer, GetArmy());
    }

    public override Color Color()
    {
        return new Color(204f/255f, 255f/255f, 153f/255f);
    }

    public override bool OneTimeUsed()
    {
        return true;
    }
}