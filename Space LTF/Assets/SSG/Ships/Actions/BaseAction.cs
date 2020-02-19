using UnityEngine;

public enum ActionType
{
    none,
    attack,
    moveToBase,
    returnToBattle,
    closeStrikeAction,
    evade,
    afterAttack,
    waitHeal,
    defence,
    mineField,
    attackSide,
    repairAction,
    waitEnemy,
    goToCurrentPointAction,
    goToHide,
    waitEnemySec,
    waitEdnGame,
    readyToAttack,
    attackHalfLoop,
    shootFromPlace,
}

public abstract class BaseAction
{
    protected Vector3? _targetPoint;
    //    protected MoveWayNoLerp _moveWay = null;
    protected const float RECALC_WAY_DELTA = 0.3f;
    protected ShipBase _owner;
    private CauseAction[] _causes;
    protected ShipDesicionDataBase _shipDesicionDataBase;
    public ActionType ActionType { get; private set; }

    public BaseAction(ShipBase owner, ActionType actionType)
    {
        ActionType = actionType;
        _causes = GetEndCauses();
        _owner = owner;
    }

    public void SetDesider(ShipDesicionDataBase shipDesicionDataBase)
    {
        _shipDesicionDataBase = shipDesicionDataBase;
    }

    public abstract void ManualUpdate();

    //    public abstract void ShallEndUpdate();

    public void ShallEndUpdate2()
    {
        for (int i = 0; i < _causes.Length; i++)
        {
            var c1 = _causes[i];
            if (c1.Act())
            {
                EndAction(c1.Name);
                return;
            }
        }
    }

    protected abstract CauseAction[] GetEndCauses();

    public virtual void DrawGizmos()
    {

    }


    protected virtual void Dispose()
    {

    }

    public void EndAction(string causeEndAction)
    {
        Dispose();
        Debug.Log(Namings.Format("<color=green>End Action  Id:{0}  Action:{1}  Cause:{2}  Time:{3}</color>"
            , _owner.Id, ActionType.ToString(), causeEndAction, Time.time.ToString()));
        _owner.EndAction();
    }

    //    public virtual Vector3? LastWayPoint()
    //    {
    //        if (_moveWay != null)
    //        {
    //            return _moveWay.Points[_moveWay.Points.Length - 1].Vector3;
    //        }
    //        return null;
    //    }
    public bool ShallEndByTime()
    {

        return false;
    }

    public virtual Vector3? GetTargetToArrow()
    {
        return null;
    }
}

