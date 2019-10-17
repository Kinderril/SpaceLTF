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
        WindowManager.Instance.WindowMainCanvas.interactable = false;
        WindowManager.Instance.WindowSubCanvas.interactable = true;
        this.onOK = onOK;
        textField.text = msg;
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
        MyCanvasGroup.ignoreParentGroups = true;
    }

    public void OnClickOk()
    {
        WindowManager.Instance.WindowMainCanvas.interactable = true;
        WindowManager.Instance.WindowSubCanvas.interactable = false;
        if (onOK != null)
            onOK();
        gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            OnClickOk();
        }
    }

    public void Close()
    {
        WindowManager.Instance.WindowMainCanvas.interactable = true;
        WindowManager.Instance.WindowSubCanvas.interactable = false;
        gameObject.SetActive(false);
    }
}

