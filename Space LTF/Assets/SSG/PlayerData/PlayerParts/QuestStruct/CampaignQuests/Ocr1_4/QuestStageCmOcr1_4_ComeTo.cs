using System.Collections.Generic;
using System.Linq;


[System.Serializable]
public class QuestStageCmOcr1_4_ComeTo : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;

    public QuestStageCmOcr1_4_ComeTo()    
        :base(QuestsLib.QuestStageCmOcr1_4_ComeTo)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.droid);
        cell1 = FindAndMarkCellRandom(sectorId, GetDialog, player.MapData.CurrentCell) as FreeActionGlobalMapCell;
        if (cell1 == null)
        {
            return false;
        }
        return true;

    }

    private MessageDialogData GetDialog()
    {
        return DialogsLibrary.GetPairDialogByTag(GetDialogsTag(), DialogEnds);
    }

    private void DialogEnds()
    {
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr1_4_ComeTo);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmOcr1_4_dialog_3_M1");
        list.Add("cmOcr1_4_dialog_3_A1");   
        list.Add("cmOcr1_4_dialog_3_M2");
        list.Add("cmOcr1_4_dialog_3_A2");   
        list.Add("cmOcr1_4_dialog_3_M3");
        list.Add("cmOcr1_4_dialog_3_A3");   
        list.Add("cmOcr1_4_dialog_3_M4");
        list.Add("cmOcr1_4_dialog_3_A4");   
        list.Add("cmOcr1_4_dialog_3_M5");
        list.Add("cmOcr1_4_dialog_3_A5");   
        list.Add("cmOcr1_4_dialog_3_M6");
        list.Add("cmOcr1_4_dialog_3_A6");   
        list.Add("cmOcr1_4_dialog_3_M7");
        list.Add("cmOcr1_4_dialog_3_A7");   
        list.Add("cmOcr1_4_dialog_3_M8");
        list.Add("cmOcr1_4_dialog_3_A8");   
        list.Add("cmOcr1_4_dialog_3_M9");
        list.Add("cmOcr1_4_dialog_3_A9");   
        list.Add("cmOcr1_4_dialog_3_M10");
        list.Add("cmOcr1_4_dialog_3_A10");   
        list.Add("cmOcr1_4_dialog_3_M11");
        list.Add("cmOcr1_4_dialog_3_A11");   
        list.Add("cmOcr1_4_dialog_3_M12");
        list.Add("cmOcr1_4_dialog_3_A12");  
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
        return $"{Namings.Tag("cmComeToPoint")}";
    }
}
