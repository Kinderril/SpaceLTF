using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class HoldClearCoinUI : MonoBehaviour
{
    public const float HOLD_TIME = 0.8f;
    public const float BOT_LINE = 0.2f;
    private ShipBase _holdedShip;
    public Slider Slider;
    private bool _haveHoldShip = false;
    private bool _holdComplete = false;
    private CameraController cam;
    private Action<ShipBase> holdComplete;

    public void Init(Action<ShipBase> holdComplete)
    {
        this.holdComplete = holdComplete;
        cam = CamerasController.Instance.GameCamera;
        Release();
    }

    public void Hold(ShipBase ptBase, bool left, float delta)
    {
        if (_holdComplete)
        {
            return;
        }
        if (_haveHoldShip)
        {
            Slider.transform.position = cam.WorldToScreenPoint(_holdedShip.Position);
            var d = (delta- BOT_LINE )/ HOLD_TIME;
            Slider.value = d;
            if (d > 1f)
            {
                _holdComplete = true;
                holdComplete(_holdedShip);
                Slider.gameObject.SetActive(false);
            }
        }
        else
        {
            if (ptBase != null)
            {
                if (!left)
                {
                    if (delta > BOT_LINE)
                    {
                        _holdedShip = ptBase;
                        _holdedShip.OnDispose += OnDispose;
                        _haveHoldShip = true;
                        Slider.gameObject.SetActive(_haveHoldShip);
                    }
                }
            }
        }
    }

    private void OnDispose(ShipBase obj)
    {
        Release();
    }

    public void Release()
    {
        if (_holdedShip != null)
        {
            _holdedShip.OnDispose -= OnDispose;
        }
        _haveHoldShip = false;
        _holdComplete = false;
        _holdedShip = null;
        Slider.gameObject.SetActive(false);
    }
}

