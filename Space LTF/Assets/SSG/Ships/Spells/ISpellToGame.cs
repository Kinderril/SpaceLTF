public interface ISpellToGame
{
    BulleStartParameters BulleStartParameters { get; }
    WeaponInventoryAffectTarget AffectAction { get; }
    CreateBulletDelegate CreateBulletAction { get; }
    BulletDestroyDelegate BulletDestroyDelegate { get; }
    CastActionSpell CastSpell { get; }
    SpellDamageData RadiusAOE();
    Bullet GetBulletPrefab();
    //    float ShowCircle { get; }
    bool ShowLine { get; }
    SubUpdateShowCast SubUpdateShowCast { get; }
    CanCastAtPoint CanCastAtPoint { get; }
}
