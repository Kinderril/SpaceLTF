


using UnityEngine;

public class DragableSpellItem : DragableItem
{
    public BaseSpellModulInv Spell { get { return ContainerItem as BaseSpellModulInv; } }

    public override Sprite GetIcon()
    {
        return DataBaseController.Instance.DataStructPrefabs.GetSpellIcon(Spell.SpellType);
    }

    public override string GetInfo()
    {
        return Spell.GetInfo();
    }

    protected override void OnClickComplete()
    {
        if (CanShowWindow())
            WindowManager.Instance.ItemInfosController.Init(Spell);
    }
}

