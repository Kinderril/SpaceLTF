using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

public static class QuestsLib 
{
    public static string QuestStageCmRdr1_3_Fight2 = "QuestStageCmRdr1_3_Fight2";
    public static string QuestStageCmRdr1_3_Fight = "QuestStageCmRdr1_3_Fight";
    public static string QuestStageCmRdr1_3_Start = "QuestStageCmRdr1_3_Start";
    public static string QuestStageCmRdr1_2_Start = "QuestStageCmRdr1_2_Start";
    public static string QuestStageCmRdr1_1_ComeToWithRarity = "QuestStageCmRdr1_1_ComeToWithRarity";
    public static string QuestStageCmRdr1_1_Start = "QuestStageCmRdr1_1_Start";
    public static string QuestStageCmRdr1_1_Fight = "QuestStageCmRdr1_1_Fight";
    public static string QuestStageCmRdr1_1_ComeToExit2 = "QuestStageCmRdr1_1_ComeToExit2";
    public static string QuestStageCmRdr1_1_ComeToExit1 = "QuestStageCmRdr1_1_ComeToExit1";
    public static string QuestStageCmRdr1_1_ComeTo3 = "QuestStageCmRdr1_1_ComeTo3";
    public static string QuestStageCmRdr1_1_ComeTo = "QuestStageCmRdr1_1_ComeTo";
    public static string QuestStageCmRdr1_1_ComeTo2 = "QuestStageCmRdr1_1_ComeTo2";
    public static string QuestStageCmRdr1_2_Fight = "QuestStageCmRdr1_2_Fight";
    public static string QuestStageCmOcr2_1_FightDouble2 = "QuestStageCmOcr2_1_FightDouble2";
    public static string QuestStageCmOcr3_3_ComeToFinal = "QuestStageCmOcr3_3_ComeToFinal";
    public static string QuestStageCmOcr3_3_FightFinal = "QuestStageCmOcr3_3_FightFinal";
    public static string QuestStageCmOcr3_1_Start = "QuestStageCmOcr3_1_Start";
    public static string QuestStageCmOcr2_3_Start = "QuestStageCmOcr2_3_Start";
    public static string QuestStageCmOcr3_2_ComeTo = "QuestStageCmOcr3_2_ComeTo";
    public static string QuestStageCmOcr3_2_Start = "QuestStageCmOcr3_2_Start";
    public static string QuestStageCmOcr3_3_Start = "QuestStageCmOcr3_3_Start";
    public static string QuestStageCmOcr3_3_Fight2 = "QuestStageCmOcr3_3_Fight2";
    public static string QuestStageCmOcr3_2_Fight = "QuestStageCmOcr3_2_Fight";
    public static string QuestStageCmOcr3_3_ComeTo1 = "QuestStageCmOcr3_3_ComeTo1";
    public static string QuestStageCmOcr3_3_Fight3 = "QuestStageCmOcr3_3_Fight3";
    public static string QuestStageCmOcr3_1_Fight = "QuestStageCmOcr3_1_Fight";
    public static string QuestStageCmOcr2_3_ComeTo1 = "QuestStageCmOcr2_3_ComeTo1";
    public static string QuestStageCmOcr2_3_ComeToWithMoney = "QuestStageCmOcr2_3_ComeToWithMoney";
    public static string QuestStageCmOcr2_3_Fight = "QuestStageCmOcr2_3_Fight";
    public static string QuestStageCmOcr2_4_ComeTo = "QuestStageCmOcr2_4_ComeTo";
    public static string QuestStageCmOcr2_1_SpecForce = "QuestStageCmOcr2_1_SpecForce";
    public static string QuestStageCmOcr2_1_Fight = "QuestStageCmOcr2_1_FightDouble";
    public static string QuestStageCmOcr2_1_ComeTo4 = "QuestStageCmOcr2_1_ComeTo4";
    public static string QuestStageCmOcr2_1_ComeTo3 = "QuestStageCmOcr2_1_ComeTo3";
    public static string QuestStageCmOcr2_1_ComeTo2 = "QuestStageCmOcr2_1_ComeTo2";
    public static string QuestStageCmOcr_GetMoney = "QuestStageCmOcr_GetMoney";
    public static string QuestStageCmOcr2_2_Start2 = "QuestStageCmOcr2_2_Start2";
    public static string QuestStageCmOcr2_2_Start3 = "QuestStageCmOcr2_2_Start3";
    public static string QuestStageCmOcr2_2_Start = "QuestStageCmOcr2_2_Start";
    public static string QuestStageCmOcr2_2_SpecForce = "QuestStageCmOcr2_2_SpecForce";
    public static string QuestStageCmOcr2_2_ComeTo3 = "QuestStageCmOcr2_2_ComeTo3";
    public static string QuestStageCmOcr2_2_ComeTo1 = "QuestStageCmOcr2_2_ComeToWithRarity";
    public static string QuestStageCmOcr_GetParameterItem = "QuestStageCmOcr_GetParameterItem";
    public static string QuestStageCmOcr2_1_ComeTo1 = "QuestStageCmOcr2_1_ComeTo1";
    public static string QuestStageCmOcr1_4_Fight2 = "QuestStageCmOcr1_4_Fight2";
    public static string QuestStageCmOcr1_4_Fight = "QuestStageCmOcr1_4_Fight";
    public static string QuestStageCmOcr1_4_ComeTo = "QuestStageCmOcr1_4_ComeTo";
    public static string QuestStageCmOcr1_3_Start = "QuestStageCmOcr1_3_Start";
    public static string QuestStageCmOcr1_3_Fight = "QuestStageCmOcr1_3_Fight";
    public static string QuestStageCmOcr1_3_ComeTo = "QuestStageCmOcr1_3_ComeTo";
    public static string QuestStageCmOcr1_2_Start = "QuestStageCmOcr1_2_Start";
    public static string QuestStageCmOcr1_2_GetModul = "QuestStageCmOcr1_2_GetModul";
    public static string QuestStageCmOcr1_2_Fight = "QuestStageCmOcr1_2_Fight";
    public static string QuestStageCmOcr1_2_ComeTo = "QuestStageCmOcr1_2_ComeTo";
    public static string CM_OCR_1_1_START = "CM_OCR_1_1_START";
    public static string QuestStageCmOcr1_1_Fight = "QuestStageCmOcr1_1_Fight";
    public static string CM_OCR_1_1_COMETO = "CM_OCR_1_1_COMETO";
    public static string QuestStageCmOcr1_4_ComeTo2 = "QuestStageCmOcr1_4_ComeTo2";
    public static string CM_MERC_3_2_END = "CM_MERC_3_2_END";
    public static string CM_MERC_3_3_END = "CM_MERC_3_3_END";
    public static string CmMerc3_3_FinalFight = "CmMerc3_3_FinalFight";
    public static string CmMerc3_3_Fight = "CmMerc3_3_Fight";
    public static string CM_MERC_3_1_START = "CM_MERC_3_1_START";
    public static string CmMerc3_1_Fight = "CmMerc3_1_Fight";
    public static string CM_MERC_2_4_START = "CM_MERC_2_4_START";
    public static string CmMerc2_4_Fight = "CmMerc2_4_Fight";
    public static string CM_MERC_2_4_TALK2 = "CM_MERC_2_4_TALK2";
    public static string CM_MERC_2_4_END = "CM_MERC_2_4_END";
    public static string CM_MERC_2_4_TALK1 = "CM_MERC_2_4_TALK1";
    public static string CM_MERC_2_1_MID1 = "CM_MERC_2_1_MID1";
    public static string CmMerc1_2_Fight2 = "CmMerc1_2_Fight2";
    public static string CmMerc2_1_Fight1 = "CmMerc2_1_Fight1";
    public static string CM_MERC_2_2_END = "CM_MERC_2_2_END";
    public static string CM_MERC_2_2_START = "CM_MERC_2_2_START";
    public static string QuestStageCmMerc2_3_SpecForce = "QuestStageCmMerc2_3_SpecForce";
    public static string CM_MERC_2_1_START = "CM_MERC_2_1_START";
    public static string CM_MERC_2_3_M1 = "CM_MERC_2_3_M1";
    public static string CM_MERC_2_3_START = "CM_MERC_2_3_START";
    public static string CmMerc1_4_Fight = "CmMerc1_4_Fight";
    public static string CM_MERC_1_4_END = "CM_MERC_1_4_END";
    public static string CM_MERC_1_4_START = "CM_MERC_1_4_START";
    public static string CmMerc1_3_Fight2 = "CmMerc1_3_Fight2";
    public static string CM_MERC_1_3_START = "CM_MERC_1_3_START";
    public static string CmMerc1_3_Fight = "CmMerc1_3_Fight";
    public static string CmMerc1_2_Fight = "CmMerc1_2_Fight";
    public static string QuestStageCmMerc1_2_SpecForce = "QuestStageCmMerc1_2_SpecForce";
    public static string CM_MERC_1_2_START_ComeToOcr = "CM_MERC_1_2_START_ComeToOcr";
    public static string CM_MERC_1_2_START = "CM_MERC_1_2_START";
    public static string CM_MERC_1_1_START = "CM_MERC_1_1_START";
    public static string CM_MERC_1_1_END = "CM_MERC_1_1_END";
    public static string CM_MERC_1_2_END = "CM_MERC_1_2_END";
    public static string CM_MERC_1_1 = "CM_MERC_1_1";
    public static string CM_START_QUEST = "CM_START_QUEST";
    public static string CM_START_QUEST_ATTACK = "CM_START_QUEST_ATTACK";
    public static string QuestStageCmRdr1_4_ComeTo1 = "QuestStageCmRdr1_4_ComeTo1";
    public static string QuestStageCmRdr1_4_ComeTo2 = "QuestStageCmRdr1_4_ComeTo2";
    public static string QuestStageCmRdr1_4_ComeTo3 = "QuestStageCmRdr1_4_ComeTo3";
    public static string QuestStageCmRdr1_4_ComeTo4 = "QuestStageCmRdr1_4_ComeTo4";
    public static string QuestStageCmRdr2_1_Start = "QuestStageCmRdr2_1_Start";
    public static string QuestStageCmRdr1_4_Start = "QuestStageCmRdr1_4_Start";
    public static string QuestStageCmRdr2_3_SpecForce = "QuestStageCmRdr2_3_SpecForce";
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
    public static string QuestStageCmRdr2_2_ComeTo1 = "QuestStageCmRdr2_2_ComeTo1";
    public static string QuestStageCmRdr2_3_ComeTo1 = "QuestStageCmRdr2_3_ComeTo1";
    public static string QuestStageCmRdr2_4_Start = "QuestStageCmRdr2_4_Start";
    public static string QuestStageCmRdr2_3_Fight3 = "QuestStageCmRdr2_3_Fight3";
    public static string QuestStageCmRdr2_1_Fight = "QuestStageCmRdr2_1_Fight";
    public static string QuestStageCmRdr2_1_ComeTo1 = "QuestStageCmRdr2_1_ComeTo1";
    public static string QuestStageCmRdr2_2_Fight2= "QuestStageCmRdr2_2_Fight2";
    public static string QuestStageCmRdr2_4_Fight = "QuestStageCmRdr2_4_Fight";
    public static string QuestStageCmRdr2_4_End= "QuestStageCmRdr2_4_End";
    public static string QuestStageCmRdr2_4_ComeTo2 = "QuestStageCmRdr2_4_ComeTo2";


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
        QuestContainer qust1 = new QuestContainer(Library.Configs().RandomElement(),data, stages, player,Namings.Tag("deliverQuestName"), GetDialog);
        return qust1;
    }

    public static QuestContainer CmStartQuest(PlayerQuestData data, Player player,Action endCallback)
    {
        var stage = new QuestStageCampStartComeToPoint();
        var stage1 = new QuestStageCampStartAttackDroid();
        var stages = new QuestStage[]
        {
            stage1,
            stage,
        };
        QuestContainer qust1 = new QuestContainer(null,data, stages, player, Namings.Tag("cmComeToPoint"), null, endCallback);
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
        QuestContainer qust1 = new QuestContainer(Library.Configs().RandomElement(),data, stages, player,Namings.Tag("questKidnappingFinalName"), GetDialog);
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
        QuestContainer qust1 = new QuestContainer(Library.Configs().RandomElement(),data, stages, player,Namings.Tag("questFinalStageName"), GetDialog);
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
        QuestContainer qust1 = new QuestContainer(Library.Configs().RandomElement(),data, stages, player,Namings.Tag("questProtectNameBig"), GetDialog);
        return qust1;
    }

    private static SimpleModulType GetModulTypeForQuest(Player player,ShipConfig cfg)
    {
        var shops = player.MapData.GalaxyData.GetAllContainersNotNull().Where(x => x.Data is ShopGlobalMapCell && x.Data.ConfigOwner == cfg).ToList();
        if (shops.Count == 0)
        {
            return SimpleModulType.ShipTurnSpeed;
        }
        var rndShop = shops.RandomElement();
        var shop = rndShop.Data as ShopGlobalMapCell;

        if (shop.ShopInventory.Moduls.Count > 0)
        {
            var modul = shop.ShopInventory.Moduls.RandomElement();
            return modul.Type;
        }

        return SimpleModulType.ShipSpeed;
    }
    public static QuestContainer SearchAndKill(PlayerQuestData data,Player player)
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
        QuestContainer qust1 = new QuestContainer(Library.Configs().RandomElement(),data, stages, player,Namings.Tag("mercQuest1Name"), GetDialog);
        return qust1;
    }

    #region RAIDERS 1
    public static QuestContainer CmRdr1_1(PlayerQuestData data, Player player, Action endCallback)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.ocrons);  
        
        var cell1 = sectorId.ListCells.Where(x => x != null && x.Data != null && x.Data is FreeActionGlobalMapCell).ToList().RandomElement();
            
        var cellRnd= cell1.Data as FreeActionGlobalMapCell;
        var stages = new QuestStage[]
        {
            new QuestStageCmRdr1_1_Start(),
            new QuestStageCmRdr1_1_ComeTo1(), 
            new QuestStageCmRdr1_1_ComeToExit1(), 
            new QuestStageCmRdr1_1_ComeTo2(cellRnd),
            new QuestStageCmRdr1_1_ComeToWithRarity(EParameterItemRarity.improved,cellRnd),
            new QuestStageCmRdr1_1_Fight(1),
            new QuestStageCmRdr1_1_ComeToExit2(),
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.mercenary, data, stages, player, Namings.Tag("CmRdr1_1"), null, endCallback);
        return qust1;
    }       
    
    public static QuestContainer CmRdr1_2(PlayerQuestData data, Player player, Action endCallback)
    {
        var stages = new QuestStage[]
        {
            new QuestStageCmRdr1_2_Start(),
            new QuestStageCmRdr1_2_Fight(1), 
            new QuestStageCmRdr1_2_Fight(2), 
            new QuestStageCmRdr1_2_Fight(3), 
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.mercenary, data, stages, player, Namings.Tag("CmRdr1_2"), null, endCallback);
        return qust1;
    }    
    public static QuestContainer CmRdr1_3(PlayerQuestData data, Player player, Action endCallback)
    {
        var stages = new QuestStage[]
        {
            new QuestStageCmRdr1_3_Start(),
            new QuestStageCmRdr1_3_Fight(1), 
            new QuestStageCmRdr1_3_Fight2(1), 
            new QuestStageCmRdr1_3_Fight2(2), 
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.mercenary, data, stages, player, Namings.Tag("CmRdr1_3"), null, endCallback);
        return qust1;
    }    
    public static QuestContainer CmRdr1_4(PlayerQuestData data, Player player, Action endCallback)
    {
        var stages = new QuestStage[]
        {
            new QuestStageCmRdr1_4_Start(),
            new QuestStageCmRdr1_4_ComeTo1(), 
            new QuestStageCmRdr1_4_ComeTo2(), 
            new QuestStageCmRdr1_4_ComeTo3(), 
            new QuestStageCmRdr1_4_ComeTo4(), 
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.mercenary, data, stages, player, Namings.Tag("CmRdr1_4"), null, endCallback);
        return qust1;
    }

    #endregion

    #region RAIDERS 2
    public static QuestContainer CmRdr2_1(PlayerQuestData data, Player player, Action endCallback)
    {
        var stages = new QuestStage[]
        {
            new QuestStageCmRdr2_1_Start(), 
            new QuestStageCmRdr2_1_ComeTo1(), 
            new QuestStageCmRdr2_1_Fight(1), 
            new QuestStageCmRdr2_1_Fight(2), 
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.mercenary, data, stages, player, Namings.Tag("CmRdr2_1"), null, endCallback);
        return qust1;
    }    
    public static QuestContainer CmRdr2_2(PlayerQuestData data, Player player, Action endCallback)
    {
        var stages = new QuestStage[]
        {
            new QuestStageCmRdr2_2_ComeTo1(1), 
            new QuestStageCmRdr2_2_ComeTo1(2), 
            new QuestStageCmRdr2_2_ComeTo1(3), 
            new QuestStageCmRdr2_2_Fight2(1), 
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.mercenary, data, stages, player, Namings.Tag("CmRdr2_2"), null, endCallback);
        return qust1;
    }   
    public static QuestContainer CmRdr2_3(PlayerQuestData data, Player player, Action endCallback)
    {
        var stages = new QuestStage[]
        {
            new QuestStageCmRdr2_3_SpecForce(), 
            new QuestStageCmOcr_GetModul(SimpleModulType.armor,45981), 
            new QuestStageCmRdr2_3_ComeTo2(1), 
            new QuestStageCmRdr2_3_ComeTo2(2), 
            new QuestStageCmRdr2_3_Fight3(1),  
            new QuestStageCmRdr2_3_Fight3(2),  
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.mercenary, data, stages, player, Namings.Tag("CmRdr2_3"), null, endCallback);
        return qust1;
    }     
    public static QuestContainer CmRdr2_4(PlayerQuestData data, Player player, Action endCallback)
    {
        var stages = new QuestStage[]
        {
            new QuestStageCmRdr2_4_Start(), 
            new QuestStageCmRdr2_4_ComeTo2(1),  
            new QuestStageCmRdr2_4_ComeTo2(2),  
            new QuestStageCmRdr2_4_Fight(1),  
            new QuestStageCmRdr2_4_End(),  
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.mercenary, data, stages, player, Namings.Tag("CmRdr2_4"), null, endCallback);
        return qust1;
    }



    #endregion

    #region OCRONS 1

    public static QuestContainer CmOcr1_1(PlayerQuestData data, Player player, Action endCallback)
    {
//        var stage0 = new QuestStageCmOcr1_1_Start();
        var stage1 = new QuestStageCmOcr1_1_ComeTo();
        var stage2 = new QuestStageCmOcr1_1_Fight(1);
        var stage3 = new QuestStageCmOcr1_1_Fight(2);
        var stage4 = new QuestStageCmOcr1_1_Fight(3);
        var stages = new QuestStage[]
        {
//            stage0,
            stage1,
            stage2,
            stage3,
            stage4,
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.ocrons, data, stages, player, Namings.Tag("CmOcr1_1"), null, endCallback);
        return qust1;
    }     

    public static QuestContainer CmOcr1_2(PlayerQuestData data, Player player, Action endCallback)
    {
        var stage0 = new QuestStageCmOcr1_2_Start();
        var stage1 = new QuestStageCmOcr1_2_Fight();
        var stage2 = new QuestStageCmOcr_GetModul(GetModulTypeForQuest(player, ShipConfig.ocrons), 325744);
        var stage3 = new QuestStageCmOcr1_2_ComeTo();
        var stages = new QuestStage[]
        {
            stage0,
            stage1,
            stage2,
            stage3,
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.ocrons, data, stages, player, Namings.Tag("CmOcr1_2"), null, endCallback);
        return qust1;
    }       

    public static QuestContainer CmOcr1_3(PlayerQuestData data, Player player, Action endCallback)
    {
        var stage0 = new QuestStageCmOcr1_3_Start();
        var stage1 = new QuestStageCmOcr1_3_Fight(1);
        var stage2 = new QuestStageCmOcr1_3_Fight(2);
        var stage3 = new QuestStageCmOcr1_3_Fight(3);
        var stage4 = new QuestStageCmOcr1_3_ComeTo();
        var stages = new QuestStage[]
        {
            stage0,
            stage1,
            stage2,
            stage3,
            stage4,
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.ocrons, data, stages, player, Namings.Tag("CmOcr1_3"), null, endCallback);
        return qust1;
    }  
    public static QuestContainer CmOcr1_4(PlayerQuestData data, Player player, Action endCallback)
    {
        var stage1 = new QuestStageCmOcr1_4_Fight();
        var stage2 = new QuestStageCmOcr1_4_ComeTo();
        var stage3 = new QuestStageCmOcr1_4_Fight2();
        var stage4 = new QuestStageCmOcr1_4_ComeTo2();
        var stages = new QuestStage[]
        {
            stage1,
            stage2,
            stage3,
            stage4,
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.ocrons, data, stages, player, Namings.Tag("CmOcr1_4"), null, endCallback);
        return qust1;
    }

    #endregion

    #region OCRONS 2
    public static QuestContainer CmOcr2_1(PlayerQuestData data, Player player, Action endCallback)
    {
        var stage1 = new QuestStageCmOcr2_1_ComeTo1();
        var stage2 = new QuestStageCmOcr2_1_ComeTo2();
        var stage3 = new QuestStageCmOcr2_1_SpecForce();

        var stage4 = new QuestStageCmOcr2_1_ComeTo3();
        var stage5 = new QuestStageCmOcr2_1_ComeTo4();
        var stage6 = new QuestStageCmOcr_GetModul(GetModulTypeForQuest(player,ShipConfig.ocrons), 437865);

        var stage7 = new QuestStageCmOcr2_1_FightDouble();
        var stage8 = new QuestStageCmOcr2_1_FightDouble2();

        var stages = new QuestStage[]
        {
            stage1,
            stage2,
            stage3,
            stage4,     
            stage5,
            stage6,
            stage7,
            stage8,
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.ocrons, data, stages, player, Namings.Tag("CmOcr2_1"), null, endCallback);
        return qust1;
    }

    public static QuestContainer CmOcr2_2(PlayerQuestData data, Player player, Action endCallback)
    {
        var stage1 = new QuestStageCmOcr2_2_Start();
        var stage2 = new QuestStageCmOcr_GetParameterItem(EParameterItemRarity.perfect, 43915);
        var stage3 = new QuestStageCmOcr2_2_ComeToWithRarity(EParameterItemRarity.perfect);

        var stage4 = new QuestStageCmOcr2_2_Start2();
        var stage5 = new QuestStageCmOcr_GetModul(GetModulTypeForQuest(player, ShipConfig.ocrons), 7866);
        var stage6 = new QuestStageCmOcr2_2_SpecForce(SimpleModulType.shieldRegen);

        var stage7 = new QuestStageCmOcr2_2_Start3();
        var stage8 = new QuestStageCmOcr_GetMoney(123, 09743);
        var stage9 = new QuestStageCmOcr2_2_ComeTo3(123);

        var stages = new QuestStage[]
        {
            stage1,
            stage2,
            stage3,
            stage4,
            stage5,
            stage6,
            stage7,
            stage8,
            stage9,
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.ocrons, data, stages, player, Namings.Tag("CmOcr2_2"), null, endCallback);
        return qust1;
    }
    public static QuestContainer CmOcr2_3(PlayerQuestData data, Player player, Action endCallback)
    {
        var stages = new QuestStage[]
        {

        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.ocrons, data, stages, player, Namings.Tag("CmOcr2_2"), null, endCallback);
        return qust1;
    }

    public static QuestContainer CmOcr2_4(PlayerQuestData data, Player player, Action endCallback)
    {
        var stages = new QuestStage[]
        {

        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.ocrons, data, stages, player, Namings.Tag("CmOcr2_2"), null, endCallback);
        return qust1;
    }

    #endregion
    
    #region OCRONS 3
    public static QuestContainer CmOcr3_1(PlayerQuestData data, Player player, Action endCallback)
    {
        var stage1 = new QuestStageCmOcr3_1_Start();
        var stage2 = new QuestStageCmOcr3_1_Fight(1);
        var stage3 = new QuestStageCmOcr3_1_Fight(2);
        var stage4 = new QuestStageCmOcr3_1_Fight(3);

        var stages = new QuestStage[]
        {
            stage1,
            stage2,
            stage3,
            stage4,
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.ocrons, data, stages, player, Namings.Tag("CmOcr3_1"), null, endCallback);
        return qust1;
    }
    public static QuestContainer CmOcr3_2(PlayerQuestData data, Player player, Action endCallback)
    {
        var stage1 = new QuestStageCmOcr3_2_Start();
        var stage2 = new QuestStageCmOcr3_2_ComeTo(1);
        var stage3 = new QuestStageCmOcr3_2_ComeTo(2);
        var stage4 = new QuestStageCmOcr3_2_ComeTo(3);
        var stage6 = new QuestStageCmOcr3_2_Fight2();

        var stages = new QuestStage[]
        {
            stage1,
            stage2,
            stage3,
            stage4,
            stage6,
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.ocrons, data, stages, player, Namings.Tag("CmOcr3_2"), null, endCallback);
        return qust1;
    }
    public static QuestContainer CmOcr3_3(PlayerQuestData data, Player player, Action endCallback)
    {
        var stage1 = new QuestStageCmOcr3_3_Start();
        var stage2 = new QuestStageCmOcr3_3_ComeTo1();
        var stage3 = new QuestStageCmOcr3_3_Fight2(2);
        var stage4 = new QuestStageCmOcr3_3_Fight3(4);
        var stage5 = new QuestStageCmOcr3_3_FightFinal();
        var stage6 = new QuestStageCmOcr3_3_ComeToFinal();

        var stages = new QuestStage[]
        {
            stage1,
            stage2,
            stage3,
            stage4,
            stage5,
            stage6,
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.ocrons, data, stages, player, Namings.Tag("CmOcr3_3"), null, endCallback);
        return qust1;
    }
    #endregion

    public static QuestContainer CmMerc2_1(PlayerQuestData data, Player player, Action endCallback)
    {
        var stage0 = new QuestStageCmMerc2_1_Start();
        var stage1 = new QuestStageCmMerc2_1_Fight();
        var stage2 = new QuestStageCmMerc2_1_Mid();
        var stage3 = new QuestStageCmMerc2_1_Fight2();
        var stages = new QuestStage[]
        {
            stage0,
            stage1,
            stage2,
            stage3,
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.mercenary, data, stages, player, Namings.Tag("CmMerc2_1"), null, endCallback);
        return qust1;
    }  
    public static QuestContainer CmMerc2_4(PlayerQuestData data, Player player, Action endCallback)
    {
        var stage0 = new QuestStageCmMerc2_4_Start();
        var stage1 = new QuestStageCmMerc2_4_Talk();
        var stage2 = new QuestStageCmMerc2_4_Talk2();
        var stage3 = new QuestStageCmMerc2_4_Fight();
        var stage4 = new QuestStageCmMerc2_4_End();
        var stages = new QuestStage[]
        {
            stage0,
            stage1,
            stage2,
            stage3,
            stage4,
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.mercenary, data, stages, player, Namings.Tag("CmMerc2_4"), null, endCallback);
        return qust1;
    }   
    public static QuestContainer CmMerc3_1(PlayerQuestData data, Player player, Action endCallback)
    {
        var stage0 = new QuestStageCmMerc3_1_Start(1);
        var stage1 = new QuestStageCmMerc3_1_Start(2);
        var stage2 = new QuestStageCmMerc3_1_Fight();
        var stages = new QuestStage[]
        {
            stage0,
            stage1,
            stage2,
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.mercenary, data, stages, player, Namings.Tag("CmMerc3_1"), null, endCallback);
        return qust1;
    } 
    public static QuestContainer CmMerc2_3(PlayerQuestData data, Player player, Action endCallback)
    {
        var stage0 = new QuestStageCmMerc2_3_Start();
        var stage1 = new QuestStageCmMerc2_3_Ctp();
        var stage2 = new QuestStageCmMerc2_3_SpecForce(1);
        var stage3 = new QuestStageCmMerc2_3_SpecForce(2);
        var stage4 = new QuestStageCmMerc2_3_SpecForce(3);
        var stages = new QuestStage[]
        {
            stage0,
            stage1,
            stage2,
            stage3,
            stage4,
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.mercenary, data, stages, player, Namings.Tag("CmMerc2_3"), null, endCallback);
        return qust1;
    }    
    public static QuestContainer CmMerc3_2(PlayerQuestData data, Player player, Action endCallback)
    {
        var stage0 = new QuestStageCmMerc3_2_Find(1);
        var stage1 = new QuestStageCmMerc3_2_Find(2);
        var stage2 = new QuestStageCmMerc3_2_Find(3);
        var stage3 = new QuestStageCmMerc3_2_Find(4);
        var stage4 = new QuestStageCmMerc3_2_Find(5);
        var stage5 = new QuestStageCmMerc3_2_End();
        var stages = new QuestStage[]
        {
            stage0,
            stage1,
            stage2,
            stage3,
            stage4,
            stage5,
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.mercenary, data, stages, player, Namings.Tag("CmMerc3_2"), null, endCallback);
        return qust1;
    } 
    public static QuestContainer CmMerc3_3(PlayerQuestData data, Player player, Action endCallback)
    {
        var stage0 = new QuestStageCmMerc3_3_Fight(1);
        var stage0_1 = new QuestStageCmMerc3_3_Fight(2);   
        var stage0_2 = new QuestStageCmMerc3_3_Fight(3);
        var stage0_3 = new QuestStageCmMerc3_3_Fight(4);
        var stage1 = new QuestStageCmMerc3_3_FinalFight();
        var stage2 = new QuestStageCmMerc3_3_End();
        var stages = new QuestStage[]
        {
            stage0,
            stage0_1,
            stage0_2,
            stage0_3,
            stage1,
            stage2,
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.mercenary, data, stages, player, Namings.Tag("CmMerc3_3"), null, endCallback);
        return qust1;
    } 
    public static QuestContainer CmMerc2_2(PlayerQuestData data, Player player, Action endCallback)
    {
        var stage0 = new QuestStageCmMerc2_2_Start();
        var stage1_1 = new QuestStageCmMerc2_2_Search(ShipConfig.federation,0);
        var stage1_2 = new QuestStageCmMerc2_2_Search(ShipConfig.ocrons, 1);
        var stage1_3 = new QuestStageCmMerc2_2_Search(ShipConfig.krios,2);
        var stage1_4 = new QuestStageCmMerc2_2_Search(ShipConfig.raiders,3);
        var stage2 = new QuestStageCmMerc2_2_End();
        var stages = new QuestStage[]
        {
            stage0,
            stage1_1,
            stage1_2,
            stage1_3,
            stage1_4,
            stage2,
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.mercenary, data, stages, player, Namings.Tag("CmMerc2_2"), null, endCallback);
        return qust1;
    }
    public static QuestContainer CmMerc1_1(PlayerQuestData data, Player player, Action endCallback)
    {
        var stage0 = new QuestStageCmMerc1_1_Start();
        var stage1 = new QuestStageCmMerc1_1(1);
        var stage2 = new QuestStageCmMerc1_1(2);
        var stage3 = new QuestStageCmMerc1_1(3);
        var stage4 = new QuestStageCmMerc1_1_End();
        //        var stage2 = new QuestStartKillMerc1();
        var stages = new QuestStage[]
        {
            stage0,
            stage1,
            stage2,
            stage3,
            stage4,
        };

        QuestContainer qust1 = new QuestContainer(ShipConfig.mercenary, data, stages, player, Namings.Tag("CmMerc1_2"), null, endCallback);
        return qust1;
        
    }     
    public static QuestContainer CmMerc1_2(PlayerQuestData data, Player player, Action endCallback)
    {
        var stage0 = new QuestStageCmMerc1_2_Start();
        var stage1 = new QuestStageCmMerc1_2_ComeToOcr();
        var stage2 = new QuestStageCmMerc1_2_Fight();//Destroy fleet double
        var stage3 = new QuestStageCmMerc1_2_SpecForce();//Destroy fleet double
        var stage4 = new QuestStageCmMerc1_2_End();//Return to start
        var stages = new QuestStage[]
        {
            stage0,
            stage1,
            stage2,
            stage3,
            stage4,
//            stage4,
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.mercenary, data, stages, player, Namings.Tag("CmMerc1_1"), null, endCallback);
        return qust1;
    }
    public static QuestContainer CmMerc1_3(PlayerQuestData data, Player player, Action endCallback)
    {
        var stage0 = new QuestStageCmMerc1_3_Start();
        var stage1 = new QuestStageCmMerc1_3_Fight();
        var stage2 = new QuestStageCmMerc1_3_Fight2();
        var stages = new QuestStage[]
        {
            stage0,
            stage1,
            stage2,
//            stage4,
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.mercenary, data, stages, player, Namings.Tag("CmMerc1_3"), null, endCallback);
        return qust1;
    }
    public static QuestContainer CmMerc1_4(PlayerQuestData data, Player player, Action endCallback)
    {
        var stage0 = new QuestStageCmMerc1_4_Start();
        var stage1 = new QuestStageCmMerc1_4_Fight();
        var stage2 = new QuestStageCmMerc1_4_End();

        var stages = new QuestStage[]
        {
            stage0,
            stage1,
            stage2,
//            stage4,
        };
        QuestContainer qust1 = new QuestContainer(ShipConfig.mercenary, data, stages, player, Namings.Tag("CmMerc1_4"), null, endCallback);
        return qust1;
    }



    public static QuestContainer CmFraction0(ShipConfig config,PlayerQuestData data,Player player,Action endCallback)
    {
        var stage = new QuestStageCmConfig0(config);
//        var stage2 = new QuestStartKillMerc1();
        var stages = new QuestStage[]
        {
            stage,
        };
    
        QuestContainer qust1 = new QuestContainer(config, data, stages, player, stage.GetDesc(), null,endCallback);
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
        QuestContainer qust1 = new QuestContainer(ShipConfig.mercenary, data, stages, player,Namings.Tag("fedQuest1Name"), GetDialog);
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

        var cfg = Library.ConfigsNoDroid().RandomElement();
        QuestContainer qust1 = new QuestContainer(cfg, data, stages, player, stage.Name, GetDialog);
        return qust1;
    }

}
