using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class QuestStartKillMerc1 : QuestStage
{
    private MovingArmy _army;
    private GalaxyEnemiesArmyController _enemiesController;
    public QuestStartKillMerc1()
        : base(QuestsLib.QUEST_MERC_BATTLE_TARGET)
    {

    }

    protected override bool StageActivate(Player player)
    {
        var allSectors = player.MapData.GalaxyData.GetAllList();

        var posibleCells = allSectors.Where(x =>
        x is FreeActionGlobalMapCell && x.CurMovingArmy.NoAmry() 
                                     && x.indX > player.MapData.CurrentCell.indX
                                     && player.MapData.CurrentCell.SectorId != x.Id ).ToList();

        if (posibleCells.Count == 0)
        {
            posibleCells = allSectors.Where(x => !(x is GlobalMapNothing) && x.CurMovingArmy.NoAmry()).ToList();
        }

        if (posibleCells.Count == 0)
        {
            return false;
        }

        var cell = posibleCells.RandomElement();
        if (cell != null)
        {
            _enemiesController = player.MapData.GalaxyData.GalaxyEnemiesArmyController;
            _army = _enemiesController.BornArmyAtCell(cell,false, (int)(player.Army.GetPower() * 1.1f));
//            _army.SetDestroyCallback(ArmyDestroyed);
            return true;
        }

        return false;

    }

    private bool isInited = false;

    protected override void SubAfterLoad()
    {
        if (isInited)
        {
            return;
        }

        isInited = true;
        _enemiesController.OnAddMovingArmy += OnAddMovingArmy;
    }

    private void OnAddMovingArmy(MovingArmy arg1, bool arg2)
    {
        if (!arg2)
        {
            if (arg1.Id == _army.Id)
            {
                _playerQuest.QuestIdComplete(QuestsLib.QUEST_MERC_BATTLE_TARGET);
            }
        }
    }


public override bool CloseWindowOnClick => true;
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

        return Namings.Tag("questNameKillMerc");
    }
}
