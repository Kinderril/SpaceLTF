using System.Collections.Generic;
using UnityEngine;


public class ShipVisual : MonoBehaviour
{
    public EngineEffect EngineEffect;
    public List<Renderer> SignalFlares = new List<Renderer>();

    public void Init(ShipBase shipBase)
    {
        if (EngineEffect != null)
            EngineEffect.Init(shipBase);
        var data = DataBaseController.Instance.DataStructPrefabs;
        foreach (var flare in SignalFlares)
        {
            var material = shipBase.TeamIndex == TeamIndex.green ? data.GreenFlare : data.RedFlare;
            flare.material = material;
        }
    }
}

