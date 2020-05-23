using UnityEngine;
using System.Collections;

public class ReinforsmentStack
{
    public int Count;
    public float Time;
}

public class CommanderReinforcements
{
    private int nextIndex = 0;
    public ReinforsmentStack[] _stsuks;
    private bool _noOne;
                                                                                                                                                  
    public bool Remain => _stsuks.Length > nextIndex;

    public CommanderReinforcements(int count,float startPeriod,float nextPeriod)
    {
        _noOne = count == 0;
        _stsuks = new ReinforsmentStack[count];
        for (int i = 0; i < count; i++)
        {
            _stsuks[i] = new ReinforsmentStack()
            {
                Count = count, Time = startPeriod + nextPeriod * i
            };
        }
    }

    public void ManualUpdate()
    {
        if (_noOne)
        {
            return;
        }
        if (Remain)
        {
            var next = _stsuks[nextIndex];
            if (next.Time < Time.time)
            {
                SpawnReinforcement(next.Count);
                nextIndex++;
            }
        }
        else
        {
            _noOne = true;
        }
    }

    private void SpawnReinforcement(int nextCount)
    {
        

    }
}
