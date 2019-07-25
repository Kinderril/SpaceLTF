using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArmyCreatorLogs 
{
    Dictionary<int,Tuple<float, string>> _messages  = new Dictionary<int, Tuple<float, string>>();
    private float _lastRemainPoints;
    public void AddLog(float pointsRemain,string action)
    {
        if (_lastRemainPoints != 0f)
        {
            if (_lastRemainPoints < pointsRemain)
            {
                Debug.LogError($"Wrong remian points more than was _lastRemainPoints:{_lastRemainPoints}_pointsRemain:{pointsRemain}");
            }
        }

        _lastRemainPoints = pointsRemain;
        _messages.Add(_messages.Count,new Tuple<float, string>(pointsRemain,action));
    }

    public void LogToConsole()
    {
#if UNITY_EDITOR

        string info = "ARMY CREATOR:";
        foreach (var message in _messages)
        {
            info += $"\n {message.Key}. Remain:{message.Value.val1}. {message.Value.val2}";
        }
        Debug.Log(info.Green());

#endif
    }

}
