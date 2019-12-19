
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

    public override void Init()
    {
        GlobalEventDispatcher.OnWinBattle += OnWinBattle;
    }

    private void OnWinBattle(ShipConfig obj)
    {
        if (obj == _type)
        {
            AddCount();
        }
    }


    public override void Dispose()
    {
        GlobalEventDispatcher.OnWinBattle -= OnWinBattle;
    }
}