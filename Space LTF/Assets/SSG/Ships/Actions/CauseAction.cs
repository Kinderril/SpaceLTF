
using System;

public class CauseAction
{
    public string Name;
    public Func<bool> Act;

    public CauseAction(string Name, Func<bool> Act)
    {
        this.Name = Name;
        this.Act = Act;
    }
}