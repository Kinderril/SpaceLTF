using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

public static class QuestsLib 
{
    public const string QUEST_MERC_FIND_TARGET1 = "QUEST_MERC_FIND_TARGET1";
    public const string QUEST_MERC_FIND_TARGET2 = "QUEST_MERC_FIND_TARGET2";
    public const string QUEST_MERC_BATTLE_TARGET = "QUEST_MERC_BATTLE_TARGET";
    public const string QUEST_FED_CLEAR_SECTOR = "QUEST_FED_CLEAR_SECTOR";
    public const string QUEST_DELIVER_1 = "QUEST_DELIVER_1";
    public const string QUEST_DELIVER_2 = "QUEST_DELIVER_2";
    public const string QUEST_START_DELIVER1 = "QUEST_START_DELIVER1";
    public const string QUEST_START_PROTECT_FORTRESS1 = "QUEST_START_PROTECT_FORTRESS1";
    public const string QUEST_START_PROTECT_FORTRESS2 = "QUEST_START_PROTECT_FORTRESS2";
    public const string QUEST_KIDNAPPING1 = "QUEST_KIDNAPPING1";
    public const string QUEST_KIDNAPPING2 = "QUEST_KIDNAPPING2";
    public const string QUEST_FINALQUEST = "QUEST_FINALQUEST";

    public static QuestContainer GetDeliverQuest(PlayerQuestData data,Player player)
    {
        var stage = new QuestStageStartDeliver();
        var types = new List<ItemType>(){ ItemType.cocpit,ItemType.wings,ItemType.engine};
        var rnd1 = types.RandomElement();
        types.Remove(rnd1);
        int stage1TagetCount = 5;
        int stage2TagetCount = 3;
#if UNITY_EDITOR
        stage1TagetCount = 1;
        stage2TagetCount = 1;
#endif

        var stage2 = new QuestStageDeliver1(stage1TagetCount, QUEST_DELIVER_1,EParameterItemRarity.normal, rnd1);
        var stage3 = new QuestStageDeliver1(stage2TagetCount, QUEST_DELIVER_2,EParameterItemRarity.improved, types.RandomElement());
        var stages = new QuestStage[]
        {
            stage,
            stage2,
            stage3
        };
        MessageDialogData GetDialog()
        {
            List<AnswerDialogData> ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData(Namings.Tag("Ok"), null,
                null, true, false));
            string str = Namings.Tag("questDeliverFinal");

            var dataDialog = new MessageDialogData(str, ans);
            return dataDialog;
        }
        QuestContainer qust1 = new QuestContainer(data, stages, player,Namings.Tag("deliverQuestName"), GetDialog);
        return qust1;
    }

    public static QuestContainer GetKidnappingQuest(PlayerQuestData data,Player player)
    {
        var stage = new QuestStageKidnapping();
        var stages = new QuestStage[]
        {
            stage,
        };
        MessageDialogData GetDialog()
        {
            List<AnswerDialogData> ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData(Namings.Tag("Ok"), null,
                null, true, false));
            string str = Namings.Tag("questKidnappingFinaldialog");

            var dataDialog = new MessageDialogData(str, ans);
            return dataDialog;
        }
        QuestContainer qust1 = new QuestContainer(data, stages, player,Namings.Tag("questKidnappingFinalName"), GetDialog);
        return qust1;
    }

    public static QuestContainer GetFinalQuest(PlayerQuestData data,Player player)
    {
        var stage = new QuestStageFinalQuest();
        var stages = new QuestStage[]
        {
            stage,
        };
        MessageDialogData GetDialog()
        {
            List<AnswerDialogData> ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData(Namings.Tag("Ok"), null,
                null, true, false));
            string str = Namings.Tag("finalEnd"); 

            var dataDialog = new MessageDialogData(str, ans);
            return dataDialog;
        }
        QuestContainer qust1 = new QuestContainer(data, stages, player,Namings.Tag("questFinalStageName"), GetDialog);
        return qust1;
    }   

    public static QuestContainer GetProtectQuest(PlayerQuestData data,Player player)
    {
        var stage = new QuestStageProtectFortress1();
        var stage2 = new QuestStageProtectFortress2();
        var stages = new QuestStage[]
        {
            stage,
            stage2
        };
        MessageDialogData GetDialog()
        {
            List<AnswerDialogData> ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData(Namings.Tag("Ok"), null,
                null, true, false));
            string str = Namings.Tag("questProtectFinalText");

            var dataDialog = new MessageDialogData(str, ans);
            return dataDialog;
        }
        QuestContainer qust1 = new QuestContainer(data, stages, player,Namings.Tag("questProtectNameBig"), GetDialog);
        return qust1;
    }  
    public static QuestContainer GetMercQuest(PlayerQuestData data,Player player)
    {
        var stage = new QuestStageSearchDialogsMerc1();
        var stage2 = new QuestStartKillMerc1();
        var stages = new QuestStage[]
        {
            stage,
            stage2
        };
        MessageDialogData GetDialog()
        {
            List<AnswerDialogData> ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData(Namings.Tag("Ok"), null,
                null, true, false));
            string str = Namings.Tag("questSearachFleetMercFinal");

            var dataDialog = new MessageDialogData(str, ans);
            return dataDialog;
        }
        QuestContainer qust1 = new QuestContainer(data, stages, player,Namings.Tag("mercQuest1Name"), GetDialog);
        return qust1;
    }

    public static QuestContainer GetFedQuest(PlayerQuestData data,Player player)
    {
        var stage = new QuestStartClearSectorFed1();
        var stages = new QuestStage[]
        {
            stage,
        };
        MessageDialogData GetDialog()
        {
            List<AnswerDialogData> ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData(Namings.Tag("Ok"), null,
                null, true, false));
            string str = Namings.Tag("questFedClearSectorFinal");

            var dataDialog = new MessageDialogData(str, ans);
            return dataDialog;
        }
        QuestContainer qust1 = new QuestContainer(data, stages, player,Namings.Tag("fedQuest1Name"), GetDialog);
        return qust1;
    }

    public static QuestContainer GetCalcQuest(PlayerQuestData data, Player player,EQuestOnStart eQuestOnStart, float coef)
    {

        var stage = BaseQuestOnStart.Create(eQuestOnStart, coef);

        var stages = new QuestStage[]
        {
            stage,
        };

        MessageDialogData GetDialog()
        {
            List<AnswerDialogData> ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData(Namings.Tag("Ok"), null,
                null, true, false));
            string str = Namings.Tag("standartStatQuestComplete");

            var dataDialog = new MessageDialogData(str, ans);
            return dataDialog;
        }
        QuestContainer qust1 = new QuestContainer(data, stages, player, stage.Name, GetDialog);
        return qust1;
    }

}
