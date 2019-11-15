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


    public override void Init<T>(T obj)
    {
        _shopInventory = obj as ShopInventory;
        _greenPlayer = MainController.Instance.MainPlayer;
        
        MoneyField.Init(_greenPlayer.MoneyData.MoneyCount);
        _greenPlayer.MoneyData.OnMoneyChange += OnMoneyChange;
        _greeArmyUi = DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.PlayerArmyUIPrefab);
        _greeArmyUi.Init(_greenPlayer, MyPlayersLayout, true, new ConnectInventory(_greenPlayer.Inventory));
        base.Init(obj);
        PlayersInventory.Init(_greenPlayer.Inventory, null);
        ShoInventoryUI.Init(_shopInventory, new ConnectInventory(_greenPlayer.Inventory));
        InitValuables();
    }

    private void InitValuables()
    {
        ValuableLayout.ClearTransform();
        NotValuableLayout.ClearTransform();
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

    private void OnMoneyChange(int obj)
    {
        MoneyField.Init(_greenPlayer.MoneyData.MoneyCount);
    }

//    public override void Init()
//    {
//        base.Init();
//    }

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

