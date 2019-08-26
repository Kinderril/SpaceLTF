using UnityEngine;
using System.Collections;

public class PilotParamsInUI : IShipAffectableParams
{
    public float MaxHealth { get; set; }
    public float MaxSpeed { get; set; }
    public float TurnSpeed { get; set; }
    public float MaxShield { get; set; }
}
