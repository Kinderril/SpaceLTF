using System;
using TMPro;
using UnityEngine;


public class ConfirmWindow : MonoBehaviour
{
    private Action onConfirm;
    private Action onReject;
    public TextMeshProUGUI labelField;
    public TextMeshProUGUI OkField;
    public TextMeshProUGUI CancelField;
    public virtual void Init(Action onConfirm, Action onReject, string ss,string okField = null,string cancelField = null)
    {
        WindowManager.Instance.WindowMainCanvas.interactable = false;
        WindowManager.Instance.WindowSubCanvas.interactable = true;
        if (okField != null)
        {
            OkField.text = okField;
        }
        else
        {
            OkField.text = Namings.Tag("Ok");

        }
        if (cancelField != null)
        {
            CancelField.text = cancelField;
        }
        else
        {
            CancelField.text = Namings.Tag("Cancel");

        }
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
        if (gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                OnConfirmClick();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnRejectClick();
            }
        }
    }
}

