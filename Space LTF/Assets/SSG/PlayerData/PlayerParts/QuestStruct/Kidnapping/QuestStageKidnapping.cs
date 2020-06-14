using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageKidnapping : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private GalaxyEnemiesArmyController _enemiesController;
    private ShipConfig _configToKill = ShipConfig.droid;
    private bool _armyKilled;
    private bool _armyKilledInited;


    public QuestStageKidnapping()    
        :base(QuestsLib.QUEST_KIDNAPPING1, QuestsLib.QUEST_KIDNAPPING2)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var posibleSectors = GetSectors(player, 0, 4,1);
        if (posibleSectors.Count < 1)
        {
            return false;
        }


        cell1 = FindAndMarkCell(posibleSectors.RandomElement(), Dialog) as FreeActionGlobalMapCell;
        if (cell1 == null)
        {
            return false;
        }
        _enemiesController = player.MapData.GalaxyData.GalaxyEnemiesArmyController;
        return true;

    }

    private bool isInited = false;
    protected override void SubAfterLoad()
    {
        if (isInited)
        {
            return;
        }

        isInited = true;
        _enemiesController.OnAddMovingArmy += OnAddMovingArmy;
    }

    private void OnAddMovingArmy(MovingArmy arg1, bool arg2)
    {
        if (!_armyKilledInited)
        {
            return;
        }
        if (!arg2)
        {
            if (arg1.StartConfig == _configToKill)
            {
                _armyKilled = true;
            }
        }
    }

    private GlobalMapCell FindAndMarkCell(SectorData posibleSector,Func<MessageDialogData> dialogFunc)
    {
        var cells = posibleSector.ListCells.Where(x => x.Data  != null && x.Data is FreeActionGlobalMapCell && !(x.Data as FreeActionGlobalMapCell).HaveQuest).ToList();
        if (cells.Count == 0)
        {
            return null;
        }

        var cell = cells.RandomElement().Data as FreeActionGlobalMapCell;
        if (cell == null)
        {
            return null;
        }

        cell.SetQuestData(dialogFunc);
        return cell;

    }

    private float CHANCE_TO_FAIL_ATTACK = 50f;
    private int MONEY_TO_BUY = 120;
    private int SCOUTS_LVL = 2;


    private MessageDialogData Dialog()
    {
        _playerQuest.QuestIdComplete(QuestsLib.QUEST_KIDNAPPING1);
        _armyKilledInited = true;
        List<AnswerDialogData> ans = new List<AnswerDialogData>();
        //        var player = MainController.Instance.MainPlayer;

        var scouts = _player.Parameters.Scouts.Level;
        var chance = GetPercent(scouts, SCOUTS_LVL);
        _configToKill = _player.ReputationData.BestFaction();
        if (_armyKilled)
        {
            ans.Add(new AnswerDialogData(Namings.Tag("questKidnappingArmyKilled"), subQuestComplete, null));
        }
        ans.Add(new AnswerDialogData($"{Namings.Tag("questKidnappingScouts")} {Namings.Tag("Chance")}: {chance.ToString("0")}", null, TryScouts));
        if (_player.MoneyData.HaveMoney(MONEY_TO_BUY))
        {
            ans.Add(new AnswerDialogData(Namings.Tag("questKidnappingBuyout"), subQuestComplete, null));
        }

        ans.Add(new AnswerDialogData(Namings.Format(Namings.Tag("AttackWithFail"), CHANCE_TO_FAIL_ATTACK.ToString("0")),Fight));
        ans.Add(new AnswerDialogData(Namings.Tag("leave"),
            () =>
            {
//                cell1.SetQuestData(null);
//                TextChangeEvent();
            },
            null, true, false));
        string str = Namings.Format(Namings.Tag("questKidnappingDialogStart"),Namings.ShipConfig(_configToKill), MONEY_TO_BUY);
        var msg = new MessageDialogData(str,ans,true);
        return msg;
    }

    private MessageDialogData TryScouts()
    {
        cell1.SetQuestData(null);
        TextChangeEvent();
        string str;
        List<AnswerDialogData> ans = new List<AnswerDialogData>();
        var isWork = SkillWork(SCOUTS_LVL, _player.Parameters.Scouts.Level);
#if UNITY_EDITOR
        isWork = false;
#endif
        if (isWork)
        {
            str = Namings.Format(Namings.Tag("questKidnappingScoutsFine"), null, null);
            _playerQuest.QuestIdComplete(QuestsLib.QUEST_KIDNAPPING2);
        }
        else
        {
            str = Namings.Format(Namings.Tag("questKidnappingScoutsFail"), null, null);
            Fail();
        }

        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));

  var msg = new MessageDialogData(str, ans, true);
        return msg;
    }

    void subQuestComplete()
    {
        _playerQuest.QuestIdComplete(QuestsLib.QUEST_KIDNAPPING2);
        cell1.SetQuestData(null);
        TextChangeEvent();
    }

    private void Fight()
    {
        cell1.SetQuestData(null);
        TextChangeEvent();
        var isFailed = MyExtensions.IsTrue01(CHANCE_TO_FAIL_ATTACK / 100f);
        if (!isFailed)
        {
            Fail();
        }
        else
        {
            _playerQuest.QuestIdComplete(QuestsLib.QUEST_KIDNAPPING2);
        }
        MainController.Instance.PreBattle(_player, PlayerToDefeat(),false,false);

    }

    public Player PlayerToDefeat()
    {
        var player = MainController.Instance.MainPlayer;
        var power = player.Army.GetPower();
        var config = player.ReputationData.WorstFaction(ShipConfig.droid);
        var withBase = power > 25;
        var playerEnemy = new PlayerAIWithBattleEvent("Kidnapping", true, EBattleType.baseDefence);
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 2, 6, ArmyCreatorLibrary.GetArmy(config), withBase,
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return player;
    }

    protected override void StageDispose()
    {

    }

    public override bool CloseWindowOnClick => true;
    public override void OnClick()
    {
        TryNavigateToCell(GetCurCellTarget());
    }

    public GlobalMapCell GetCurCellTarget()
    {
        return cell1;

    }

    public override string GetDesc()
    {

        return $"{Namings.Tag("questKidnappingStageName")}";
    }
}
