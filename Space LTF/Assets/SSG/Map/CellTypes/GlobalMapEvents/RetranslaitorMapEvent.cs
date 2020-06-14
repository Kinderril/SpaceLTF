
using System;
using System.Collections.Generic;

[System.Serializable]
public class RetranslaitorMapEvent : BaseGlobalMapEvent
{
    // private ShipConfig _shipConfig;
    private int _credits = -1;
    private bool _doFight;
    public override string Desc()
    {
        return Namings.Tag("Energy storage");
    }

    public RetranslaitorMapEvent(ShipConfig _shipConfig)
        : base(_shipConfig)
    {
        // this._shipConfig = _shipConfig;
    }

    public override MessageDialogData GetDialog()
    {
        var player = MainController.Instance.MainPlayer;
        var myArmyPower = ArmyCreator.CalcArmyPower(player.Army) * 1.9f;
        _credits = (int)(MyExtensions.GreateRandom(myArmyPower) * Library.MONEY_QUEST_COEF * player.SafeLinks.CreditsCoef);
        var mesData = new MessageDialogData(Namings.Format(Namings.Tag("StoragePower"), Namings.ShipConfig(_config)), new List<AnswerDialogData>()
        {
            new AnswerDialogData(Namings.Tag("Fight"),null,Figth),
//            new AnswerDialogData("Send fail message",null,FailMessage),
            new AnswerDialogData(Namings.Tag("Steal"),null,Steal),
            new AnswerDialogData(Namings.Tag("leave"),null),
        });
        return mesData;
    }

    //    private MessageDialogData FailMessage()
    //    {
    //        
    //
    //    }

    private MessageDialogData Steal()
    {
        if (SkillWork(3, ScoutsLevel))
        {
            var mesData = new MessageDialogData(Namings.Format( Namings.Tag("creditYours"), _credits), new List<AnswerDialogData>()
            {
                new AnswerDialogData(Namings.Tag("Take"),() =>
                    MainController.Instance.MainPlayer.MoneyData.AddMoney(_credits)
                ),
            });
            return mesData;
        }
        else
        {
            var mesData = new MessageDialogData(Namings.Tag("scoutsFailed"), new List<AnswerDialogData>()
            {
                new AnswerDialogData(Namings.Tag("Fight"),() =>
                    DoFight(1.3f)
                ),
            });
            return mesData;
        }
    }

    private MessageDialogData Figth()
    {
        var mesData = new MessageDialogData( Namings.Tag("destrySeveralEnemies"), new List<AnswerDialogData>()
        {
            new AnswerDialogData(Namings.Tag("Fight"),() =>
                DoFight(1f)
            ),
        });
        return mesData;
    }

    public override MessageDialogData GetLeavedActionInner()
    {
        if (_doFight)
        {
            _doFight = false;
            var mesData = new MessageDialogData(Namings.Format(Namings.Tag("creditYours"), _credits), new List<AnswerDialogData>()
            {
                new AnswerDialogData(Namings.Tag("Take"),() =>
                    MainController.Instance.MainPlayer.MoneyData.AddMoney(_credits)
                ),
            });
            return mesData;
        }
        else
        {
            return null;
        }
    }

    private void DoFight(float powerCoef = 1f)
    {
        var myArmyPower = ArmyCreator.CalcArmyPower(MainController.Instance.MainPlayer.Army) * 0.8f;
        MainController.Instance.PreBattle(MainController.Instance.MainPlayer,
            GetArmy(_config, (int)(powerCoef * myArmyPower)));
    }
}

