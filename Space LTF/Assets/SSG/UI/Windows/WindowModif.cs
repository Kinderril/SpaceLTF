using UnityEngine;


public class WindowModif : MonoBehaviour
{
    public Transform MainShipModificatorsLayout;
    //    public Transform WeaponsModificationsLayout;

    public MainShipUpgradeUI ModifPrefab;
    //    public UpgradeWeaponUI WeaponUpgPrefab;
    public ChangingCounter MoneyField;
    private Player _player;

    public void Enable()
    {
        _player = MainController.Instance.MainPlayer;
        InitParams(_player);
        //        InitWeapons(_player.Army);
        //        InitWeapons2(_player.Inventory);
        MoneyField.Init(_player.MoneyData.MoneyCount);
        _player.MoneyData.OnMoneyChange += OnMoneyChange;
    }

    //    private void InitWeapons2(PlayerInventory inventory)
    //    {
    //        foreach (var weaponInv in inventory.Weapons)
    //        {
    //            var weaponInfo = DataBaseController.GetItem(WeaponUpgPrefab);
    //            weaponInfo.transform.SetParent(WeaponsModificationsLayout);
    //            weaponInfo.Init(weaponInv);
    //        }
    //    }

    private void OnMoneyChange(int obj)
    {
        MoneyField.Init(_player.MoneyData.MoneyCount);
    }

    private void InitParams(Player player)
    {
        AddParameter(player.Parameters.ChargesCount);
        AddParameter(player.Parameters.ChargesSpeed);
        AddParameter(player.Parameters.Scouts);
        AddParameter(player.Parameters.Repair);
        AddParameter(player.Parameters.EnginePower);
    }

    private void AddParameter(PlayerParameter pParameter)
    {
        var pp = DataBaseController.GetItem(ModifPrefab);
        pp.Init(pParameter);
        pp.transform.SetParent(MainShipModificatorsLayout, false);
    }

    public void Disable()
    {
        if (_player != null)
            _player.MoneyData.OnMoneyChange -= OnMoneyChange;
        MainShipModificatorsLayout.ClearTransform();
        //        WeaponsModificationsLayout.ClearTransform();
    }

    public void OnClickEnd()
    {
        //        MainController.Instance.ReturnToMapFromCell();
    }
}

