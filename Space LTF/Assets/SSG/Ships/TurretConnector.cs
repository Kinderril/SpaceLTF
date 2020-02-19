using System;
using UnityEngine;
using System.Collections;

public class TurretConnector : MonoBehaviour
{
    private const float SIDE_OFFSET = 3f;
    private const float FRONT_OFFSET = 2f;
    private int _connectedShips = 0;
    private Vector3 _lastPos;
    public bool Moved;
//    public event Action OnConnectorDestroyed;
    private MovingObject _objectMov;
    public Vector3 Position => transform.position;
    public Vector3 LookDirection => _objectMov.LookDirection;
    public Vector3 LookRight => _objectMov.LookRight;

    public bool ConnectByWay = true;
    private Vector3 _baseVector;

    public event Action OnDeath;
    public bool CanConnect => _connectedShips < 2;

    public void Init(MovingObject objectMov)
    {
        _objectMov = objectMov;
        _objectMov.OnMainDispose += OnMainDispose;
        _baseVector =
            Utils.NormalizeFastSelf(new Vector3(MyExtensions.Random(-1f, 1f), 0, MyExtensions.Random(-1f, 1f)));
    }

    private void OnMainDispose(MovingObject obj)
    {
         _objectMov.OnMainDispose -= OnMainDispose;
        OnDeath?.Invoke();
    }

    void Update()
    {
        var delta = (_lastPos - transform.position).sqrMagnitude;
        _lastPos = transform.position;
        Moved = delta > 0.00001f;
    }
    public Vector2 Connect()
    {
        _connectedShips++;
        if (ConnectByWay)
        {
            float sideOffset;
            float frontOffset;
            var halfCount = _connectedShips / 2;
            if (Utils.IsEven(_connectedShips))
            {
                sideOffset = SIDE_OFFSET;
            }
            else
            {
                sideOffset = -SIDE_OFFSET;
            }

            frontOffset = halfCount * FRONT_OFFSET;

            return new Vector2(frontOffset, sideOffset);
        }
        else
        {
            _baseVector = Utils.RotateOnAngUp(_baseVector, MyExtensions.Random(60, 170));
            var r = new Vector2(_baseVector.x, _baseVector.z);
            var offset = r * FRONT_OFFSET;
//            Debug.LogError($"Offset {offset}");
            return offset;
        }
    }

    void OnDestroy()
    {
        if (_objectMov != null)
            _objectMov.OnMainDispose -= OnMainDispose;
        OnDeath?.Invoke();
    }
}
