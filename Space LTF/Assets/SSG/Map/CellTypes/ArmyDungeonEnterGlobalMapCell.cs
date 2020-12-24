using System.Collections.Generic;

[System.Serializable]
public class ArmyDungeonEnterGlobalMapCell : ArmyGlobalMapCell
{
//    private float _coefPower;
//    public override int Power => (int)(_power * _coefPower);
    public ArmyDungeonEnterGlobalMapCell(int power, ShipConfig config, int id, int Xind, int Zind, SectorData sector, float coefPower)
        : base(power, config, id, Xind, Zind, sector)
    {
            _powerCoef = 1f + coefPower;
    }

    protected override MessageDialogData GetDialog()
    {
        var myPlaer = MainController.Instance.MainPlayer;
        string masinMsg;

        var ans = new List<AnswerDialogData>();
        var scoutData = GetArmy().ScoutData.GetInfo(myPlaer.Parameters.Scouts.Level);
        string scoutsField = "";
        for (int i = 0; i < scoutData.Count; i++)
        {
            var info = scoutData[i];
            scoutsField = $"{scoutsField}\n{info}\n";
        }


        masinMsg = Namings.Format(Namings.DialogTag("dungeogArmyEnterStart"), scoutsField); ;
//        ans.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("dungeogArmyFriendGoIn"), rep), null, null));
        ans.Add(new AnswerDialogData(Namings.DialogTag("Attack"), Take));
        ans.Add(new AnswerDialogData(Namings.Tag("leave"), null, null, false, true));
        var mesData = new MessageDialogData(masinMsg, ans);
        return mesData;
    }
    public override bool OneTimeUsed()
    {
        return true;
    }
}

