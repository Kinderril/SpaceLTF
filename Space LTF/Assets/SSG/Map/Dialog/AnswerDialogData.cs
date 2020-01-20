using JetBrains.Annotations;
using System;


public class AnswerDialogData
{
    public string Message;
    public Action Callback;
    public Action<AnswerDialogData> EndCallback;
    public Func<MessageDialogData> NextDialog;
    public bool ShallCompleteCell = true;
    public bool ShallReturnToLastCell = false;

    public AnswerDialogData(string Message)
    {
        SubInit(Message);
    }

    public AnswerDialogData(string Message, [CanBeNull]Action callback)
    {
        SubInit(Message, callback);
    }
    public AnswerDialogData(string Message, [CanBeNull]Action callback, Func<MessageDialogData> nextDialog)
    {
        SubInit(Message, callback, nextDialog);
    }

    public AnswerDialogData(string Message, [CanBeNull]Action callback = null, Func<MessageDialogData> nextDialog = null, bool shallCompleteCell = true, bool shallReturnToLastCell = false)
    {
        ShallCompleteCell = shallCompleteCell;
        ShallReturnToLastCell = shallReturnToLastCell;
        SubInit(Message, callback, nextDialog);
    }

    private void SubInit(string Message, [CanBeNull]Action callback = null, Func<MessageDialogData> nextDialog = null)
    {

        this.Message = Message;
        NextDialog = nextDialog;
        this.Callback = () =>
        {
            if (callback != null)
            {
                callback();
            }
            EndCallback(this);
        };
    }


    public void AddCallback(Action<AnswerDialogData> onOnAnswer)
    {
        EndCallback = onOnAnswer;

    }
}
