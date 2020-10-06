using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmMerc2_2_Search : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private ShipConfig _config;
    private int SCOUTS_LVL = 3;
    private int _index;
    public QuestStageCmMerc2_2_Search(ShipConfig config,int index)    
        :base(QuestsLib.CM_MERC_2_2_START + config.ToString())
    {
        _config = config;
        _index = index;///777bb
    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x=>x.ShipConfig == _config);
        cell1 = FindAndMarkCellClosest(sectorId, GetDialog, player.MapData.CurrentCell) as FreeActionGlobalMapCell;
        if (cell1 == null)
        {
            return false;
        }
        return true;

    }

    protected override Func<MessageDialogData> AfterCompleteDialog()
    {
        var dialog = DialogsLibrary.GetPairDialogByTag(GetDialogsTagAfter(), null);
        return () => dialog;
    }

    private MessageDialogData GetDialog()
    {
        if (_index == 0)
        {

            var dialogFirst = DialogsLibrary.GetPairDialogByTag(GetDialogsTagFist(), null, StartAskDialog);
            return dialogFirst;
        }
        else
        {
            return StartAskDialog();
        }

    }

    private MessageDialogData StartAskDialog()
    {
        List<AnswerDialogData> ans = new List<AnswerDialogData>();
        var scouts = _player.Parameters.Scouts.Level;
        var chance = GetPercent(scouts, SCOUTS_LVL);
        ans.Add(new AnswerDialogData($"{Namings.Tag("cm_Merc_2_2_startTakeTryScouts")} {Namings.Tag("Chance")}: {chance.ToString("0")}", null, TryScouts));
        ans.Add(new AnswerDialogData(Namings.Tag("Attack"), Fight));
        string str = Namings.Tag("cm_Merc_2_2_startTakeItems");
        var msg = new MessageDialogData(str, ans, true);
        return msg;
    }
                                                                                                            
    private MessageDialogData TryScouts()
    {
        TextChangeEvent();
        string str;
        List<AnswerDialogData> ans = new List<AnswerDialogData>();
        var isWork = SkillWork(SCOUTS_LVL, _player.Parameters.Scouts.Level);
#if UNITY_EDITOR
//        isWork = false;
#endif
        if (isWork)
        {
            str = Namings.Format(Namings.Tag("cmMerc_2_2_StealFail"), null, null);
            ans.Add(new AnswerDialogData(Namings.Tag("Ok"), FightFail));
        }
        else
        {
            str = Namings.Format(Namings.Tag("cmMerc_2_2_StealFine"), null, null);
            ans.Add(new AnswerDialogData(Namings.Tag("Ok"), DialogEnds, AfterCompleteDialog()));
        }


        var msg = new MessageDialogData(str, ans, true);
        return msg;
    }

    private void FightFail()
    {
        DialogEnds();
        MainController.Instance.PreBattle(_player, PlayerToDefeat(1.2f), false, false);
    }

    private void Fight()
    {
        DialogEnds();
        MainController.Instance.PreBattle(_player, PlayerToDefeat(1f), false, false);
    }

    public Player PlayerToDefeat(float coef)
    {
        var power = cell1.Power * coef;
        var playerEnemy = new PlayerAIWithBattleEvent("Army", false);
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 2, 4,
            ArmyCreatorLibrary.GetArmy(_config), MyExtensions.IsTrueEqual(),
            playerEnemy);
        playerEnemy.Army.SetArmy(army);
        return playerEnemy;
    }


    private void DialogEnds()
    {
        _playerQuest.QuestIdComplete(QuestsLib.CM_MERC_2_2_START + _config.ToString());
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTagFist()
    {

        var list = new List<string>();
        list.Add("cmMerc2_2_dialog_first_M1");     
        list.Add("cmMerc2_2_dialog_first_A1");   
        list.Add("cmMerc2_2_dialog_first_M2");     
        list.Add("cmMerc2_2_dialog_first_A2");   
        list.Add("cmMerc2_2_dialog_first_M3");     
        list.Add("cmMerc2_2_dialog_first_A3");   
        list.Add("cmMerc2_2_dialog_first_M4");     
        list.Add("cmMerc2_2_dialog_first_A4");
        return list;
    }
    private List<string> GetDialogsTagAfter()
    {
        var list = new List<string>();
        switch (_config)
        {
            case ShipConfig.raiders:
                list.Add("cmMerc2_2_dialog_raiders_2_M1");     
                list.Add("cmMerc2_2_dialog_raiders_2_A1");
                break;
            case ShipConfig.federation:
                list.Add("cmMerc2_2_dialog_federation_2_M1");      
                list.Add("cmMerc2_2_dialog_federation_2_A1");
                break;
            case ShipConfig.ocrons:
                list.Add("cmMerc2_2_dialog_ocrons_2_M1");    
                list.Add("cmMerc2_2_dialog_ocrons_2_A1");
                break;            
            case ShipConfig.krios:
                list.Add("cmMerc2_2_dialog_krios_2_M1");    
                list.Add("cmMerc2_2_dialog_krios_2_A1");
                break;
        }

        return list;
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
        return $"{Namings.Tag("cmSearchPoint")}";
    }
}
