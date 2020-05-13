using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class ShipUIOnMap : MonoBehaviour
{
    public Slider Loaded;
    public Image WeaponLoad;
    public ShipSlidersInfo ShipSlidersInfo;
    private bool _withWeapons = true;
    public GameObject AutoActive;

    private ShipBase _ship;

    public void Init(ShipBase ship,bool withWeaons)
    {
        AutoActive.SetActive(false);
        _withWeapons = withWeaons;
        //        AttackIcon.gameObject.SetActive(false);
        //        DefenceIcon.gameObject.SetActive(false);
        //        MarksText.gameObject.SetActive(false);
        _ship = ship;
        BattleController.Instance.GreenAutoAICommander.OnSpellActivated += OnSpellActivated;
        _ship.OnDispose += OnShipDisposed;
        ShipSlidersInfo.Init(_ship);
        Loaded.gameObject.SetActive(_withWeapons);
        WeaponLoad.gameObject.SetActive(_withWeapons);
//        _ship.OnAttackRewardChange += OnAttackRewardChange;
//        _ship.OnDefenceRewardChange += OnDefenceRewardChange;
    }

    private void OnSpellActivated(bool arg1, int shipId)
    {
        if (shipId == _ship.Id)
        {
            AutoActive.SetActive(arg1);
        }
    }

    private void OnShipDisposed(ShipBase obj)
    {
        Destroy(gameObject);
    }
    

    void OnDestroy()
    {
        var battle = BattleController.Instance;
        if (battle.GreenAutoAICommander != null)
            battle.GreenAutoAICommander.OnSpellActivated -= OnSpellActivated;
        if (_ship != null)
        {
            _ship.OnDispose -= OnShipDisposed;
            ShipSlidersInfo.Dispose();
//            _ship.OnAttackRewardChange -= OnAttackRewardChange;
//            _ship.OnDefenceRewardChange -= OnDefenceRewardChange;
        }

    }

    void Update()
    {
        if (_withWeapons)
        {
            float bestLoad;
            bool isLoaded = _ship.WeaponsController.BestLoadedWeapon(out bestLoad);
            WeaponLoad.gameObject.SetActive(isLoaded);
            Loaded.gameObject.SetActive(!isLoaded);
            if (!isLoaded)
            {
                Loaded.value = 1f - bestLoad;
            }
        }
        transform.position = CamerasController.Instance.GameCamera.WorldToScreenPoint(_ship.Position) + Vector3.up*20;
    }
    
}

