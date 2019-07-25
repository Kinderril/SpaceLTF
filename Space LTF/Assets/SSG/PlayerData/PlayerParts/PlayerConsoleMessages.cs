using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PlayerConsoleMessages
{
    private List<string> messages = new List<string>();

    public void AddMsg(string msg)
    {
        messages.Add(msg);
    }

    public List<string> GetAllAndClear()
    {
        var list = messages.ToList();
        messages.Clear();
        return list;
    }
}

