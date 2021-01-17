using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class QuestStageCmMerc1_2_SpecForce : QuestStage
{
    private MovingArmy _army;
    private GalaxyEnemiesArmyController _enemiesController;

    private bool isInited = false;
    public override bool CloseWindowOnClick => true;

    public QuestStageCmMerc1_2_SpecForce()
        : base(QuestsLib.QuestStageCmMerc1_2_SpecForce)
    {
    }

    protected override bool StageActivate(Player player)
    {
        var sectorId = player.MapData.GalaxyData.AllSectors.FirstOrDefault(x => x.ShipConfig == ShipConfig.ocrons);


        var posibleCells = sectorId.ListCells
            .Where(x => x.Data != null && !(x.Data is GlobalMapNothing) && x.Data.CurMovingArmy.NoAmry()).ToList();

        var cell = posibleCells.RandomElement();
        if (cell != null)
        {
            _enemiesController = player.MapData.GalaxyData.GalaxyEnemiesArmyController;
            _army = _enemiesController.BornArmyAtCell(cell.Data,false,(int)(player.Army.GetPower() * 1.1f));
//            _army.SetDestroyCallback(ArmyDestroyed);
            return true;
        }

        return false;
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
                _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmMerc1_2_SpecForce);
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
        return _army?.CurCell.Data;
    }

    public override string GetDesc()
    {
        return Namings.Tag("destroyFleet");
    }
}