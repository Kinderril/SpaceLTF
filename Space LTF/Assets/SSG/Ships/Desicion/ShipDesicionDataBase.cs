using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public abstract class ShipDesicionDataBase : IShipDesicion
{
    private const bool PERCENT_CHOOSE = false;
    public const float _defDist = 12f;
    public const float POSIBLE_UNLOAD_WEAPONS = 2f;
    protected ShipBase _owner;
    protected ActionType _lastAction;
    public EGlobalTactics GlobalTactics { get; private set; }
    public ECommanderPriority1 CommanderPriority1 { get; private set; }
    public ESideAttack SideAttack { get; private set; }

    public ShipDesicionDataBase(ShipBase owner, PilotTactic tactic)
    {
        _owner = owner;
        SideAttack = tactic.SideAttack;
        CommanderPriority1 = tactic.Priority;
    }

    public static ShipDesicionDataBase Create(ShipBase ship, PilotTactic tactic)
    {

        return new ShipDesicionDataAttack(ship, tactic);
    }

    public BaseAction CalcAction()
    {
        var type = CalcTask(out var ship);
        var task = ActivateTask(type, ship);
        task.SetDesider(this);
        return task;
    }

    public void ChangePriority(ESideAttack SideAttack)
    {
        this.SideAttack = SideAttack;
        OnChagePriority?.Invoke();
        _owner.EndAction();
    }

    public void ChangePriority(EGlobalTactics globalTactics)
    {
        this.GlobalTactics = globalTactics;
        OnChagePriority?.Invoke();
        _owner.EndAction();
    }

    public void ChangePriority(ECommanderPriority1 CommanderPriority1)
    {
        this.CommanderPriority1 = CommanderPriority1;
        OnChagePriority?.Invoke();
        _owner.EndAction();
    }

    public event Action OnChagePriority;


    public ActionType FightTask(out ShipBase ship)
    {
        if (SideAttack == ESideAttack.BaseDefence)
        {
            if (HaveEnemyInDangerZoneDefenceBase(out ship))
            {
                var attack = DoAttackAction(ship);
                if (attack.HasValue)
                {
                    return attack.Value;
                }
            }
            else
            {
                ship = null;
                return ActionType.waitEnemy;
            }
        }
        else if (HaveEnemyInDangerZone(out ship))
        {
            var attack = DoAttackAction(ship);
            if (attack.HasValue)
            {
                return attack.Value;
            }
        }

        if (_owner.HaveClosestDamagedFriend(out ship))
        {
            if (_owner.WeaponsController.AnySupportWeaponIsLoaded(0f, out var fullLoadSupport))
            {
                if (ship != null)
                {
                    if (fullLoadSupport)
                    {
                        return ActionType.support;
                    }
                }
            }
        }

        return OptionalTask(out ship);
    }
    public ActionType CalcTask(out ShipBase ship)
    {
        if (IsOutOField())
        {
            ship = null;
            return ActionType.returnToBattle;
        }

        if (_owner.Enemies.Count == 0)
        {
            ship = null;
            return ActionType.waitEdnGame;
        }

        if (GlobalTactics == EGlobalTactics.Fight)
        {
            return FightTask(out ship);
        }

        if (_owner.HaveClosestDamagedFriend(out ship))
        {
            if (_owner.WeaponsController.AnySupportWeaponIsLoaded(0f, out var fullLoadSupport))
            {
                if (ship != null)
                {
                    if (fullLoadSupport)
                    {
                        return ActionType.support;
                    }
                }
            }
        }
        ship = null;
        return ActionType.evade;
    }


    public BaseAction ActivateTask(ActionType task, ShipBase target)
    {
        switch (task)
        {
            case ActionType.none:
                break;
            case ActionType.goToHide:
                return new GoToHideAction(_owner);
            case ActionType.readyToAttack:
                return new ReadyToAttackAction(_owner, _owner.Enemies[target]);
            case ActionType.shootFromPlace:
                return new StayAttackAction(_owner, _owner.Enemies[target]);
            case ActionType.attack:
                return new AttackAction(_owner, _owner.Enemies[target]);
            case ActionType.support:
                return new SupportAction(_owner, _owner.Commander.GetSameSide(target, _owner));
            // case ActionType.attackHalfLoop:
            //     return new AttackTrickAction(_owner, _owner.Enemies[target]);
            case ActionType.moveToBase:
                return (new GoToBaseAction(_owner, target, false));
            case ActionType.returnToBattle:
                return (new ReturnActionToBattlefield(_owner));
            case ActionType.closeStrikeAction:
                var mineCS = _owner.ShipModuls.Moduls.FirstOrDefault(x => x is CloseStrikeModul) as CloseStrikeModul;
                return (new CloseStrikeAction(_owner, target, mineCS));
            case ActionType.evade:
                return (new EvadeAction(_owner));
            case ActionType.repairAction:
                return (new RepairAction(_owner));
            case ActionType.afterAttack:
                return (new AfterAttackAction(_owner));
            case ActionType.waitHeal:
                return (new WaitHealAction(_owner, _owner.Commander.GetWaitPosition(_owner)));
            case ActionType.defence:
                return (new DefenceAction(_owner, target));
            case ActionType.mineField:
                var mineModul = _owner.ShipModuls.Moduls.FirstOrDefault(x => x is MineAbstractModul) as MineAbstractModul;
                var mainShip = _owner.Commander.MainShip;
                AICell cellToProtect = _owner.Cell;
                if (mainShip != null)
                {
                    cellToProtect = mainShip.Cell;
                }
                var closestEnemy = BattleController.Instance.ClosestShipToPos(_owner.Position,
                    BattleController.OppositeIndex(_owner.TeamIndex));
                Vector3 cellToCare;
                if (closestEnemy != null)
                {
                    cellToCare = closestEnemy.Position;
                }
                else
                {
                    cellToCare = _owner.Commander.StartMyPosition;
                }
                return (new MineFieldAction(_owner, mineModul, cellToProtect, cellToCare));
            case ActionType.attackSide:
                Vector3 controlPoint = AttackSideAction.FindControlPoint(_owner.Position, target.Position, _owner.Commander.Battlefield);
                return new AttackSideAction(_owner, _owner.Enemies[target], controlPoint);
            case ActionType.waitEnemy:
                var wait = new WaitEnemy(_owner);
                return (wait);
            case ActionType.goToCurrentPointAction:
                Debug.LogError("DEBUG ACTION. HOW IT HAPPENS? ");
                break;
            case ActionType.waitEdnGame:
                var endGame = new EndGameWaitAction(_owner);
                return endGame;
        }
        Debug.LogError("ZER task " + task + "   type:" + this.ToString());
        return null;
    }

    protected abstract ActionType? DoAttackAction(ShipBase ship);
    protected abstract ActionType OptionalTask(out ShipBase ship);
    protected abstract bool HaveEnemyInDangerZone(out ShipBase ship);
    public abstract bool HaveEnemyInDangerZoneDefenceBase(out ShipBase ship);
    public abstract string GetName();

    protected ActionType HideOrWait()
    {
        //TODO wait hide
        return ActionType.goToHide;
    }
    public abstract bool HaveClosestDamagedFriend(out ShipBase ship);

    protected ActionType? DoOrWait(ActionType defaultAction, ShipBase ship)
    {
        if (_owner.WeaponsController.AnyDamagedWeaponIsLoaded(POSIBLE_UNLOAD_WEAPONS, out var fullLoad))
        {
            if (ship != null && ship.VisibilityData.Visible)
            {
                if (fullLoad)
                {
                    return defaultAction;
                }
                else
                {
                    return ActionType.readyToAttack;
                }
            }
        }



        return null;
    }

    protected ActionType AttackOrAttackSide(ShipBase target)
    {
        // if (_owner.Boost.IsReady && !_owner.Boost.IsActive && _owner.Boost.BoostHalfLoop.CanUse)
        // {
        //     var enemy = _owner.Enemies[target];
        //     // var isBack = enemy.IsInBack();
        //     // if (isBack && enemy.Dist > 10)
        //     // {
        //     //     return ActionType.attackHalfLoop;
        //     // }
        // }

        switch (SideAttack)
        {
            case ESideAttack.Straight:
                return ActionType.attack;
            case ESideAttack.Flangs:
                return ActionType.attackSide;
        }

        return ActionType.attack;
    }

    protected bool IsCloseToBase()
    {
        var homeShip = _owner.Commander.MainShip;
        if (homeShip != null)
        {
            var dir = (homeShip.Position - _owner.Position);
            var sDist = dir.sqrMagnitude;
            if (sDist < GoToBaseAction.CloseToMainShipSQRT2)
            {
                return true;
            }
        }
        return false;
    }

    protected bool IsEnemySlower(ShipPersonalInfo enemy)
    {
        return enemy.ShipLink.ShipParameters.MaxSpeed < _owner.ShipParameters.MaxSpeed;
    }

    protected bool HaveEnemyClose(out ShipBase ship, float closeDist)
    {
        return HaveEnemyClose(out ship, _owner, closeDist);
    }

    protected bool HaveEnemyClose(out ShipBase ship, ShipBase testedShip, float closeDist)
    {
        foreach (var info in testedShip.Enemies)
        {
            if (info.Value.Dist < closeDist)
            {
                ship = info.Key;
                return true;
            }
        }
        ship = null;
        return false;
    }

    [CanBeNull]
    protected MineAbstractModul GetMineModul()
    {
        var m = _owner.ShipModuls.Moduls.FirstOrDefault(x => x is MineAbstractModul) as MineAbstractModul;
        if (m != null)
        {
            if (m.IsReady())
            {
                return m;
            }
        }
        return null;
    }

    protected bool IsOutOField()
    {
        return !_owner.InBattlefield;
    }

    protected bool IsUnderAttack(out ShipBase attacker)
    {
        foreach (var shipBase in _owner.Enemies)
        {
            var d = shipBase.Value.Dist;
            if (d > 4)
            {
                continue;
            }

            var enemy = shipBase.Key;

            if (enemy.Target != null && enemy.Target.ShipLink == _owner)
            {
                var isEnemyAtBack = Vector3.Dot(_owner.LookDirection, _owner.Position - enemy.Position) > 0;
                if (isEnemyAtBack)
                {
                    attacker = enemy;
                    return true;
                }
            }
        }
        attacker = null;
        return false;
    }

    [CanBeNull]
    public ShipPersonalInfo CalcBestEnemy(Dictionary<ShipBase, ShipPersonalInfo> posibleTargets)
    {
        return CalcBestEnemy(out var rating, posibleTargets);
    }

    [CanBeNull]
    protected ShipPersonalInfo CalcBestEnemy(out float rating, Dictionary<ShipBase, ShipPersonalInfo> posibleTargets)
    {
        //        if (GlobalTactics == EGlobalTactics.Fight && _owner.Commander.MainShip != null)
        //        {
        //            if (HaveEnemyClose(out var defSHip, _owner.Commander.MainShip, _defDist))
        //            {
        //                ShipPersonalInfo trg = posibleTargets[defSHip];
        //                rating = ShipValue(trg);
        //                return trg;
        //            }
        //        }

        switch (CommanderPriority1)
        {
            case ECommanderPriority1.MinShield:
            case ECommanderPriority1.MinHealth:
            case ECommanderPriority1.MaxShield:
            case ECommanderPriority1.MaxHealth:
            case ECommanderPriority1.Fast:
            case ECommanderPriority1.Slow:
                ShipPersonalInfo trg2 = BestParameter(posibleTargets, CommanderPriority1);
                if (trg2 != null)
                {
                    rating = ShipValue(trg2);
                    return trg2;
                }
                break;
            case ECommanderPriority1.Base:
                foreach (var shipPersonalInfo in posibleTargets)
                {
                    if (shipPersonalInfo.Key.ShipParameters.StartParams.ShipType == ShipType.Base)
                    {
                        rating = 1000;
                        return shipPersonalInfo.Value;
                    }
                }
                var bestEnemy2 = BestByParams(out rating, posibleTargets);
                return bestEnemy2;
        }

        var bestEnemy = BestByParams(out rating, posibleTargets);
        if (bestEnemy != null)
        {
            return bestEnemy;
        }

        return posibleTargets.FirstOrDefault().Value;
    }

    private ShipPersonalInfo BestByParams(out float rating, Dictionary<ShipBase, ShipPersonalInfo> posibleTargets)
    {
        rating = 0f;
        ShipPersonalInfo bestEnemy = null;
        foreach (var shipInfo in posibleTargets) //_owner.Enemies)
        {
            var ship = shipInfo.Value;
            // if (ship.CommanderShipEnemy.IsPriority)
            // {
            //     return ship;
            // }
            var cRating = ShipValue(ship);
            if (cRating > rating)
            {
                rating = cRating;
                bestEnemy = ship;
            }
        }
        return bestEnemy;
    }


    private ShipPersonalInfo BestParameter(Dictionary<ShipBase, ShipPersonalInfo> posibleTargets, ECommanderPriority1 commanderPriority1)
    {
        float curPercent = 0f;
        float curShield;
        float curHp;
        switch (commanderPriority1)
        {
            case ECommanderPriority1.MinShield:
            case ECommanderPriority1.MinHealth:
            case ECommanderPriority1.Slow:
                curPercent = Single.MaxValue;
                break;
            case ECommanderPriority1.MaxShield:
            case ECommanderPriority1.MaxHealth:
            case ECommanderPriority1.Fast:
                curPercent = Single.MinValue;
                break;
        }
        ShipPersonalInfo curTarget = null;
        foreach (var target in posibleTargets)
        {
            switch (commanderPriority1)
            {
                case ECommanderPriority1.MinShield:
                    if (target.Key.ShipParameters.MaxShield > 0)
                    {
                        var p1 = target.Key.ShipParameters.CurShiled / target.Key.ShipParameters.MaxShield;
                        curShield = PERCENT_CHOOSE ? p1 : target.Key.ShipParameters.CurShiled;
                        if (curShield < curPercent)
                        {
                            curPercent = curShield;
                            curTarget = target.Value;
                        }
                    }

                    break;
                case ECommanderPriority1.MinHealth:
                    var p2 = target.Key.ShipParameters.CurHealth / target.Key.ShipParameters.MaxHealth;
                    curHp = PERCENT_CHOOSE ? p2 : target.Key.ShipParameters.CurHealth;
                    if (curHp < curPercent)
                    {
                        curPercent = curHp;
                        curTarget = target.Value;
                    }
                    break;
                case ECommanderPriority1.MaxShield:
                    if (target.Key.ShipParameters.MaxShield > 0)
                    {
                        var p3 = target.Key.ShipParameters.CurShiled / target.Key.ShipParameters.MaxShield;
                        curShield = PERCENT_CHOOSE ? p3 : target.Key.ShipParameters.CurShiled;
                        if (curShield > curPercent)
                        {
                            curPercent = curShield;
                            curTarget = target.Value;
                        }
                    }

                    break;
                case ECommanderPriority1.MaxHealth:
                    var p4 = target.Key.ShipParameters.CurHealth / target.Key.ShipParameters.MaxHealth;
                    curHp = PERCENT_CHOOSE ? p4 : target.Key.ShipParameters.CurHealth;
                    if (curHp > curPercent)
                    {
                        curPercent = curHp;
                        curTarget = target.Value;
                    }
                    break;
                case ECommanderPriority1.Fast:
                    curHp = target.Key.ShipParameters.MaxSpeed + target.Key.ShipParameters.TurnSpeed * .008f;
                    if (curHp > curPercent)
                    {
                        curPercent = curHp;
                        curTarget = target.Value;
                    }
                    break;
                case ECommanderPriority1.Slow:
                    curHp = target.Key.ShipParameters.MaxSpeed + target.Key.ShipParameters.TurnSpeed * .008f;
                    if (curHp < curPercent)
                    {
                        curPercent = curHp;
                        curTarget = target.Value;
                    }
                    break;
            }
        }

        return curTarget;
    }

    protected virtual float ShipValue(ShipPersonalInfo info)
    {
        float cRating = 0;
        var dot = Utils.FastDot(info.DirNorm, _owner.LookDirection);
        var isFront = dot > 0;
        if (info.Dist < 15)
        {
            if (isFront)
            {
                cRating = 1000 + info.Dist;
            }
            else
            {
                cRating = 300 + info.Dist;
            }
        }
        else
        {
            cRating = info.Dist;
        }

        if (info.ShipLink.ShipParameters.StartParams.ShipType == ShipType.Base)
        {
            cRating -= 400;
        }
        if (!info.Visible)
        {
            cRating -= 999999;
        }

        return cRating;
    }

    public void SetLastAction(ActionType actionType)
    {
        _lastAction = actionType;
    }

    public virtual void Select(bool val)
    {

    }

    public virtual void DrawUpdate()
    {

    }

    public virtual void Dispose()
    {

    }
}

