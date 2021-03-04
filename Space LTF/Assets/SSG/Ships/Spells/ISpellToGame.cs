using System;
using UnityEngine;

public interface ISpellToGame
{
    BulleStartParameters BulleStartParameters { get; }
    WeaponInventoryAffectTarget AffectAction { get; }
    CreateBulletDelegate CreateBulletAction { get; }
    ShallCastToTaregtAI ShallCastToTaregtAIAction { get; }
    BulletDestroyDelegate BulletDestroyDelegate { get; }
    CastActionSpell CastSpell { get; }
    SpellDamageData RadiusAOE { get; }
    Bullet GetBulletPrefab();
    //    float ShowCircle { get; }
    bool ShowLine { get; }
    void ResetBulletCreateAtion();
    SubUpdateShowCast SubUpdateShowCast { get; }
    CanCastAtPoint CanCastAtPoint { get; }
    void SetBulletCreateAction(CreateBulletDelegate bulletCreate);
    void DisposeAfterBattle();
    UpdateCastDelegate UpdateCast { get; }
    EndCastDelegateSpell EndCastPeriod { get; }
}
