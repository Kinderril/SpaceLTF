using System;
using System.Collections.Generic;

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
        return Namings.Tag("Anomaly");
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

        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("anomalySendScouts"), null, scouts));
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("anomalyBeliveMysielf"), null, comeClose));
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("anomalyGoAway"), null));

        var mesData = new MessageDialogData(Namings.DialogTag("anomalyStart"), mianAnswers);
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
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("anomalyFreeze"), null, freezing));
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("anomalyExplosive"), null, explosive));
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("anomalyImpulse"), null, impulse));
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("anomalyFire"), null, fire));
        string move = "";
        if (_withInfo)
        {
            switch (_lastPlus)
            {
                case 2:
                    move = Namings.DialogTag("anomalyChange");
                    break;
                case 1:
                    move = Namings.DialogTag("anomalyRight");
                    break;
                case 0:
                    move = Namings.DialogTag("anomalyWrong");
                    break;
            }
        }
        string ifno = _withInfo ? Namings.Format(Namings.DialogTag("anomalyNoInfo"),
                remainTryies)
            : Namings.Format(Namings.DialogTag("anomalyInfo"), move, _curPoints,
                COUNT_TO_UNLOCK, remainTryies);
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
        else if (good2 == option)
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
            foreach (var shipPilotData in MainController.Instance.MainPlayer.Army.Army)
            {
                if (shipPilotData.Ship.ShipType != ShipType.Base)
                {
                    improved++;
                    shipPilotData.Pilot.UpgradeRandomLevel(false, true);
                }
            }
            mianAnswers.Add(new AnswerDialogData(Namings.Tag("Yes"), null, null));
            var mesData = new MessageDialogData(Namings.Format(Namings.DialogTag("anomalyDeactivated"), improved), mianAnswers);
            return mesData;
        }
        else
        {

            var mianAnswers = new List<AnswerDialogData>();
            mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("anomalyNotDay"), null, null));
            var mesData = new MessageDialogData(Namings.DialogTag("anomalyDestroyed"), mianAnswers);
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

    public AnomalyMapEvent(ShipConfig config)
        : base(config)
    {
    }
}

