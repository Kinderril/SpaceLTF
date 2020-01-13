using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;


public class ReputationMapUI : MonoBehaviour
{
    public ReputationElement MercField;
    public ReputationElement RaidersField;
    public ReputationElement FederationField;
    public ReputationElement OcronsField;
    public ReputationElement KriosField;
    public ReputationElement DroidField;

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
        MercField.Init(ShipConfig.mercenary);
        RaidersField.Init(ShipConfig.raiders);
        FederationField.Init(ShipConfig.federation);
        OcronsField.Init(ShipConfig.ocrons);
        KriosField.Init(ShipConfig.krios);
        DroidField.Init(ShipConfig.droid);
    }

    public void Dispose()
    {
        gameObject.SetActive(false);
        MainController.Instance.MainPlayer.ReputationData.OnReputationNationChange -= OnReputationChange;
    }
}

