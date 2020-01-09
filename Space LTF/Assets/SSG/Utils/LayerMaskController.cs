using UnityEngine;
using System.Collections;

public static class LayerMaskController
{
    public static string AimGreen = "AimingGreen";
    public static string AimRed = "AimingRed";



    public static int AimingGreenLayer = LayerMask.NameToLayer(AimGreen);
    public static int AimingRedLayer = LayerMask.NameToLayer(AimRed);    
    // public static int AimingMaskGreen = LayerMask.GetMask(AimGreen);
    // public static int AimingMaskRed = LayerMask.GetMask(AimRed);
}
