using System.Collections.Generic;


[System.Serializable]
public class SpaceGarbageMapEvent : BaseGlobalMapEvent
{
    public override string Desc()
    {
        return Namings.DialogTag("Space garbage");
    }

    public override MessageDialogData GetDialog()
    {
        var mianAnswers = new List<AnswerDialogData>();

        mianAnswers.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("scDive"), ScoutsLevel), null, HalfGood));
        mianAnswers.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("scSearch"), ScoutsLevel), null, DiveDeep));
        mianAnswers.Add(new AnswerDialogData(Namings.DialogTag("scNoWant"), null));
        var mesData = new MessageDialogData(Namings.DialogTag("scGarbage"), mianAnswers);
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
                var modul = Library.CreateDamageWeapon(false);
                MainController.Instance.MainPlayer.Inventory.TryAddWeaponModul(modul, slot);

                mianAnswers.Add(new AnswerDialogData(Namings.Tag("Ok"), null, null));
                var mesData = new MessageDialogData(
                    Namings.Format(Namings.DialogTag("scWeapon"), Namings.Weapon(modul.WeaponType)), mianAnswers);
                return mesData;
            }
            else
            {
                mianAnswers.Add(new AnswerDialogData(Namings.Tag("Ok"), null, null));
                var mesData = new MessageDialogData(Namings.DialogTag("nothing"), mianAnswers);
                return mesData;
            }
        }
        else
        {
            mianAnswers.Add(new AnswerDialogData(Namings.Tag("Fight"), Fight, null));
            var mesData = new MessageDialogData(Namings.DialogTag("scAmbush"), mianAnswers);
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
        mianAnswers.Add(new AnswerDialogData(Namings.Tag("Ok"), null, null));
        if (SkillWork(2, ScoutsLevel))
        {

            var cellsToScout = MainController.Instance.MainPlayer.MapData.ScoutedCells(3, 5);
            var d = Namings.Format(Namings.DialogTag("scScouted"), cellsToScout);
            var mesData = new MessageDialogData(Namings.Format(Namings.DialogTag("scMap"), d), mianAnswers);
            return mesData;
        }
        else
        {

            var mesData = new MessageDialogData(Namings.DialogTag("nothing"), mianAnswers);
            return mesData;
        }

    }

    public SpaceGarbageMapEvent(ShipConfig config) : base(config)
    {
    }
}

