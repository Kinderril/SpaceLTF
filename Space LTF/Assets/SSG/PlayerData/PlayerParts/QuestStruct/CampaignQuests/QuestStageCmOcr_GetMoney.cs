
using System;

[System.Serializable]
public class QuestStageCmOcr_GetMoney : QuestStage
{
    private int _count;
    private Player _player;
    private int _id;

    public QuestStageCmOcr_GetMoney(int count,int id)
        : base(QuestsLib.QuestStageCmOcr_GetMoney + id.ToString())
    {
        _id = id;
        _count = count;
    }

    protected override bool StageActivate(Player player)
    {
        if (player.MoneyData.HaveMoney(_count))
        {
            Complete();
            return true;
        }
        _player = player;
        player.SafeLinks.OnCreditsChange += OnCreditsChange;
        return true;
    }

    private void OnCreditsChange(int obj)
    {
        if (obj > _count)
        {
            Complete();
        }
    }


    private void Complete()
    {

        _playerQuest.QuestIdComplete(QuestsLib.QuestStageCmOcr_GetMoney + _id.ToString());
    }


    protected override void StageDispose()
    {
        if (_player != null)
            _player.SafeLinks.OnCreditsChange -= OnCreditsChange;
    }

    public override bool CloseWindowOnClick { get; }
    public override void OnClick()
    {
        WindowManager.Instance.InfoWindow.Init(null, GetDesc());
    }

    public override string GetDesc()
    {

        return $"{Namings.Tag("cmGetCredits")}: {_count.ToString()}";
    }
}