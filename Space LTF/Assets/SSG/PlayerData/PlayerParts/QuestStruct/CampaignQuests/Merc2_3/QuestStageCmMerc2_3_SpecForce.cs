using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class QuestStageCmMerc2_3_SpecForce : QuestStage
{
    private MovingArmy _army;
    private GalaxyEnemiesArmyController _enemiesController;

    private bool isInited = false;
    private int _index;
    public override bool CloseWindowOnClick => true;

    public QuestStageCmMerc2_3_SpecForce(int index)
        : base(QuestsLib.QuestStageCmMerc2_3_SpecForce + index)
    {
        _index = index;
    }


    protected Func<MessageDialogData> AfterCompleteDialog1()
    {
        var list = new List<string>();
        list.Add("cmMerc2_3_SpecOps_1_M1");  
        list.Add("cmMerc2_3_SpecOps_1_A1");  
        list.Add("cmMerc2_3_SpecOps_1_M2");  
        list.Add("cmMerc2_3_SpecOps_1_A2");
        var dialog = DialogsLibrary.GetPairDialogByTag(list, null);
        return () => dialog;
    }

    protected Func<MessageDialogData> AfterCompleteDialog2()
    {
        var list = new List<string>();
        list.Add("cmMerc2_3_SpecOps_2_M1"); 
        list.Add("cmMerc2_3_SpecOps_2_A1"); 
//        list.Add("cmMerc2_3_SpecOps_2_M2"); 
//        list.Add("cmMerc2_3_SpecOps_2_A3"); 
//        list.Add("cmMerc2_3_SpecOps_2_M3"); 
        list.Add("DC");
        var dialog = DialogsLibrary.GetPairDialogByTag(list, null);
        return () => dialog;
    }  
    protected Func<MessageDialogData> AfterCompleteDialog3()
    {
        var list = new List<string>();
        list.Add("cmMerc2_3_SpecOps_3_M1"); 
        list.Add("cmMerc2_3_SpecOps_3_A1"); 
        list.Add("cmMerc2_3_SpecOps_3_M2"); 
        list.Add("cmMerc2_3_SpecOps_3_A2"); 
        list.Add("cmMerc2_3_SpecOps_3_M3"); 
        list.Add("cmMerc2_3_SpecOps_3_A3"); 
        list.Add("cmMerc2_3_SpecOps_3_M4"); 
        list.Add("cmMerc2_3_SpecOps_3_A4"); 
        list.Add("cmMerc2_3_SpecOps_3_M5"); 
        list.Add("DC");
        var dialog = DialogsLibrary.GetPairDialogByTag(list, null);
        return () => dialog;
    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.mercenary);


        var posibleCells = sectorId.ListCells
            .Where(x => x.Data != null && !(x.Data is GlobalMapNothing) && x.Data.CurMovingArmy.NoAmry()).ToList();

        var cell = posibleCells.RandomElement();
        if (cell != null)
        {
            _enemiesController = player.MapData.GalaxyData.GalaxyEnemiesArmyController;
            _army = _enemiesController.BornArmyAtCell(cell.Data,false, (int)(player.Army.GetPower() * 1.1f));
            switch (_index)
            {
                case 1:
                    _army.SetEndDialog(AfterCompleteDialog1());
                    _army.SetStartDialog(ArmyStart);
                    break;
                case 2:
                    _army.SetEndDialog(AfterCompleteDialog2());
                    break;
                case 3:
                    _army.SetEndDialog(AfterCompleteDialog3());
                    break;
            }

            ;
//            _army.SetDestroyCallback(ArmyDestroyed);
            return true;
        }

        return false;
    }

    private MessageDialogData ArmyStart(Action fight)
    {
        var msg2 = new MessageDialogData(Namings.Tag("cmMerc2_3_SpecOps_0_M2"),
            new List<AnswerDialogData>() { new AnswerDialogData(Namings.Tag("cmMerc2_3_SpecOps_0_A2"), fight) }, true);
        var msg1 = new MessageDialogData(Namings.Tag("cmMerc2_3_SpecOps_0_M1"),
            new List<AnswerDialogData>() { new AnswerDialogData(Namings.Tag("cmMerc2_3_SpecOps_0_A1"), null, () => msg2) }, true);

        return msg1;
    }

    protected override void SubAfterLoad()
    {
        if (isInited) return;

        isInited = true;
        _enemiesController.OnAddMovingArmy += OnAddMovingArmy;
    }

    private void OnAddMovingArmy(MovingArmy arg1, bool arg2)
    {
        if (!arg2)
            if (arg1.Id == _army.Id)
                _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmMerc2_3_SpecForce + _index);
    }



    public override void OnClick()
    {
        TryNavigateToCell(GetCurCellTarget());
    }

    protected override void StageDispose()
    {
    }

    public GlobalMapCell GetCurCellTarget()
    {
        return _army?.CurCell;
    }

    public override string GetDesc()
    {
        return Namings.Tag("destroyFleet");
    }
}