using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum GlobalMapEventType
{
    nothing = 0,
    spaceGarbage = 1,//Находишь чутка денег в зависимости от уровня скаутов
    battleField = 2,//Находишь некоторый лут (оружие/модули) + шанс покоцать корабли
    creditStorage = 3,//хранение бабла
    asteroidsField = 4,//можно послать какойнить корабль. Если ок - +бабки, если фейл то дамаг.
    scienceLab = 5, //Можно попытаться выклянчить в зависимости от скила торговли (спелы или модуль)
    anomaly = 6,//Гонки. Можно выставить корабль и ставка в 20, 30, 50 кредитов. Если победа то корабль + к скорости.

    brokenNavigation = 7,//Провести закоулками. Если уровень скаутов больче чем 2 то проводит. + пушка.
    prisoner = 9,//Побег заключенного.
    teach = 10,//Обучение.
    trade = 11,//Обмен.
    mercHideout = 12,//Наемники.
    change = 13,//Вещть на вещь.
    excavation = 14,//Раскопки
//    repairShip = 8,//Если увроень ремонта больше чем 2 то чиним. + деньги.
}

[System.Serializable]
public class EventGlobalMapCell : GlobalMapCell
{
    private GlobalMapEventType _eventType;
    private BaseGlobalMapEvent _mapEvent;

    public EventGlobalMapCell(GlobalMapEventType eventType, int id, int intX, int intZ, SectorData secto) : base( id, intX, intZ, secto)
    {
        _eventType = eventType;
        switch (_eventType)
        {
            case GlobalMapEventType.nothing:
                _mapEvent = new NothingGlobalMapEvent();
                break;
            case GlobalMapEventType.spaceGarbage:
                _mapEvent = new SpaceGarbageMapEvent();
                break;
            case GlobalMapEventType.creditStorage:
                List<ShipConfig>  configs = new List<ShipConfig>()
                {
                    ShipConfig.federation,ShipConfig.krios,ShipConfig.ocrons
                };
                _mapEvent = new RetranslaitorMapEvent(configs.RandomElement());
                break;
            case GlobalMapEventType.asteroidsField:
                _mapEvent = new AsteroidFieldMapEvent();
                break;
            case GlobalMapEventType.scienceLab:
                _mapEvent = new ScienceLabMapEvent();
                break;
            case GlobalMapEventType.anomaly:
                _mapEvent = new AnomalyMapEvent();
                break;
            case GlobalMapEventType.battleField:
                _mapEvent = new BattlefieldMapEvent();
                break; 
            case GlobalMapEventType.excavation:
                _mapEvent = new ExcavationsEvent();
                break; 
            case GlobalMapEventType.mercHideout:
                _mapEvent = new MercenaryHideout();
                break;
            case GlobalMapEventType.brokenNavigation:
                _mapEvent = new BrokenNavigationMapEvent();
                break;
            case GlobalMapEventType.prisoner:
                _mapEvent = new PrisonerCatchMapEvent();
                break;     

            case GlobalMapEventType.trade:
                _mapEvent = new TradeMapEvent();
                break;   
            case GlobalMapEventType.change:
                _mapEvent = new ChangeItemMapEvent();
                break;     
            case GlobalMapEventType.teach:
                _mapEvent = new TeacherMapEvent();
                break;
//            case GlobalMapEventType.repairShip:
//                _mapEvent = new RepairShipMapEvent();
//                break;

        }
        if (_mapEvent == null)
        {
            Debug.LogError("_mapEvent " + _eventType.ToString() + "   not implimented");
        }
        else
        {
            _mapEvent.Init();
        }
    }

    public override bool CanCellDestroy()
    {
        return true;
    }

    public override string Desc()
    {
        return _mapEvent.Desc();
    }

    public override void Take()
    {

    }

    public override MessageDialogData GetDialog()
    {
        return _mapEvent.GetDialog();
    }    

    protected override MessageDialogData GetLeavedActionInner()
    {
        return _mapEvent.GetLeavedActionInner();
    }

    public override Color Color()
    {
        return new Color(204f/255f, 255f/255f, 153f/255f);
    }

    public override bool OneTimeUsed()
    {
        return true;
    }
}