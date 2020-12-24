using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class ArmyBornCenterGlobalCell : GlobalMapCell
{
    private int _lastStepBornArmy = -100;
    private bool _shallTryBornNextStep = false;
    public bool FightComplete { get; private set; }     
    private int _nextBornStep = 15;
    private const int _minBornStep = 15;
//    private int _nextBornStep = 15;
    public ArmyBornCenterGlobalCell(int id, int iX, int iZ, SectorData sector, ShipConfig config) : base(id, iX, iZ, sector, config)
    {
        _nextBornStep = _minBornStep + MyExtensions.Random(1, 15);
    }

    public override string Desc()
    {
        return Namings.Tag("ArmyBornCenterGlobalCell");
    }

    public override void Take()
    { 
        //FREE
    }

    public override void UpdateStep(int step)
    {
        if (_sector.IsSectorMy)
        {
            if (_shallTryBornNextStep)
            {
                if (TryBorn(true))
                {
                    _shallTryBornNextStep = false;
                }
            }
        }
        else
        {
            if (_nextBornStep <= step)
            {
                if (TryBorn(false))
                {
                    _nextBornStep = step + MyExtensions.Random(9, 19);
                }
            }
        }
    }

    private bool TryBorn(bool isAllies)
    {
        var player = MainController.Instance.MainPlayer;
        var army = player.MapData.GalaxyData.GalaxyEnemiesArmyController.BornArmyAtCell(this, isAllies);
        if (army != null)
        {
            return true;
        }
        return false;
    }

    protected override MessageDialogData GetDialog()
    {
        string masinMsg;
        var ans = new List<AnswerDialogData>();
        //        ans.Add(new AnswerDialogData(Namings.Format(Namings.DialogTag("dungeogArmyFriendGoIn"), rep), null, null));
        if (_sector.IsSectorMy)
        {
            return DialogAsMy();

        }
        else
        {
            if (FightComplete)
            {

                masinMsg = Namings.Format(Namings.Tag("armyBornCenterComplete"));
//                ans.Add(new AnswerDialogData(Namings.Tag("leave"), null, null, false, true));
            }
            else
            {
                masinMsg = Namings.Format(Namings.Tag("armyBornCenterEnemy"));
                ans.Add(new AnswerDialogData(Namings.DialogTag("Attack"), TryAttack));
//                ans.Add(new AnswerDialogData(Namings.Tag("leave"), null, null, false, true));
            }

        }
        ans.Add(new AnswerDialogData(Namings.Tag("leave"), DoUncomplete, null, false));
        var mesData = new MessageDialogData(masinMsg, ans);
        return mesData;
    }

    private MessageDialogData DialogAsMy()
    {
        string masinMsg;

        var ans = new List<AnswerDialogData>();
        var player = MainController.Instance.MainPlayer;
        var cost = (int)(player.Army.GetPower() * 2f);
        masinMsg = Namings.Format(Namings.Tag("armyBornCenterMy"));
        ans.Add(new AnswerDialogData(Namings.Format(Namings.Tag("armyBornCenterRequestArmy"), cost), null, () =>
        {
            return TryRequest(cost);
        }));
        var freeCells = (Sector.ListCells.Where(x => x.Data is FreeActionGlobalMapCell)).ToList();
        if (freeCells.Count > 0)
        {

            var rndFreeCell = freeCells.RandomElement();
            ans.Add(new AnswerDialogData(Namings.Format(Namings.Tag("armyBornCenterRequestMercenary"), cost), null, () =>
            {
                return TryRequestBuildMerc(cost, rndFreeCell);
            }));  
            ans.Add(new AnswerDialogData(Namings.Format(Namings.Tag("armyBornCenterRequestShop"), cost), null, () =>
            {
                return TryRequestBuildShop(cost, rndFreeCell);
            }));

        }
        ans.Add(new AnswerDialogData(Namings.Tag("leave"), null, null, false));
        var mesData = new MessageDialogData(masinMsg, ans);
        return mesData;
    }


    private MessageDialogData TryRequestBuildMerc(int cost, SectorCellContainer rndFreeCell)
    {

        return TryRequestSmt(cost, BuildMerc(rndFreeCell), "armyBornCenterMercBuilded");
    }

    private Action BuildMerc(SectorCellContainer rndFreeCell)
    {
        var cell = rndFreeCell;
        void Act()
        {
            var player = MainController.Instance.MainPlayer;
            var newShop = new EventGlobalMapCell(GlobalMapEventType.mercHideout, Utils.GetId(), cell.indX, cell.indZ, _sector, (int)player.Army.GetPower(),_sector.ShipConfig);
            cell.SetData(newShop);
        }
        return Act;
    }

    private MessageDialogData TryRequestBuildShop(int cost, SectorCellContainer rndFreeCell)
    {

        return TryRequestSmt(cost,BuildShop(rndFreeCell), "armyBornCenterShopBuilded");
    }

    private Action BuildShop(SectorCellContainer rndFreeCell)
    {
        var cell = rndFreeCell;
        void Act()
        {
            var cell1 = cell;
            var player = MainController.Instance.MainPlayer;
            var newShop = new ShopGlobalMapCell(player.Army.GetPower(), Utils.GetId(), cell1.Data.indX, cell1.Data.indZ, _sector, _sector.ShipConfig);
            cell1.SetData(newShop);
        }
        return Act;
    }

    private void DoUncomplete()
    {
        Uncomplete();
    }

    private MessageDialogData TryRequest(int cost)
    {
        return TryRequestSmt(cost, () => { _shallTryBornNextStep = true; }, "armyBornCenterFleetRequested");
    }  

    private MessageDialogData TryRequestSmt(int cost,Action doRequest,string mainMsgKey)
    {
        var ans = new List<AnswerDialogData>();
        string masinMsg;

        var player = MainController.Instance.MainPlayer;
        if (player.MoneyData.HaveMoney(cost))
        {
            player.MoneyData.RemoveMoney(cost);
            doRequest();
            masinMsg = Namings.Tag(mainMsgKey);
        }
        else
        {
            masinMsg = Namings.Tag("NotEnoughtMoney");
        }

        ans.Add(new AnswerDialogData(Namings.Tag("Ok")));
        var mesData = new MessageDialogData(masinMsg, ans);
        return mesData;
    }

    private void TryAttack()
    {
        var player = MainController.Instance.MainPlayer;
        var power = player.Army.GetPower() * 1.6f;
        ArmyCreatorData data = ArmyCreatorLibrary.GetArmy(ConfigOwner);
        var aiPlayer = new PlayerAIMilitaryFinal("baseDef");
        var army = ArmyCreator.CreateArmy(power, ArmyCreationMode.equalize, 3, 8, data, true, aiPlayer);
        aiPlayer.Army.SetArmy(army);
        MainController.Instance.PreBattle(player, aiPlayer, false,false);
        FightComplete = true;
    }

    public override Color Color()
    {
        return new Color(51f / 255f, 102f / 255f, 153f / 255f);
    }

    public override bool OneTimeUsed()
    {
        return false;
    }

    public override bool CanCellDestroy()
    {
        return false;
    }
}
