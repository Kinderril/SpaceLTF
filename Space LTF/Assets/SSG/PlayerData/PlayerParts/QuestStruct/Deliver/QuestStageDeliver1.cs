using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


[System.Serializable]
public class QuestStageDeliver1 : QuestStage
{

    private FreeActionGlobalMapCell cell1 = null;
    private int TagetCount = 5;
    private EParameterItemRarity _itemRarity;
    private ItemType _itemSubType;
    private string _key;

    public QuestStageDeliver1(int tagetCount,string key, EParameterItemRarity itemRarity, ItemType itemSubType)    
        :base(key)
    {
        _key = key;
        _itemSubType = itemSubType;
        _itemRarity = itemRarity;
        TagetCount = tagetCount;
    }

    protected override bool StageActivate(Player player)
    {
     
        var posibleSectors = GetSectors(player, 0, 4,1);
        if (posibleSectors.Count < 1)
        {
            return false;
        }


        cell1 = FindAndMarkCell(posibleSectors.RandomElement(), Dialog) as FreeActionGlobalMapCell;
        if (cell1 == null)
        {
            return false;
        }
        return true;

    }

    private GlobalMapCell FindAndMarkCell(SectorData posibleSector,Func<MessageDialogData> dialogFunc)
    {
        var cells = posibleSector.ListCells.Where(x => x.Data  != null && x.Data is FreeActionGlobalMapCell && !(x.Data as FreeActionGlobalMapCell).HaveQuest).ToList();
        if (cells.Count == 0)
        {
            return null;
        }

        var cell = cells.RandomElement().Data as FreeActionGlobalMapCell;
        if (cell == null)
        {
            return null;
        }

        cell.SetQuestData(dialogFunc);
        return cell;

    }

    private MessageDialogData Dialog()
    {
        List<AnswerDialogData> ans = new List<AnswerDialogData>();
        string str = Namings.Tag("questReadyToDeliver");
        var player = MainController.Instance.MainPlayer;
        var paramItems = player.Inventory.ParamItems;
        var filtered = paramItems.Where(x => x.Rarity == _itemRarity && x.ItemType == _itemSubType).ToList();
        if (filtered.Count >= TagetCount)
        {

            ans.Add(new AnswerDialogData(Namings.Tag("deliverItems"),
                () =>
                {
                    cell1.SetQuestData(null);
                    TextChangeEvent();
                    _playerQuest.QuestIdComplete(_key);
                    for (int i = 0; i < TagetCount; i++)
                    {
                        var item = filtered[i];
                        player.Inventory.RemoveItem(item);
                    }
                },
                null, true, false));
        }
        ans.Add(new AnswerDialogData(Namings.Tag("haveNotEnought"),null,null,true,false));
        var msg = new MessageDialogData(str,ans,true);
        return msg;
    }

    protected override void StageDispose()
    {

    }

    public override bool CloseWindowOnClick => true;
    public override void OnClick()
    {
        TryNavigateToCell(GetCurCellTarget());
    }

    public GlobalMapCell GetCurCellTarget()
    {
        return cell1;

    }

    public override string GetDesc()
    {
        var rar = Namings.Tag($"EParameterItemRarity{_itemRarity.ToString()}");
        var typ = Namings.ParameterModulName(_itemSubType);
        return $"{Namings.Tag("questDeliverReadyToDeliver")}. {Namings.Tag("Type")}:{rar},{typ}  {Namings.Tag("Count")}: {TagetCount}";
    }
}
