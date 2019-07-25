using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InfoWindow : MonoBehaviour
{
    public Button OkButton;
    private Action onOK;
    public TextMeshProUGUI textField;
    public CanvasGroup MyCanvasGroup;

    public virtual void Init(Action onOK,string msg)
    {
        transform.SetAsLastSibling();
        WindowManager.Instance.CurrentWindow.CanvasGroup.interactable = false;
        this.onOK = onOK;
        textField.text = msg;
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
        MyCanvasGroup.ignoreParentGroups = true;
    }

    public void OnClickOk()
    {
        WindowManager.Instance.CurrentWindow.CanvasGroup.interactable = true;
        if (onOK != null)
            onOK();
        gameObject.SetActive(false);
    }
}

