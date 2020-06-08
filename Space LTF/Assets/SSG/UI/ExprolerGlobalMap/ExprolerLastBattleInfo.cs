using UnityEngine;
using System.Collections;
using TMPro;

public class ExprolerLastBattleInfo : MonoBehaviour
{
    public GameObject Win;
    public GameObject Lose;
    public TextMeshProUGUI Field;
    public void Init(ExprolerStartPlayInfo exprolerPlayInfo)
    {
        gameObject.SetActive(true);
        int unblocks = exprolerPlayInfo.Unblocks.Count;

        Win.SetActive(exprolerPlayInfo.Win);
        Lose.SetActive(!exprolerPlayInfo.Win);

        var cell = exprolerPlayInfo.Cell;
        var tag = exprolerPlayInfo.Win?"MissionComplete":"MissonFail";
        var misson = Namings.Tag(tag);
        string info =
            $"{Namings.Tag("Galaxy")}: {cell.Id}\n{Namings.ShipConfig(cell.Config)}\n{misson}";
        if (unblocks > 0)
        {
            string unblocksInfo = $"{Namings.Tag("unblockGalaxies")}: {unblocks}";
            info = $"{info}\n{unblocksInfo}";
        }

        Field.text = info;
    }

    public void OnClose()
    {
        gameObject.SetActive(false);

    }
}
