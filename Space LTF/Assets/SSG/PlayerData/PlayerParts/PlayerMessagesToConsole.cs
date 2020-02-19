using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[System.Serializable]
public class PlayerMessagesToConsole
{
    private List<string> messages = new List<string>();
    [field: NonSerialized]
    public event Action<string> OnAddMessage;

    public void AddMsg(string msg)
    {
        messages.Add(ModifWithDay(msg));
        if (OnAddMessage != null)
        {
            OnAddMessage(msg);
        }
    }

    public static string ModifWithDay(string msg)
    {
        int day = MainController.Instance.MainPlayer.MapData.Step;
        return (Namings.Format("Day {0}: {1}", day, msg));
    }

    public List<string> GetAllAndClear()
    {
        var list = messages.ToList();
        messages.Clear();
        return list;
    }

    public void ClearAll()
    {
        messages.Clear();
    }
}

