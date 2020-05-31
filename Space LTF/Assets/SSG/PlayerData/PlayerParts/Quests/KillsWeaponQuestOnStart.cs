using System;

[System.Serializable]
public class KillsWeaponQuestOnStart : BaseQuestOnStart
{
    private WeaponType _type;

    public KillsWeaponQuestOnStart(WeaponType type, int target, EQuestOnStart typeQuest)
        : base(target, typeQuest)
    {
        _type = type;
    }

    protected override bool StageActivate(Player player)
    {
        GlobalEventDispatcher.OnShipDamage += OnShipDamage;
        return true;
    }

    private void OnShipDamage(ShipBase arg1, float shield, float body, WeaponType weaponType)
    {
        if (arg1.TeamIndex == TeamIndex.green)
        {
            if (weaponType == _type)
            {
                AddCount((int)(body+ shield));
            }
        }
    }


    protected override void StageDispose()
    {
        GlobalEventDispatcher.OnShipDamage -= OnShipDamage;
    }
}