using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class QuestStageCmOcr2_1_SpecForce : QuestStage
{
    private MovingArmy _army;
    private GalaxyEnemiesArmyController _enemiesController;

    private bool isInited = false;
    public override bool CloseWindowOnClick => true;

    public QuestStageCmOcr2_1_SpecForce()
        : base(QuestsLib.QuestStageCmOcr2_1_SpecForce)
    {
    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.droid);


        var posibleCells = sectorId.ListCells
            .Where(x => x.Data != null && !(x.Data is GlobalMapNothing) && x.Data.CurMovingArmy == null).ToList();

        var cell = posibleCells.RandomElement();
        if (cell != null)
        {
            _enemiesController = player.MapData.GalaxyData.GalaxyEnemiesArmyController;
            _army = _enemiesController.BornArmyAtCell(cell.Data,7);
            _army.SetEndDialog(AfterCompleteDialog1());
            //            _army.SetDestroyCallback(ArmyDestroyed);
            return true;
        }

        return false;
    }

    private Func<MessageDialogData> AfterCompleteDialog1()
    {
        var list = new List<string>();
        list.Add("cmOcr2_1_dialog_3_M1");
        list.Add("cmOcr2_1_dialog_3_A1");    
        list.Add("cmOcr2_1_dialog_3_M2");
        list.Add("cmOcr2_1_dialog_3_A2");
        var dialog = DialogsLibrary.GetPairDialogByTag(list, null);
        return () => dialog;
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
                _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr2_1_SpecForce);
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