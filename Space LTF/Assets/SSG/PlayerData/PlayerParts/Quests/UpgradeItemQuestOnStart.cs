
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

    public override void Init()
    {
        GlobalEventDispatcher.OnUpgradeWeapon += OnUpgradeWeapon;
    }

    private void OnUpgradeWeapon(WeaponInv obj)
    {
        AddCount();
    }


    public override void Dispose()
    {
        GlobalEventDispatcher.OnUpgradeWeapon -= OnUpgradeWeapon;
    }
}