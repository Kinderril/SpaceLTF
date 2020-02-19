using System.Collections.Generic;


[System.Serializable]
public class SpaceGarbageMapEvent : BaseGlobalMapEvent
{
    public override string Desc()
    {
        return "Space garbage";
    }

    public override MessageDialogData GetDialog()
    {
        var mianAnswers = new List<AnswerDialogData>();

        mianAnswers.Add(new AnswerDialogData(string.Format("Dive deep. [Scouts:{0}]", ScoutsLevel), null, HalfGood));
        mianAnswers.Add(new AnswerDialogData(string.Format("Search on border. [Scouts:{0}].", ScoutsLevel), null, DiveDeep));
        mianAnswers.Add(new AnswerDialogData("I don't want to dig in the trash.", null));
        var mesData = new MessageDialogData("In front of you a huge count of garbage.", mianAnswers);
        return mesData;
    }

    private MessageDialogData DiveDeep()
    {
        var mianAnswers = new List<AnswerDialogData>();
        if (SkillWork(0, ScoutsLevel))
        {
            int slot;
            if (MyExtensions.IsTrue01(.5f) && MainController.Instance.MainPlayer.Inventory.GetFreeWeaponSlot(out slot))
            {
                var modul = Library.CreateWeapon(false);
                MainController.Instance.MainPlayer.Inventory.TryAddWeaponModul(modul, slot);

                mianAnswers.Add(new AnswerDialogData("Ok.", null, null));
                var mesData = new MessageDialogData(
                    string.Format("This is new weapon {0}", Namings.Weapon(modul.WeaponType)), mianAnswers);
                return mesData;
            }
            else
            {
                mianAnswers.Add(new AnswerDialogData("Ok.", null, null));
                var mesData = new MessageDialogData("Nothing.", mianAnswers);
                return mesData;
            }
        }
        else
        {
            mianAnswers.Add(new AnswerDialogData("Fight.", Fight, null));
            var mesData = new MessageDialogData("Ambush!", mianAnswers);
            return mesData;
        }
    }

    private void Fight()
    {
        var myArmyPower = ArmyCreator.CalcArmyPower(MainController.Instance.MainPlayer.Army);
        MainController.Instance.PreBattle(MainController.Instance.MainPlayer,
            GetArmy(_config, (int)myArmyPower));
    }
    private MessageDialogData HalfGood()
    {
        var mianAnswers = new List<AnswerDialogData>();
        mianAnswers.Add(new AnswerDialogData("Ok.", null, null));
        if (SkillWork(2, ScoutsLevel))
        {

            var cellsToScout = MainController.Instance.MainPlayer.MapData.ScoutedCells(3, 5);
            var d = string.Format("{0} points on global map scouted.", cellsToScout);
            var mesData = new MessageDialogData(string.Format("This is galaxy map! {0}", d), mianAnswers);
            return mesData;
        }
        else
        {

            var mesData = new MessageDialogData($"Nothing.", mianAnswers);
            return mesData;
        }

    }

    public SpaceGarbageMapEvent(ShipConfig config) : base(config)
    {
    }
}

