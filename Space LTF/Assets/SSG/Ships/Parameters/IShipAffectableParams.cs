using UnityEngine;
using System.Collections;

public interface IShipAffectableParams 
{
   float MaxHealth { get; set; }
   float MaxSpeed { get; set; }
   float TurnSpeed { get; set; }
   float MaxShield { get; set; }

}
