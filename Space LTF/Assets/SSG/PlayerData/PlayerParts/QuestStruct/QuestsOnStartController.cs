using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum EQuestType
{
    searchAndkill,
    fedStart,
    deliver,
    protectFortress,
    kidnapping ,
    cmStart,
    cmMerc0,
    cmFed0,
    cmKrs0,
    cmOcr0,
    cmRdr0,
    cmOcr1_1,
    cmOcr1_2,
    cmOcr1_3,
    cmOcr1_4,
    cmOcr2_2,
    cmOcr2_3,
    cmOcr2_4,
    cmOcr3_1,
    cmOcr3_2,
    cmOcr3_3,
    cmMerc1_1,
    cmMerc1_2,
    cmMerc1_3,
    cmMerc1_4,
    cmMerc2_1,
    cmMerc2_2,
    cmMerc2_3,
    cmMerc2_4,
    cmOcr2_1,
    cmFed2_1,
    cmRdrc2_1,
    cmKrs2_1,
    cmMerc3_1,
    cmMerc3_2,
    cmMerc3_3,
    cmMerc4_1
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


    public HashSet<QuestContainer> GetStartRandomQuests()
    {
        List<EQuestType> _list = new List<EQuestType>() { EQuestType.deliver, EQuestType.fedStart, EQuestType.kidnapping, EQuestType.searchAndkill, EQuestType.protectFortress };
        var half1 = _questsOnStart/2;
        var half2 = _questsOnStart - half1;
        var max = Mathf.Max(half2, half1);
        var min = Mathf.Min(half2, half1);
        var quests = new HashSet<QuestContainer>();
        for (int i = 0; i < max; i++)
        {
            var rnd = _list.RandomElement();
            _list.Remove(rnd);
            QuestContainer quets = GetQuest(rnd,null);
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

    public QuestContainer GetQuest(EQuestType rnd, Action endQuestCallback)
    {
        switch (rnd)
        {
            case EQuestType.cmMerc0:
                return QuestsLib.CmFraction0(ShipConfig.mercenary,_data, _player,endQuestCallback);      
            case EQuestType.cmFed0:
                return QuestsLib.CmFraction0(ShipConfig.federation,_data, _player,endQuestCallback);      
            case EQuestType.cmKrs0:
                return QuestsLib.CmFraction0(ShipConfig.krios,_data, _player,endQuestCallback);      
            case EQuestType.cmOcr0:
                return QuestsLib.CmFraction0(ShipConfig.ocrons, _data, _player, endQuestCallback);
            case EQuestType.cmRdr0:
                return QuestsLib.CmFraction0(ShipConfig.raiders,_data, _player,endQuestCallback); 

            case EQuestType.cmOcr1_1:
                return QuestsLib.CmOcr1_1(_data, _player, endQuestCallback);    
            case EQuestType.cmOcr1_2:
                return QuestsLib.CmOcr1_2(_data, _player, endQuestCallback);    
            case EQuestType.cmOcr1_3:
                return QuestsLib.CmOcr1_3(_data, _player, endQuestCallback);    
            case EQuestType.cmOcr1_4:
                return QuestsLib.CmOcr1_4(_data, _player, endQuestCallback);   
            case EQuestType.cmOcr2_1:
                return QuestsLib.CmOcr2_1(_data, _player, endQuestCallback);    
            case EQuestType.cmOcr2_2:
                return QuestsLib.CmOcr2_2(_data, _player, endQuestCallback);    
            case EQuestType.cmOcr2_3:
                return QuestsLib.CmOcr2_3(_data, _player, endQuestCallback);    
            case EQuestType.cmOcr2_4:
                return QuestsLib.CmOcr2_4(_data, _player, endQuestCallback);
            case EQuestType.cmOcr3_1:
                return QuestsLib.CmOcr3_1(_data, _player, endQuestCallback);
            case EQuestType.cmOcr3_2:
                return QuestsLib.CmOcr3_2(_data, _player, endQuestCallback);
            case EQuestType.cmOcr3_3:
                return QuestsLib.CmOcr3_3(_data, _player, endQuestCallback);

            case EQuestType.cmMerc1_1:
                return QuestsLib.CmMerc1_1(_data, _player,endQuestCallback);   
            case EQuestType.cmMerc1_2:
                return QuestsLib.CmMerc1_2(_data, _player,endQuestCallback);   
            case EQuestType.cmMerc1_3:
                return QuestsLib.CmMerc1_3(_data, _player,endQuestCallback);       
            case EQuestType.cmMerc1_4:
                return QuestsLib.CmMerc1_4(_data, _player,endQuestCallback);
            case EQuestType.cmMerc2_1:
                return QuestsLib.CmMerc2_1(_data, _player,endQuestCallback);   
            case EQuestType.cmMerc2_2:
                return QuestsLib.CmMerc2_2(_data, _player,endQuestCallback);   
            case EQuestType.cmMerc2_3:
                return QuestsLib.CmMerc2_3(_data, _player,endQuestCallback);       
            case EQuestType.cmMerc2_4:
                return QuestsLib.CmMerc2_4(_data, _player,endQuestCallback);  
            case EQuestType.cmMerc3_1:
                return QuestsLib.CmMerc3_1(_data, _player,endQuestCallback);  
            case EQuestType.cmMerc3_2:
                return QuestsLib.CmMerc3_2(_data, _player,endQuestCallback);  
            case EQuestType.cmMerc3_3:
                return QuestsLib.CmMerc3_3(_data, _player,endQuestCallback);    


            case EQuestType.searchAndkill:
                return QuestsLib.SearchAndKill(_data, _player);
            case EQuestType.fedStart:
                return QuestsLib.GetFedQuest(_data, _player);
            case EQuestType.deliver:
                return QuestsLib.GetDeliverQuest(_data, _player);
            case EQuestType.protectFortress:
                return QuestsLib.GetProtectQuest(_data, _player);
            case EQuestType.kidnapping:
                return QuestsLib.GetKidnappingQuest(_data, _player);
            case EQuestType.cmStart:
                return QuestsLib.CmStartQuest(_data, _player, endQuestCallback);
            default:
                return null;
        }

    }

    public IEnumerable<QuestContainer> GetStartCampaingQuests(int act,ShipConfig config)
    {
        var quests = new HashSet<QuestContainer>();
        switch (act)
        {
            case 0:
                quests.Add(GetQuest(EQuestType.cmStart, EndStartQuest));
                break;
            case 1:
                switch (config)
                {
                    case ShipConfig.raiders:
                        Debug.LogError("TODO");
                        break;
                    case ShipConfig.federation:
                        Debug.LogError("TODO");
                        break;
                    case ShipConfig.mercenary:
                        quests.Add(GetQuest(EQuestType.cmMerc2_1, EndMerc2_1));
                        break;
                    case ShipConfig.ocrons:
                        quests.Add(GetQuest(EQuestType.cmOcr2_1, EndOcr2_1));
                        break;
                    case ShipConfig.krios:
                        Debug.LogError("TODO");
                        break;
                }
                break;
            case 2:

                switch (config)
                {
                    case ShipConfig.raiders:
                        Debug.LogError("TODO");
                        break;
                    case ShipConfig.federation:
                        Debug.LogError("TODO");
                        break;
                    case ShipConfig.mercenary:
                        quests.Add(GetQuest(EQuestType.cmMerc3_1, EndMerc3_1));
                        break;
                    case ShipConfig.ocrons:
                        quests.Add(GetQuest(EQuestType.cmOcr3_1, EndOcr3_1));
                        break;
                    case ShipConfig.krios:
                        Debug.LogError("TODO");
                        break;
                }
                break;
        }
        return quests;

    }


    private void EndStartQuest()
    {
        _data.AddQuest(GetQuest(EQuestType.cmMerc0, EndStartMerc));

#if UNITY_EDITOR
        _data.AddQuest(GetQuest(EQuestType.cmFed0, EndStartFed));
        _data.AddQuest(GetQuest(EQuestType.cmRdr0, EndStartRdr));
        _data.AddQuest(GetQuest(EQuestType.cmKrs0, EndStartKrs));
        _data.AddQuest(GetQuest(EQuestType.cmOcr0, EndStartOcr));
#endif
    }


    private void EndStartKrs()
    {
    }

    private void EndStartRdr()
    {
    }

    private void EndStartFed()
    {
    }

    #region OCR_ACT1
    private void EndOcr2_1()
    {

        _data.AddQuest(GetQuest(EQuestType.cmOcr2_2, EndOcr2_2));
    }
    private void EndOcr2_2()
    {
        _data.AddQuest(GetQuest(EQuestType.cmOcr2_3, EndOcr2_3));
    }

    private void EndOcr2_3()
    {
        _data.AddQuest(GetQuest(EQuestType.cmOcr2_4, EndOcr2_4));
    }

    private void EndOcr2_4()
    {
        MainController.Instance.BattleData.EndGame(true);//End act 2 Ocr
    }

    #endregion

    #region OCR_ACT3  
    private void EndOcr3_1()
    {
        _data.AddQuest(GetQuest(EQuestType.cmOcr3_2, EndOcr3_2));
    }

    private void EndOcr3_2()
    {
        _data.AddQuest(GetQuest(EQuestType.cmOcr3_3, EndOcr3_3));
    }

    private void EndOcr3_3()
    {
        MainController.Instance.BattleData.EndGame(true);//End act 2 Merc
    }
    #endregion

    #region OCR_ACT1

    private void EndStartOcr()
    {
        _data.AddQuest(GetQuest(EQuestType.cmOcr1_1, EndOcr1_1));
    }
    private void EndOcr1_1()
    {
        _data.AddQuest(GetQuest(EQuestType.cmOcr1_2, EndOcr1_2));
    }
    private void EndOcr1_2()
    {
        _data.AddQuest(GetQuest(EQuestType.cmOcr1_3, EndOcr1_3));
    }
    private void EndOcr1_3()
    {
        _data.AddQuest(GetQuest(EQuestType.cmOcr1_4, EndOcr1_4));
    }
    private void EndOcr1_4()
    {
        _player.ReputationData.SetAllies(ShipConfig.ocrons);
        MainController.Instance.BattleData.EndGame(true);//End act 1
    }
    #endregion 
    
    #region MERC_ACT1
    private void EndStartMerc()
    {
        _data.AddQuest(GetQuest(EQuestType.cmMerc1_1, EndMerc1_1));
    }
    private void EndMerc1_1()
    {
        _data.AddQuest(GetQuest(EQuestType.cmMerc1_2, EndMerc1_2));
    }
    private void EndMerc1_2()
    {
        _data.AddQuest(GetQuest(EQuestType.cmMerc1_3, EndMerc1_3));
    }
    private void EndMerc1_3()
    {
        _data.AddQuest(GetQuest(EQuestType.cmMerc1_4, EndMerc1_4));
    }
    private void EndMerc1_4()
    {
        _player.ReputationData.SetAllies(ShipConfig.mercenary);
        MainController.Instance.BattleData.SetCampWinAct();
        MainController.Instance.BattleData.EndGame(true);//End act 1
    }
    #endregion

#region MERC_ACT2
    private void EndMerc2_1()
    {
        _data.AddQuest(GetQuest(EQuestType.cmMerc2_2, EndMerc2_2));
    }

    private void EndMerc2_2()
    {
        _data.AddQuest(GetQuest(EQuestType.cmMerc2_3, EndMerc2_3));
    }

    private void EndMerc2_3()
    {
        _data.AddQuest(GetQuest(EQuestType.cmMerc2_4, EndMerc2_4));
    }

    private void EndMerc2_4()
    {
        MainController.Instance.BattleData.EndGame(true);//End act 2 Merc
    }
#endregion

#region MERC_ACT3  
    private void EndMerc3_1()
    {
        _data.AddQuest(GetQuest(EQuestType.cmMerc3_2, EndMerc3_2));
    }

    private void EndMerc3_2()
    {
        _data.AddQuest(GetQuest(EQuestType.cmMerc3_3, EndMerc3_3));
    }

    private void EndMerc3_3()
    {
        MainController.Instance.BattleData.EndGame(true);//End Merc
    }
#endregion

}