using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ShipVisual : MonoBehaviour
{
    public EngineEffect EngineEffect;
//    public List<MeshRenderer> SideEffects = new List<MeshRenderer>(); 

    public void Init(ShipBase shipBase)
    {
        if (EngineEffect != null)
            EngineEffect.Init(shipBase);

//        foreach (var meshRenderer in SideEffects)
//        {
//             var materialCopy = Material.Instantiate(meshRenderer.material);
//             meshRenderer.material = materialCopy;
//             var lColor = shipBase.TeamIndex == TeamIndex.green ? Color.green : Color.red;
//            meshRenderer.material.SetColor("_SpecColor", lColor);
//        }
    }
}

