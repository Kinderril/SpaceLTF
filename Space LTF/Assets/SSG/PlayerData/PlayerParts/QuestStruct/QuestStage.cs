using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public abstract class QuestStage
{
    private QuestContainer _questContainer;
    protected PlayerQuestData _playerQuest;
    protected Player _player;
    protected bool _activated = false;
    protected bool _complete = false;
    protected bool _failed = false;

    public bool Activated => _activated;
    public bool IsComplete => _complete;
    public bool Failed => _failed;

    [field: NonSerialized]
    public event Action<QuestStage> OnFailed;  

    [field: NonSerialized]
    public event Action<QuestStage> OnComplete;   

    [field: NonSerialized]
    public event Action<QuestStage> OnTextChange;  
    [field: NonSerialized]
    public event Action<QuestStage> OnStageActivated;

    public void AfterLoad()
    {
        if (!_complete && _activated)
            SubAfterLoad();
    }

    protected virtual void SubAfterLoad()
    {

    }
    protected abstract bool StageActivate(Player player);

    protected abstract void StageDispose();

    private Dictionary<string,bool> _idToComplete = new Dictionary<string, bool>();

    protected QuestStage(string id)
    {
        _idToComplete.Add(id,false);
    }

    protected QuestStage(params string[] id)
    {
        foreach (var s in id)
        {
            _idToComplete.Add(s, false);
        }
    }

    public override string ToString()
    {
        string ids = "ids:";
        foreach (var b in _idToComplete)
        {
            ids = $"{ids}_{b}";
        }

        var str = $"{base.ToString()}_{ids}";
        return str;
    }

    protected int comparator(SectorCellContainer x, SectorCellContainer y)
    {

        return x.Data.Id > y.Data.Id ? 1 : -1;
    }
    protected virtual Func<MessageDialogData> AfterCompleteDialog()
    {
        return null;
    }
    protected List<string> GetDialogsTagAttack()
    {
        var list = new List<string>();
        list.Add("dialog_armyShallFight");
        list.Add("dialog_Attack");
        return list;
    }
    public List<SectorData> GetSectors(Player player,int minXindex,int maxXindex,int minCount)
    {
        var sectorId = player.MapData.CurrentCell.SectorId;
        var allSectors = player.MapData.GalaxyData.AllSectors.Where(x => !(x is SectorDungeon)
                                                                         && x.XIndex > minXindex
                                                                         && x.XIndex < maxXindex
                                                                         && x.Id != sectorId).ToList();
        if (allSectors.Count < minCount)
        {
            allSectors = player.MapData.GalaxyData.AllSectors.Where(x => !(x is SectorDungeon)).ToList();
        }

        if (allSectors.Count < minCount)
        {
            var list = player.MapData.GalaxyData.AllSectors.ToList();
            return list;
        }

        return allSectors;
    }
    protected float GetPercent(float baseVal, float curVal)
    {
        if (curVal < 0)
        {
            return 0;
        }

        return (curVal / (baseVal + curVal)) * 100;
    }

    public abstract bool CloseWindowOnClick { get; }
    public abstract void OnClick();

    public bool Activate(Player player)
    {
        _player = player;
#if UNITY_EDITOR
        if (_idToComplete.Count == 0)
        {
            Debug.LogError($"Quest stage without ids:{this}");
        }
#endif
        var isOk = StageActivate(player);
        if (!isOk)
        {
            Debug.LogError($"can't activate stage {this}");
            return false;
        }

        _activated = true;
        SubAfterLoad();
        _playerQuest.OnQuestId += OnQuestId;
        OnStageActivated?.Invoke(this);
        return true;
    }

    public void Fail()
    {
        _failed = true;
        OnFailed?.Invoke(this);
    }
    protected bool SkillWork(int baseVal, int skillVal)
    {
        WDictionary<bool> wd = new WDictionary<bool>(new Dictionary<bool, float>()
        {
            {true,skillVal },
            {false,baseVal},
        });
        return wd.Random();
    }
    public GlobalMapCell FindAndMarkCellClosest(SectorData posibleSector, Func<MessageDialogData> dialogFunc, GlobalMapCell playerCell)
    {
        var cells = posibleSector.ListCells.Where(x => x.Data != null && x.Data is FreeActionGlobalMapCell && !(x.Data as FreeActionGlobalMapCell).HaveQuest).ToList();
        if (cells.Count == 0)
        {
            return null;
        }
        int deltaMax = Int32.MaxValue;

        SectorCellContainer container = null;
        foreach (var sectorCellContainer in cells)
        {
            var deltaX = Mathf.Abs(sectorCellContainer.indX - playerCell.indX);
            var deltaZ = Mathf.Abs(sectorCellContainer.indZ - playerCell.indZ);
            var deltaSum = deltaZ + deltaX;
            if (deltaSum < deltaMax)
            {
                deltaMax = deltaSum;
                container = sectorCellContainer;
            }
        }
        var cell = container.Data as FreeActionGlobalMapCell;

        cell.SetQuestData(dialogFunc);
        return cell;
    }
    protected GlobalMapCell FindAndMarkCellFarest(SectorData posibleSector, Func<MessageDialogData> dialogFunc, GlobalMapCell playerCell)
    {
        var cells = posibleSector.ListCells.Where(x => x.Data != null && x.Data is FreeActionGlobalMapCell && !(x.Data as FreeActionGlobalMapCell).HaveQuest).ToList();
        if (cells.Count == 0)
        {
            return null;
        }
        int deltaMax = Int32.MinValue;

        SectorCellContainer container = null;
        foreach (var sectorCellContainer in cells)
        {
            var deltaX = Mathf.Abs(sectorCellContainer.indX - playerCell.indX);
            var deltaZ = Mathf.Abs(sectorCellContainer.indZ - playerCell.indZ);
            var deltaSum = deltaZ + deltaX;
            if (deltaSum > deltaMax)
            {
                deltaMax = deltaSum;
                container = sectorCellContainer;
            }
        }
        var cell = container.Data as FreeActionGlobalMapCell;

        cell.SetQuestData(dialogFunc);
        return cell;
    }

    public GlobalMapCell FindAndMarkCellRandom(SectorData posibleSector, Func<MessageDialogData> dialogFunc, GlobalMapCell playerCell)
    {
        var cells = posibleSector.ListCells.Where(x => x.Data != null && x.Data is FreeActionGlobalMapCell && !(x.Data as FreeActionGlobalMapCell).HaveQuest).ToList();
        if (cells.Count == 0)
        {
            return null;
        }
        int deltaMax = Int32.MaxValue;

        SectorCellContainer container = cells.RandomElement();
        var cell = container.Data as FreeActionGlobalMapCell;
        cell.SetQuestData(dialogFunc);
        return cell;
    }

    protected void TryNavigateToCell(GlobalMapCell cell)
    {
        var curWindow = WindowManager.Instance.CurrentWindow as MapWindow;
        if (curWindow != null)
        {
            curWindow.NavigationList.SetAndLightPoint(cell);
        }
    }

    private void OnQuestId(string obj)
    {
        if (_idToComplete.ContainsKey(obj))
        {
            _idToComplete[obj] = true;
            Debug.Log($"OnQuestId complete {obj}");
        }

        bool isComplete = true;

        foreach (var b in _idToComplete)
        {
            if (!b.Value)
            {
                isComplete = false;
                break;
            }
        }

        if (isComplete)
        {
            Debug.Log($"Stage complete key:{obj}   stage:{this}");
            Complete();
        }

    }

    private void Complete()
    {
        if (!_failed)
        {
            _complete = true;
            var dialog = AfterCompleteDialog();
            if(dialog != null)
                _playerQuest.AddLeaveDialogToQuery(dialog);
            OnComplete?.Invoke(this);
            _questContainer.CompleteStage(this);
        }
        Dispose();
    }

    public void TextChangeEvent()
    {
        OnTextChange?.Invoke(this);
    }

    public void Dispose()
    {
        StageDispose();
        OnComplete = null;
        OnStageActivated = null;
        OnTextChange = null;
        OnFailed = null;
        _playerQuest.OnQuestId -= OnQuestId;
    }
//    public abstract void Dispose();

    public void SetController(QuestContainer questContainer, PlayerQuestData playerQuest)
    {
        _playerQuest = playerQuest;
        _questContainer = questContainer;
    }

//    public abstract GlobalMapCell GetCurCellTarget();
    public abstract string GetDesc();
}
