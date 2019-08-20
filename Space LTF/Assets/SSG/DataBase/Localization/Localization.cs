using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class Localization
{

    private static LocLanguage _curLanguage;
    //    private LocLanguage _curLanguage;

    private static void AutoLanguage()
    {
        
    }


    private static string Get(string key)
    {
        return _curLanguage.Get(key);
    }

}
