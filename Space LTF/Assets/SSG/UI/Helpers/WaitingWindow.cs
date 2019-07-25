using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class WaitingWindow : MonoBehaviour
{
    public Text textField;
    public virtual void Init(string msg)
    {
        textField.text = msg;
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}

