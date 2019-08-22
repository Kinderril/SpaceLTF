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

    public Camera Camera;
    private RenderTextureWithPool _textureWithPool;
    private RawImage _rawImage;
    private bool _isInited = false;
    private ShipBase _ship;

    private Vector3 _targetCameraPos;
    private Vector3 _targetLookPos;

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
        if (!_isInited)
        {
            return;
        }
        Vector3? target = _ship.CurAction == null ? null :_ship.CurAction.GetTargetToArrow();
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


        _curLookPos =  Vector3.Lerp(_curLookPos, nextLookPos, Demppher);
        var lookAtFinal = _curLookPos;


        Camera.transform.position = cameraFinal;
        Camera.transform.LookAt(lookAtFinal);


        Debug.DrawLine(cameraFinal, lookAtFinal, Color.blue);
        DrawUtils.DebugPoint(cameraFinal,Color.yellow);

        //        Camera.transform.position = CheckPointPos(_targetCameraPos,Camera.transform.position);
        //        Camera.transform.LookAt(_curLookPos);
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

