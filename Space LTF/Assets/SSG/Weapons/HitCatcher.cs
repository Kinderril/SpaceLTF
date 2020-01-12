using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

  [RequireComponent(typeof(Collider))]
public class HitCatcher : MonoBehaviour
{
    public virtual void GetHit(IWeapon weapon, Bullet bullet)
    {

    }
}

