using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

[Serializable]
public struct TutorPage
{
    public GameObject Object;
    public string Tag;
}

public class TutorialPages : MonoBehaviour
{
//    private GameObject _curActivePage;
    private int _curActiveIndex = 0;
    public List<TutorPage> Pages;
    public TextMeshProUGUI Name;
    public Button NextButton;
    public Button PrevButton;
    public Button CloseButton;
    public GameObject ExitConfirm;
    public TextMeshProUGUI ConfirmText;
    public TextMeshProUGUI ToggleText;
    private Action _closeCallback;
    public bool WithConfirm;

    //    void Start()
    //    {
    //        Init();
    //    }

    public void Init(Action closeCallback)
    {
        _closeCallback = closeCallback;
        CloseAllExept(0);
        ExitConfirm.SetActive(false);
        gameObject.SetActive(true);
        ConfirmText.text = Namings.Tag("ConfirmCloseTutor");
        ToggleText.text = Namings.Tag("TutorCloseForever");
    }



    public void OnPrevPage()
    {
        _curActiveIndex--;
        _curActiveIndex = Mathf.Clamp(_curActiveIndex, 0, Pages.Count - 1);   
        CloseAllExept(_curActiveIndex);
    }    

    public void OnNextPage()
    {
        _curActiveIndex++;
        _curActiveIndex = Mathf.Clamp(_curActiveIndex, 0, Pages.Count - 1);
        CloseAllExept(_curActiveIndex);
    }

    private void CloseAllExept(int index)
    {
        for (int i = 0; i < Pages.Count; i++)
        {
            var page = Pages[i];
            var shallOpen = (i == index);
            page.Object.SetActive(shallOpen);
            if (shallOpen)
            {
                Name.text = Namings.Tag(page.Tag);
            }

        }

        if (PrevButton != null)
        {
            if (Pages.Count <= 1)
            {
                PrevButton.gameObject.SetActive(false);
            }
            else
            {
                PrevButton.gameObject.SetActive(true);
            }
        }

        if (index == Pages.Count - 1)
        {
            NextButton.gameObject.SetActive(false);
            CloseButton.gameObject.SetActive(true);
        }
        else
        {
            NextButton.gameObject.SetActive(true);
            CloseButton.gameObject.SetActive(false);
        }
    }

    public void OnExit()
    {
        if (WithConfirm)
        {
            ExitConfirm.gameObject.SetActive(true);
        }
        else
        {
            if (_closeCallback != null)
                _closeCallback();
        }
    }

    public void OnCloseConfirmClick()
    {
        _closeCallback();
    }

    public void OnCloseRejectClick()
    {
        ExitConfirm.gameObject.SetActive(false);
    }

}
