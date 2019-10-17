
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[System.Serializable]
public class RetranslaitorMapEvent : BaseGlobalMapEvent
{
    private ShipConfig _shipConfig;
    private int _credits = -1;
    private bool _doFight;
    public override string Desc()
    {
        return "Energy storage.";
    }

    public RetranslaitorMapEvent(ShipConfig _shipConfig)
    {
        this._shipConfig = _shipConfig;
    }

    public override MessageDialogData GetDialog()
    {
        var myArmyPower = ArmyCreator.CalcArmyPower(MainController.Instance.MainPlayer.Army) * 1.9f;
        _credits = (int)MyExtensions.GreateRandom(myArmyPower);
         var mesData = new MessageDialogData(String.Format("Power fleet of {0} protect storage of energy.",Namings.ShipConfig(_shipConfig)), new List<AnswerDialogData>()
        {
            new AnswerDialogData("Fight",null,Figth),
//            new AnswerDialogData("Send fail message",null,FailMessage),
            new AnswerDialogData("Steal",null,Steal),
            new AnswerDialogData(Namings.leave,null),
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
            var mesData = new MessageDialogData(String.Format("{0} credits are yours.", _credits), new List<AnswerDialogData>()
            {
                new AnswerDialogData(Namings.Take,() =>
                    MainController.Instance.MainPlayer.MoneyData.AddMoney(_credits)
                ),
            });
            return mesData;
        }
        else
        {
            var mesData = new MessageDialogData("Your scouts failed. And all protectors will fight you.", new List<AnswerDialogData>()
            {
                new AnswerDialogData("Fight",() =>
                    DoFight(1.3f)
                ),
            });
            return mesData;
        }
    }

    private MessageDialogData Figth()
    {
        var mesData = new MessageDialogData("You destroy several enemy ship before battle. And now you should fight other.", new List<AnswerDialogData>()
        {
            new AnswerDialogData("Fight",() =>
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
            var mesData = new MessageDialogData(String.Format( "{0} credits are yours.", _credits), new List<AnswerDialogData>()
            {
                new AnswerDialogData(Namings.Take,() =>
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
            GetArmy(_shipConfig, ArmyCreatorType.laser, (int)(powerCoef * myArmyPower)));
    }
}

