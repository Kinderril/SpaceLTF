using UnityEngine;
using System.Collections;

public class FireVortex : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        OnBulletHit(other);
    }     
    void OnTriggerExit(Collider other)
    {
        OnBulletExit(other);
    }

    private void OnBulletExit(Collider other)
    {

        if (other.CompareTag(TagController.OBJECT_MOVER_TAG))
        {
            var ship = other.GetComponent<ShipHitCatcher>();

            if (ship != null)
            {
                ship.ShipBase.PeriodDamage.StopConstantFire(1f);
            }
        }

    }

    private void OnBulletHit(Collider other)
    {
        if (other.CompareTag(TagController.OBJECT_MOVER_TAG))
        {
            var ship = other.GetComponent<ShipHitCatcher>();

            if (ship != null)
            {
                ship.ShipBase.PeriodDamage.StartConstantFire(1f);
            }
        }
    }
}
