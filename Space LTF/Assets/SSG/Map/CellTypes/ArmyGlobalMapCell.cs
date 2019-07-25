using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[System.Serializable]
public class ArmyGlobalMapCell : GlobalMapCell
{
    public float HIRE_CHANCE = 0.01f;
    protected ArmyCreatorType _armyType;
    private ShipConfig _config;
    private bool canHire = false;

    public int Power
    {
        get { return _power; }
    }

    protected int _power;

    public Player GetArmy()
    {
        ArmyCreatorData data = new ArmyCreatorData(_config, true);
        switch (_armyType)
        {
            case ArmyCreatorType.rocket:
                data = new ArmyCreatorRocket(_config, true);
                break;
            case ArmyCreatorType.laser:
                data = new ArmyCreatorLaser(_config, true);
                break;
            case ArmyCreatorType.mine:
                data = new ArmyCreatorAOE(_config, true);
                break;
            case ArmyCreatorType.destroy:
                data = new ArmyCreatorDestroy(_config, true);
                break;
        }
        //        var data = new ArmyCreatorRocket(ShipConfig.mercenary);
        var player = new Player(name);
        float coreShipChance = 0;
        switch (_config)
        {
            case ShipConfig.droid:
                coreShipChance = 0f;
                break;  
            case ShipConfig.raiders:
                coreShipChance = 0.1f;
                break;
            case ShipConfig.federation:
                coreShipChance = 0.9f;
                break;
            case ShipConfig.mercenary:
                coreShipChance = 0.5f;
                break;
        }
        bool withCore = MyExtensions.IsTrue01(coreShipChance);
        var army = ArmyCreator.CreateSimpleEnemyArmy(_power,  data,  player);
        player.Army = army;
        switch (_config)
        {
            case ShipConfig.raiders:
            case ShipConfig.mercenary:
                canHire = MyExtensions.IsTrue01(HIRE_CHANCE);
                break;
        }
        return player;
    }

    public override bool CanCellDestroy()
    {
        return true;
    }

    public ArmyGlobalMapCell(int power, ShipConfig config, int id, ArmyCreatorType type, int Xind, int Zind) : base(id, Xind, Zind)
    {
        _config = config;
        _armyType = type;
        _power = power;
    }


    public override MessageDialogData GetDialog()
    {
        bool isSameSide = MainController.Instance.MainPlayer.MainShip.Ship.ShipConfig == _config;
        string masinMsg;

        var ans = new List<AnswerDialogData>();
        var rep = MainController.Instance.MainPlayer.ReputationData.Reputation;
        if (isSameSide && rep > 40)
        {
            masinMsg = String.Format("This fleet looks friendly. [Power:{0}]", Power);
            ans.Add(new AnswerDialogData(String.Format("Ask for help. [Reputation:{0}]", rep), DimlomatyOption));
        }
        else
        {
            masinMsg = String.Format("You see enemies. Shall we fight? [Power:{0}]", Power);
        }

        ans.Add(new AnswerDialogData("Fight", Take));

        if (isSameSide)
        {
            ans.Add(new AnswerDialogData("Leave.", null));
        }
        else
        {
            ans.Add(new AnswerDialogData(
                String.Format("Try Run. [Scouts:{0}]", ScoutsLevel),
                () =>
                {
                    bool doRun = MyExtensions.IsTrue01((float)ScoutsLevel / 4f);
#if UNITY_EDITOR
                    doRun = true;
#endif
                    if (doRun)
                    {
                        WindowManager.Instance.InfoWindow.Init(null, "Running away complete.");
                    }
                    else
                    {
                        WindowManager.Instance.InfoWindow.Init(Take, "Fail! Now you must fight.");
                    }
                }));
        }
        var mesData = new MessageDialogData(masinMsg, ans);
        return mesData;
    }

