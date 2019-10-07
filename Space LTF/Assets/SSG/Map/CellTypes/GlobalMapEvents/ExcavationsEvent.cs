
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[System.Serializable]
public class ExcavationsEvent : BaseGlobalMapEvent
{
    private int weaponTryies = 0;
    private const int excvScounts = 3;

    public override string Desc()
    {
        return "Excavations";
    }

    public override MessageDialogData GetDialog()
    {
        var mesData = new MessageDialogData("You see excavations. Small fleet searching something at this place", Serach());
        return mesData;
    }

    private List<AnswerDialogData> Serach()
    {
        var ans = new List<AnswerDialogData>()
        {
            new AnswerDialogData("Start search at other side", null,  bothSearch),
            new AnswerDialogData("Come closer.", null,   comeClose),
            new AnswerDialogData(Namings.leave, null,   null),
        };
        return ans;
    }

    private MessageDialogData comeClose()
    {
        throw new NotImplementedException();
    }

    private MessageDialogData bothSearch()
    {
        MessageDialogData mesData;
        List<AnswerDialogData> ans;
        WDictionary<int> testResult = new WDictionary<int>(new Dictionary<int, float>()
        {
            { 0,ScoutsLevel},   
              {1,3},
              {2,excvScounts},
        });
        var r = testResult.Random();
        switch (r)
        {
            case 0:   //Я вин
                ans = new List<AnswerDialogData>()
                {
                    new AnswerDialogData(Namings.Ok, null,  bothSearch),
                };
                mesData = new MessageDialogData(String.Format("You found a lot of credits {0}", 200), ans);
                return mesData;
            
            case 2:   //Они вин     
                ans = new List<AnswerDialogData>()
                {
                    new AnswerDialogData(Namings.Ok, null,  bothSearch),
                };
                mesData = new MessageDialogData(String.Format("They found credits first {0}", 200), ans);
                return mesData;
        }
        mesData = new MessageDialogData("No one can't find anything", Serach());
        return mesData;

    }
}

