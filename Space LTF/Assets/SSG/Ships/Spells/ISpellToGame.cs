using UnityEngine;
using System.Collections;

public interface ISpellToGame
{
    BulleStartParameters BulleStartParameters { get; }
    WeaponInventoryAffectTarget AffectAction { get;  }
    CreateBulletDelegate CreateBulletAction { get;}
    CastActionSpell CastSpell { get;}
    SpellDamageData RadiusAOE();
    Bullet GetBulletPrefab();
}
