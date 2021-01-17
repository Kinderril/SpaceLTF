using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class QuestStartClearSectorFed1 : QuestStage
{

    private GalaxyEnemiesArmyController _enemiesController;
    private int _countOnStart = 0;                                                                                                                    
    public QuestStartClearSectorFed1()
        : base(QuestsLib.QUEST_FED_CLEAR_SECTOR)
    {

    }

    private HashSet<MovingArmy> _idsToKill = new HashSet<MovingArmy>();

    protected override bool StageActivate(Player player)
    {
        var allSectors = GetSectors(player,0, 4,1);
        var targetSector = allSectors.RandomElement();
        foreach (var targetSectorListCell in targetSector.ListCells)
        {
            if (targetSectorListCell.Data != null && targetSectorListCell.Data.CurMovingArmy.HaveArmy())
            {
                foreach (MovingArmy id in targetSectorListCell.Data.CurMovingArmy.GetAllArmies())
                {
                    _idsToKill.Add(id);
                }
            }
        }
        _enemiesController = player.MapData.GalaxyData.GalaxyEnemiesArmyController;
        _countOnStart = _idsToKill.Count;
        return true;



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
            if (_idsToKill.Contains(arg1))
            {
                _idsToKill.Remove(arg1);
            }

            if (_idsToKill.Count == 0)
            {
                _playerQuest.QuestIdComplete(QuestsLib.QUEST_FED_CLEAR_SECTOR);
            }
            else
            {
                TextChangeEvent();
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
        if (_idsToKill.Count > 0)
        {
            var army = _idsToKill.First();
            return army.CurCell.Data;
        }
        return null;
    }
    public override string GetDesc()
    {

        return $"{Namings.Tag("questNameSectorClearFedMerc")} {(_countOnStart -_idsToKill.Count)}/{_countOnStart}";
    }
}
