using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class AnswerDialogData
{
    public string Message;
    public Action Callback;
    public Action<AnswerDialogData> EndCallback;
    public Func<MessageDialogData> NextDialog;
    public bool ShallCompleteCell = true;

    public AnswerDialogData(string Message, [CanBeNull]Action callback = null, Func<MessageDialogData> nextDialog = null, bool shallCompleteCell = true)
    {
        ShallCompleteCell = shallCompleteCell;
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
