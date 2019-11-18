
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum AnomalyTypeUnlock
{
    freezing,
     explosive,
     fire,
     impulse
}

[System.Serializable]
public class AnomalyMapEvent : BaseGlobalMapEvent
{
    private int remainTryies = 0;
    private bool _withInfo;
    private const int COUNT_TO_UNLOCK = 4;
    private const int COUNT_WITH_SCOUTS = 4;
    private const int COUNT_WITHOUT_SCOUTS = 6;
    private int _curPoints = 0;
    private int _lastPlus = 0;

    private AnomalyTypeUnlock good2;
    private AnomalyTypeUnlock good1;

    public override string Desc()
    {
        return "Anomaly";
    }

    public override MessageDialogData GetDialog()
    {
        List<AnomalyTypeUnlock> list = new List<AnomalyTypeUnlock>()
        {
            AnomalyTypeUnlock.explosive,AnomalyTypeUnlock.fire,AnomalyTypeUnlock.freezing,AnomalyTypeUnlock.impulse
        };
        good1 = list.RandomElement();
        list.Remove(good1);
        good2 = list.RandomElement();

        var mianAnswers = new List<AnswerDialogData>();

        mianAnswers.Add(new AnswerDialogData($"Send scouts and try to get some info.", null,scouts));
        mianAnswers.Add(new AnswerDialogData($"I believe at myself. Come close to it.",null,comeClose));
        mianAnswers.Add(new AnswerDialogData("No. Go away.", null));

        var mesData = new MessageDialogData("You find a strange anomaly. Do you want to investigate it?", mianAnswers);
        return mesData;
    }

    private MessageDialogData comeClose()
    {
        _withInfo = false;
        remainTryies = COUNT_WITHOUT_SCOUTS;
        return UnlockDialog();
    }

    private MessageDialogData scouts()
    {
        _withInfo = true;
        remainTryies = COUNT_WITH_SCOUTS;
        return UnlockDialog();
    }

    public MessageDialogData UnlockDialog()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData($"Use freezing gun", null, freezing));
        mianAnswers.Add(new AnswerDialogData($"Use explosive", null, explosive));
        mianAnswers.Add(new AnswerDialogData("Use impulse", null,impulse));
        mianAnswers.Add(new AnswerDialogData("Use fire", null,fire));
        string move = "";
        if (_withInfo)
        {
            switch (_lastPlus)
            {
                case 2:
                    move = "Something change.";
                    break;
                case 1:
                    move = "Looks like you doing right.";
                    break;
                case 0:
                    move = "Wrong.";
                    break;
            }
        }
        string ifno = _withInfo ? $"No info from scouts .Choose next move. Tries remain:{remainTryies}." : $"{move}{_curPoints}/{COUNT_TO_UNLOCK} parts unlocked. Choose next move. Tries remain:{remainTryies}.";
        var mesData = new MessageDialogData(ifno, mianAnswers);
        return mesData;
    }

    private bool ShallShowFinalDialog()
    {
        if (Fail)
        {
            return true;
        }

        if (Complete)
        {
            return true;
        }

        return false;
    }

    private bool Fail => (remainTryies <= 0);
    private bool Complete => (_curPoints >= COUNT_TO_UNLOCK);


    private void TestOption(AnomalyTypeUnlock option)
    {
        remainTryies--;
        if (good1 == option)
        {
            _lastPlus = 1;
            _curPoints += _lastPlus;
        }
        else  if (good2 == option)
        {
            _lastPlus = 2;
            _curPoints += _lastPlus;
            var tmp = good1;
            good1 = good2;
            good2 = tmp;
        }
        else
        {
            _lastPlus = 0;
        }
    }



    private MessageDialogData FinalDialog()
    {
        if (Complete)
        {
            var mianAnswers = new List<AnswerDialogData>();
            int improved = 0;
            foreach (var shipPilotData in MainController.Instance.MainPlayer.Army)
            {
                if (shipPilotData.Ship.ShipType != ShipType.Base)
                {
                    improved++;
                    shipPilotData.Pilot.UpgradeRandomLevel(false,true);
                }
            }
            mianAnswers.Add(new AnswerDialogData($"Yes.", null, null));
            var mesData = new MessageDialogData($"You successfully deactivate anomaly. {improved} of your ships improved.", mianAnswers);
            return mesData;
        }
        else
        {

            var mianAnswers = new List<AnswerDialogData>();
            mianAnswers.Add(new AnswerDialogData($"Not my day.", null, null));
            var mesData = new MessageDialogData("Anomaly was destroyed.", mianAnswers);
            return mesData;
        }
    }

    private MessageDialogData freezing()
    {
        TestOption(AnomalyTypeUnlock.freezing);
        if (ShallShowFinalDialog())
        {
            return FinalDialog();
        }
        else
        {
            return UnlockDialog();
        }
    }
    private MessageDialogData explosive()
    {
        TestOption(AnomalyTypeUnlock.explosive);
        if (ShallShowFinalDialog())
        {
            return FinalDialog();
        }
        else
        {
            return UnlockDialog();
        }
    }

    private MessageDialogData impulse()
    {
        TestOption(AnomalyTypeUnlock.impulse);
        if (ShallShowFinalDialog())
        {
            return FinalDialog();
        }
        else
        {
            return UnlockDialog();
        }
    }

    private MessageDialogData fire()
    {
        TestOption(AnomalyTypeUnlock.fire);
        if (ShallShowFinalDialog())
        {
            return FinalDialog();
        }
        else
        {
            return UnlockDialog();
        }
    }
}

