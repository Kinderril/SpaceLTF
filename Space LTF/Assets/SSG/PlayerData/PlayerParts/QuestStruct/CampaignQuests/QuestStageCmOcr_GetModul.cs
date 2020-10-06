
using System;

[System.Serializable]
public class QuestStageCmOcr_GetModul : QuestStage
{
    private SimpleModulType _type;
    private Player _player;
    private int _id;

    public QuestStageCmOcr_GetModul(SimpleModulType type,int id)
        : base(QuestsLib.QuestStageCmOcr1_2_GetModul + id.ToString())
    {
        _id = id;
        _type = type;
    }

    protected override bool StageActivate(Player player)
    {
        foreach (var inventoryModul in player.Inventory.Moduls)
        {
            if (inventoryModul.Type == _type)
            {
                SubComplete();
                return true;
            }
        }
        _player = player;
        player.Inventory.OnItemAdded += OnItemAdded;
        return true;
    }

    private void OnItemAdded(IItemInv item, bool val)
    {
        var modul = item as BaseModulInv;
        if (modul != null)
        {
            if (modul.Type == _type)
            {
                SubComplete();
            }
        }
    }

    private void SubComplete()
    {
        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr1_2_GetModul + _id.ToString());

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

        return $"{Namings.Tag("cmGetModul")} {Namings.SimpleModulName(_type)}";
    }
}