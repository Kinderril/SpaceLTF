using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class WindowShop : BaseWindow
{
    public Transform MyPlayersLayout;
    public InventoryUI PlayersInventory;
    public InventoryUI ShoInventoryUI;
    private ShopInventory _shopInventory;
    private Player _greenPlayer;
    private PlayerArmyUI _greeArmyUi;
    public ChangingCounter MoneyField;
    public Transform ValuableLayout;
    public Transform NotValuableLayout;
    public ObjectWithTextMeshPro ValuableIteMeshProPrefab;
    public ObjectWithTextMeshPro NotValuableIteMeshProPrefab;
    public SimpleTutorialVideo SimpleTutorialVideo;
    private bool _isTutor;

    public override void Init<T>(T obj)
    {
        SimpleTutorialVideo.Init();
        _shopInventory = obj as ShopInventory;
        _greenPlayer = MainController.Instance.MainPlayer;
        _isTutor = _shopInventory.GetAllItems().Count(x=>x!=null) <= 1;
        if (_isTutor)
        {
            SimpleTutorialVideo.Open();
        }
        MoneyField.Init(_greenPlayer.MoneyData.MoneyCount);
        _greenPlayer.MoneyData.OnMoneyChange += OnMoneyChange;
        _greeArmyUi = DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.PlayerArmyUIPrefab);
        _greeArmyUi.Init(_greenPlayer, MyPlayersLayout, true, new ConnectInventory(_greenPlayer.Inventory));
        base.Init(obj);
        PlayersInventory.Init(_greenPlayer.Inventory, null, true);
        bool canDrop = !_isTutor;
        ShoInventoryUI.Init(_shopInventory, new ConnectInventory(_greenPlayer.Inventory), canDrop);
        InitValuables();
    }

    private void InitValuables()
    {
        ValuableLayout.ClearTransform();
        NotValuableLayout.ClearTransform();
        try
        {

            foreach (var weaponType in _shopInventory.ValuableTypesWeaponList)
            {
                var valItem = DataBaseController.GetItem(ValuableIteMeshProPrefab);
                valItem.Field.text = Namings.Weapon(weaponType);
                valItem.transform.SetParent(ValuableLayout);
            }
            foreach (var modulType in _shopInventory.ValuableTypesModulsList)
            {
                var valItem = DataBaseController.GetItem(ValuableIteMeshProPrefab);
                valItem.Field.text = Namings.SimpleModulName(modulType);
                valItem.transform.SetParent(ValuableLayout);
            }
            foreach (var weaponType in _shopInventory.NotValuableTypesWeaponList)
            {
                var valItem = DataBaseController.GetItem(NotValuableIteMeshProPrefab);
                valItem.Field.text = Namings.Weapon(weaponType);
                valItem.transform.SetParent(NotValuableLayout);
            }
            foreach (var modulType in _shopInventory.NotValuableTypesModulsList)
            {
                var valItem = DataBaseController.GetItem(NotValuableIteMeshProPrefab);
                valItem.Field.text = Namings.SimpleModulName(modulType);
                valItem.transform.SetParent(NotValuableLayout);
            }
        }
        catch (Exception e)
        {
            
            Debug.LogError($"Error SHOP UI \n {e}");
        }

    }

    private void OnMoneyChange(int obj)
    {
        MoneyField.Init(_greenPlayer.MoneyData.MoneyCount);
    }

//    public override void Init()
//    {
//        base.Init();
//    }

    public override void OnToMap()
    {
        if (_isTutor)
        {
            _greenPlayer = MainController.Instance.MainPlayer;
            var haveWeapons = _greenPlayer.Inventory.Weapons.Count > 0;
            var battleShip =
                _greenPlayer.Army.Army.FirstOrDefault(x => x != null && x.Ship.ShipType != ShipType.Base);
            bool shipHaveWeapon = false;
            if (battleShip != null)
            {
                shipHaveWeapon = battleShip.Ship.WeaponsModuls.Where(x => x != null).ToList().Count > 0;
            }

            var weaponOk = shipHaveWeapon || haveWeapons;
            if (!weaponOk)
            {
                SimpleTutorialVideo.Open();
                return;
            }
        }

        base.OnToMap();
    }

    public void OnClickEnd()
    {
        MainController.Instance.ReturnToMapFromCell();
    }

    public override void Dispose()
    {
        _greenPlayer.MoneyData.OnMoneyChange -= OnMoneyChange;
        _greeArmyUi.Dispose();
        PlayersInventory.Dispose();
        ShoInventoryUI.Dispose();
        ClearTransform(MyPlayersLayout);
        base.Dispose();
    }
}

