using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmConfig0 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    private ShipConfig _config;
    public QuestStageCmConfig0(ShipConfig config)    
        :base( QuestsLib.CM_START_QUEST + config.ToString())
    {
        _config = config;
    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == _config);

        cell1 = FindAndMarkCell(sectorId, GetDialog, player.MapData.CurrentCell) as FreeActionGlobalMapCell;
        if (cell1 == null)
        {
            return false;
        }
        return true;

    }

    private MessageDialogData GetDialog()
    {
        switch (_config)
        {
            case ShipConfig.raiders:
                return DialogsLibrary.GetPairDialogByTag(GetDialogsTagRdr(), DialogEnds);
            case ShipConfig.federation:
                break;
            case ShipConfig.mercenary:
                return DialogsLibrary.GetPairDialogByTag(GetDialogsTagMerc(), DialogEnds);
            case ShipConfig.ocrons:
                return DialogsLibrary.GetPairDialogByTag(GetDialogsTagOcr(), DialogEnds);
            case ShipConfig.krios:
                break;
        }

        return null;
    }

    private List<string> GetDialogsTagOcr()
    {
        var list = new List<string>();
        list.Add("cmOcr1_1_dialog_0_M1");
        list.Add("cmOcr1_1_dialog_0_A1");
        list.Add("cmOcr1_1_dialog_0_M2");
        list.Add("cmOcr1_1_dialog_0_A2");
        list.Add("cmOcr1_1_dialog_0_M3");
        list.Add("cmOcr1_1_dialog_0_A3");
        list.Add("cmOcr1_1_dialog_0_M4");
        list.Add("cmOcr1_1_dialog_0_A4");
        list.Add("cmOcr1_1_dialog_0_M5");
        list.Add("cmOcr1_1_dialog_0_A5");
        list.Add("cmOcr1_1_dialog_0_M6");
        list.Add("cmOcr1_1_dialog_0_A6");
        list.Add("cmOcr1_1_dialog_0_M7");
        list.Add("cmOcr1_1_dialog_0_A7");
        list.Add("cmOcr1_1_dialog_0_M8");
        list.Add("cmOcr1_1_dialog_0_A8");
        list.Add("cmOcr1_1_dialog_0_M9");
        list.Add("cmOcr1_1_dialog_0_A9");
        list.Add("cmOcr1_1_dialog_0_M10");
        list.Add("cmOcr1_1_dialog_0_A10");
        list.Add("cmOcr1_1_dialog_0_M11");
        list.Add("cmOcr1_1_dialog_0_A11");
        list.Add("cmOcr1_1_dialog_0_M12");
        list.Add("cmOcr1_1_dialog_0_A12");
        return list;
    }

    private void DialogEnds()
    {
        _playerQuest.QuestIdComplete(QuestsLib.CM_START_QUEST + _config.ToString());
        cell1.SetQuestData(null);
    }

    private List<string> GetDialogsTagMerc()
    {
        var list = new List<string>();
        list.Add("cmStartQuestMerc0EndM1");
        list.Add("cmStartQuestMerc0EndA1");   
        list.Add("cmStartQuestMerc0EndM2");
        list.Add("DC"); 
        list.Add("cmStartQuestMerc0EndM3");
        list.Add("cmStartQuestMerc0EndA3"); 
        list.Add("cmStartQuestMerc0EndM4");
        list.Add("DC"); 
        list.Add("cmStartQuestMerc0EndM5");
        list.Add("DC"); 
        list.Add("cmStartQuestMerc0EndM6");
        list.Add("DC"); 
        list.Add("cmStartQuestMerc0EndM7");
        list.Add("cmStartQuestMerc0EndA7"); 
        list.Add("cmStartQuestMerc0EndM8");
        list.Add("DC"); 
        list.Add("cmStartQuestMerc0EndM9");
        list.Add("cmStartQuestMerc0EndA9"); 
        list.Add("cmStartQuestMerc0EndM10");
        list.Add("DC"); 
        list.Add("cmStartQuestMerc0EndM11");
        list.Add("cmStartQuestMerc0EndA11"); 
        list.Add("cmStartQuestMerc0EndM12");
        list.Add("cmStartQuestMerc0EndA12");
        return list;
    }

    private List<string> GetDialogsTagRdr()
    {
        var list = new List<string>();
        list.Add("cmStartQuestRdr0EndM1");
        list.Add("cmStartQuestRdr0EndA1");  
        list.Add("cmStartQuestRdr0EndM2");
        list.Add("cmStartQuestRdr0EndA2");  
        list.Add("cmStartQuestRdr0EndM3");
        list.Add("cmStartQuestRdr0EndA3");  
        list.Add("cmStartQuestRdr0EndM4");
        list.Add("DC");  
        list.Add("cmStartQuestRdr0EndM5");
        list.Add("cmStartQuestRdr0EndA5");  
        list.Add("cmStartQuestRdr0EndM6");
        list.Add("cmStartQuestRdr0EndA6");  
        list.Add("cmStartQuestRdr0EndM7");
        list.Add("cmStartQuestRdr0EndA7");  
        list.Add("cmStartQuestRdr0EndM8");
        list.Add("cmStartQuestRdr0EndA8");  
        list.Add("cmStartQuestRdr0EndM9");
        list.Add("cmStartQuestRdr0EndA9");  
        list.Add("cmStartQuestRdr0EndM10");
        list.Add("cmStartQuestRdr0EndA10");  
        list.Add("cmStartQuestRdr0EndM11");
        list.Add("cmStartQuestRdr0EndA11");  
        list.Add("cmStartQuestRdr0EndM12");
        list.Add("cmStartQuestRdr0EndA12");  
        list.Add("cmStartQuestRdr0EndM13");
        list.Add("cmStartQuestRdr0EndA13");   
        return list;
    }

    private GlobalMapCell FindAndMarkCell(SectorData posibleSector,Func<MessageDialogData> dialogFunc,GlobalMapCell playerCell)
    {
        var cells = posibleSector.ListCells.Where(x => x.Data  != null && x.Data is FreeActionGlobalMapCell && !(x.Data as FreeActionGlobalMapCell).HaveQuest).ToList();
        if (cells.Count == 0)
        {
            return null;
        }

        SectorCellContainer container = cells.RandomElement();
        var cell = container.Data as FreeActionGlobalMapCell;

        cell.SetQuestData(dialogFunc);
        return cell;
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
        switch (_config)
        {
            case ShipConfig.raiders:
                break;
            case ShipConfig.federation:
                break;
            case ShipConfig.mercenary:
                return $"{Namings.Tag("cmStartQuestMerc0Desc")}";
            case ShipConfig.ocrons:
                return $"{Namings.Tag("cmStartQuestOcr0Desc")}";
            case ShipConfig.krios:
                break;
            case ShipConfig.droid:
                break;
        }

        return "Error";

    }
}
