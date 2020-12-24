using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PlayerQuestData
{
//    public int mainElementsFound = 0;
//    public int MaxMainElements = 4;
    public FinalBattleData LastBattleData { get; private set; }

    public QuestsOnStartController QuestsOnStartController;

    //    [field: NonSerialized]
    //    public event Action OnElementFound;   

    [field: NonSerialized]
    public event Action<string> OnQuestId;  
    [field: NonSerialized]
    public event Action<QuestContainer> OnQuestAdd;
    [field: NonSerialized]
    public event Action<QuestContainer> OnReadyToComplete;
    [field: NonSerialized]
    public event Action<QuestContainer> OnComplete;  
    [field: NonSerialized]
    public event Action<QuestContainer> OnStageChange;

    private List<QuestContainer> _quests = new List<QuestContainer>();
    private QuestContainer _activeQuest;
    private Player _player;
    private int _currentCompleteQuest = 0;
    private int _questsToActivate = 0;
    private EGameMode _eGameMode;
    private Queue<Func<MessageDialogData>> _queueDialogs = new Queue<Func<MessageDialogData>>();

    public List<QuestContainer> AllQuests => _quests;



    public PlayerQuestData(Player player,int questsOnStart)
    {
        _player = player;
        var mid1 = (Library.MIN_GLOBAL_SECTOR_SIZE + Library.MAX_GLOBAL_SECTOR_SIZE) * .5f;
        var mid2 = (Library.MIN_GLOBAL_MAP_SECTOR_COUNT + Library.MAX_GLOBAL_MAP_SECTOR_COUNT) * .5f;

        var midSize = mid1 * mid2;
        var size = ((float)(_player.MapData.GalaxyData.SizeOfSector * _player.MapData.GalaxyData.AllSectors.Count / 3f));
        var coef = size / midSize;
        QuestsOnStartController = new QuestsOnStartController(coef,this,player, questsOnStart);
        //        mainElementsFound = 1;
        //        MaxMainElements = targetElements + 1; 
        LastBattleData = new FinalBattleData();
    }

    public void SetActiveQuest(QuestContainer active)
    {
        _activeQuest = active;
    }

    public void StartGame(EGameMode eGameMode,int act,ShipConfig config)
    {
        _eGameMode = eGameMode;
        switch (eGameMode)
        {
            case EGameMode.champaing:
                CampaingActivation(act,config);
                break;
            default:
                SandBoxActivation();
                break;
        }
    }

    private void CampaingActivation(int act,ShipConfig config)
    {
        var quests = QuestsOnStartController.GetStartCampaingQuests(act, config);
        foreach (var quest in quests)
        {
            AddQuest(quest);
        }

    }
    private void SandBoxActivation()
    {
        var quests = QuestsOnStartController.GetStartRandomQuests();
        foreach (var quest in quests)
        {
            AddQuest(quest);
        }

        _questsToActivate = _quests.Count;
        if (quests.Count == 0)
        {
            AddFinalQuests();
        }
    }

    public void DebugCompleteRndQuest()
    {
        var notCompleted = _quests.Where(x => !x.IsComplete).ToList();
        if (notCompleted.Count > 0)
        {
            var rnd = notCompleted.RandomElement();
            rnd.ReadyIsComplete = true;
            rnd.Complete(null);
        }
    }

    public void AddQuest(QuestContainer quest)
    {
        if (quest == null)
        {
            Debug.LogError("Try add null quest");
            return;
        }
        _quests.Add(quest);
        _activeQuest = quest;
        OnQuestAdd?.Invoke(quest);
    }

    public void AfterLoadCheck()
    {
        foreach (var questContainer in _quests)
        {
            questContainer.SafeInit();
            questContainer.ReActivateStageIfStarted();
        }
    }


    public void AddFinalQuests()
    {
        var finalQuest = QuestsLib.GetFinalQuest(this, _player);
        AddQuest(finalQuest);
    }

    public void QuestIdComplete(string keyComplete)
    {
        Debug.Log($"QuestIdComplete global {keyComplete}");
        OnQuestId?.Invoke(keyComplete);
    }

    public QuestContainer GetCurActiveQuest()
    {
        if (_activeQuest != null && !_activeQuest.ReadyIsComplete)
        {
            return _activeQuest;
        }

        foreach (var questContainer in _quests)
        {
            if (!questContainer.ReadyIsComplete)
            {
                return questContainer;
            }
        }

        return null;

    }

    public void CompleteQuest(QuestContainer questContainer)
    {
        _currentCompleteQuest++;
        if (_eGameMode == EGameMode.sandBox)
        {
            if (_currentCompleteQuest >= _questsToActivate)
            {
                AddFinalQuests();
            }
        }

        OnComplete?.Invoke(questContainer);
    }


    

    public void ReadyToCompleteQuest(QuestContainer questContainer)
    {
        OnReadyToComplete?.Invoke(questContainer);
    }

    public void StageChange(QuestContainer questContainer)
    {
        OnStageChange?.Invoke(questContainer);
    }

    public void AddLeaveDialogToQuery(Func<MessageDialogData> dialog)
    {
        Debug.Log($"AddLeaveDialogToQuery");
        _queueDialogs.Enqueue(dialog);
    }

    public bool TryGetDialog(out MessageDialogData o)
    {
        if (_queueDialogs.Count>0)
        {

            var dialog = _queueDialogs.Dequeue();
                o = dialog();
            if (o != null)
            {
                return true;
            }

        }

        o = null;
        return false;
    }
}
