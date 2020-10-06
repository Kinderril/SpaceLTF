using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class DialogsLibrary 
{
    public static MessageDialogData GetStartCampDialog(ShipConfig config,int act)
    {
        List<string> tags = new List<string>();
        if (act == 0)
        {
            tags.Add("campStealMrcM1");
            tags.Add("campStealMrcA1");
            tags.Add("campStealMrcM2");
            tags.Add("campStealMrcA2");
            tags.Add("campStealMrcM3");
            tags.Add("campStealMrcA3");
            tags.Add("campStealMrcM4");
            tags.Add("campStealMrcA4");
            tags.Add("campStealMrcM5");
            tags.Add("campStealMrcA5");
        }

        switch (config)
        {
            case ShipConfig.raiders:
                tags.Add("campStealRdrM1");
                tags.Add("campStealRdrA1");      
                tags.Add("campStealRdrM2");
                tags.Add("campStealRdrA2");      
                tags.Add("campStealRdrM3");
                tags.Add("campStealRdrA3");
                break;
            case ShipConfig.federation:
                tags.Add("campStealFedM1");
                tags.Add("campStealFedA1");
                tags.Add("campStealFedM2");
                tags.Add("campStealFedA2");
                tags.Add("campStealFedM3");
                tags.Add("campStealFedA3");
                break;
            case ShipConfig.mercenary:
                switch (act)
                { 
                    case 1:
                        tags.Add("campStartMercAct2M1");
                        tags.Add("campStartMercAct2A1");  
                        tags.Add("campStartMercAct2M2");
                        tags.Add("campStartMercAct2A2");  
                        tags.Add("campStartMercAct2M3");
                        tags.Add("campStartMercAct2A3");  
                        tags.Add("campStartMercAct2M4");
                        tags.Add("campStartMercAct2A4");  
                        tags.Add("campStartMercAct2M5");
                        tags.Add("campStartMercAct2A5");  
                        tags.Add("campStartMercAct2M6");
                        tags.Add("campStartMercAct2A6");  
                        tags.Add("campStartMercAct2M7");
                        tags.Add("campStartMercAct2A7");  
                        tags.Add("campStartMercAct2M8");
                        tags.Add("DC");  
                        tags.Add("campStartMercAct2M9");
                        tags.Add("DC");  
                        tags.Add("campStartMercAct2M10");
                        tags.Add("campStartMercAct2A10");
                        break;
                    case 2:
                        tags.Add("campStartMercAct3M1"); 
                        tags.Add("campStartMercAct3A1"); 
                        tags.Add("campStartMercAct3M2"); 
                        tags.Add("campStartMercAct3A2"); 
                        tags.Add("campStartMercAct3M3"); 
                        tags.Add("campStartMercAct3A3"); 
                        tags.Add("campStartMercAct3M4"); 
                        tags.Add("campStartMercAct3A4"); 
                        tags.Add("campStartMercAct3M5"); 
                        tags.Add("campStartMercAct3A5"); 
                        tags.Add("campStartMercAct3M6"); 
                        tags.Add("campStartMercAct3A6"); 
                        tags.Add("campStartMercAct3M7"); 
                        tags.Add("campStartMercAct3A7"); 
                        tags.Add("campStartMercAct3M8"); 
                        tags.Add("DC");    
                        tags.Add("campStartMercAct3M9"); 
                        tags.Add("DC");    
                        tags.Add("campStartMercAct3M10");
                        tags.Add("DC");  
                        tags.Add("campStartMercAct3M11");
                        tags.Add("DC");

                        break;
                }
                break;
            case ShipConfig.ocrons:
                switch (act)
                {
                    case 1:
                        tags.Add("campStealOcrM1");
                        tags.Add("campStealOcrA1");
                        tags.Add("campStealOcrM2");
                        tags.Add("campStealOcrA2");
                        tags.Add("campStealOcrM3");
                        tags.Add("campStealOcrA3");
                        tags.Add("campStealOcrM4");
                        tags.Add("campStealOcrA4");
                        tags.Add("campStealOcrM5");
                        tags.Add("campStealOcrA5");
                        tags.Add("campStealOcrM6");
                        tags.Add("campStealOcrA6");
                        tags.Add("campStealOcrM7");
                        tags.Add("campStealOcrA7");
                        tags.Add("campStealOcrM8");
                        tags.Add("DC");
                        tags.Add("campStealOcrM9");
                        tags.Add("campStealOcrA9");
                        tags.Add("campStealOcrM10");
                        tags.Add("campStealOcrA10");
                        tags.Add("campStealOcrM11");
                        tags.Add("campStealOcrA11");
                        tags.Add("campStealOcrM12");
                        tags.Add("campStealOcrA12");
                        tags.Add("campStealOcrM13");
                        tags.Add("campStealOcrA13");
                        break;
                    case 2:
                        tags.Add("campStealOcr2M1");
                        tags.Add("campStealOcr2A1");  
                        tags.Add("campStealOcr2M2");
                        tags.Add("campStealOcr2A2");  
                        tags.Add("campStealOcr2M3");
                        tags.Add("campStealOcr2A3");  
                        tags.Add("campStealOcr2M4");
                        tags.Add("campStealOcr2A4");  
                        tags.Add("campStealOcr2M5");
                        tags.Add("campStealOcr2A5");  
                        tags.Add("campStealOcr2M6");
                        tags.Add("campStealOcr2A6");  
                        tags.Add("campStealOcr2M7");
                        tags.Add("campStealOcr2A7");  
                        tags.Add("campStealOcr2M8");
                        tags.Add("campStealOcr2A8");
                        break;
                }
                break;
            case ShipConfig.krios:
                tags.Add("campStealKrsM1");
                tags.Add("campStealKrsA1");
                tags.Add("campStealKrsM2");
                tags.Add("campStealKrsA2");
                tags.Add("campStealKrsM3");
                tags.Add("campStealKrsA3");
                break;
            default:
                Debug.LogError($"error start dialog  {config.ToString()}  act:{act}");
                return null;
        }


        return GetPairDialogByTag(tags, null);
    }

    public static MessageDialogData GetPairDialogByTag(List<string> tags, Action lastCallback,Func<MessageDialogData> nextDialog = null)
    {
        //        List<MessageDialogData> dialogs = new List<MessageDialogData>();
        var dialog = GetNext(null, tags, tags.Count - 1, lastCallback, nextDialog);
        return dialog;
        //        if (dialogs.Count > 0)
        //        {
        //            return dialogs[0];
        //        }
        //        return null;
    }

    private static MessageDialogData GetNext(MessageDialogData nextDialog, List<string> tags, int ind, Action lastCallback, Func<MessageDialogData> nextDialogAdd = null)
    {
        var i1 = ind;
        var i2 = ind - 1;
        if (i2 < tags.Count && i1 >= 0)
        {
            bool isLast = i1 == tags.Count - 1;
            var opt = Namings.Tag(tags[i2]);
            var answ = Namings.Tag(tags[i1]);
            var answers = new List<AnswerDialogData>();

            var nextId = ind - 2;
            if (isLast)
            {
                answers.Add(new AnswerDialogData(answ, lastCallback, nextDialogAdd));
                MessageDialogData data1 = new MessageDialogData(opt, answers, true);
                var next = GetNext(data1, tags, nextId, lastCallback, nextDialogAdd);
                return next;
            }
            else
            {
                answers.Add(new AnswerDialogData(answ, null, () => nextDialog));
                MessageDialogData data = new MessageDialogData(opt, answers, true);
                var prevDialog = GetNext(data, tags, nextId, lastCallback, nextDialogAdd);
                return prevDialog;
            }

        }
        return nextDialog;
    }
}
