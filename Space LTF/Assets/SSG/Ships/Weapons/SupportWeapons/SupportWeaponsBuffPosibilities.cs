public class SupportWeaponsBuffPosibilities
{
    public bool BodyHeal { get; private set; }
    public bool ShieldHeal { get; private set; }
    public bool Buff { get; private set; }
    public bool HaveAny { get; private set; }

    public void AddWepon(WeaponInGame weapon)
    {
        var support = weapon as SupportWeaponInGame;
        if (support != null)
        {
            HaveAny = true;
            switch (support.BuffType)
            {
                case EWeaponBuffType.Body:
                    BodyHeal = true;
                    break;
                case EWeaponBuffType.Shield:
                    ShieldHeal = true;
                    break;
                case EWeaponBuffType.Buff:
                    Buff = true;
                    break;
            }
        }

    }
}

