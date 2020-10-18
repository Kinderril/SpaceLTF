using UnityEngine;
using System.Collections;
using TMPro;

public class WindowCampaingEnd : BaseWindow
{
    public TextMeshProUGUI Field;
//    public TextMeshProUGUI Field;
    public override void Init()
    {
        var player = MainController.Instance.Campaing.PlayerChampaingContainer;
        var curAct = player.Act;
        Field.text = Namings.Format(Namings.Tag("Actend"), (curAct+1));
        base.Init();
    }

    public void OnClickNextAct()
    {
        var player = MainController.Instance.Campaing.PlayerChampaingContainer;
        player.EndAct();
    }

}
