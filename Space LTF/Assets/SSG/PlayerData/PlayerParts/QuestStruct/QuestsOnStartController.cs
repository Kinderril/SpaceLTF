using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum EQuestType
{
     mercStart,
     fedStart,
     deliver,
     protectFortress,
     kidnapping
}


[System.Serializable]
public class QuestsOnStartController
{
//    public const int QUESTS_TAKEN = 1;
//    public List<BaseQuestOnStart> ActiveQuests = new List<BaseQuestOnStart>();
    private List<EQuestOnStart> _allTypes;
    private float _coef;
    private PlayerQuestData _data;
    private Player _player;
    private int _questsOnStart;
    public QuestsOnStartController(float coef,PlayerQuestData data,Player player, int questsOnStart)
    {
        _data = data;
        _questsOnStart = questsOnStart;
        _player = player;
        var allTypes = (EQuestOnStart[])Enum.GetValues(typeof(EQuestOnStart));
        _allTypes = allTypes.ToList();
        _coef = coef;        
    }

    public HashSet<QuestContainer> GetStartQuests()
    {


        List<EQuestType> _list = new List<EQuestType>() { EQuestType.deliver, EQuestType.fedStart, EQuestType.kidnapping, EQuestType.mercStart, EQuestType.protectFortress };

        var half1 = _questsOnStart/2;
        var half2 = _questsOnStart - half1;
        var max = Mathf.Max(half2, half1);
        var min = Mathf.Min(half2, half1);
        var quests = new HashSet<QuestContainer>();
        for (int i = 0; i < max; i++)
        {
            var rnd = _list.RandomElement();
            _list.Remove(rnd);
            QuestContainer quets = GetQuest(rnd);
            if (quets != null)
            {
                quests.Add(quets);
            }
        }

        for (int i = 0; i < min; i++)
        {
            var selectedTypes = _allTypes.ToList().RandomElement();   
            var quest = QuestsLib.GetCalcQuest(_data, _player, selectedTypes, _coef);
            quests.Add(quest);
        }

#if UNITY_EDITOR
//        quests.Add(QuestsLib.GetDeliverQuest(_data, _player));
//        var quest1 = QuestsLib.GetCalcQuest(_data, _player, EQuestOnStart.sellModuls, _coef);
//        quests.Add(quest1);
//

#endif

        return quests;
    }

    private QuestContainer GetQuest(EQuestType rnd)
    {
        switch (rnd)
        {
            default:
            case EQuestType.mercStart:
                return QuestsLib.GetMercQuest(_data, _player);
            case EQuestType.fedStart:
                return QuestsLib.GetFedQuest(_data, _player);
            case EQuestType.deliver:
                return QuestsLib.GetDeliverQuest(_data, _player);
            case EQuestType.protectFortress:
                return QuestsLib.GetProtectQuest(_data, _player);
            case EQuestType.kidnapping:
                return QuestsLib.GetKidnappingQuest(_data, _player);
        }

    }
}