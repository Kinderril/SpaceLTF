using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;


public class CameraController : MonoBehaviour
{
    public Transform GameCameraHolder;
    public Transform GameCameraHolderY;
    private float SpeedXZ = 0.5f;
    private float SpeedY = 1.5f;
    public float MIN_Y = 10f;
    public float MAX_Y = 19f;
    private Vector3 _min;
    private Vector3 _max;
    private Vector3? _targetPos;
    private float startMoveTime;
    private float _period;
    public Camera Camera;
    public CameraShake MainCameraShake;
    private bool _isEnable = true;
    public AudioSource SourceAmbient;
    public AudioListener MainListerer;
    private bool isAudioEnabled = true;

    public void InitBorders(Vector3 min, Vector3 max)
    {
        _min = min;
        _max = max;
    }

    public void MainListenerSwitch()
    {
        isAudioEnabled = !isAudioEnabled;

        AudioListener.volume = isAudioEnabled?1f:0f;
//        MainListerer.
    }

    public void Enable(bool val)
    {
        _isEnable = val;
    }

    void Update()
    {
        if (_targetPos.HasValue)
        {
            var oPos = GameCameraHolder.transform.position;
            if (_period > 0)
            {
                var p = (Time.time - startMoveTime) / _period;
                if (p > 1)
                {
                    GameCameraHolder.transform.position = _targetPos.Value;
                    _targetPos = null;
                }
                else
                {
                    GameCameraHolder.transform.position = Vector3.Lerp(oPos, _targetPos.Value, p);
                    //                transform.position = Vector3.Lerp(_from, _bulletTarget.transform.position, p);
                }
            }
            else
            {
                GameCameraHolder.transform.position = _targetPos.Value;
                _targetPos = null;
            }
        }

        UpdateWheel();
    }
    public void SetCameraTo(Vector3 v, float coef = 1f)
    {
        startMoveTime = Time.time;
        _targetPos = v;
        _period = coef * 1f;
    }

    public void MoveMainCamToDir(Vector3 keybordDir)
    {
        if (_isEnable)
        {
            keybordDir.y = 0;
            var nextPos = GameCameraHolder.transform.position + keybordDir * SpeedXZ;
            nextPos.z = Mathf.Clamp(nextPos.z, _min.z, _max.z);
            nextPos.x = Mathf.Clamp(nextPos.x, _min.x, _max.x);
            GameCameraHolder.transform.position = nextPos;
        }
    }

    private void UpdateWheel()
    {
        var mouseScroll = Input.mouseScrollDelta;
        if (Math.Abs(mouseScroll.y) > 0.1f)
        {
           MoveMainCamUp(mouseScroll.y);
        }
    }

    public void MoveMainCamUp(float delta)
    {
        var p = GameCameraHolderY.transform.localPosition;
        var nextPos = Mathf.Clamp(p.y - delta * SpeedY, MIN_Y, MAX_Y);

        GameCameraHolderY.transform.localPosition = new Vector3(p.x, nextPos, p.z);
    }

    public void Stop()
    {
        _targetPos = null;
    }

    public Vector3 WorldToScreenPoint(Vector3 shipPosition)
    {
        return Camera.WorldToScreenPoint(shipPosition);
    }

    public Ray ScreenPointToRay(Vector3 pos)
    {
        return Camera.ScreenPointToRay(pos);
    }
}

