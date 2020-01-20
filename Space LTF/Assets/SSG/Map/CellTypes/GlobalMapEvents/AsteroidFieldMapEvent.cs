using System.Collections.Generic;
using System.Linq;


[System.Serializable]
public class AsteroidFieldMapEvent : BaseGlobalMapEvent
{
    private int weaponTryies = 0;

    public override string Desc()
    {
        return "Asteroid field";
    }

    public override MessageDialogData GetDialog()
    {
        var mesData = new MessageDialogData("Big asteroid field. You need somehow get to other side.", StandartOptions());
        return mesData;
    }

    private List<AnswerDialogData> StandartOptions()
    {
        var ans = new List<AnswerDialogData>()
        {
            new AnswerDialogData("Fire from all weapons", null,  weaponsFire),
            new AnswerDialogData("Slow go through field.", null,   throughtField),
        };
        return ans;
    }

    private MessageDialogData throughtField()
    {
        if (MyExtensions.IsTrueEqual())
        {
            var ans = new List<AnswerDialogData>()
            {
                new AnswerDialogData(Namings.Ok)
            };
            MessageDialogData mesData;
            if (SkillWork(2, ScoutsLevel))
            {
                mesData = new MessageDialogData("Nothing happens. You successfully complete way", ans);
            }
            else
            {
                BrokeShipWithRandom();
                mesData = new MessageDialogData("Nothing happens, but some ships need to be repaired. You successfully complete way", ans);

            }
            return mesData;
        }
        else
        {
            var ans = new List<AnswerDialogData>()
            {
                new     AnswerDialogData($"Try to repair it [Repair:{RepairLevel}].",null,repairResult),
                new     AnswerDialogData($"Try to decompile it for money.",null,()=> moneyResult(10,20))
            };
            var mesData = new MessageDialogData("You see broken ship.", ans);
            return mesData;
        }
    }

    private MessageDialogData repairResult()
    {
        if (SkillWork(4, RepairLevel))
        {
            var ans = new List<AnswerDialogData>()
            {
                new AnswerDialogData("Ok.")
            };
            MessageDialogData mesData;
            if (MainController.Instance.MainPlayer.Inventory.GetFreeWeaponSlot(out var slot))
            {
                var modul = Library.CreateWeapon(false);
                mesData = new MessageDialogData($"This ship can't be fully repaired but now you can use some parts. {Namings.Weapon(modul.WeaponType)}", ans);
                MainController.Instance.MainPlayer.Inventory.TryAddWeaponModul(modul, slot);
            }
            else
            {
                mesData = new MessageDialogData($"This ship can't be fully repaired but you have no place for items.", ans);
            }
            return mesData;
        }
        else
        {
            return failResult();
        }
    }


    private MessageDialogData weaponsFire()
    {
        weaponTryies++;
        var player = MainController.Instance.MainPlayer;
        var rockectWeapons = player.Army.Army.Where(x =>
            x.Ship.WeaponsModuls.FirstOrDefault(y => y != null && (y.WeaponType == WeaponType.rocket || y.WeaponType == WeaponType.casset)) != null);
        if (rockectWeapons.Any())
        {
            var ans = new List<AnswerDialogData>()
            {
                new     AnswerDialogData(Namings.Ok)
            };
            var mesData = new MessageDialogData("Explosive weapons easily destroy enough asteroid. Now you have a way.", ans);
            return mesData;
        }
        else
        {
            BrokeShipWithRandom();
            MessageDialogData mesData;
            if (weaponTryies >= 2)
            {
                var ans = new List<AnswerDialogData>()
                {
                    new     AnswerDialogData(Namings.Ok)
                };
                mesData = new MessageDialogData("Finally you have a way! But some of your ships get damage", ans);
            }
            else
            {

                mesData = new MessageDialogData("Your weapons are have not much power to create a way throug. And some of your ships get damage", StandartOptions());
            }
            return mesData;

        }
    }

    private void BrokeShipWithRandom()
    {
        var player = MainController.Instance.MainPlayer;
        foreach (var data in player.Army.Army)
        {
            if (MyExtensions.IsTrueEqual())
            {
                var per = data.Ship.HealthPercent;
                data.Ship.SetRepairPercent(per * 0.8f);
            }
        }
    }
    public AsteroidFieldMapEvent(ShipConfig config) : base(config)
    {
    }
}

