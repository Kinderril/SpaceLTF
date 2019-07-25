using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ShipModulsUI : MonoBehaviour
{
    public Transform ModulsLayout;
    public Transform WeaponsLayout;
    public Text ShipName;
    private List<WeaponModulUI> _weaponModuls = new List<WeaponModulUI>();
    private List<ModulUI> _moduls = new List<ModulUI>();
    private List<SupportModulUI> _supportModuls = new List<SupportModulUI>();
    public ShipSlidersInfo SlidersInfo;
    private ShipBase _ship;
    public CurActionUI CurActionUI;
//    public PilotBattleUI PilotBattleUI;
    public EngineUI EngineUI;
    public ShipBattleUI ShipBattleUI;
    public GameObject HolderBigInfo;
    public Toggle ToggleShip;

    void Awake()
    {
        if (_moduls.Count == 0)
        {
            for (int i = 0; i < 6; i++)
            {
                var prefab = DataBaseController.Instance.DataStructPrefabs.ModulUI;
                var e = DataBaseController.GetItem(prefab);
                _moduls.Add(e);
                e.transform.SetParent(ModulsLayout);
                e.gameObject.SetActive(false);
            }
        }

        if (_supportModuls.Count == 0)
        {
            for (int i = 0; i < 6; i++)
            {
                var prefab = DataBaseController.Instance.DataStructPrefabs.SupportModulUI;
                var e = DataBaseController.GetItem(prefab);
                _supportModuls.Add(e);
                e.transform.SetParent(ModulsLayout);
                e.gameObject.SetActive(false);
            }
        }
        if (_weaponModuls.Count == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                var prefab = DataBaseController.Instance.DataStructPrefabs.WeaponModulUI;
                var e = DataBaseController.GetItem(prefab);
                _weaponModuls.Add(e);
                e.transform.SetParent(WeaponsLayout);
                e.gameObject.SetActive(false);
            }
        }
    }

    public void OnClickToggle()
    {
        HolderBigInfo.gameObject.SetActive(ToggleShip.isOn);
    }
    
    public void Init(ShipBase ship)
    {
        EngineUI.Init(ship);
//        PilotBattleUI.Init(ship.PilotParameters);
        ShipBattleUI.Init(ship.ShipParameters,ship.PilotParameters);
        ShipName.text = ship.Id.ToString();
        ClearAllModuls();
        SlidersInfo.Init(ship);
        _ship = ship;
        _ship.OnStop += OnStop;
        CurActionUI.Init(_ship);
        HolderBigInfo.gameObject.SetActive(ToggleShip.isOn);
        _ship.OnDispose += OnDispose;
        ModulsDraw(ship);
        ModulsSupportDraw(ship);

        //        Debug.Log("=======>>> " + _ship.Id + "   " );
        //        _ship.ShipParameters.DebugInfo();

        int weaponIndex = 0;

        var weapons = ship.WeaponsController.GelAllWeapons();
        WeaponsLayout.gameObject.SetActive(weapons.Count > 0);
        foreach (var baseModul in weapons)
        {
            if (baseModul != null)
            {
                var wModul = _weaponModuls[weaponIndex];
                wModul.Init(baseModul);
                weaponIndex++;
            }
        }
    }

    private void ModulsDraw(ShipBase ship)
    {
        int index = 0;
        ModulsLayout.gameObject.SetActive(ShowModuls);
        foreach (var baseModul in ship.ShipModuls.Moduls)
        {
            var m = _moduls[index];
            m.Init(baseModul);
            index++;
        }
    }     
    private void ModulsSupportDraw(ShipBase ship)
    {
        int index = 0;
        ModulsLayout.gameObject.SetActive(ShowModuls);
        foreach (var baseModul in ship.ShipModuls.SupportModuls)
        {
            var m = _supportModuls[index];
            m.Init(baseModul);
            index++;
        }
    }

    private bool ShowModuls => _ship.ShipModuls.SupportModuls.Count > 0 || _ship.ShipModuls.Moduls.Count > 0;

    private void OnStop(MovingObject arg1, bool arg2)
    {
        
    }

    private void OnDispose(ShipBase obj)
    {
        ClearAllModuls();
    }

    private void ClearAllModuls()
    {
        EngineUI.Dispose();
        if (_ship != null)
        {
            _ship.OnStop -= OnStop;
            foreach (var modulUi in _moduls)
            {
                if (modulUi.gameObject.activeSelf)
                {
                    modulUi.Dispose();
                    modulUi.gameObject.gameObject.SetActive(false);
                }
            }
            foreach (var supportModulUi in _supportModuls)
            {
                if (supportModulUi.gameObject.activeSelf)
                {
                    supportModulUi.gameObject.gameObject.SetActive(false);
                }
            }
            foreach (var modulUi in _weaponModuls)
            {
                if (modulUi.gameObject.activeSelf)
                {
                    modulUi.Dispose();
                    modulUi.gameObject.gameObject.SetActive(false);
                }
            }
//            Debug.Log("<<<<=======" + _ship.Id);
//            _ship.ShipParameters.DebugInfo();
            _ship.OnDispose -= OnDispose;
            SlidersInfo.Dispose();
            //            _ship.ShipParameters.DebugInfo();
        }
    }
}

