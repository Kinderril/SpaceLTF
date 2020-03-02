
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

    public override void Init()
    {
        GlobalEventDispatcher.OnSellModul += OnSellModul;
    }

    private void OnSellModul(ActionModulInGame obj)
    {
        if (!_selledModuls.Contains(obj))
        {
            AddCount();
            _selledModuls.Add(obj);
        }
    }


    public override void Dispose()
    {
        GlobalEventDispatcher.OnSellModul -= OnSellModul;
    }
}