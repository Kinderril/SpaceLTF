using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

public enum PilotTcatic
{
    //    balance,
    defenceBase,
//    support,
    attack,
//    sneakAttack,
    attackBase,
}

public enum EnemyChooseType
{
    standart,
    moreHalfShield,
    minorShield,

}

public abstract class ShipDesicionDataBase : IShipDesicion
{
    protected ShipBase _owner;
    protected ActionType _lastAction;

    public ShipDesicionDataBase(ShipBase owner)
    {
        _owner = owner;
    }

    public static ShipDesicionDataBase Create(ShipBase ship,PilotTcatic tcatic)
    {
        switch (tcatic)
        {
            case PilotTcatic.attackBase:
                var myIndex = ship.TeamIndex;
                var enemyIndex = BattleController.OppositeIndex(myIndex);
                var enemyCommander = BattleController.Instance.GetCommander(enemyIndex);
                var mainShip1 = enemyCommander.MainShip;
                if (mainShip1 != null)
                {
                    return new ShipDesicionDataAttackBase(ship, mainShip1);
                }
                else
                {
                    return new ShipDesicionDataAttack(ship);
                }
            case PilotTcatic.defenceBase:
                var mainShip = ship.Commander.MainShip;
                if (mainShip != null)
                {
                    return new ShipDesicionDataDefenceBase(ship, mainShip);
                }
                else
                {
                    return new ShipDesicionDataAttack(ship);
                }
//                case PilotTcatic.support:
//                return new ShipDesicionDataSupport(ship);
            case PilotTcatic.attack:
                return new ShipDesicionDataAttack(ship);
//            case PilotTcatic.sneakAttack:
//                return new ShipDesicionDataSneakyAttack(ship);
        }
        return new ShipDesicionDataBalanced(ship);
    }

    public BaseAction CalcAction()
    {
        var type = CalcTask(out var ship);
        var task = ActivateTask(type, ship);
        task.SetDesider(this);
        return task;
    }

    public void TryChangeTactic(PilotTcatic nextTactic)
    {
        if (GetTacticType() != nextTactic)
        {
            _owner.SetDesision(ShipDesicionDataBase.Create(_owner, nextTactic));
        }
    }

    protected ActionType RepairOrGoHide()
    {
        int dist;
        AICell cell = _owner.CellController.Data.FindClosestCellByType(_owner.Cell, CellType.Clouds,false,out dist);
        if (cell != null && dist <= 2)
        {
            return ActionType.goToHide;
        }
        return ActionType.repairAction;
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
        if (HaveEnemyInDangerZone(out ship))
        {
            return DoAttackAction(ship);
        }
        return OptionalTask(out ship);
    }

    public BaseAction ActivateTask(ActionType task, ShipBase target)
    {
        switch (task)
        {
            case ActionType.none:
                break;
            case ActionType.goToHide:
                return new GoToHideAction(_owner);
            case ActionType.attack:
                return new AttackAction(_owner, _owner.Enemies[target]);
            case ActionType.moveToBase:
                return (new GoToBaseAction(_owner, target));
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
                return new AttackSideAction(_owner,_owner.Enemies[target], controlPoint);
            case ActionType.waitEnemy:
                var wait = new WaitEnemy(_owner, _owner.Commander.GetWaitPosition(_owner));
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

    protected abstract ActionType DoAttackAction(ShipBase ship);
    protected abstract ActionType OptionalTask(out ShipBase ship);
    protected abstract bool HaveEnemyInDangerZone(out ShipBase ship);
    public abstract string GetName();

    protected ActionType HideOrWait()
    {
        //TODO wait hide
        return ActionType.goToHide;
    }
    
    protected ActionType DoOrWait(ActionType def,ShipBase ship,ActionType defNo = ActionType.afterAttack)
    {
        if (_owner.WeaponsController.AnyWeaponIsLoaded() && ship != null && ship.VisibilityData.Visible)
        {
            return def;
        }
        if (_owner.Locator.DangerEnemy != null)
        {
            return ActionType.evade;
        }
        return defNo;
    }

    protected ActionType AttackOrAttackSide(ShipBase target)
    {
        if (target.ShipParameters.MaxSpeed < _owner.ShipParameters.MaxSpeed)
        {
            var cellsDistX = Mathf.Abs(_owner.Cell.Xindex - target.Cell.Xindex);
            var cellsDistZ = Mathf.Abs(_owner.Cell.Zindex - target.Cell.Zindex);

            var distCells = Mathf.Sqrt(cellsDistX*cellsDistX + cellsDistZ*cellsDistZ);
            if (distCells > 6)
            {
                return ActionType.attackSide;
            } 
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

    protected bool HaveEnemyClose(out ShipBase ship,float closeDist)
    {
        return HaveEnemyClose(out ship,_owner , closeDist);
    }

    protected bool HaveEnemyClose(out ShipBase ship,ShipBase testedShip,float closeDist)
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
        float rating;
        return CalcBestEnemy(out rating, posibleTargets);
    }

    [CanBeNull]
    protected ShipPersonalInfo CalcBestEnemy(out float rating, Dictionary<ShipBase, ShipPersonalInfo> posibleTargets)
    {
        ShipPersonalInfo bestEnemy = null;
        rating = Single.MinValue;
        foreach (var shipInfo in posibleTargets) //_owner.Enemies)
        {
            var ship = shipInfo.Value;
            if (ship.CommanderShipEnemy.IsPriority)
            {
                return ship;
            }
            var cRating = ShipValue(ship);
            if (cRating > rating)
            {
                rating = cRating;
                bestEnemy = ship;
            }
        }
        return bestEnemy;
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

    public abstract PilotTcatic GetTacticType();

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

