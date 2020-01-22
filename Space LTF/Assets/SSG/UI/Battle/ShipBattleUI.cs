using TMPro;
using UnityEngine;


public class ShipBattleUI : MonoBehaviour
{
    public TextMeshProUGUI ShipType;
    public TextMeshProUGUI MaxSpeed;
    public TextMeshProUGUI ShieldRegenPerSec;
    public TextMeshProUGUI LevelField;

    public void Init(ShipParameters shipParameters, IPilotParameters pilotParams)
    {
        LevelField.text = Namings.Tag("Level") + ":" + pilotParams.CurLevel.ToString();
        ShipType.text = Namings.Type + ":" + shipParameters.StartParams.ShipType.ToString();
        MaxSpeed.text = Namings.Tag("Speed") + ":" + shipParameters.MaxSpeed.ToString("0.0");
        ShieldRegenPerSec.text = Namings.Regen + ":" + shipParameters.ShieldRegenPerSec.ToString("0.0");
    }

}

