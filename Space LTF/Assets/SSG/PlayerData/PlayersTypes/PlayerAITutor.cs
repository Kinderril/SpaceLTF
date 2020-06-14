using System.Collections.Generic;


[System.Serializable]
public class PlayerAITutor : PlayerAI
{
    public PlayerAITutor(string name)
        : base(name)
    {

    }

    public override LastReward GetReward(Player winner)
    {
        var reward = new LastReward();


        if (winner.Inventory.GetFreeSpellSlot(out var slotIndex))
        {
            var ite1m = Library.CreateSpell(SpellType.engineLock);
            winner.Inventory.TryAddSpellModul(ite1m, slotIndex);
            reward.Spells.Add(ite1m);
        } 
        reward.Money = 50;
        return reward;
    }
}

