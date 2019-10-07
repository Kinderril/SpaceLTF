using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class TutorialElement : MonoBehaviour
{
    public string Id;
    public bool _isCompleted;
    public Toggle DontShowNoreToggle;
    public TextMeshProUGUI Field;
    public List<TutorialFieldElement> elements = new List<TutorialFieldElement>();
    public TextMeshProUGUI ShallCloseForever;

    void Awake()
    {
        gameObject.SetActive(false);
        _isCompleted = PlayerPrefs.GetInt(Id, 0) == 1;
    }

    public virtual void Init()
    {
        Field.text = LocalizationTutorial.GetKey(Id);
        ShallCloseForever.text = Namings.TutorCloseForever;
    }

    public void Open()
    {
        if (!_isCompleted)
        {
            gameObject.SetActive(true);
            foreach (var element in elements)
            {
                element.gameObject.SetActive(true);
            }
        }
    } 
    public void OnClose()
    {
        if (DontShowNoreToggle.isOn)
        {
            PlayerPrefs.SetInt(Id,1);
            gameObject.SetActive(false);
            foreach (var element in elements)
            {
                element.gameObject.SetActive(false);
            }
            _isCompleted = true;
            Dispose();
        }
    }

    public virtual void Dispose()
    {

    }
}
