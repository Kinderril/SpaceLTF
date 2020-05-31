
using System;
using System.Collections.Generic;

[System.Serializable]
public class UpgradeItemQuestOnStart : BaseQuestOnStart
{
    private ItemType _type;

    public UpgradeItemQuestOnStart(ItemType type, int target, EQuestOnStart typeQuest)
        : base(target, typeQuest)
    {
        _type = type;
    }

    protected override bool StageActivate(Player player)
    {
        GlobalEventDispatcher.OnUpgradeWeapon += OnUpgradeWeapon;
        return true;
    }

    private void OnUpgradeWeapon(WeaponInv obj)
    {
        AddCount();
    }


    protected override void StageDispose()
    {
        GlobalEventDispatcher.OnUpgradeWeapon -= OnUpgradeWeapon;
    }
}