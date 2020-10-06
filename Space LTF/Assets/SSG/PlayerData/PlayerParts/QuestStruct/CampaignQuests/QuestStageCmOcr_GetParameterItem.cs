
using System;

[System.Serializable]
public class QuestStageCmOcr_GetParameterItem : QuestStage
{
    private EParameterItemRarity _type;
    private Player _player;
    private int _id;

    public QuestStageCmOcr_GetParameterItem(EParameterItemRarity type,int id)
        : base(QuestsLib.QuestStageCmOcr_GetParameterItem + id.ToString())
    {
        _id = id;
        _type = type;
    }

    protected override bool StageActivate(Player player)
    {
        foreach (var inventoryParamItem in player.Inventory.ParamItems)
        {
            if (inventoryParamItem.Rarity == _type)
            {
                Complete();
                return true;
            }
        }

        _player = player;
        player.Inventory.OnItemAdded += OnItemAdded;
        return true;
    }

    private void OnItemAdded(IItemInv item, bool val)
    {
        var modul = item as ParameterItem;
        if (modul != null)
        {
            if (modul.Rarity == _type)
            {
                Complete();
            }
        }
    }

    private void Complete()
    {

        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr_GetParameterItem + _id.ToString());
    }


    protected override void StageDispose()
    {
        if (_player != null)
            _player.Inventory.OnItemAdded -= OnItemAdded;
    }

    public override bool CloseWindowOnClick { get; }
    public override void OnClick()
    {
        WindowManager.Instance.InfoWindow.Init(null, GetDesc());
    }

    public override string GetDesc()
    {

        return $"{Namings.Tag("cmGetParameterItem")}: {Namings.Tag(_type.ToString())}";
    }
}