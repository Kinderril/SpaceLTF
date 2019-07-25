
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[System.Serializable]
public class RetranslaitorMapEvent : BaseGlobalMapEvent
{
    public override string Desc()
    {
        return "Energy tower.";
    }

    public MessageDialogData TakeDialog()
    {
        var mesData = new MessageDialogData("Check map. New info of core targets.", new List<AnswerDialogData>()
        {
            new AnswerDialogData("Ok.",MapFound),
        });
        return mesData;
    }

    private void MapFound()
    {
        MainController.Instance.MainPlayer.MapData.OpenRetranslatorInfo();
    }

    public override MessageDialogData GetDialog()
    {
        var mesData = new MessageDialogData("Map found.", new List<AnswerDialogData>()
        {
            new AnswerDialogData("Take",null,TakeDialog),
        });
        return mesData;
    }

}

