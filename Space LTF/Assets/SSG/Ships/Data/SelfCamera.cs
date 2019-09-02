using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class SelfCamera : MonoBehaviour
{
    public const float SPEED = 2f;
    public const float xOffse = 3;
    public const float ZOffse = 3;
    public const float yOffse = 2;
    public const float Y_ANG_COEF =0.1f;
    public const float Demppher =0.1f;

    public float SPEED_VAR_2 = 3;
    public float DISTAXZ = 10;
    public float DISTAY = 5;

    public Camera Camera;
    public Transform HolderXZ;
    public Transform HolderY;



    private RenderTextureWithPool _textureWithPool;
    private RawImage _rawImage;
    private bool _isInited = false;
    private ShipBase _ship;

    private Vector3 _targetCameraPos;
    private Vector3 _targetLookPos;

    public Vector3 _targetHZPos;
    public Vector3 _targetYPos;

    private Vector3 _curLookPos;

    public void Init(RawImage rawImage,ShipBase ship)
    {

        _ship = ship;
        Camera.depth = 20;
        Camera.enabled = true;
        _rawImage = rawImage;
        _textureWithPool = DataBaseController.Instance.PoolRenderTextures.GetTexture();
//        _textureWithPool.RenderTexture.height
        Camera.targetTexture = _textureWithPool.RenderTexture;
        rawImage.texture = _textureWithPool.RenderTexture;
        _isInited = true;
    }

    void Update()
    {
        if (!_isInited || Time.deltaTime == 0f)
        {
            return;
        }

        UpdateCamPos1();
        MoveCameraToTarget();
    }

    private void UpdateCamPos1()
    {
        Vector3? target = _ship.CurAction == null ? null : _ship.CurAction.GetTargetToArrow();
        if (!target.HasValue)
        {
            target = _ship.Position +  _ship.LookDirection * 15f;
        }

        var dir = target.Value - _ship.Position;
        dir.y = 0f;
        var isInFront = Vector3.Dot(dir, _ship.LookDirection) > 0;

        var hzPos = _ship.Position - Utils.NormalizeFastSelf(dir) * DISTAXZ;
        _targetHZPos = hzPos;
        _targetYPos = new Vector3(0, DISTAY, 0);

    }

    private void MoveCameraToTarget()
    {
        var dirHZ = _targetHZPos - HolderXZ.position;
//        var dirY = _targetHZPos - HolderY.localPosition;
        var distHZ = dirHZ.magnitude;
//        var distY = dirY.magnitude;
        var speedDelta = Time.deltaTime * SPEED_VAR_2;
        if (distHZ < speedDelta)
        {
            HolderXZ.position =_targetHZPos;
        }
        else
        {
            HolderXZ.position = HolderXZ.position + Utils.NormalizeFastSelf(dirHZ) * speedDelta;
        }
//        if (distY < speedDelta)
//        {
            HolderY.localPosition =_targetYPos;
//        }
//        else
//        {
//            HolderY.localPosition = HolderY.localPosition + Utils.NormalizeFastSelf(dirY) * speedDelta;
//        }
        Camera.transform.LookAt(_ship.Position);

    }

    private void UpdateCameraPos2()
    {
        Vector3? target = _ship.CurAction == null ? null : _ship.CurAction.GetTargetToArrow();
        var shipPos = _ship.ShipVisual.transform.position;
        Vector3 nextLookPos;
        Vector3 nextCamPos;
        if (target.HasValue)
        {
            var midDist = (shipPos + target.Value).magnitude;
            var midPos = Vector3.Lerp(shipPos, target.Value, 0.3f);// (shipPos + target.Value) / 2f;
            nextCamPos = new Vector3(midPos.x + xOffse, midDist * Y_ANG_COEF, midPos.z);
            nextLookPos = midPos;
            //            Debug.DrawLine(_targetCameraPos,_curLookPos,Color.yellow);
        }
        else
        {
            //UP AND OFFSET 
            nextCamPos = new Vector3(shipPos.x + xOffse, yOffse, shipPos.z + ZOffse);
            nextLookPos = shipPos;
            //            Debug.DrawLine(_targetCameraPos, _curLookPos, Color.yellow);
        }

        _curLookPos = CheckPointPos(_targetLookPos, _curLookPos);


        _targetCameraPos = Vector3.Lerp(_targetCameraPos, nextCamPos, Demppher);
        var cameraFinal = _targetCameraPos;


        _curLookPos = Vector3.Lerp(_curLookPos, nextLookPos, Demppher);
        var lookAtFinal = _curLookPos;


        Camera.transform.position = cameraFinal;
        Camera.transform.LookAt(lookAtFinal);


        Debug.DrawLine(cameraFinal, lookAtFinal, Color.blue);
        DrawUtils.DebugPoint(cameraFinal, Color.yellow);
    }

    private Vector3 CheckPointPos(Vector3 trg,Vector3 oldPos)
    {
        var dirToTarget = (trg - oldPos);
        var dist = dirToTarget.sqrMagnitude;
        if (dist > 0.1f)
        {
            var deltaMove = Utils.NormalizeFastSelf(dirToTarget) * Time.deltaTime * SPEED;
            if (deltaMove.sqrMagnitude > dist)
            {
                oldPos = trg;
            }
            else
            {
                var nCamPos = Camera.transform.position + deltaMove;
                oldPos = nCamPos;
            }
        }
        else
        {
            oldPos = trg;
        }

        return oldPos;
    }

    public void Dispose()
    {
        if (_isInited)
        {
            Camera.enabled = false;
            Camera.targetTexture = null;
            if (_rawImage != null)
                _rawImage.texture = null;
            _textureWithPool.SetFree();
            _rawImage = null;
        }
    }
}

