using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum EQuestOnStart
{
    killLight,
    killMed,
    killHeavy,
    killRaiders,
    killMerc,
    killFed,
    killKrios,
    killOcrons,
    killDroids,
    // bodyDamage,
    // shieldDamage,
    mainShipKills,
    laserDamage,
    rocketDamage,
    impulseDamage,
    emiDamage,
    cassetDamage,
    // collectMoney,
    upgradeWeapons,
    sellModuls,
    winRaiders,
    winMerc,
    winFed,
    winKrios,
    winOcrons,
    winDroids,

}

[System.Serializable]
public  abstract class BaseQuestOnStart   : QuestStage
{
    public int TargetCounter { get; private set; }
    public int CurCount { get; private set; }
    public bool IsReady { get; private set; }      //готов к получению награды
    public bool IsCompleted { get; private set; }                             //полностью завершен
//    public WeaponInv WeaponReward { get; private set; }
//    public BaseModulInv ModulReward { get; private set; }
//    public int MoneyCount { get; private set; }
    public override bool CloseWindowOnClick => false;
    public string Name => Namings.QuestName(_type);
//    public string NameTag => _type.ToString();

    private EQuestOnStart _type;
    private string _id;


    protected BaseQuestOnStart(int targetCounter,EQuestOnStart type)  
        :base(type.ToString())
    {
        _id = type.ToString();
        _type = type;
        TargetCounter = targetCounter;
#if UNITY_EDITOR
//        TargetCounter = 1;
#endif




    }
    public override void OnClick()
    {

    }

    public override string GetDesc()
    {
        return $"{Name} {CurCount}/{TargetCounter}";
    }
    //    public virtual void Init()
    //    {
    //        CurCount = 0;
    //    }
    //    public abstract void Dispose();

    public void AddCount()
    {
        CurCount++;
        TextChangeEvent();
        CheckEnd();
    }  
    
    public void AddCount(int c)
    {
        CurCount += c;
        CheckEnd();
    }

    private void CheckEnd()
    {
        if (IsReady)
        {
            CurCount = TargetCounter;
        }
        else
        {
            if (CurCount >= TargetCounter)
            {
                CurCount = TargetCounter;
                IsReady = true;
                _playerQuest.QuestIdComplete(_id);
//                Dispose();
            }
        }

    }

    public static BaseQuestOnStart Create(EQuestOnStart eQuestOnStart,float coef)
    {
        switch (eQuestOnStart)
        {
            case EQuestOnStart.killLight:
                return new KillsTypeQuestOnStart(ShipType.Light,(int)(20 * coef), eQuestOnStart);
            case EQuestOnStart.killMed:
                return new KillsTypeQuestOnStart(ShipType.Middle, (int)(20 * coef), eQuestOnStart);
            case EQuestOnStart.killHeavy:
                return new KillsTypeQuestOnStart(ShipType.Heavy, (int)(20 * coef), eQuestOnStart);
            case EQuestOnStart.killRaiders:
                return new KillsConfigQuestOnStart(ShipConfig.raiders, (int)(24 * coef), eQuestOnStart);
            case EQuestOnStart.killMerc:
                return new KillsConfigQuestOnStart(ShipConfig.mercenary, (int)(24 * coef), eQuestOnStart);
            case EQuestOnStart.killFed:
                return new KillsConfigQuestOnStart(ShipConfig.federation, (int)(24 * coef), eQuestOnStart);
            case EQuestOnStart.killKrios:
                return new KillsConfigQuestOnStart(ShipConfig.krios, (int)(24 * coef), eQuestOnStart);
            case EQuestOnStart.killOcrons:
                return new KillsConfigQuestOnStart(ShipConfig.ocrons, (int)(24 * coef), eQuestOnStart);
            case EQuestOnStart.killDroids:
                return new KillsConfigQuestOnStart(ShipConfig.droid, (int)(24 * coef), eQuestOnStart);
            case EQuestOnStart.mainShipKills:   
                return new KillsByTypeQuestOnStart(ShipType.Base, (int)(15 * coef), eQuestOnStart);
            case EQuestOnStart.laserDamage:
                return new KillsWeaponQuestOnStart(WeaponType.laser, (int)(250 * coef), eQuestOnStart);
            case EQuestOnStart.rocketDamage:
                return new KillsWeaponQuestOnStart(WeaponType.rocket, (int)(250 * coef), eQuestOnStart);
            case EQuestOnStart.impulseDamage:
                return new KillsWeaponQuestOnStart(WeaponType.impulse, (int)(250 * coef), eQuestOnStart);
            case EQuestOnStart.emiDamage:
                return new KillsWeaponQuestOnStart(WeaponType.eimRocket, (int)(250 * coef), eQuestOnStart);
            case EQuestOnStart.cassetDamage:
                return new KillsWeaponQuestOnStart(WeaponType.casset, (int)(250 * coef), eQuestOnStart);
            case EQuestOnStart.upgradeWeapons:
                return new UpgradeItemQuestOnStart(ItemType.weapon, (int)(10 * coef), eQuestOnStart);
            case EQuestOnStart.sellModuls:
                return new SellItemQuestOnStart(ItemType.modul, (int)(20 * coef), eQuestOnStart);
            case EQuestOnStart.winRaiders:
                return new WinConfigQuestOnStart(ShipConfig.raiders, (int)(10 * coef), eQuestOnStart);
            case EQuestOnStart.winMerc:
                return new WinConfigQuestOnStart(ShipConfig.mercenary, (int)(10 * coef), eQuestOnStart);
            case EQuestOnStart.winFed:
                return new WinConfigQuestOnStart(ShipConfig.federation, (int)(8 * coef), eQuestOnStart);
            case EQuestOnStart.winKrios:
                return new WinConfigQuestOnStart(ShipConfig.krios, (int)(8 * coef), eQuestOnStart);
            case EQuestOnStart.winOcrons:
                return new WinConfigQuestOnStart(ShipConfig.ocrons, (int)(8 * coef), eQuestOnStart);
            case EQuestOnStart.winDroids:
                return new WinConfigQuestOnStart(ShipConfig.droid, (int)(14 * coef), eQuestOnStart);
        }
        Debug.LogError($"BaseQuestOnStart Create {eQuestOnStart.ToString()}");
        return null;
    }
}

