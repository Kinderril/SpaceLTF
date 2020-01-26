using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[System.Serializable]
public class QuestsOnStartController
{
    public const int QUESTS_TAKEN = 3;
    public List<BaseQuestOnStart> ActiveQuests = new List<BaseQuestOnStart>();
    public QuestsOnStartController(float coef)
    {
        var allTypes = (EQuestOnStart[])Enum.GetValues(typeof(EQuestOnStart));
//#if UNITY_EDITOR
//        ActiveQuests.Add(BaseQuestOnStart.Create(EQuestOnStart.killDroids, 0.1f));
//        ActiveQuests.Add(BaseQuestOnStart.Create(EQuestOnStart.winDroids, 0.1f));
//        ActiveQuests.Add(BaseQuestOnStart.Create(EQuestOnStart.laserDamage, 0.1f));
//#else
        var selectedTypes = allTypes.ToList().RandomElement(QUESTS_TAKEN);
        foreach (var eQuestOnStart in selectedTypes)
        {
            BaseQuestOnStart quest = BaseQuestOnStart.Create(eQuestOnStart, coef);
            ActiveQuests.Add(quest);
        }
        //#endif
#if UNITY_EDITOR

        BaseQuestOnStart quest1 = BaseQuestOnStart.Create(EQuestOnStart.mainShipKills, coef);
        ActiveQuests.Add(quest1);
#endif

    }

    public void InitQuests()
    {
        foreach (var baseQuestOnStart in ActiveQuests)
        {
            baseQuestOnStart.Init();
        }
    }     
    
    public void DisposeQuests()
    {
        foreach (var baseQuestOnStart in ActiveQuests)
        {
            baseQuestOnStart.Dispose();
        }
    }
}

