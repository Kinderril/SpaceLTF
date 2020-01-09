using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;


public class ReputationMapUI : MonoBehaviour
{
    public TextMeshProUGUI MercField;
    public TextMeshProUGUI RaidersField;
    public TextMeshProUGUI FederationField;
    public TextMeshProUGUI OcronsField;
    public TextMeshProUGUI KriosField;
    public TextMeshProUGUI DroidField;

    public void Init()
    {
        gameObject.SetActive(true);
        UpdateData();
        MainController.Instance.MainPlayer.ReputationData.OnReputationNationChange += OnReputationChange;
    }

    private void OnReputationChange(ShipConfig config, int curval, int delta)
    {
        UpdateData();
    }

    public void UpdateData()
    {
        MercField.text = Info(ShipConfig.mercenary);
        RaidersField.text = Info(ShipConfig.raiders);
        FederationField.text = Info(ShipConfig.federation);
        OcronsField.text = Info(ShipConfig.ocrons);
        KriosField.text = Info(ShipConfig.krios);
        DroidField.text = Info(ShipConfig.droid);
    }

    private string Info(ShipConfig mercenary)
    {
        var rep = MainController.Instance.MainPlayer.ReputationData;
        return String.Format(Namings.Reputation, Namings.ShipConfig(mercenary), rep.ReputationFaction[mercenary]);
    }

    public void Dispose()
    {
        gameObject.SetActive(false);
        MainController.Instance.MainPlayer.ReputationData.OnReputationNationChange -= OnReputationChange;
    }
}

