using System;
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
        var mesData = new MessageDialogData(Namings.DialogTag("asteroid_start"), StandartOptions());
        return mesData;
    }

    private List<AnswerDialogData> StandartOptions()
    {
        var ans = new List<AnswerDialogData>()
        {
            new AnswerDialogData(Namings.DialogTag("asteroid_fireAll") , null,  weaponsFire),
            new AnswerDialogData(Namings.DialogTag("asteroid_slow") , null,   throughtField),
        };
        return ans;
    }

    private MessageDialogData throughtField()
    {
        if (MyExtensions.IsTrueEqual())
        {
            var ans = new List<AnswerDialogData>()
            {
                new AnswerDialogData(Namings.Tag("Ok"))
            };
            MessageDialogData mesData;
            if (SkillWork(2, ScoutsLevel))
            {
                mesData = new MessageDialogData(Namings.DialogTag("asteroid_nothingComplete"), ans);
            }
            else
            {
                BrokeShipWithRandom();
                mesData = new MessageDialogData(Namings.DialogTag("asteroid_nothingCompleteRepair"), ans);

            }
            return mesData;
        }
        else
        {
            var ans = new List<AnswerDialogData>()
            {
                new     AnswerDialogData(Namings.Format(Namings.DialogTag("asteroid_brokenShipRepair"),RepairLevel) ,null,repairResult),
                new     AnswerDialogData(Namings.DialogTag("asteroid_brokenShipdecompile"),null,()=> moneyResult(10,20))
            };
            var mesData = new MessageDialogData(Namings.Format(Namings.DialogTag("asteroid_brokenShip"), Namings.ShipConfig(_config)), ans);
            return mesData;
        }
    }

    private MessageDialogData repairResult()
    {
        if (SkillWork(4, RepairLevel))
        {
//            _reputation.AddReputation(_config, 10);
            var ans = new List<AnswerDialogData>()
            {
                new AnswerDialogData(Namings.Tag("Ok"))
            };
            MessageDialogData mesData;
            if (MainController.Instance.MainPlayer.Inventory.GetFreeWeaponSlot(out var slot))
            {
                var modul = Library.CreateDamageWeapon(false);
                mesData = new MessageDialogData(Namings.Format(Namings.DialogTag("repairResultFull"), Namings.Weapon(modul.WeaponType)), ans);
                MainController.Instance.MainPlayer.Inventory.TryAddWeaponModul(modul, slot);
            }
            else
            {
                mesData = new MessageDialogData(Namings.DialogTag("repairResultCant"), ans);
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
            x.Ship.WeaponsModuls.GetNonNullActiveSlots().FirstOrDefault(y => (y.WeaponType == WeaponType.rocket || y.WeaponType == WeaponType.casset)) != null);
        if (rockectWeapons.Any())
        {
            var ans = new List<AnswerDialogData>()
            {
                new     AnswerDialogData(Namings.Tag("Ok"))
            };
            var mesData = new MessageDialogData(Namings.DialogTag("asteroid_weaponsFire"), ans);
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
                    new     AnswerDialogData(Namings.Tag("Ok"))
                };
                mesData = new MessageDialogData(Namings.DialogTag("asteroid_haveWay"), ans);
            }
            else
            {

                mesData = new MessageDialogData(Namings.DialogTag("asteroid_weaponsFail"), StandartOptions());
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

