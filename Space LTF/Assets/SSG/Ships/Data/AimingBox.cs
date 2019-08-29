using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class AimingBox : MonoBehaviour
{
    private ShipBase _owner;

    public void Init(ShipBase owner)
    {
        _owner = owner;
        gameObject.layer = _owner.TeamIndex == TeamIndex.green
            ? LayerMaskController.AimingGreenLayer
            : LayerMaskController.AimingRedLayer;
    }



}
