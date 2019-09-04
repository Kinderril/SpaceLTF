using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//public enum AfterbattleType
//{
//
//}

[System.Serializable]
public class PlayerAfterBattleOptions 
{
//    private   List<AfterbattleType> _lastUsed = new List<AfterbattleType>();
    private List<Func<float,MessageDialogData>> _posibleOptionsList = new List<Func<float, MessageDialogData>>();
    private List<Func<float,MessageDialogData>> _posibleOptionsListTmp = new List<Func<float, MessageDialogData>>();

//    private Dictionary<Func<float,MessageDialogData>,float> _posibleOptions = new Dictionary<Func<float, MessageDialogData>, float>();
//    private Dictionary<Func<float,MessageDialogData>,float> _posibleOptionsTmp = new Dictionary<Func<float, MessageDialogData>, float>();
    private int _lastStepGetDialog;
    private const int DialogFrequancy = 3;


    public PlayerAfterBattleOptions() {
        _posibleOptionsList.Add(Federation);
        _posibleOptionsList.Add(Krions);
        _posibleOptionsList.Add(Raides);
        _posibleOptionsList.Add(Mercenaries);
        _posibleOptionsList.Add(Ocrons);
        RefreshPosibleList();
//        _posibleOptions.Add(Federation,0f);
//        _posibleOptions.Add(Krions,0);
//        _posibleOptions.Add(Raides,0f);
//        _posibleOptions.Add(Mercenaries,0f);
//        _posibleOptions.Add(Ocrons,0f);
    }

    private void RefreshPosibleList()
    {
        _posibleOptionsListTmp.Clear();
        foreach (var posibleOption in _posibleOptionsList)
        {
            _posibleOptionsListTmp.Add(posibleOption);
        }
    }

    public MessageDialogData GetDialog(int step,float cellPower)
    {
        var delta = step - _lastStepGetDialog;
        if (SkillWork(DialogFrequancy, delta))
        {
            if (_posibleOptionsListTmp.Count == 0)
            {
                RefreshPosibleList();
            }

            var option = _posibleOptionsListTmp.RandomElement();
            _posibleOptionsListTmp.Remove(option);
            _lastStepGetDialog = step;
            return option(cellPower);

//               WDictionary<Func<float, MessageDialogData>> _wdic = new WDictionary<Func<float, MessageDialogData>>(_posibleOptions);
//               var option = _wdic.Random();
//               var curVal = _posibleOptions[option];
//               _posibleOptions[option] = curVal - 1f;
//               return option(cellPower);
        }

        return null;

    }

