using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class QuestStageCmOcr2_2_SpecForce : QuestStage
{
    private MovingArmy _army;
    private GalaxyEnemiesArmyController _enemiesController;

    private bool isInited = false;
    public override bool CloseWindowOnClick => true;
    private Player _player;
    private SimpleModulType _type;

    public QuestStageCmOcr2_2_SpecForce(SimpleModulType type)
        : base(QuestsLib.QuestStageCmOcr2_2_SpecForce)
    {
        _type = type;
    }

    protected override bool StageActivate(Player player)
    {
        _player = player;
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.mercenary);


        var posibleCells = sectorId.ListCells
            .Where(x => x.Data != null && !(x.Data is GlobalMapNothing) && x.Data.CurMovingArmy.NoAmry()).ToList();

        var cell = posibleCells.RandomElement();
        if (cell != null)
        {
            _enemiesController = player.MapData.GalaxyData.GalaxyEnemiesArmyController;
            _army = _enemiesController.BornArmyAtCell(cell.Data, false, 7);
            _army.SetStartDialog(AfterCompleteDialog1);
            //            _army.SetDestroyCallback(ArmyDestroyed);
            return true;
        }

        return false;
    }

    private MessageDialogData AfterCompleteDialog1(Action arg)
    {
        var list = new List<string>();
        foreach (var inventoryModul in _player.Inventory.Moduls)
        {
            if (inventoryModul.Type == _type)
            {

                list.Add("cmOcr2_2_dialog_4_M1");
                list.Add("DC");  
                list.Add("cmOcr2_2_dialog_4_M2");
                list.Add("cmOcr2_2_dialog_4_A2");  
                list.Add("cmOcr2_2_dialog_4_M3");
                list.Add("cmOcr2_2_dialog_4_A3");  
                list.Add("cmOcr2_2_dialog_4_M4");
                list.Add("cmOcr2_2_dialog_4_A4");  
                list.Add("cmOcr2_2_dialog_4_M5");
                list.Add("DC");  
                list.Add("cmOcr2_2_dialog_4_M6");
                list.Add("cmOcr2_2_dialog_4_A6");  
                list.Add("cmOcr2_2_dialog_4_M7");
                list.Add("cmOcr2_2_dialog_4_A7");  
                list.Add("cmOcr2_2_dialog_4_M8");
                list.Add("cmOcr2_2_dialog_4_A8");  
                list.Add("cmOcr2_2_dialog_4_M9");
                list.Add("cmOcr2_2_dialog_4_A9");  
                list.Add("cmOcr2_2_dialog_4_M10");
                list.Add("cmOcr2_2_dialog_4_A10");  
                list.Add("cmOcr2_2_dialog_4_M11");
                list.Add("cmOcr2_2_dialog_4_A11");
                var dialog = DialogsLibrary.GetPairDialogByTag(list, SComplete);

                return dialog;
            }
        }

        list.Add("cmOcr2_2_dialog_4fail_M1");
        list.Add("cmOcr2_2_dialog_4fail_A1");
        var dialog2 = DialogsLibrary.GetPairDialogByTag(list, null);
        return dialog2;

    }

    private void SComplete()
    {
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr2_2_SpecForce);
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
        return _army?.CurCell;
    }

    public override string GetDesc()
    {
        return Namings.Tag("destroyFleet");
    }
}