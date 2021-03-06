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
    protected float _additionalPower = 0f;
    protected float _collectedPower = 0f;
    protected float _powerCoef = 1.001f;

    private bool _bornArmy;
    //    protected int _additionalPower;
    public int Power
    {
        get;
        private set;
    }
    public FreeActionGlobalMapCell(int power, ShipConfig config, int id, int Xind, int Zind,
        SectorData sector, GalaxyEnemiesArmyController enemiesArmyController, float powerPerTurn,bool bornArmy = true)
        : base(id, Xind, Zind, sector, config)
    {
        _bornArmy = bornArmy;
        _powerPerTurn = powerPerTurn;
        _power = power;
        _enemiesArmyController = enemiesArmyController;
    }

    private static bool _lockedInof = false;

    public void BornArmy()
    {
#if UNITY_EDITOR
        if (DebugParamsController.NoAmyBorn)
        {
            if (!_lockedInof)
            {
                _lockedInof = true;
                Debug.LogError("Born army locked");
            }

            return;
        }
#endif
        if (MyExtensions.IsTrue01(0.07f))
        {
            return;
        }
        if (!_bornArmy)
        {
            return;
        }
        var army = new StandartMovingArmy(this.Container,
            _enemiesArmyController.SimpleArmyDestroyed, _power,_enemiesArmyController, _powerPerTurn);
        _enemiesArmyController.AddArmy(army);
    }

    public override bool IsPossibleToChange()
    {
        return true;
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
        var dialog = _questData();
        return dialog;
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
    public override void UpdateAdditionalPower(int additionalPower)
    {
        _additionalPower = additionalPower;
        SubUpdatePower();
    }

    protected void SubUpdatePower()
    {
        Power = (int)((_power + _collectedPower + _additionalPower) * _powerCoef);
    }

    public override void UpdateCollectedPower(float powerDelta)
    {
        _collectedPower += powerDelta;
        SubUpdatePower();
    }
}

