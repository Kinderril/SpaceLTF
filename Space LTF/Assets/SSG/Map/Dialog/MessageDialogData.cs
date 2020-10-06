using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//[System.Serializable]
public class MessageDialogData
{
    public string Message;
    public List<AnswerDialogData> Answers;

    private bool _activateAnyway = false;
//    public event Action OnAnswer;

    public MessageDialogData(string Message, List<AnswerDialogData> Answers,bool activateAnyway = false)
    {
        _activateAnyway = activateAnyway;
        this.Message = Message;
        int number = 1;
        foreach (var answer in Answers)
        {
            answer.Message = Namings.Format("{0}. {1}", number,answer.Message);
            number++;
        }
        this.Answers = Answers;
    }

    public void InitDialog(Action<AnswerDialogData> OnAnswer)
    {
        foreach (var answerDialogData in Answers)
        {
            answerDialogData.AddCallback(OnAnswer);
        }
    }

    public bool ActivateAnyWay()
    {

        return _activateAnyway;
    }

}

