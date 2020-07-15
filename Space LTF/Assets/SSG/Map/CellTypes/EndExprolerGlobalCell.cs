using System.Linq;
using UnityEngine;


[System.Serializable]
public class EndExprolerGlobalCell : EndGlobalCell
{

    public EndExprolerGlobalCell(int power, int id, int intX, int intZ, SectorData sector)
        : base((int)(power * 1.025f), id,intX,intZ,sector)
    {
    }

    public override bool OneTimeUsed()
    {
        return false;
    }

    protected override MessageDialogData GetDialog()
    {
        if (_data == null)
        {
            var questData = MainController.Instance.MainPlayer.QuestData;
            _data = questData.LastBattleData;
            _data.Init(Power);
        }
        _data.SetReady();
        return _data.GetDialog();
    }


}

