﻿using UnityEngine;


public class CamerasController : Singleton<CamerasController>
{
    private const string KEY_FXAA = "KEY_FXAA";
    private const string KEY_SOUND = "SoundKey";
    private const string KEY_NO_MOUSE_MOVE = "KEY_NO_MOUSE_MOVE";
    public CameraController GameCamera;
    public BackgroundCamera BackgroundCamera;
    public CameraController GlobalMapCamera;
    // public AudioListener BattleListerer;
    // public AudioListener MenuListerer;
    private bool _noMouseMove = false;
    private bool _isAudioEnabled = true;
    public AudioSourceMusicControl MusicControl;
    public Camera UICamera;

    public Vector3 keybordDir;
    private CameraController _activeCamera;
    private const float MinOffset = 40;
    private const float MaxOffset = 20;
    public bool IsNoMouseMove => _noMouseMove;
    public bool IsAudioEnable => _isAudioEnabled;
    private bool _fxaaEnable;

    public bool FxaaEnable => _fxaaEnable;

    void Awake()
    {
        OpenUICamera();
    }

    public void CheckNoMouseMoveOnStart()
    {
        var key = PlayerPrefs.GetInt(KEY_NO_MOUSE_MOVE, 1);
        _noMouseMove = key == 1;
    }

    // public void MainNoMouseMoveSwitch()
    // {
    //     _noMouseMove = !_noMouseMove;
    //     PlayerPrefs.SetInt(KEY_NO_MOUSE_MOVE, _noMouseMove ? 1 : 0);
    // }

    public void CheckSoundOnStart()
    {
        var key = PlayerPrefs.GetInt(KEY_SOUND, 1);
        _isAudioEnabled = key == 1;
        CheckListaner();
    }

    public void MainListenerSwitch()
    {
        _isAudioEnabled = !_isAudioEnabled;
        PlayerPrefs.SetInt(KEY_SOUND, _isAudioEnabled ? 1 : 0);
        CheckListaner();
    }

    public void SetAudioMainListener(bool val)
    {
        if (_isAudioEnabled == val)
        {
            return;
        }
        _isAudioEnabled = val;
        PlayerPrefs.SetInt(KEY_SOUND, _isAudioEnabled ? 1 : 0);
        CheckListaner();

    }

    private void CheckListaner()
    {
        AudioListener.volume = _isAudioEnabled ? 1f : 0f;
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
                if (_noMouseMove)
                {
                    return;
                }

                // #if UNITY_EDITOR
                //                 if (DebugParamsController.NoMouseMove)
                //                 {
                //                     return;
                //                 }
                // #endif
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
        BackgroundCamera.EndBattleGame();
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
        MusicControl.StopMenuAudio();
        MusicControl.StartGlobalAudio();
        CloseGameCamera();
        GlobalMapCamera.gameObject.SetActive(true);
        _activeCamera = GlobalMapCamera;
        UICamera.gameObject.SetActive(false);
    }

    public void OpenGameCamera()
    {
        MusicControl.StopGlobalAudio();
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

    public void SetCameraTo(Vector3 p, float period = 1f)
    {
        if (_activeCamera != null)
        {
            var position = new Vector3(p.x, p.y, p.z);
            _activeCamera.SetCameraTo(position, period);
        }
    }

    public void StartGlobalMap()
    {
        OpenGlobalCamera();
    }

    public void StartCheck()
    {
        StartCheckAA();
        CheckSoundOnStart();
        CheckNoMouseMoveOnStart();
    }

    private void CheckAntiAlysing(bool fxaaEnable)
    {
        GameCamera.CheckAntiAlysing(fxaaEnable);
        GlobalMapCamera.CheckAntiAlysing(fxaaEnable);
    }

    public void EnableAA(bool val)
    {
        _fxaaEnable = val;
        PlayerPrefs.SetInt(KEY_FXAA, _fxaaEnable ? 1 : 0);
        CheckAntiAlysing(_fxaaEnable);
    }

    public void StartCheckAA()
    {
        var isEnableFxaa = PlayerPrefs.GetInt(KEY_FXAA, 0) == 1;
        _fxaaEnable = isEnableFxaa;
        CheckAntiAlysing(_fxaaEnable);
    }
    public void FXAASwitch()
    {
        EnableAA(!_fxaaEnable);
    }

    public void SetNoMouseMove(bool isOn)
    {
        _noMouseMove = isOn;
    }
}

