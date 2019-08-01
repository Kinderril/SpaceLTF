﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;


public class InputManager : MonoBehaviour
{
    private bool isLastFramePressed;
    private bool isOverUI;
    private bool isEnbale = false;
    private InGameMainUI inGameMainUi;
//    public Vector3 keybordDir;
    private float _rightDown;
    private Commander _commander;

    public void Init(InGameMainUI inGameMainUi,Commander commander)
    {
        this.inGameMainUi = inGameMainUi;
        _commander = commander;
    }

    void LateUpdate()
    {
        if (!isEnbale)
        {
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
//            Debug.Log("Pressed left click.");
            isOverUI = EventSystem.current.IsPointerOverGameObject();
            inGameMainUi.Clicked(Input.mousePosition,true,0);
            //            isLastFramePressed = true;
        }
        if (Input.GetMouseButtonDown(1))
        {
            _rightDown = Time.time;
//            Debug.Log("d");
//            isOverUI = EventSystem.current.IsPointerOverGameObject();
//            inGameMainUi.Clicked(Input.mousePosition, false, delta);
        }
        if (Input.GetMouseButton(1))
        {
            var delta = Time.time - _rightDown;
            isOverUI = EventSystem.current.IsPointerOverGameObject();
            inGameMainUi.Hold(Input.mousePosition, false, delta);
        }

        if (Input.GetMouseButtonUp(1))
        {
//            Debug.Log("u");
            var delta = Time.time - _rightDown;
//            Debug.Log("Pressed left click:" + delta);
//            var isLong = delta > 0.6f;
            isOverUI = EventSystem.current.IsPointerOverGameObject();
            inGameMainUi.Clicked(Input.mousePosition,false, delta);
//            isLastFramePressed = true;
        }

//        if (Input.GetMouseButtonUp(0))
//        {
//            Debug.Log("Pressed left click.");
//            isLastFramePressed = false;
//            if (isOverUI)
//            {
//                EndPress(true);
//                return;
//            }
//        }
    }

    void Update()
    {
        if (!isEnbale)
        {
            return;
        }
        UpdateMouseBorder();
        UpdateKeyboard();
//        UpdateWheel();
    }

    private void UpdateMouseBorder()
    {

    }


    private void TrySelectSpell(int index)
    {
        inGameMainUi.SpellModulsContainer.TrySelectSpell(index);
    }

    private void TryGetShipAndSelectIt(int index)
    {
        var ships = _commander.Ships.Values;
        var list = ships.ToList();
        if (index < list.Count)
        {
           var s=  list[index];
            inGameMainUi.ActionShipSelected(s);
        }
    }

    private int _lastShip = 0;
    private void TryGetShipAndSelectItNext()
    {
        var ships = _commander.Ships.Values;
        var list = ships.ToList();
        if (list.Count > 0)
        {
            if (_lastShip < list.Count)
            {
                var s = list[_lastShip];
                inGameMainUi.ActionShipSelected(s);
                _lastShip++;
            }
            else
            {
                _lastShip = 0;
                var s = list[_lastShip];
                inGameMainUi.ActionShipSelected(s);
            }

        }

    }

    private void SpellsKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TryGetShipAndSelectItNext();
        }

        //        if (Input.GetKeyDown(KeyCode.Q))
            //        {
            //            TrySelectSpell(0);
            //        }
            //        if (Input.GetKeyDown(KeyCode.E))
            //        {
            //            TrySelectSpell(1);
            //        }
            //        if (Input.GetKeyDown(KeyCode.R))
            //        {
            //            TrySelectSpell(2);
            //        }
            //        if (Input.GetKeyDown(KeyCode.T))
            //        {
            //            TrySelectSpell(3);
            //        }
        }

    private void NumbersClick()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TrySelectSpell(0);
//            TryGetShipAndSelectIt(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TrySelectSpell(1);
//            TryGetShipAndSelectIt(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TrySelectSpell(2);
//            TryGetShipAndSelectIt(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TrySelectSpell(3);
//            TryGetShipAndSelectIt(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            TrySelectSpell(4);
            //            TryGetShipAndSelectIt(4);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BattleController.Instance.PauseChange();
        }
    }

    private void MainShipControls()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _commander.ChangeShieldControlCenter();
        }
    }

//    private void CameraMove()
//    {
//        var w = Input.GetKey(KeyCode.W);
//        var s = Input.GetKey(KeyCode.S);
//        var d = Input.GetKey(KeyCode.D);
//        var a = Input.GetKey(KeyCode.A);
//        int x = 0;
//        int y = 0;
//        if (!w && !s && !d && !a)
//        {
//            return;
//        }
//        if (w)
//        {
//            x = 1;
//        }
//        else if (s)
//        {
//            x = -1;
//        }
//
//        if (d)
//        {
//            y = 1;
//        }
//        else if (a)
//        {
//            y = -1;
//        }
//        keybordDir = new Vector3(y, 0, x);
//        CamerasController.Instance.GameCamera.MoveMainCamToDir(keybordDir);
//
//    }

    private void UpdateKeyboard()
    {
        MainShipControls();
        SpellsKeyboard();
        NumbersClick();
//        CameraMove();
    }

    private void EndPress(bool b)
    {
        
    }

    public void SetActivaCamera(CamerasController activeCamera)
    {
        
    }

    public void DisableCamera()
    {
        
    }

    public void SetEnable(bool isInGame)
    {
        isEnbale = isInGame;
    }

    public void Dispose()
    {
        SetEnable(false);
    }
}

