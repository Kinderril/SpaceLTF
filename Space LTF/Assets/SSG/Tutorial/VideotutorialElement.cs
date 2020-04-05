using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VideoTutorialElement : MonoBehaviour
{

    public string Id;
    public bool _isCompleted;
    public TutorialPages TutorialPages;
    public Toggle DontShowNoreToggle;
    public virtual void Init()
    {
        TutorialPages.gameObject.SetActive(false);
        _isCompleted = PlayerPrefs.GetInt(Id, 0) == 1;

    }

    protected void OpenIfNotCompleted()
    {
        if (!_isCompleted)
        {
            Open();
        }
    }

    public virtual void Open()
    {
        TutorialPages.gameObject.SetActive(true);
        TutorialPages.Init(OnClose);
    }

    protected virtual void OnClose()
    {
        if (DontShowNoreToggle.isOn)
        {
            PlayerPrefs.SetInt(Id, 1);
            _isCompleted = true;
            Dispose();
        }

        TutorialPages.gameObject.SetActive(false);
    }

    public virtual void Dispose()
    {

    }
}
