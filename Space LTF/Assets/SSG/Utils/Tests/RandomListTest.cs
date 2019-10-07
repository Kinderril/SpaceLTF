using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomListTest : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {

            var list = new List<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);
            var r1 = list.RandomElement(3);
            DebugData(r1);
        }
    }

    private void DebugData(List<int> l)
    {
        string ss = "";
        foreach (var i in l)
        {
            ss = $"{i}  {ss}";
        }
        Debug.Log(ss);
    }

}
