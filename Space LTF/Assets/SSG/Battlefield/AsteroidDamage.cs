using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class AsteroidDamage : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag(TagController.OBJECT_MOVER_TAG))
        {
            var ship = collider.GetComponent<ShipBase>();
            if (ship != null)
            {
//                ship.AsteroidDamage.AsteroidDamage(true);
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag(TagController.OBJECT_MOVER_TAG))
        {
            var ship = collider.GetComponent<ShipBase>();
            if (ship != null)
            {
//                ship.AsteroidDamage.AsteroidDamage(false);
            }
        }
    }
}

