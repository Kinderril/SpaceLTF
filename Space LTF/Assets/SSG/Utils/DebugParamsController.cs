using UnityEngine;
using System.Collections;

public class DebugParamsController
{
#if UNITY_EDITOR  
    public static bool EngineOff { get; private set; }
    public static bool NoDamage { get; private set; }
    public static bool NoMouseMove { get; private set; }

    public static void SwitchEngine()
    {
        EngineOff = !EngineOff;
    }
    public static void SwitchNoDamage()
    {
        NoDamage = !NoDamage;
    }    
    public static void SwitchNoMouseMove()
    {
        NoMouseMove = !NoMouseMove;
    }
#endif

}
