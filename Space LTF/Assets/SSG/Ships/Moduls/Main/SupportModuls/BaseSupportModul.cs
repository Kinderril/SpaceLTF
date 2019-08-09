using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class BaseSupportModul :  BaseModulInv
{
    public BaseSupportModul(SimpleModulType type, int level)
        : base(type, level)
    {

    }

    public override bool IsSupport => true;

    protected virtual bool AffectTargetImplement => false;

    protected virtual WeaponInventoryAffectTarget AffectTarget(WeaponInventoryAffectTarget affections)
    {
        return affections;
    }

    public void ChangeTargetAffect(IAffectable weaponInGame)
    {
        if (AffectTargetImplement)
        {
            AffectTarget(weaponInGame.AffectAction);
        }
    }

    protected virtual bool BulletImplement => false;

    protected virtual CreateBulletDelegate BulletCreate(CreateBulletDelegate standartDelegate)
    {
        return standartDelegate;
    }




    public void ChangeBullet(IAffectable weaponInGame)
    {
        if (BulletImplement)
        {
            var modif = BulletCreate(weaponInGame.CreateBulletAction);
            weaponInGame.SetBulletCreateAction(modif);
        }
    }
    //public abstract bool ParamsImplement { get; }
    public virtual void ChangeParams(IAffectParameters weapon)
    {

    }

    public virtual void ChangeParamsShip(IShipAffectableParams weapon)
    {

    }

    public abstract string DescSupport();

}
