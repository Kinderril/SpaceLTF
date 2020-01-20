


using System;
using System.Collections.Generic;
using System.Linq;

public enum TeachType
{
    mainShip,
    pilots,
}

[System.Serializable]
public class TeacherMapEvent : BaseGlobalMapEvent
{
    private TeachType _teachType;
    private int _cost;

    public override string Desc()
    {
        return "Science ship";
    }

    public override MessageDialogData GetDialog()
    {
        _teachType = MyExtensions.IsTrue01(.7f) ? TeachType.pilots : TeachType.mainShip;
        var mianAnswers = new List<AnswerDialogData>();
        MessageDialogData mesData;
        switch (_teachType)
        {
            case TeachType.mainShip:
                //                PlayerParameterType tt = PlayerParameterType.diplomaty;
                List<PlayerParameterType> posibleUpg = new List<PlayerParameterType>();
                var player = MainController.Instance.MainPlayer.Parameters;
                PlayerParameter param = null;
                foreach (PlayerParameterType pp in Enum.GetValues(typeof(PlayerParameterType)))
                {
                    switch (pp)
                    {
                        case PlayerParameterType.scout:
                            param = player.Scouts;
                            break;
                        case PlayerParameterType.repair:
                            param = player.Repair;
                            break;
                        case PlayerParameterType.chargesCount:
                            param = player.ChargesCount;
                            break;
                        case PlayerParameterType.chargesSpeed:
                            param = player.ChargesSpeed;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (param.CanUpgrade())
                    {
                        posibleUpg.Add(pp);
                    }
                }

                var paramToUpgrade = posibleUpg.RandomElement();
                if (param != null)
                {
                    _cost = (int)(param.UpgradeCost() * MyExtensions.Random(0.5f, 0.75f));
                    if (MainController.Instance.MainPlayer.MoneyData.HaveMoney(_cost))
                    {
                        mianAnswers.Add(new AnswerDialogData($"Ok.", () => DoUpgradeMain(paramToUpgrade), null));
                    }
                }
                mianAnswers.Add(new AnswerDialogData("No, thanks.", null));
                mesData = new MessageDialogData($"This is science ship. They can improve your fleet [credits:{_cost}]. Do you want to upgrade {Namings.ParameterName(paramToUpgrade)}?", mianAnswers);
                return mesData;
            case TeachType.pilots:
                var army = MainController.Instance.MainPlayer.Army;
                var midLvl = (army.Army.Sum(x => x.Pilot.CurLevel) / army.Count);
                var costMidLvl = Library.PilotLvlUpCost(midLvl);
                _cost = (int)(costMidLvl * MyExtensions.Random(0.5f, 0.75f));
                if (MainController.Instance.MainPlayer.MoneyData.HaveMoney(_cost))
                {
                    mianAnswers.Add(new AnswerDialogData($"Ok.", () => DoPilotTeach(), null));
                }
                mianAnswers.Add(new AnswerDialogData("No, thanks.", null));
                mesData = new MessageDialogData($"This is science ship. They can teach some of your pilots [credits:{_cost}].", mianAnswers);
                return mesData;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }

    private MessageDialogData DoPilotTeach()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData($"Ok.", null, null));
        MainController.Instance.MainPlayer.MoneyData.RemoveMoney(_cost);
        var army = MainController.Instance.MainPlayer.Army.Army.Suffle();
        var points = 1000f;
        foreach (var pilotData in army)
        {
            if (pilotData.Ship.ShipType != ShipType.Base)
            {
                if (ArmyCreator.TryUpgradePilot(new ArmyRemainPoints(points), pilotData.Pilot, new ArmyCreatorLogs()))
                {
                    var mesData = new MessageDialogData(
                        $"Ship {pilotData.Ship.Name} Improved. Pilot level:{pilotData.Pilot.CurLevel}", mianAnswers);
                    return mesData;
                }
            }
        }
        return new MessageDialogData($"Fail.", mianAnswers); ;
    }

    private void DoUpgradeMain(PlayerParameterType paramToUpgrade)
    {
        var player = MainController.Instance.MainPlayer;
        switch (paramToUpgrade)
        {
            case PlayerParameterType.scout:
                player.Parameters.Scouts.TryUpgrade();
                break;
            case PlayerParameterType.repair:
                player.Parameters.Repair.TryUpgrade();
                break;
            case PlayerParameterType.chargesCount:
                player.Parameters.ChargesCount.TryUpgrade();
                break;
            case PlayerParameterType.chargesSpeed:
                player.Parameters.ChargesSpeed.TryUpgrade();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(paramToUpgrade), paramToUpgrade, null);
        }
    }

    public TeacherMapEvent(ShipConfig config) : base(config)
    {
    }
}

