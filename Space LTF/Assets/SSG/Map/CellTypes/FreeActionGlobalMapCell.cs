using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class FreeActionGlobalMapCell : GlobalMapCell
{
    protected int _power;
//    private List<MovingArmy> _connectedArmies = new List<MovingArmy>();
    private GalaxyEnemiesArmyController _enemiesArmyController;
    private float _powerPerTurn;
    private Func<MessageDialogData> _questData = null;

    [field: NonSerialized]
    public event Action<bool> OnQuestDialogChanges;
    public bool HaveQuest => _questData != null;

//    protected int _additionalPower;

    public FreeActionGlobalMapCell(int power, ShipConfig config, int id, int Xind, int Zind,
        SectorData sector, GalaxyEnemiesArmyController enemiesArmyController, float powerPerTurn)
        : base(id, Xind, Zind, sector, config)
    {
        _powerPerTurn = powerPerTurn;
        _power = power;
        _enemiesArmyController = enemiesArmyController;
    }

    public void BornArmy()
    {
        var army = new StandartMovingArmy(this,_enemiesArmyController.SimpleArmyDestroyed, _power,_enemiesArmyController, _powerPerTurn);
        _enemiesArmyController.AddArmy(army);
    }


    public override string Desc()
    {
        return Namings.Tag("Nothing");
    }

    public override void Take()
    {

    }

    public void SetQuestData(Func<MessageDialogData> data)
    {
        _questData = data;
        OnQuestDialogChanges?.Invoke(HaveQuest);
    }
//    public override void UpdateAdditionalPower(int visitedSectors, int startPower, int additionalPower)
//    {
//        _additionalPower = additionalPower;
//        var nextPower = SectorData.CalcCellPower(visitedSectors, _sector.Size, startPower, _additionalPower);
//        SetPower(nextPower);
//    }

//    private void SetPower(float power)
//    {
//        foreach (var connectedArmy in _connectedArmies)
//        {
//            connectedArmy.UpdatePower(power);
//        }
//    }

    public override bool CanCellDestroy()
    {
        return true;
    }

    protected override MessageDialogData GetDialog()
    {
        if (_questData == null)
        {
            return null;
        }
        return _questData();
//        var mesData = new MessageDialogData("Nothing here.", new List<AnswerDialogData>()
//        {
//            new AnswerDialogData("Ok",null),
//        });
//        return mesData;
    }

    public override Color Color()
    {
        return new Color(51f / 255f, 102f / 255f, 153f / 255f);
    }

    public override bool OneTimeUsed()
    {
        return true;
    }
}

