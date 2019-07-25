using UnityEngine;
using System.Collections;

public interface ISpellToGame
{
    BulleStartParameters BulleStartParameters { get; }
    WeaponInventoryAffectTarget AffectAction { get;  }
    CreateBulletDelegate CreateBulletAction { get;}
    SpellDamageData RadiusAOE();
    Bullet GetBulletPrefab();
}
