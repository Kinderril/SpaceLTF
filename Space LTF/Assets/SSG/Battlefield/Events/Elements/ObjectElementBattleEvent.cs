using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.UIElements;

public abstract class ObjectElementBattleEvent : MonoBehaviour
{
    protected bool _applyShip = false;
    protected bool _applyAsteroid = false;
    protected bool _applyPart = false;
    protected bool _applyBullet = false;
    void Awake()
    {
        Init();
    }

    protected abstract void Init();

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

        if (_applyShip && other.CompareTag(TagController.OBJECT_MOVER_TAG))
        {
            var ship = other.GetComponent<ShipHitCatcher>();

            if (ship != null)
            {
                ExitEvent(ship);
            }
        }
        else if (_applyAsteroid && other.CompareTag(TagController.ASTEROID_TAG))
        {
            var asteroid = other.GetComponent<Asteroid>();

            if (asteroid != null)
            {
                AsteroidExitEvent(asteroid);
            }
        }
        else if (_applyPart && other.CompareTag(TagController.DESTROYEDPART))
        {
            CheckDestroyedPartExit(other);
        }   
        else if (_applyBullet && other.CompareTag(TagController.BULLET_TAG))
        {
            var bullet = other.GetComponent<Bullet>();

            if (bullet != null)
            {
                BulletExitEvent(bullet);
            }
        }

    }

    protected virtual void BulletExitEvent(Bullet bullet)
    {

    }   
    protected virtual void BulletEnterEvent(Bullet bullet)
    {

    }

    protected virtual void CheckDestroyedPartExit(Collider other)
    {
        
    }
    protected virtual void CheckDestroyedPartEnter(Collider other)
    {
        
    }

    private void OnBulletHit(Collider other)
    {
        if (other.CompareTag(TagController.OBJECT_MOVER_TAG))
        {
            var ship = other.GetComponent<ShipHitCatcher>();

            if (ship != null)
            {
                EnterEvent(ship);
            }
        }
        if (other.CompareTag(TagController.ASTEROID_TAG))
        {
            var asteroid = other.GetComponent<Asteroid>();

            if (asteroid != null)
            {
                AsteroidEnterEvent(asteroid);
            }
        }
        else if (other.CompareTag(TagController.DESTROYEDPART))
        {
            CheckDestroyedPartEnter(other);
        }
        else if (_applyBullet && other.CompareTag(TagController.BULLET_TAG))
        {
            var bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                BulletEnterEvent(bullet);
            }
        }
    }

    protected virtual void AsteroidEnterEvent(Asteroid asteroid)
    {

    }  
    protected virtual void AsteroidExitEvent(Asteroid asteroid)
    {

    }

    protected abstract void ExitEvent(ShipHitCatcher ship);
    protected abstract void EnterEvent(ShipHitCatcher ship);
}
