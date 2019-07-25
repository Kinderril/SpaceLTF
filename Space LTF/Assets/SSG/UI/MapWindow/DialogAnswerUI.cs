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

    public void Init(AnswerDialogData data)
    {
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

    public void OnPointerExit(PointerEventData eventData)
    {

        Field.color = _baseColor;
    }
}

