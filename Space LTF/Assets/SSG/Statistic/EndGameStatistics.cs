using System;
using System.Collections.Generic;

[Serializable]
public class EndGameStatistics 
{
    public List<EndGameResult> AllResults = new List<EndGameResult>();
    public EndGameResult LastResult;

    [field: NonSerialized]
    public event Action<EndGameResult> OnAddResult;

    public EndGameStatistics()
    {

    }

    public void AddResult(EndGameResult result)
    {
        if (result.Win)
        {
            AllResults.Add(result);
        }
        LastResult = result;
        if (OnAddResult != null)
        {
            OnAddResult(result);
        }
        
    }

    public void Init()
    {
        
    }
}
