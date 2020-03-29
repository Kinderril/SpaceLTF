using UnityEngine;
using System.Collections;

public static class LibraryChecker 
{
    public static void DoCheck()
    {
#if !UNITY_EDITOR
     return;   
#endif
        var ship = DataBaseController.Instance.DataStructPrefabs.Ships;
        foreach (var shipStruct in ship)
        {
            var data = shipStruct.ShipBase;
            if (data.RamBoostEffect == null)
            {
                Debug.LogError($"Ship {data.name} have no RamBoostEffect effect");
            }      
            if (data.MoveBoostEffect == null)
            {
                Debug.LogError($"Ship {data.name} have no MoveBoostEffect effect");
            }
        }
    }

}
