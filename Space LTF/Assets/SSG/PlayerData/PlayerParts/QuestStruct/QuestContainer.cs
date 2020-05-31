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

    public QuestContainer(PlayerQuestData playerQuest,QuestStage[] stages,Player player,string name,Func<MessageDialogData> endingDialog)
    {
        reward = new QuestContainerReward();
        reward.Init(90);
        _endingDialog = endingDialog;
        _name = name;
        _player = player;
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
            ReadyToComplete();
            Debug.Log($"ReadyToComplete {this}");
        }

    }

    public void Complete(Action closeWindowCallback)
    {
        if (ReadyIsComplete && !IsComplete)
        {
            var mapWindow = WindowManager.Instance.CurrentWindow as MapWindow;
            if (mapWindow)
            {
                if (closeWindowCallback!= null)
                    closeWindowCallback();
                IsComplete = true;
                OnComplete?.Invoke(this);
                _player.QuestData.CompleteQuest(this);

                void CloseObject()
                {
                    mapWindow.TakeRewardObject.gameObject.SetActive(false);
                }

                mapWindow.StartExternalDialog(_endingDialog(), () =>
                {
                    mapWindow.TakeRewardObject.Init(reward, NameQuest(),CloseObject);

                });
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
