using UnityEngine;
using System.Collections;
using TMPro;

public class CampLoadSlot : MonoBehaviour
{
    public TextMeshProUGUI Field;

    public void Init(string name)
    {
        Field.text = name;
    }

    public void OnClick()
    {
        MainController.Instance.Campaing.CampaingLoader.TryLoad(Field.text);
    }
}
