using UnityEngine;
using System.Collections;
using TMPro;

public class TutorialFieldElement : MonoBehaviour
{
    public TextMeshProUGUI Field;
    public string KeyId;

    void Awake()
    {
        gameObject.SetActive(false);
        Field.text = LocalizationTutorial.GetKey(KeyId);
    }
}
