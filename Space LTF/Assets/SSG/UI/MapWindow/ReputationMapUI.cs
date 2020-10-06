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
        MainController.Instance.MainPlayer.ReputationData.OnReputationRankChange += OnReputationRankChange;
    }

    private void OnReputationRankChange(ShipConfig config, EReputationAlliesRank rank)
    {
        UpdateData();
    }

    private void OnReputationChange(ShipConfig config, int curval, int delta)
    {
        UpdateData();
    }

    public void UpdateData()
    {
        var rep = MainController.Instance.MainPlayer.ReputationData;
        MercField.Init(ShipConfig.mercenary, rep.Allies,rep.AlliesRank);
        RaidersField.Init(ShipConfig.raiders, rep.Allies, rep.AlliesRank);
        FederationField.Init(ShipConfig.federation, rep.Allies, rep.AlliesRank);
        OcronsField.Init(ShipConfig.ocrons, rep.Allies, rep.AlliesRank);
        KriosField.Init(ShipConfig.krios, rep.Allies, rep.AlliesRank);
        DroidField.Init(ShipConfig.droid, rep.Allies, rep.AlliesRank);
    }

    public void Dispose()
    {
        gameObject.SetActive(false);
        MainController.Instance.MainPlayer.ReputationData.OnReputationNationChange -= OnReputationChange;
        MainController.Instance.MainPlayer.ReputationData.OnReputationRankChange -= OnReputationRankChange;
    }
}

