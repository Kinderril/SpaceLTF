using UnityEngine;
using System.Collections;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextMeshProLocalizer : MonoBehaviour
{
    public string Tag;

    void Awake()
    {
        var txt = GetComponent<TextMeshProUGUI>();
        txt.text = Namings.Tag(Tag);
    }

}
