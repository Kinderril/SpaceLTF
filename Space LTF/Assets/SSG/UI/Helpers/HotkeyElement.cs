using UnityEngine;
using System.Collections;
using TMPro;

public class HotkeyElement : MonoBehaviour
{
    public TextMeshProUGUI NameKey;
    public TextMeshProUGUI DescKey;

    public void Init(KeyCode key)
    {
        NameKey.text = key.ToString();
        DescKey.text = Namings.KeyKode(key);
    }

}
