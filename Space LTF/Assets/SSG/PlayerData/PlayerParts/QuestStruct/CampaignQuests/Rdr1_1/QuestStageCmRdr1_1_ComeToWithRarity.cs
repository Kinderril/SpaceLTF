using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageCmRdr1_1_ComeToWithRarity : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private Player _player;
    private EParameterItemRarity _rarLvl;
    public QuestStageCmRdr1_1_ComeToWithRarity(EParameterItemRarity rarity, FreeActionGlobalMapCell cellToUse)    
        :base(QuestsLib.QuestStageCmRdr1_1_ComeToWithRarity)
    {
        cell1 = cellToUse;
        _rarLvl = rarity;
    }

    protected override bool StageActivate(Player player)
    {
        _player = player;
        return true;

    }

    private MessageDialogData GetDialog()
    {
        if (_player == null)
        {
            return null;
        }

        foreach (var item in _player.Inventory.ParamItems)
        {
            if (item.Rarity == _rarLvl)
            {
                return DialogsLibrary.GetPairDialogByTag(GetDialogsTag(), DialogEnds);
            }
        }

        return DialogsLibrary.GetPairDialogByTag(GetDialogsTagNull(), null); ;

    }

    private List<string> GetDialogsTagNull()
    {

        var list = new List<string>();
        list.Add("cmOcr2_2_dialog_null_M1");
        list.Add("cmOcr2_2_dialog_null_A1");
        return list;

    }

    private void DialogEnds()
    {
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr1_1_ComeToWithRarity);
        cell1.SetQuestData(null);
    }


    private List<string> GetDialogsTag()
    {
        var list = new List<string>();
        list.Add("cmRdr1_1_dialog_4_M1");
        list.Add("cmRdr1_1_dialog_4_A1");  
        list.Add("cmRdr1_1_dialog_4_M2");
        list.Add("cmRdr1_1_dialog_4_A2"); 
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
