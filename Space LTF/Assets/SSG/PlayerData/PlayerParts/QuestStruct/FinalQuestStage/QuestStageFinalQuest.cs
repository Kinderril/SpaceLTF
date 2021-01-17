using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageFinalQuest : QuestStage
{

    private GlobalMapCell cell1 = null;


    public QuestStageFinalQuest()    
        :base(QuestsLib.QUEST_FINALQUEST)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var cellFinal = player.MapData.GalaxyData.GetAllContainersNotNull().FirstOrDefault(x => x.Data is EndGlobalCell || x.Data is EndTutorGlobalCell);
        player.QuestData.LastBattleData.SetReady();
        cell1 = cellFinal.Data;
        return true;

    }


    protected override void StageDispose()
    {

    }

    public override bool CloseWindowOnClick => true;
    public override void OnClick()
    {
        var cell = GetCurCellTarget();
        if (cell != null)
            TryNavigateToCell(cell);
    }

    public GlobalMapCell GetCurCellTarget()
    {
        return cell1;

    }

    public override string GetDesc()
    {

        return $"{Namings.Tag("questFinalStageName")}";
    }
}
