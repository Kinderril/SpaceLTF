using System.Linq;
using UnityEngine;


[System.Serializable]
public class EndGlobalCell : ArmyGlobalMapCell
{
    private FinalBattleData _data;

    public EndGlobalCell(int power, int id, int intX, int intZ, SectorData sector)
        : base(power, ShipConfig.droid, id, intX, intZ, sector)
    {
        Power = SectorData.CalcCellPower(0, sector.Size, power, _additionalPower);
        InfoOpen = true;
        Scouted();
    }
    protected override void CacheArmy()
    {
        // ArmyCreatorData data = ArmyCreatorLibrary.GetArmy(ConfigOwner);
        var player = new PlayerAIMainBoss(name);
        var rep = MainController.Instance.MainPlayer.ReputationData;
        var array = rep.ReputationFaction.OrderBy(x => x.Value).ToArray();
        var conf1 = array[0].Key;
        var conf2 = array[1].Key;
        var armyType = ArmyCreatorLibrary.GetArmy(conf1, conf2);
        armyType.MainShipCount = 2;
        var power = Mathf.Clamp(Power, 30, 999);
        var army = ArmyCreator.CreateSimpleEnemyArmy(power, armyType, player);
        // ArmyCreator.cre
        player.Army.SetArmy(army);
        _player = player;
    }

    public override string Desc()
    {
        return "Galaxy gate";
    }

    public override void Take()
    {

    }

    public override bool CanCellDestroy()
    {
        return false;
    }

    protected override MessageDialogData GetLeavedActionInner()
    {
        var getDialog = _data.GetAfterBattleDialog();
        return getDialog;
    }

    protected override MessageDialogData GetDialog()
    {
        if (_data == null)
        {
            var questData = MainController.Instance.MainPlayer.QuestData;
            questData.ComeToLastPoint();
            //            questData.CheckIfOver();
            _data = questData.LastBattleData;
            _data.Init(Power);
        }
        return _data.GetDialog();
    }

    public override Color Color()
    {
        return new Color(51f / 255f, 102f / 255f, 153f / 255f);
    }

    public override void ComeTo(GlobalMapCell from)
    {
        Power = SectorData.CalcCellPower(0, _sector.Size, _power, _additionalPower);
        if (_data == null)
        {
            var questData = MainController.Instance.MainPlayer.QuestData;
            questData.ComeToLastPoint();
            //            questData.CheckIfOver();
            _data = questData.LastBattleData;
            _data.Init(Power);
        }
    }

    public override bool OneTimeUsed()
    {
        return true;
    }
}

