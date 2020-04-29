
using System.Collections.Generic;

[System.Serializable]
public class ArmyDungeonGlobalMapCell : ArmyGlobalMapCell
{
    protected int _constanctPowerPower;
    protected int _stepPowerCoef = 3;
//    public override int Power => (int)(_power * _coefPower);
    private float _coefPower;
    public ArmyDungeonGlobalMapCell(int power, ShipConfig config, int id, int Xind, int Zind, SectorData sector, float coefPower) : base(
        power, config, id, Xind, Zind, sector)
    {
        _coefPower = coefPower;
    }
//    public ov void Complete()
//    {
//        Scouted();
//    }

    public override int Power => (int)((_power + _constanctPowerPower) * _coefPower);
    protected override MessageDialogData GetDialog()
    {

        if (Completed)
        {
            var ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData(Namings.DialogTag("Ok")));
            var masinMsg = Namings.Format(Namings.DialogTag("sectorClear"));
            var mesData = new MessageDialogData(masinMsg, ans);
            return mesData;
        }
        else
        {
            string masinMsg;
            var myPlaer = MainController.Instance.MainPlayer;
            string scoutsField;
            var scoutData = GetArmy().ScoutData.GetInfo(myPlaer.Parameters.Scouts.Level);
            if (_eventType.HasValue)
            {
                scoutsField = Namings.Format(Namings.DialogTag("armySectorEvent"), Namings.BattleEvent(_eventType.Value)); ;
            }
            else
            {
                scoutsField = "";
            }
            for (int i = 0; i < scoutData.Count; i++)
            {
                var info = scoutData[i];
                scoutsField = $"{scoutsField}\n{info}\n";
            }
            var ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData(Namings.DialogTag("Attack"), Take));
            masinMsg = Namings.Format(Namings.DialogTag("armyShallFight"), scoutsField);
            ans.Add(new AnswerDialogData(
                Namings.Format(Namings.DialogTag("armyRun"), scoutsField),
                () =>
                {
                }, null, false, true));

            var mesData = new MessageDialogData(masinMsg, ans);
            return mesData;
        }

    }

    protected override void CacheArmy()
    {
        ArmyCreatorData data = ArmyCreatorLibrary.GetArmy(ConfigOwner);
        var player = new PlayerAIMilitary(name);
        var army = ArmyCreator.CreateSimpleEnemyArmy(Power, data, player);
        player.Army.SetArmy(army);
        _enemyPlayer = player;
    }

    public override bool OneTimeUsed()
    {
        return false;
    }
    public override void LeaveFromCell()
    {
        Uncomplete();
        _constanctPowerPower += _stepPowerCoef;
        CacheArmy();
    }
}