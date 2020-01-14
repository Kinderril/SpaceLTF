﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public delegate void DialogEndsCallback(bool shallCompleteCell);

public class DialogWindow : MonoBehaviour
{
    public Transform Layout;
    public TextMeshProUGUI MessageField;
    public DialogAnswerUI DialogAnswerPrefab;
    private MessageDialogData _dialog;
    private DialogEndsCallback _endCallback;

    public void Init(MessageDialogData dialog, DialogEndsCallback endCallback)
    {
        Utils.ClearTransform(Layout);
        gameObject.SetActive(true);
        MessageField.text = dialog.Message;
        int index = 1;
        foreach (var answerDialogData in dialog.Answers)
        {
            var dialogAnswer = DataBaseController.GetItem(DialogAnswerPrefab);
            dialogAnswer.transform.SetParent(Layout);
            dialogAnswer.Init(answerDialogData, index);
            index++;
        }

        _endCallback = endCallback;
        _dialog = dialog;
        dialog.InitDialog(OnAnswer);
    }

    private void OnAnswer(AnswerDialogData data)
    {
        if (data.NextDialog == null)
        {
            _endCallback(data.ShallCompleteCell);
            Dispose();
        }
        else
        {
            Init(data.NextDialog(), _endCallback);
        }
    }

    public void Dispose()
    {
        gameObject.SetActive(false);
    }
}

