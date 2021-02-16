using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class QuestStageCmRdr2_3_SpecForce : QuestStage
{
    private MovingArmy _army;
    private GalaxyEnemiesArmyController _enemiesController;

    private bool isInited = false;
    private bool _shallFight = false;
    public override bool CloseWindowOnClick => true;

    public QuestStageCmRdr2_3_SpecForce()
        : base(QuestsLib.QuestStageCmRdr2_3_SpecForce)
    {

    }

    protected override bool StageActivate(Player player)
    {
        _player = player;
        _shallFight =  MyExtensions.IsTrueEqual();
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.mercenary);


        var posibleCells = sectorId.ListCells
            .Where(x => x.Data != null && !(x.Data is GlobalMapNothing) && x.Data.CurMovingArmy.NoAmry()).ToList();

        var cell = posibleCells.RandomElement();
        if (cell != null)
        {
            _enemiesController = player.MapData.GalaxyData.GalaxyEnemiesArmyController;
            _army = _enemiesController.BornArmyAtCell(cell.Data, false);
            _army.SetStartDialog(StartDialog);
            if (_shallFight)
                _army.SetEndDialog(AfterDiallog);
            return true;
        }

        return false;
    }

    private MessageDialogData AfterDiallog()
    {
        var list = new List<string>();
        list.Add("cmRdr2_3_dialog_2_M1");
        list.Add("cmRdr2_3_dialog_2_A1");
        list.Add("cmRdr2_3_dialog_2_M2");
        list.Add("cmRdr2_3_dialog_2_A3");
        list.Add("cmRdr2_3_dialog_2_M4");
        list.Add("cmRdr2_3_dialog_2_A5");
        list.Add("cmRdr2_3_dialog_2_M6");
        list.Add("cmRdr2_3_dialog_2_A7");
        list.Add("cmRdr2_3_dialog_2_M8");
        list.Add("DC");
        list.Add("cmRdr2_3_dialog_2_M9");
        list.Add("cmRdr2_3_dialog_2_A10");

        var dialog2 = DialogsLibrary.GetPairDialogByTag(list, null);
        return dialog2;
    }

    private MessageDialogData StartDialog(Action arg)
    {
        var list = new List<string>();

        if (_shallFight)
        {
            list.Add("cmRdr2_3_dialog_1_M1");
            list.Add("cmRdr2_3_dialog_1_A1");
            list.Add("cmRdr2_3_dialog_1_M2");
            list.Add("cmRdr2_3_dialog_1_A3");
            list.Add("cmRdr2_3_dialog_1_M4");
            list.Add("cmRdr2_3_dialog_1_A5");
            list.Add("cmRdr2_3_dialog_1_M6");
            list.Add("cmRdr2_3_dialog_1_A7");
            _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr2_3_SpecForce);
            return DialogsLibrary.GetPairDialogByTag(list, arg);
        }
        else
        {
            list.Add("cmRdr2_3_dialog_1_M1");
            list.Add("cmRdr2_3_dialog_1_A1");
            list.Add("cmRdr2_3_dialog_1_M2");
            list.Add("cmRdr2_3_dialog_1_A3");
            list.Add("cmRdr2_3_dialog_1_M4");
            list.Add("cmRdr2_3_dialog_1_A5");
            list.Add("cmRdr2_3_dialog_1_M6");
            list.Add("cmRdr2_3_dialog_1_A7");

            list.Add("cmRdr2_3_dialog_3_M1");
            list.Add("cmRdr2_3_dialog_3_A1");
            list.Add("cmRdr2_3_dialog_3_M2");
            list.Add("cmRdr2_3_dialog_3_A3");
            list.Add("cmRdr2_3_dialog_3_M4");
            list.Add("cmRdr2_3_dialog_3_A5");
            list.Add("cmRdr2_3_dialog_3_M6");
            list.Add("cmRdr2_3_dialog_3_A7");
            list.Add("cmRdr2_3_dialog_3_M8");
            list.Add("DC");
            list.Add("cmRdr2_3_dialog_3_M9");
            list.Add("cmRdr2_3_dialog_3_A10");
            list.Add("cmRdr2_3_dialog_3_M11");
            list.Add("cmRdr2_3_dialog_3_A12");
            list.Add("cmRdr2_3_dialog_3_M13");
            list.Add("cmRdr2_3_dialog_3_A14");
            list.Add("cmRdr2_3_dialog_3_M15");
            list.Add("cmRdr2_3_dialog_3_A16");
            list.Add("cmRdr2_3_dialog_3_M17");
            list.Add("cmRdr2_3_dialog_3_A18");
            list.Add("cmRdr2_3_dialog_3_M19");
            list.Add("DC");
            list.Add("cmRdr2_3_dialog_3_M20");
            list.Add("cmRdr2_3_dialog_3_A21");


            return DialogsLibrary.GetPairDialogByTag(list,SComplete);

        }

    }

    private void SComplete()
    {
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmRdr2_3_SpecForce);
        _enemiesController.SimpleArmyDestroyed(_army);

    }

//    private Func<MessageDialogData> AfterCompleteDialog1()
//    {
//        var list = new List<string>();
//        list.Add("cmOcr2_1_dialog_3_M1");
//        list.Add("cmOcr2_1_dialog_3_A1");    
//        list.Add("cmOcr2_1_dialog_3_M2");
//        list.Add("cmOcr2_1_dialog_3_A2");
//        var dialog = DialogsLibrary.GetPairDialogByTag(list, null);
//        return () => dialog;
//    }

    protected override void SubAfterLoad()
    {
        if (isInited)
            return;

        isInited = true;
//        _enemiesController.OnAddMovingArmy += OnAddMovingArmy;
    }

//    private void OnAddMovingArmy(MovingArmy arg1, bool arg2)
//    {
//        if (!arg2)
//            if (arg1.Id == _army.Id)
//                _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr2_2_SpecForce);
//    }



    public override void OnClick()
    {
        TryNavigateToCell(GetCurCellTarget());
    }

    protected override void StageDispose()
    {
    }

    public GlobalMapCell GetCurCellTarget()
    {
        return _army?.CurCell.Data;
    }

    public override string GetDesc()
    {
        return Namings.Tag("destroyFleet");
    }
}