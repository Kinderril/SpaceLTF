using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MessageDialogData
{
    public string Message;
    public List<AnswerDialogData> Answers;
//    public event Action OnAnswer;

    public MessageDialogData(string Message, List<AnswerDialogData> Answers)
    {
        this.Message = Message;
        int number = 1;
        foreach (var answer in Answers)
        {
            answer.Message = Namings.Format("{0}.{1}", number,answer.Message);
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
}

