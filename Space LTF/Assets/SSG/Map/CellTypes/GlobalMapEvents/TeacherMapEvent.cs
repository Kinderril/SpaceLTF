


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                foreach (PlayerParameterType pp in Enum.GetValues(typeof(PlayerParameterType)))
                {
                    PlayerParameter param;
                    switch (pp)
                    {
                        case PlayerParameterType.scout:
                            param = player.Scouts;
                            break;
                        case PlayerParameterType.diplomaty:
                            param = player.Diplomaty;
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

                mianAnswers.Add(new AnswerDialogData($"Ok.", ()=>DoUpgradeMain(paramToUpgrade), null));
                mianAnswers.Add(new AnswerDialogData("No, thanks.", null));
                mesData = new MessageDialogData($"This is science ship. They can improve your fleet. Do you want to upgrade {Namings.ParameterName(paramToUpgrade)}?", mianAnswers);
                return mesData;
            case TeachType.pilots:
                mianAnswers.Add(new AnswerDialogData($"Ok.", ()=>DoPilotTeach(), null));
                mianAnswers.Add(new AnswerDialogData("No, thanks.", null));
                mesData = new MessageDialogData($"This is science ship. They can teach some of your pilots.", mianAnswers);
                return mesData;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }

    private void DoPilotTeach()
    {
        var army = MainController.Instance.MainPlayer.Army.Suffle();
        var points = 1000f;
        foreach (var pilotData in army)
        {
            if (ArmyCreator.TryUpgradePilot(new ArmyRemainPoints(points), pilotData.Pilot, new ArmyCreatorLogs()))
            {
                return;
            }
        }
    }

    private void DoUpgradeMain(PlayerParameterType paramToUpgrade)
    {
        var player = MainController.Instance.MainPlayer;
        switch (paramToUpgrade)
        {
            case PlayerParameterType.scout:
                player.Parameters.Scouts.TryUpgrade();
                break;
            case PlayerParameterType.diplomaty:
                player.Parameters.Diplomaty.TryUpgrade();
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
}

