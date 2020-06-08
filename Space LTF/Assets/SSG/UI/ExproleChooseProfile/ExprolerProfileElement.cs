using System;
using UnityEngine;
using System.Collections;
using TMPro;

public class ExprolerProfileElement : MonoBehaviour
{
    public TextMeshProUGUI NameField;
    public TextMeshProUGUI PowerField;
    public TextMeshProUGUI ConfigField;
    public TextMeshProUGUI MicroChipsField;
    public TextMeshProUGUI CreditsField;
//    private WindowExproleChooseProfile _window;
    private PlayerSafe _playerSafe;
    public PlayerSafe PlayerSafe => _playerSafe;
    public RectTransform ShipsLayout;
    private Action<PlayerSafe> _OnClickRemove;
    public void Init(PlayerSafe playerSafe,Action<PlayerSafe> OnClickRemove)
    {
        _OnClickRemove = OnClickRemove;
        _playerSafe = playerSafe;
        UpdateData();

    }

    public void UpdateData()
    {
        NameField.text = _playerSafe.Name;
        ShipConfig config = ShipConfig.mercenary;
        foreach (var startShipPilotData in _playerSafe.Ships)
        {
            if (startShipPilotData.Ship.ShipType == ShipType.Base)
            {
                config = startShipPilotData.Ship.ShipConfig;
            }
            var elements =
                DataBaseController.GetItem(DataBaseController.Instance.ExprolerDataBase.ExprolerProfileStartShipCell);
            elements.Init(startShipPilotData);
            elements.transform.SetParent(ShipsLayout);
        }
        var power = ArmyCreator.CalcArmyPower(_playerSafe.Ships);
        ConfigField.text = $"{Namings.ShipConfig(config)}";
        PowerField.text = $"{Namings.Tag("Power")}: {power.ToString("0")}";
        MicroChipsField.text = $"{Namings.Tag("Microchips")}: {_playerSafe.Microchips}";
        CreditsField.text = $"{Namings.Tag("Credits")}: {_playerSafe.Credits}";
    }

    public void OnClickRemove()
    {
        WindowManager.Instance.ConfirmWindow.Init(() => { _OnClickRemove(_playerSafe); },
            null, Namings.Tag("AreYouSureWAntRemoveProfile")); 
    }

    public void OnClickPlay()
    {
        MainController.Instance.Exproler.StartGame(_playerSafe);
    }
}
