using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ConfirmWindow : MonoBehaviour
{
    private Action onConfirm;
    private Action onReject ;
    public TextMeshProUGUI labelField;
    public virtual void Init(Action onConfirm, Action onReject,string ss)
    {
        WindowManager.Instance.WindowMainCanvas.interactable = false;
        WindowManager.Instance.WindowSubCanvas.interactable = true;
        this.onConfirm = onConfirm;
        this.onReject = onReject;
        labelField.text = ss;
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
    }

    public virtual void OnConfirmClick()
    {
        if (onConfirm != null)
            onConfirm();
        gameObject.SetActive(false);
        WindowManager.Instance.WindowMainCanvas.interactable = true;
        WindowManager.Instance.WindowSubCanvas.interactable = false;
    }
    public virtual void OnRejectClick()
    {
        if (onReject != null)
            onReject();
        gameObject.SetActive(false);
        WindowManager.Instance.WindowMainCanvas.interactable = true;
        WindowManager.Instance.WindowSubCanvas.interactable = false;
    }
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            OnConfirmClick();
        }
        else
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnRejectClick();
        }
    }
}