    private void DimlomatyOption()
    {
        var rep = MainController.Instance.MainPlayer.ReputationData.Reputation;
        bool doDip = MyExtensions.IsTrue01((float)rep / PlayerReputationData.MAX_REP);
        if (doDip)
        {
            int a = 0;
            switch (_config)
            {
                case ShipConfig.droid:
                case ShipConfig.raiders:
                case ShipConfig.mercenary:
                    a = 0;
                    break;
                case ShipConfig.federation:
                case ShipConfig.ocrons:
                case ShipConfig.krios:
                    a = 1;
                    break;
            }
            string helpInfo;
            if (a == 0)
            {
                var money = AddMoney(_power/3, _power/2);
                helpInfo = String.Format("They give you some credits {0}", money);
            }
            else
            {
                var player = MainController.Instance.MainPlayer;
//                IItemInv item;
                bool canAdd;
                string itemName;
                int slot;
                if (MyExtensions.IsTrue01(.5f))
                {
                    var m = Library.CreatSimpleModul(2);
                    itemName = Namings.SimpleModulName(m.Type);
                    canAdd = player.Inventory.GetFreeSimpleSlot(out slot);
                    if (canAdd)
                    {
                        player.Inventory.TryAddSimpleModul(m, slot);
                    }
                }
                else
                {
                    var w = Library.CreateWeapon(false);
                    itemName = Namings.Weapon(w.WeaponType);
                    canAdd = player.Inventory.GetFreeWeaponSlot(out slot);
                    if (canAdd)
                    {
                        player.Inventory.TryAddWeaponModul(w, slot);
                    }
                }
                if (canAdd)
                {
                    helpInfo = String.Format("They give {0}.", itemName);
                }
                else
                {
                    helpInfo = "But you have no free space";
                }
            }
            var pr = String.Format("They help you as much as they can. {0}", helpInfo);
            WindowManager.Instance.InfoWindow.Init(null, pr);
        }
        else
        {
            WindowManager.Instance.InfoWindow.Init(null, "They can't help you now");
        }
    }

    public override Color Color()
    {
        return new Color(255f/255f, 102f/255f, 0f/255f);
    }

    public override bool OneTimeUsed()
    {
        return true;
    }

    public override string Desc()
    {
        return $"Amry:{Namings.ShipConfig(_config)} ";
    }

    public override void Take()
    {
        MainController.Instance.PreBattle(MainController.Instance.MainPlayer, GetArmy());
    }

    protected override MessageDialogData GetLeavedActionInner()
    {
        switch (_config)
        {
            case ShipConfig.raiders:
                return Raides();
            case ShipConfig.federation:
                return Federation();
            case ShipConfig.mercenary:
                return Mercenaries();
            case ShipConfig.ocrons:
                return Ocrons();
            case ShipConfig.krios:
                return Krions();
        }

        return null;
    }

