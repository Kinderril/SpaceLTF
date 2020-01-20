
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[System.Serializable]
public class EndGlobalCell : ArmyGlobalMapCell
{
    private FinalBattleData _data;

    public EndGlobalCell(int power, int id, int intX, int intZ, SectorData sector) 
        : base(power,ShipConfig.droid,id, ArmyCreatorType.destroy, intX, intZ, sector)
    {
        _power = SectorData.CalcCellPower(0, sector.Size, power, _additionalPower);
        InfoOpen = true;
        Scouted();
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
            _data.Init(_power);
        }
        return _data.GetDialog();
    }

    public override Color Color()
    {
        return new Color(51f / 255f, 102f / 255f, 153f/255f);
    }

    public override void ComeTo()
    {
        _power = SectorData.CalcCellPower(0, _sector.Size, _power, _additionalPower);
        if (_data == null)
        {
            var questData = MainController.Instance.MainPlayer.QuestData;
            questData.ComeToLastPoint();
//            questData.CheckIfOver();
            _data = questData.LastBattleData;
            _data.Init(_power);
        }
    }

    public override bool OneTimeUsed()
    {
        return true;
    }
}

