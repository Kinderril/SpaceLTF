using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleEventsDialogsLib
{
    public static MessageDialogData GetDialog(EBattleType battleType, bool fullWin,float armyPower)
    {
        if (battleType == EBattleType.standart)
        {
            return null;
        }
        string msg = "";
        var preList = new List<AnswerDialogData>();
        if (fullWin)
        {
            switch (battleType)
            {
                case EBattleType.defenceWaves:
                    var coef = armyPower * Library.MONEY_QUEST_COEF;
                    var monet = (int)(MyExtensions.Random(0.8f,  1.5f) * coef);
                    MainController.Instance.MainPlayer.MoneyData.AddMoney(monet);
                    msg = Namings.Format(Namings.Tag("defenceWavesWin"), monet);
                    break;
                case EBattleType.destroyShipPeriod:
                    MainController.Instance.MainPlayer.MoneyData.AddMicrochips(1);
                    msg = Namings.Format(Namings.Tag("destroyShipPeriodWin"), 1);
                    break;
                case EBattleType.defenceOfShip:
//                    string d = "";
                    int slot;
                    var list = new List<EParameterItemSubType>()
                        {EParameterItemSubType.middle, EParameterItemSubType.heavy, EParameterItemSubType.light};
                    var m = Library.CreateParameterItem(list.RandomElement(), EParameterItemRarity.normal);
                    var itemName = m.GetName();
                    var canAdd = MainController.Instance.MainPlayer.Inventory.GetFreeSimpleSlot(out slot);
                    if (canAdd)
                    {
                        MainController.Instance.MainPlayer.Inventory.TryAddItem(m);
                        msg = Namings.Format(Namings.Tag("defenceOfShipWin"), itemName);
                    }
                    else
                    {
                        msg = Namings.Format("noFreeSpace");
                    }
                    break;
                case EBattleType.baseDefence:
                    MainController.Instance.MainPlayer.MoneyData.AddMicrochips(1);
                    msg = Namings.Format(Namings.Tag("baseDefenceWin"), 1);
                    break;
            }
            preList.Add(new AnswerDialogData(Namings.Tag("Ok")));
        }
        else
        {
            switch (battleType)
            {
                case EBattleType.defenceWaves:
                    msg = Namings.Tag("defenceWavesFail");
                    break;
                case EBattleType.destroyShipPeriod:
                    msg = Namings.Tag("destroyShipPeriodFail");
                    break;
                case EBattleType.defenceOfShip:
                    msg = Namings.Tag("defenceOfShipFail");
                    break;
                case EBattleType.baseDefence:
                    msg = Namings.Tag("baseDefenceFail");
                    break;
            }
            preList.Add(new AnswerDialogData(Namings.Tag("leave")));
        }
        var data = new MessageDialogData(msg, preList);
        return data;
    }

}
