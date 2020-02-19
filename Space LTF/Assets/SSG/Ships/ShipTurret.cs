using System;
using System.Collections.Generic;
using UnityEngine;

public enum ETurretBehaviour
{
    nearBase,
    stayAtPoint,
}

public class ShipTurret : ShipBase
{
    private float dist2flow = 2f;
    private Vector3 StartPos;
    private TurretConnector _connetedShip;
    private bool _isConnected;
    private float _nextPosibleConnection;
    private Vector2 _moveOffset;
    private RoadMeshCreator _connector;
    private ETurretBehaviour TurretBehaviour = ETurretBehaviour.stayAtPoint;
    private List<Vector3> _array = new List<Vector3>(3);
    private float SmootCoef = .008f;
    private int _connectionTryies = 0;
    private Vector3 center;
    private float MaxRadiusSqrt;

    protected override void DesicionDataInit()
    {
        DesicionData = new TurretDesicionData(this);
    }

    public override void Init(TeamIndex teamIndex, ShipInventory shipInventory, ShipBornPosition pos, IPilotParameters pilotParams,
        Commander commander, Action<ShipBase> dealthCallback)
    {
        var battle = BattleController.Instance;
        center = (battle.CellController.Max + battle.CellController.Min) / 2f;
        var maxRadius = battle.CellController.Data.Radius;
        MaxRadiusSqrt = maxRadius * maxRadius;
        base.Init(teamIndex, shipInventory, pos, pilotParams, commander, dealthCallback);
        TurretBehaviour = commander.Player.GetTurretBehaviour();
        switch (TurretBehaviour)
        {
            case ETurretBehaviour.nearBase:

                break;
            case ETurretBehaviour.stayAtPoint:
                WeaponsController.IncreaseShootsDist(1.4f);
                break;
        }
    }

    protected override void UpdateAction()
    {
        if (!_isConnected)
        {
            CheckConnection();
        }
        CheckIsOutside();

        UpdateShieldRegen();
        if (CurAction != null)
        {
            if (CurAction.ShallEndByTime())
            {
                CurAction.EndAction("By time");
                return;
            }

            CurAction.ManualUpdate();
            if (CurAction != null)
            {
                CurAction.ShallEndUpdate2();
            }
            // EngineUpdate();
            // ApplyMove();
        }
        else
        {
            var task = DesicionData.CalcAction();
            //            Debug.Log(("Task1:" + Id + "  " + task.ToString()).Yellow());
            SetAction(task);
        }

        MoveUpdate();
    }

    private void CheckIsOutside()
    {
        if (Time.frameCount % 5 == 0)
        {
            var distFromCenter = (Position - center).sqrMagnitude;
            if (distFromCenter > MaxRadiusSqrt)
            {
                ShipParameters.Damage(200, 200, null, null);
            }
        }
    }

    private void MoveUpdate()
    {
        if (!_isConnected)
        {
            return;
        }
        var targetPosition = _connetedShip.Position + _connetedShip.LookDirection * _moveOffset.x +
                             _connetedShip.LookRight * _moveOffset.y;


        var p2 = Vector3.Lerp(transform.position, targetPosition, SmootCoef);
        transform.position = p2;
        var p1 = _connetedShip.Position;
        var m = (p1 + p2) / 2f;
        _array[0] = p1;
        _array[1] = m;
        _array[2] = p2;
        _connector.CreateByPoints(_array);

        Debug.DrawLine(p1, p2, Color.yellow);
    }

    private void CheckConnection()
    {
        if (_nextPosibleConnection < Time.time)
        {
            _connectionTryies++;
            if (_connectionTryies > 10)
            {
                Debug.LogError($"can't find place to connect {gameObject.name}");
                _nextPosibleConnection = Time.time + 99999f;
                return;
            }
            _nextPosibleConnection = Time.time + 2f;
            if (Commander.MainShip != null)
            {
                var ship = Commander.MainShip;
                _connetedShip = null;
                switch (TurretBehaviour)
                {
                    case ETurretBehaviour.nearBase:
                        if (ship != null)
                        {
                            _connetedShip = ship.Connector;
                        }
                        break;
                    case ETurretBehaviour.stayAtPoint:
                        break;
                }

                if (_connetedShip == null)
                {
                    var container = Commander.GetClosestConnector(Position);
                    if (container != null)
                    {
                        _connetedShip = container.Connector;
                    }
                    else
                    {
                        Debug.LogError("can't find connection for turret");
                    }
                }

                if (_connetedShip != null)
                {
                    var offset = _connetedShip.Connect();
                    _moveOffset = offset;
                    var connector = DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.ConnectorTurrets);
                    connector.transform.SetParent(transform);
                    _connector = connector;
                    _connetedShip.OnDeath += OnDeathConnected;
                    _isConnected = true;
                    var p2 = transform.position;
                    var p1 = _connetedShip.Position;
                    var m = (p1 + p2) / 2f;
                    _array.Add(p1);
                    _array.Add(m);
                    _array.Add(p2);
                }
            }
        }
    }

    private void OnDeathConnected()
    {
        _isConnected = false;
    }

    public override void Dispose()
    {
        if (_connetedShip != null)
            _connetedShip.OnDeath -= OnDeathConnected;
        base.Dispose();
    }

    public override Vector3 PredictionPos()
    {
        return Position;
    }

    protected override void GizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(StartPos, dist2flow);
    }
}

