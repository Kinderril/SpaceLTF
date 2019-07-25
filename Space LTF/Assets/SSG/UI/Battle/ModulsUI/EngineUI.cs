using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class EngineUI : MonoBehaviour
{
    private ShipBase _ship;
    public GameObject EngineNotWork;

    public void Init(ShipBase ship)
    {
        _ship = ship;
        _ship.EngineStop.OnStop += OnStop;
        OnStop(_ship, true);
    }

    private void OnStop(MovingObject arg1, bool arg2)
    {
        var isCrash = _ship.EngineStop.IsCrash();
        EngineNotWork.SetActive(isCrash);
    }

    public void Dispose()
    {
        _ship.EngineStop.OnStop -= OnStop;
    }
}

