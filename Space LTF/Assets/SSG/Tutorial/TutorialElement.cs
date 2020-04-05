using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialElement : MonoBehaviour
{
    public string Id;
    public bool _isCompleted;
    public Toggle DontShowNoreToggle;
    public TextMeshProUGUI Field;
    public List<TutorialFieldElement> elements = new List<TutorialFieldElement>();
    public TextMeshProUGUI ShallCloseForever;

    public virtual void Init()
    {
        gameObject.SetActive(false);
        _isCompleted = PlayerPrefs.GetInt(Id, 0) == 1;
    }

    protected void OpenIfNotCompleted()
    {
        if (!_isCompleted)
        {
            Field.text = LocalizationTutorial.GetKey(Id);
            ShallCloseForever.text = Namings.Tag("TutorCloseForever");
            gameObject.SetActive(true);
            foreach (var element in elements)
            {
                element.gameObject.SetActive(true);
            }
        }
    }
    public void OnClose()
    {
        gameObject.SetActive(false);
        subClose();
        if (DontShowNoreToggle.isOn)
        {
            PlayerPrefs.SetInt(Id, 1);
            foreach (var element in elements)
            {
                element.gameObject.SetActive(false);
            }
            _isCompleted = true;
            Dispose();
        }
    }

    protected virtual void subClose()
    {

    }

    public virtual void Dispose()
    {
        //        WindowManager.Instance.CurrentWindow.CanvasGroup.interactable = true;
    }
}