    private MessageDialogData Raides()
    {
        if (!MyExtensions.IsTrue01(0.4f))
        {
            var masinMsg = "Coordinates of some fleets open";
            var ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData("Ok",OpenCellsByRaiders));
            var mesData = new MessageDialogData(masinMsg, ans);
            return mesData;
        }
        else
        {
            OpenCellsByRaiders();
            var masinMsg = "After battle you find a prisoner. With a broken ship.";
            var ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData("Leave him.", LeavehimAction));
            ans.Add(new AnswerDialogData(String.Format("Try to repair his ship and leave him. [Repair:{0}]", RepairLevel), LeavehimActionRepair));
            if (MainController.Instance.MainPlayer.CanAddShip())
            {
                ans.Add(new AnswerDialogData("Hire him.", () => HireAction()));
            }
            var mesData = new MessageDialogData(masinMsg, ans);
            return mesData;
        }
    }


    private void LeavehimActionRepair()
    {
        if (SkillWork(2, RepairLevel))
        {
            MainController.Instance.MainPlayer.ReputationData.AddReputation(Library.REPUTATION_REPAIR_ADD);
            WindowManager.Instance.InfoWindow.Init(null, "Completed!");
        }
        else
        {
            MainController.Instance.MainPlayer.ReputationData.RemoveReputation(Library.REPUTATION_REPAIR_REMOVE);
            WindowManager.Instance.InfoWindow.Init(null,"His ship broken totaly. You lose reputation.");
        }
    }

    private MessageDialogData Mercenaries()
    {
        if (!MyExtensions.IsTrue01(0.4f))
        {
            var masinMsg = "Coordinates of some fleets open";
            var ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData("Ok", OpenCellsByMercenaries));
            var mesData = new MessageDialogData(masinMsg, ans);
            return mesData;
        }
        else
        {
            OpenCellsByMercenaries();
            var masinMsg = "After battle you find one of opponents army. With a ship.";
            var ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData("Leave him.", LeavehimAction));
            ans.Add(new AnswerDialogData("Kill him.", KillAction));
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

    private MessageDialogData Ocrons()
    {
        if (!MyExtensions.IsTrue01(0.15f))
        {
            var masinMsg = "Coordinates of some fleets open";
            var ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData("Ok", OpenCellsByOcrons));
            var mesData = new MessageDialogData(masinMsg, ans);
            return mesData;
        }
        else
        {
            OpenCellsByOcrons();
            var masinMsg = "After battle you find one of opponents army.";
            var ans = new List<AnswerDialogData>();
    //        var myPower = _power;
            int teachMoney = MyExtensions.Random(_power/3, _power);
            ans.Add(new AnswerDialogData($"Ask to teach pilots. [Credits:{teachMoney}]", ()=>
            {
                TeachPilots(teachMoney);
            }));
            ans.Add(new AnswerDialogData("Search for credits.", SearchFor));
            ans.Add(new AnswerDialogData("Leave him.",LeavehimAction));
            var mesData = new MessageDialogData(masinMsg, ans);
            return mesData;
        }
    }

    private MessageDialogData Krions()
    {
        if (!MyExtensions.IsTrue01(0.15f))
        {
            var masinMsg = "Coordinates of some fleets open";
            var ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData("Ok", OpenCellsByKrions));
            var mesData = new MessageDialogData(masinMsg, ans);
            return mesData;
        }
        else
        {
            OpenCellsByKrions();
            var masinMsg = "After battle you find one of opponents army.";
            var ans = new List<AnswerDialogData>();
            var myPower = _power / 2f;
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
                    shipPilotData.Pilot.UpgradeRandomLevel(false);
                }
            }
        }
        else
        {
            WindowManager.Instance.NotEnoughtMoney(moneyCost);
        }
    }


    private void OpenCellsByRaiders()
    {
        var player = MainController.Instance.MainPlayer;
        ShipConfig config = MyExtensions.IsTrue01(.5f) ? ShipConfig.federation : ShipConfig.mercenary;
        GlobalMapCell cell = player.MapData.GalaxyData.GetRandomClosestCellWithNoData(config, indX, indZ);
        if (cell != null)
            cell.Scouted();
    }

    private void OpenCellsByMercenaries()
    {
        var player = MainController.Instance.MainPlayer;
        ShipConfig config = MyExtensions.IsTrue01(.5f) ? ShipConfig.federation : ShipConfig.raiders;
        GlobalMapCell cell = player.MapData.GalaxyData.GetRandomClosestCellWithNoData(config, indX, indZ);
        if (cell != null)
            cell.Scouted();
    }

    private void OpenCellsByKrions()
    {
        var player = MainController.Instance.MainPlayer;
        ShipConfig config = MyExtensions.IsTrue01(.5f) ? ShipConfig.federation : ShipConfig.ocrons;
        GlobalMapCell cell = player.MapData.GalaxyData.GetRandomClosestCellWithNoData(config, indX, indZ);
        if (cell != null)
            cell.Scouted();
    }
    private void OpenCellsByOcrons()
    {
        var player = MainController.Instance.MainPlayer;
        GlobalMapCell cell;
        if (MyExtensions.IsTrue01(.5f))
        {
            cell = player.MapData.GalaxyData.GetRandomClosestCellWithNoData(ShipConfig.krios, indX, indZ);
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
            cell = player.MapData.GalaxyData.GetRandomClosestCellWithNoData(ShipConfig.krios, indX, indZ);
        }
        else
        {
            cell = player.MapData.GalaxyData.GetRandomConnectedCell();
        }
        if (cell != null)
            cell.Scouted();
    }

    private MessageDialogData Federation()
    {
        if (!MyExtensions.IsTrue01(0.15f))
        {
            var masinMsg = "Coordinates of some fleets open";
            var ans = new List<AnswerDialogData>();
            ans.Add(new AnswerDialogData("Ok", OpenCellsByFederation));
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

    private void KillAction()
    {
        MainController.Instance.MainPlayer.ReputationData.RemoveReputation(Library.REPUTATION_ATTACK_PEACEFULL_REMOVE);
        if (MyExtensions.IsTrue01(MainController.Instance.MainPlayer.Parameters.Scouts.Level/4f))
        {
            int monet = (int)MyExtensions.Random(_power/5f, _power*1.5f);
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
        switch (_config)
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
            MainController.Instance.MainPlayer.TryHireShip(new StartShipPilotData(pilot, ship));
        }
        else
        {
            WindowManager.Instance.NotEnoughtMoney(moneyCost);
        }
    }

    public ShipConfig GetConfig()
    {
        return _config ;
    }
}

