using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ArmyBornGlobalMapCell : GlobalMapCell
{
    protected int _power;
    private List<MovingArmy> _connectedArmies = new List<MovingArmy>();
    private GalaxyEnemiesArmyController _enemiesArmyController;
    private float _powerPerTurn;

//    protected int _additionalPower;

    public ArmyBornGlobalMapCell(int power, ShipConfig config, int id, int Xind, int Zind,
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
//    public override void UpdatePowers(int visitedSectors, int startPower, int additionalPower)
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

        return null;
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

