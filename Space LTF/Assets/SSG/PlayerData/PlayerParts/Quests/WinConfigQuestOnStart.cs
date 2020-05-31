
using System;

[System.Serializable]
public class WinConfigQuestOnStart : BaseQuestOnStart
{
    private ShipConfig _type;

    public WinConfigQuestOnStart(ShipConfig type, int target, EQuestOnStart typeQuest)
        : base(target, typeQuest)
    {
        _type = type;
    }

    protected override bool StageActivate(Player player)
    {
        GlobalEventDispatcher.OnWinBattle += OnWinBattle;
        return true;
    }

    private void OnWinBattle(ShipConfig obj)
    {
        if (obj == _type)
        {
            AddCount();
        }
    }


    protected override void StageDispose()
    {
        GlobalEventDispatcher.OnWinBattle -= OnWinBattle;
    }


}