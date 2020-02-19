using System;
using UnityEngine;


public class StringTests : MonoBehaviour
{

    void Awake()
    {
        try
        {

            var str1 = Namings.Format("1AA{0}");
            Debug.Log(str1);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }


        try
        {

            var str2 = Namings.Format("2A2A{0}", "11");
            Debug.Log(str2);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        try
        {

            var st31 = Namings.Format("3AA{0}{1}");
            Debug.Log(st31);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        try
        {

            var str41 = Namings.Format("4AA", 88);
            Debug.Log(str41);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}

