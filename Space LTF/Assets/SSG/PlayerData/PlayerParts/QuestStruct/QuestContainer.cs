using System;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class QuestContainer
{
    public int CurStage;
    private QuestStage[] _stages;
    public QuestStage[] Stages => _stages;

    private QuestContainerReward reward;

    [field: NonSerialized]
    public event Action<QuestContainer> OnStageChange;   
    [field: NonSerialized]
    public event Action<QuestContainer> OnReadyToComplete;    
    [field: NonSerialized]
    public event Action<QuestContainer> OnComplete;

    private Player _player;
    private string _name;

    public bool ReadyIsComplete { get; set; }
    public bool IsComplete { get; private set; }                                                 
    private Func<MessageDialogData> _endingDialog;
    private bool _autoEnd;
    private Action _autoEndCallback;
    public ShipConfig? Owner;

    public QuestContainer(ShipConfig? questOwner,PlayerQuestData playerQuest,QuestStage[] stages,Player player,string name,
        Func<MessageDialogData> endingDialog, Action autoEndCallback = null)
    {
        Owner = questOwner;
        _autoEnd = autoEndCallback!= null;
        _autoEndCallback = autoEndCallback;
        reward = new QuestContainerReward();
        int rewardPower = 10;
        _player = player;
        if (questOwner.HasValue)
        {
            if (_player.ReputationData.IsAllies(questOwner.Value))
            {
                switch (_player.ReputationData.AlliesRank)
                {
                    case EReputationAlliesRank.rank1:
                        rewardPower = 21;
                        break;
                    case EReputationAlliesRank.rank2:
                        rewardPower = 26;
                        break;
                    case EReputationAlliesRank.rank3:
                        rewardPower = 31;
                        break;
                    case EReputationAlliesRank.rank4:
                        rewardPower = 36;
                        break;
                    case EReputationAlliesRank.rank5:
                        rewardPower = 41;
                        break;
                }
            }
            else
            {
                var rep = _player.ReputationData.GetStatus(questOwner.Value);
                switch (rep)
                {
                    case EReputationStatus.friend:
                        rewardPower = 26;
                        break;
                    case EReputationStatus.neutral:
                        rewardPower = 21;
                        break;
                    case EReputationStatus.negative:
                        rewardPower = 16;
                        break;
                    case EReputationStatus.enemy:
                        rewardPower = 11;
                        break;
                }
            }
        }
        else
        {
            reward.Init((int)player.Army.GetPower());
        }


        reward.Init(rewardPower);
        _endingDialog = endingDialog;
        _name = name;
        CurStage = 0;
        IsComplete = false;
        ReadyIsComplete = false;
        _stages = stages;
        for (int i = 0; i < _stages.Length; i++)
        {
            var st = _stages[i];
            st.SetController(this, playerQuest);
        }

        DoNextStage();
    }

    public QuestStage GetCurStage()
    {
        return _stages[CurStage];
    }

    public void CompleteStage(QuestStage stage)
    {
        CurStage++;
        DoNextStage();
    }

    private void DoNextStage()
    {
        if (CurStage < _stages.Length)
        {
            var curStage = GetCurStage();
            Debug.Log($"DoNextStage {curStage}");
            curStage.Activate(_player);
            OnStageChange?.Invoke(this);
            _player.QuestData.StageChange(this);
        }
        else
        {
            if (_autoEnd)
            {
                Debug.Log($"AutoComplete quest {this}");
                ReadyToComplete();
                Complete(null);
            }
            else
            {
                ReadyToComplete();
                Debug.Log($"ReadyToComplete {this}");
            }
        }

    }                                                                                                            

    public void Complete(Action closeWindowCallback)
    {
        if (ReadyIsComplete && !IsComplete)
        {
            if (_autoEnd)
            {
                IsComplete = true;
                OnComplete?.Invoke(this);
                _player.QuestData.CompleteQuest(this);
                _autoEndCallback?.Invoke();
                reward.TakeRandom();
                if (Owner.HasValue)
                {
                    _player.ReputationData.AddReputation(Owner.Value,Library.QUEST_COMPLETE_REPUTATION);
                }
            }
            else
            {

                var mapWindow = WindowManager.Instance.CurrentWindow as MapWindow;
                if (mapWindow)
                {
                    closeWindowCallback?.Invoke();
                    IsComplete = true;
                    OnComplete?.Invoke(this);
                    _player.QuestData.CompleteQuest(this);

                    void CloseObject()
                    {
                        mapWindow.TakeRewardObject.gameObject.SetActive(false);
                    }

                    mapWindow.StartExternalDialog(_endingDialog(), () =>
                    {
                        mapWindow.TakeRewardObject.Init(reward, NameQuest(), CloseObject);

                    });
                }
            }

        }
    }


    private void ReadyToComplete()
    {
        ReadyIsComplete = true;
        OnReadyToComplete?.Invoke(this);
        _player.QuestData.ReadyToCompleteQuest(this);
    }

    public string NameQuest()
    {
        return _name;
    }

//    public GlobalMapCell GetCurCellTarget()
//    {
//        var stage = GetCurStage();
//        if (stage != null)
//        {
//            return stage.GetCurCellTarget();
//        }
//
//        return null;
//    }
    public void SafeInit()
    {
        foreach (var questStage in Stages)
        {
            questStage.AfterLoad();
        }
    }

}
