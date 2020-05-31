
using System;
using System.Collections.Generic;

[System.Serializable]
public class SellItemQuestOnStart : BaseQuestOnStart
{
    private ItemType _type;
    private HashSet<ActionModulInGame> _selledModuls = new HashSet<ActionModulInGame>();

    public SellItemQuestOnStart(ItemType type, int target, EQuestOnStart typeQuest)
        : base(target, typeQuest)
    {
        _type = type;
    }

    protected override bool StageActivate(Player player)
    {
        GlobalEventDispatcher.OnSellModul += OnSellModul;
        return true;
    }

    private void OnSellModul(ActionModulInGame obj)
    {
        if (!_selledModuls.Contains(obj))
        {
            AddCount();
            _selledModuls.Add(obj);
        }
    }


    protected override void StageDispose()
    {
        GlobalEventDispatcher.OnSellModul -= OnSellModul;
    }
}