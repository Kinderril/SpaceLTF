using System.Collections.Generic;

[System.Serializable]
public class ArmyDungeonEnterGlobalMapCell : ArmyGlobalMapCell
{
    public ArmyDungeonEnterGlobalMapCell(int power, ShipConfig config, int id, int Xind, int Zind, SectorData sector)
        : base(power, config, id, Xind, Zind, sector)
    {

    }

    protected override MessageDialogData GetDialog()
    {
        var myPlaer = MainController.Instance.MainPlayer;
//        var status = myPlaer.ReputationData.GetStatus(ConfigOwner);
//        bool isFriends = status == EReputationStatus.friend;
        string masinMsg;

        var ans = new List<AnswerDialogData>();
//        var rep = MainController.Instance.MainPlayer.ReputationData.ReputationFaction[ConfigOwner];
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

