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

    public virtual void Init()
    {
        gameObject.SetActive(false);
        _isCompleted = PlayerPrefs.GetInt(Id, 0) == 1;
        Field.text = LocalizationTutorial.GetKey(Id);
        ShallCloseForever.text = Namings.TutorCloseForever;
    }

    protected void OpenIfNotCompleted()
    {
        if (!_isCompleted)
        {
//            Debug.LogError($"Open tutor:{gameObject.name}  _isCompleted:{_isCompleted}");
            //            WindowManager.Instance.CurrentWindow.CanvasGroup.interactable = false;
            gameObject.SetActive(true);
            foreach (var element in elements)
            {
                element.gameObject.SetActive(true);
            }
        }
    } 
    public void OnClose()
    {
//        Debug.LogError($"Close tutor:{gameObject.name}");
        //        WindowManager.Instance.CurrentWindow.CanvasGroup.interactable = true;
        gameObject.SetActive(false);
        if (DontShowNoreToggle.isOn)
        {
            PlayerPrefs.SetInt(Id,1);
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
//        WindowManager.Instance.CurrentWindow.CanvasGroup.interactable = true;
    }
}