    private MessageDialogData Raides(float power)
    {
        var player = MainController.Instance.MainPlayer;
        if (!MyExtensions.IsTrue01(0.4f))
        {
            var masinMsg = "Coordinates of some fleets open";
            var ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData(Namings.Ok, OpenCellsByRaiders));
            var mesData = new MessageDialogData(masinMsg, ans);
            return mesData;
        }
        else
        {
            OpenCellsByRaiders();
            var masinMsg = "After battle you find a prisoner. With a broken ship.";
            var ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData("Leave him.", LeavehimAction));
            ans.Add(new AnswerDialogData(String.Format("Try to repair his ship and leave him. [Repair:{0}]", player.Parameters.Repair.Level), LeavehimActionRepair));
            if (MainController.Instance.MainPlayer.CanAddShip())
            {
                ans.Add(new AnswerDialogData("Hire him.", () => HireAction()));
            }
            var mesData = new MessageDialogData(masinMsg, ans);
            return mesData;
        }
    }
    protected bool SkillWork(int baseVal, int skillVal)
    {
        WDictionary<bool> wd = new WDictionary<bool>(new Dictionary<bool, float>()
        {
            {true,skillVal },
            {false,baseVal},
        });
        return wd.Random();
    }

    private void LeavehimActionRepair()
    {
        var player = MainController.Instance.MainPlayer;
        if (SkillWork(2, player.Parameters.Repair.Level))
        {
            MainController.Instance.MainPlayer.ReputationData.AddReputation(Library.REPUTATION_REPAIR_ADD);
            WindowManager.Instance.InfoWindow.Init(null, "Completed!");
        }
        else
        {
            MainController.Instance.MainPlayer.ReputationData.RemoveReputation(Library.REPUTATION_REPAIR_REMOVE);
            WindowManager.Instance.InfoWindow.Init(null, "His ship broken totaly. You lose reputation.");
        }
    }

    private MessageDialogData Mercenaries(float power)
    {
        if (!MyExtensions.IsTrue01(0.4f))
        {
            var masinMsg = "Coordinates of some fleets open";
            var ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData(Namings.Ok, OpenCellsByMercenaries));
            var mesData = new MessageDialogData(masinMsg, ans);
            return mesData;
        }
        else
        {
            OpenCellsByMercenaries();
            var masinMsg = "After battle you find one of opponents army. With a ship.";
            var ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData("Leave him.", LeavehimAction));
            ans.Add(new AnswerDialogData("Kill him.",
                () =>
                {
                    KillAction(power);
                }
            )
            );
            int hireMoney = 20;
            if (MainController.Instance.MainPlayer.CanAddShip())
            {
                ans.Add(new AnswerDialogData(String.Format("Hire him. {0} credits", hireMoney), () => HireAction(null, hireMoney)));
            }
            var mesData = new MessageDialogData(masinMsg, ans);
            return mesData;
        }
    }

    private void LeavehimAction()
    {
        MainController.Instance.MainPlayer.ReputationData.AddReputation(Library.REPUTATION_SCIENS_LAB_ADD);
    }

    private MessageDialogData Ocrons(float power)
    {
        if (!MyExtensions.IsTrue01(0.15f))
        {
            var masinMsg = "Coordinates of some fleets open";
            var ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData(Namings.Ok, OpenCellsByOcrons));
            var mesData = new MessageDialogData(masinMsg, ans);
            return mesData;
        }
        else
        {
            OpenCellsByOcrons();
            var masinMsg = "After battle you find one of opponents army.";
            var ans = new List<AnswerDialogData>();
            //        var myPower = _power;
            int teachMoney = (int)MyExtensions.Random(power / 3, power);
            ans.Add(new AnswerDialogData($"Ask to teach pilots. [Credits:{teachMoney}]", () =>
            {
                TeachPilots(teachMoney);
            }));
            ans.Add(new AnswerDialogData("Search for credits.", SearchFor));
            ans.Add(new AnswerDialogData("Leave him.", LeavehimAction));
            var mesData = new MessageDialogData(masinMsg, ans);
            return mesData;
        }
    }

    private MessageDialogData Krions(float power)
    {
        if (!MyExtensions.IsTrue01(0.15f))
        {
            var masinMsg = "Coordinates of some fleets open";
            var ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData(Namings.Ok, OpenCellsByKrions));
            var mesData = new MessageDialogData(masinMsg, ans);
            return mesData;
        }
        else
        {
            OpenCellsByKrions();
            var masinMsg = "After battle you find one of opponents army.";
            var ans = new List<AnswerDialogData>();
            var myPower = power / 2f;
            int moneyCost = (int)MyExtensions.Random(myPower / 3, myPower);
            ans.Add(new AnswerDialogData($"Ask for galaxy maps. [Credits:{moneyCost}]", () =>
            {
                OpenGloalMap(moneyCost);
            }));
            ans.Add(new AnswerDialogData("Search for credits.", SearchFor));
            ans.Add(new AnswerDialogData("Leave him.", LeavehimAction));
            var mesData = new MessageDialogData(masinMsg, ans);
            return mesData;
        }
    }

    private void OpenGloalMap(int moneyCost)
    {
        var player = MainController.Instance.MainPlayer;
        if (player.MoneyData.HaveMoney(moneyCost))
        {
            int planentToOpen = MyExtensions.Random(3, 6);
            var sector = player.MapData.GalaxyData;
            for (int i = 0; i < planentToOpen; i++)
            {
                var rnd = sector.GetRandomCell();
                rnd.OpenInfo();
            }
        }
        else
        {
            WindowManager.Instance.NotEnoughtMoney(moneyCost);
        }
    }

    private void TeachPilots(int moneyCost)
    {
        var player = MainController.Instance.MainPlayer;
        if (player.MoneyData.HaveMoney(moneyCost))
        {
            foreach (var shipPilotData in player.Army)
            {
                if (shipPilotData.Ship.ShipType != ShipType.Base)
                {
                    shipPilotData.Pilot.UpgradeRandomLevel(false,true);
                }
            }
        }
        else
        {
            WindowManager.Instance.NotEnoughtMoney(moneyCost);
        }
    }


    private MessageDialogData Federation(float power)
    {
        if (!MyExtensions.IsTrue01(0.15f))
        {
            var masinMsg = Namings.OpenCoordinates; 
            var ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData(Namings.Ok, OpenCellsByFederation));
            var mesData = new MessageDialogData(masinMsg, ans);
            return mesData;
        }
        else
        {
            OpenCellsByFederation();
            var masinMsg = "After battle you find one of opponents army.";
            var ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData("Ask buyout.", Buyout));
            ans.Add(new AnswerDialogData("Search for hidden things.", SearchFor));
            int hireMoney = 100;
            if (MainController.Instance.MainPlayer.CanAddShip())
            {
                ans.Add(new AnswerDialogData(String.Format("Hire him. {0} credits", hireMoney), () => HireAction(null, hireMoney)));
            }
            ans.Add(new AnswerDialogData("Leave him.", LeavehimAction));
            var mesData = new MessageDialogData(masinMsg, ans);
            return mesData;
        }
    }

    #region OpenCells
    private void OpenCellsByRaiders()
    {
        var player = MainController.Instance.MainPlayer;
        ShipConfig config = MyExtensions.IsTrue01(.5f) ? ShipConfig.federation : ShipConfig.mercenary;
        GlobalMapCell cell = player.MapData.GalaxyData.GetRandomClosestCellWithNoData(config, player.MapData.CurrentCell.indX, player.MapData.CurrentCell.indZ);
        if (cell != null)
            cell.Scouted();
    }
    private void OpenCellsByMercenaries()
    {
        var player = MainController.Instance.MainPlayer;
        ShipConfig config = MyExtensions.IsTrue01(.5f) ? ShipConfig.federation : ShipConfig.raiders;
        GlobalMapCell cell = player.MapData.GalaxyData.GetRandomClosestCellWithNoData(config, player.MapData.CurrentCell.indX, player.MapData.CurrentCell.indZ);
        if (cell != null)
            cell.Scouted();
    }
    private void OpenCellsByKrions()
    {
        var player = MainController.Instance.MainPlayer;
        ShipConfig config = MyExtensions.IsTrue01(.5f) ? ShipConfig.federation : ShipConfig.ocrons;
        GlobalMapCell cell = player.MapData.GalaxyData.GetRandomClosestCellWithNoData(config, player.MapData.CurrentCell.indX, player.MapData.CurrentCell.indZ);
        if (cell != null)
            cell.Scouted();
    }
    private void OpenCellsByOcrons()
    {
        var player = MainController.Instance.MainPlayer;
        GlobalMapCell cell;
        if (MyExtensions.IsTrue01(.5f))
        {
            cell = player.MapData.GalaxyData.GetRandomClosestCellWithNoData(ShipConfig.krios, player.MapData.CurrentCell.indX, player.MapData.CurrentCell.indZ);
        }
        else
        {
            cell = player.MapData.GalaxyData.GetRandomConnectedCell();
        }
        if (cell != null)
            cell.Scouted();
    }
    private void OpenCellsByFederation()
    {
        var player = MainController.Instance.MainPlayer;
        GlobalMapCell cell;

        if (MyExtensions.IsTrue01(.5f))
        {
            cell = player.MapData.GalaxyData.GetRandomClosestCellWithNoData(ShipConfig.krios, player.MapData.CurrentCell.indX, player.MapData.CurrentCell.indZ);
        }
        else
        {
            cell = player.MapData.GalaxyData.GetRandomConnectedCell();
        }
        if (cell != null)
            cell.Scouted();
    }


    #endregion


    #region RandomActions
    private void Buyout()
    {
        WDictionary<bool> ws = new WDictionary<bool>(new Dictionary<bool, float>()
        {
            {true, MainController.Instance.MainPlayer.Parameters.Diplomaty.Level}, {false, 2},
        });
        if (ws.Random())
        {
            MainController.Instance.MainPlayer.ReputationData.RemoveReputation(Library.REPUTATION_STEAL_REMOVE);
            int monet = MyExtensions.Random(25, 35);
            MainController.Instance.MainPlayer.MoneyData.AddMoney(monet);
            WindowManager.Instance.InfoWindow.Init(null, String.Format("Buyout confirm. {0}", monet));
        }
        else
        {
            WindowManager.Instance.InfoWindow.Init(null, String.Format("Buyout fail."));
        }
    }
    private void SearchFor()
    {
        WDictionary<bool> ws = new WDictionary<bool>(new Dictionary<bool, float>()
        {
            {true, MainController.Instance.MainPlayer.Parameters.Scouts.Level}, {false, 2},
        });
        if (ws.Random())
        {
            int monet = MyExtensions.Random(15, 35);
            MainController.Instance.MainPlayer.MoneyData.AddMoney(monet);
            WindowManager.Instance.InfoWindow.Init(null, String.Format("Credits add: {0}.", monet));
            MainController.Instance.MainPlayer.ReputationData.RemoveReputation(Library.REPUTATION_STEAL_REMOVE);
        }
        else
        {
            WindowManager.Instance.InfoWindow.Init(null, String.Format("Nothing."));
        }
    }
    private void KillAction(float power)
    {
        MainController.Instance.MainPlayer.ReputationData.RemoveReputation(Library.REPUTATION_ATTACK_PEACEFULL_REMOVE);
        if (MyExtensions.IsTrue01(MainController.Instance.MainPlayer.Parameters.Scouts.Level / 4f))
        {
            int monet = (int)MyExtensions.Random(power / 5f, power * 1.5f);
            MainController.Instance.MainPlayer.MoneyData.AddMoney(monet);
            WindowManager.Instance.InfoWindow.Init(null, String.Format("Money doesn't smell. {0}", monet));
        }
        else
        {
            WindowManager.Instance.InfoWindow.Init(null, "He running away.");
        }
    }
    private void HireAction(ShipConfig? config = null, int moneyCost = 0)
    {
        var pilot = Library.CreateDebugPilot();
        WDictionary<ShipType> types = new WDictionary<ShipType>(new Dictionary<ShipType, float>()
        {
            {ShipType.Heavy, 2}, {ShipType.Light, 2}, {ShipType.Middle, 2},
        });

        var configsD = new Dictionary<ShipConfig, float>();
        switch (config)
        {
            case ShipConfig.raiders:
                configsD.Add(ShipConfig.mercenary, 1);
                configsD.Add(ShipConfig.raiders, 2);
                break;
            case ShipConfig.mercenary:
                configsD.Add(ShipConfig.raiders, 1);
                configsD.Add(ShipConfig.mercenary, 5);
                break;
            case ShipConfig.federation:
                configsD.Add(ShipConfig.mercenary, 2);
                configsD.Add(ShipConfig.krios, 2);
                configsD.Add(ShipConfig.ocrons, 2);
                break;
            case ShipConfig.ocrons:
                configsD.Add(ShipConfig.federation, 2);
                configsD.Add(ShipConfig.krios, 2);
                break;
            case ShipConfig.krios:
                configsD.Add(ShipConfig.federation, 2);
                configsD.Add(ShipConfig.ocrons, 2);
                break;
        }

        WDictionary<ShipConfig> configs = new WDictionary<ShipConfig>(configsD);

        if (MainController.Instance.MainPlayer.MoneyData.HaveMoney(moneyCost))
        {
            MainController.Instance.MainPlayer.MoneyData.RemoveMoney(moneyCost);
            var type = types.Random();
            var cng = config.HasValue ? config.Value : configs.Random();
            var ship = Library.CreateShip(type, cng, MainController.Instance.MainPlayer);
            WindowManager.Instance.InfoWindow.Init(null, String.Format("You hired a new pilot. Type:{0}  Config:{1}", Namings.ShipConfig(cng), Namings.ShipType(type)));
            int itemsCount = MyExtensions.Random(1, 2);
            for (int i = 0; i < itemsCount; i++)
            {
                if (ship.GetFreeWeaponSlot(out var inex))
                {
                    var weapon = Library.CreateWeapon(true);
                    ship.TryAddWeaponModul(weapon, inex);
                }
            }
            MainController.Instance.MainPlayer.TryHireShip(new StartShipPilotData(pilot, ship));
        }
        else
        {
            WindowManager.Instance.NotEnoughtMoney(moneyCost);
        }
    }

    #endregion


}
