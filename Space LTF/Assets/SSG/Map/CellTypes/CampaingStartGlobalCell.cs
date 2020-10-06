using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CampaingStartGlobalCell : StartGlobalCell
{
    public int Act { get; private set; }
    public CampaingStartGlobalCell(int id, int intGlobalX, int intGlobalZ, SectorData secto, ShipConfig config,int act) : base(id, intGlobalX, intGlobalZ, secto, config)
    {
        InfoOpen = true;
        Act = act;
    }

    public MessageDialogData StartCampDialog()
    {
        var baseShipConfig = MainController.Instance.MainPlayer.Army.BaseShipConfig;
        return DialogsLibrary.GetStartCampDialog(baseShipConfig, Act);
    }

    public override string Desc()
    {
        return Namings.Tag("Start point");
    }

    public override void Take()
    {

    }


    public override bool CanCellDestroy()
    {
        return true;
    }

    protected override MessageDialogData GetDialog()
    {
        return null;
    }

    public override Color Color()
    {
        return new Color(51f / 255f, 102f / 255f, 153f / 255f);
    }

    public override bool OneTimeUsed()
    {
        return true;
    }
}

