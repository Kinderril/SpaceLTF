using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class CamerasController : Singleton<CamerasController>
{
    public CameraController GameCamera;
    public BackgroundCamera BackgroundCamera;
    public CameraController GlobalMapCamera;
    public Camera UICamera;

    public Vector3 keybordDir;
    private CameraController _activeCamera;
    private const float MinOffset = 40;
    private const float MaxOffset = 20;

    void Awake()
    {
        OpenUICamera();
    }

    private void CameraMove()
    {
        if (_activeCamera != null)
        {
            var w = Input.GetKey(KeyCode.W);
            var s = Input.GetKey(KeyCode.S);
            var d = Input.GetKey(KeyCode.D);
            var a = Input.GetKey(KeyCode.A);
            int x = 0;
            int y = 0;
            if (!w && !s && !d && !a)
            {
                CheckMouseNearBorder();
                return;
            }
            if (w)
            {
                x = 1;
            }
            else if (s)
            {
                x = -1;
            }

            if (d)
            {
                y = 1;
            }
            else if (a)
            {
                y = -1;
            }
            keybordDir = new Vector3(y, 0, x);
            _activeCamera.MoveMainCamToDir(keybordDir);
        }
    }

    private void CheckMouseNearBorder()
    {
        float x = 0;
        float y = 0;
        var w = Screen.width;
        var h = Screen.height;
        bool shallMove = false;
        float coefMove = 0.5f;
        var mousePos = Input.mousePosition;
//        Debug.LogError("Mouse pos:" + mousePos);
        if (mousePos.x < MaxOffset)
        {
            shallMove = true;
            if (mousePos.x < MinOffset)
            {
                y = -1;
            }
            else
            {
                y = -coefMove;
            }
        }
        else if (mousePos.x > w - MaxOffset)
        {
            shallMove = true;
            if (mousePos.x > w - MinOffset)
            {
                y = 1;
            }
            else
            {
                y = coefMove;
            }
        }
        if (mousePos.y < MaxOffset)
        {
            shallMove = true;
            if (mousePos.y < MinOffset)
            {
                x = -1;
            }
            else
            {
                x = -coefMove;
            }
        }
        else if (mousePos.y > h - MaxOffset)
        {
            shallMove = true;
            if (mousePos.y > h - MinOffset)
            {
                x = 1;
            }
            else
            {
                x = coefMove;
            }
        }
        if (shallMove)
        {
            keybordDir = new Vector3(y, 0, x);
            if (_activeCamera != null)
            {
#if UNITY_EDITOR
                if (DebugParamsController.NoMouseMove)
                {
                    return;
                }
#endif
                _activeCamera.MoveMainCamToDir(keybordDir);
            }
        }
    }

    void Update()
    {
        CameraMove();
#if UNITY_EDITOR
//        if (GameCamera.gameObject.activeSelf && GlobalMapCamera.gameObject.activeSelf)
//        {
//            Debug.LogError("Both cameras are active");
//        }
#endif
    }


    public void StartBattle()
    {
        OpenGameCamera();
        BackgroundCamera.StartGame();
    }

    public void Init()
    {
        CloseGameCamera();
        CloseGlobalCamera();
    }
    
    public void EndGame()
    {
        CloseGameCamera();
//        BackgroundCamera.EndGame();
        OpenUICamera();
    }


    public void OpenUICamera()
    {
        UICamera.gameObject.SetActive(true);
        CloseGameCamera();
        CloseGlobalCamera();
        _activeCamera = null;
    }

    public void OpenGlobalCamera()
    {
        CloseGameCamera();
        GlobalMapCamera.gameObject.SetActive(true);
        _activeCamera = GlobalMapCamera;
        UICamera.gameObject.SetActive(false);
    }

    public void OpenGameCamera()
    {
        CloseGlobalCamera();
        GameCamera.gameObject.SetActive(true);
        _activeCamera = GameCamera;
        UICamera.gameObject.SetActive(false);
    }

    public void CloseGameCamera()
    {
        GameCamera.gameObject.SetActive(false);
        _activeCamera = null;
    }

    public void CloseGlobalCamera()
    {
        _activeCamera = null;
        GlobalMapCamera.gameObject.SetActive(false);
    }

    public void SetCameraTo(Vector3 position)
    {
        if (_activeCamera != null)
        {
            _activeCamera.SetCameraTo(position);
        }
    }

    public void StartGlobalMap()
    {
        OpenGlobalCamera();
    }
}

