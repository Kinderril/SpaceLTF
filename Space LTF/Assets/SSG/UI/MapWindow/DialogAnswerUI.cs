using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class DialogAnswerUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI Field;
    private AnswerDialogData data;
    private Color _baseColor;
    private KeyCode _key;
    private bool _isKeyPressed = true;

    public void Init(AnswerDialogData data,int index)
    {
        switch (index)
        {
            case 1:
                _key = KeyCode.Alpha1;
                _isKeyPressed = false;
                break;
            case 2:
                _key = KeyCode.Alpha2;
                _isKeyPressed = false;
                break;
            case 3:
                _key = KeyCode.Alpha3;
                _isKeyPressed = false;
                break;
            case 4:
                _key = KeyCode.Alpha4;
                _isKeyPressed = false;
                break;
            case 5:
                _key = KeyCode.Alpha5;
                _isKeyPressed = false;
                break;
        }
        _baseColor = Field.color;
        Field.text = data.Message;
        this.data = data;
    }

    public void OnClickAnswer()
    {
        data.Callback();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Field.color = Color.yellow;
    }

    void LateUpdate()
    {
        if (_isKeyPressed)
        {
            return;
        }
        if (Input.GetKeyDown(_key))
        {
            _isKeyPressed = true;
            OnClickAnswer();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        Field.color = _baseColor;
    }
}

