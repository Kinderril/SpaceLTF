public class AntiEnergyModul : AntiWeaponModul
{

    private const float DELAY_BASE = 13;
    private const float DELAY_DELTA = 3;
    public AntiEnergyModul(BaseModulInv b)
        : base(b)
    {
        _damageType = BulletDamageType.energy;
    }

    protected override float Delay()
    {
        return DELAY_BASE - ModulData.Level * DELAY_DELTA;
    }

    protected override BulletKiller GetEffect()
    {
        return DataBaseController.Instance.SpellDataBase.AntiEnergyEffect;
    }

    protected override float destroyTime => 0.5f;
}

