using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class DialogsLibrary 
{
    public static MessageDialogData GetStartCampDialog(ShipConfig config,int act)
    {
        List<string> list
                             = new List<string>();
        if (act == 0)
        {
            list .Add("campStealMrcM1");
            list.Add("campStealMrcA1");
            list
                            .Add("campStealMrcM2");
            list
                            .Add("campStealMrcA2");
            list
                            .Add("campStealMrcM3");
            list
                            .Add("campStealMrcA3");
            list
                            .Add("campStealMrcM4");
            list
                            .Add("campStealMrcA4");
            list
                            .Add("campStealMrcM5");
            list
                            .Add("campStealMrcA5");

            return GetPairDialogByTag(list
                            , null);
        }

        switch (config)
        {
            case ShipConfig.raiders:
                switch (act)
                {
                    case 1:
                        list
                            .Add("campStealRdrM1");
                        list
                            .Add("DC");
                        list
                            .Add("campStealRdrM2");
                        list
                            .Add("DC");
                        list
                            .Add("campStealRdrM3");
                        list
                            .Add("campStealRdrA3");
                        list
                            .Add("campStealRdrM4");
                        list
                            .Add("campStealRdrA4");
                        list
                            .Add("campStealRdrM5");
                        list
                            .Add("campStealRdrA5");
                        break;    
                    case 2:
                        list
                            .Add("campStartRdrM1");
                        list
                            .Add("campStartRdrA1");   
                        list
                            .Add("campStartRdrM2");
                        list
                            .Add("campStartRdrA2");   
                        list
                            .Add("campStartRdrM3");
                        list
                            .Add("campStartRdrA3");   
                        list
                            .Add("campStartRdrM4");
                        list
                            .Add("campStartRdrA4");   
                        list
                            .Add("campStartRdrM5");
                        list
                            .Add("campStartRdrA5");   
                        list
                            .Add("campStartRdrM6");
                        list
                            .Add("campStartRdrA6");   
                        list
                            .Add("campStartRdrM7");
                        list.Add("campStartRdrA7");   
                        list.Add("campStartRdrM8");
                        list.Add("campStartRdrA8");   
                        list.Add("campStartRdrM9");
                        list.Add("campStartRdrA9");   
                        list
                            .Add("campStartRdrM10");
                        list
                            .Add("campStartRdrA10");   
                        list.Add("campStartRdrM11");
                        list.Add("campStartRdrA11");   
                        list.Add("campStartRdrM12");
                        list.Add("campStartRdrA12");   
                        list.Add("campStartRdrM13");
                        list.Add("campStartRdrA13");  
                        break;
                    case 3:
                        list.Add("cmStartAct3Rdr_M0");
                        list.Add("DC");
                        list.Add("cmStartAct3Rdr_M1");
                        list.Add("cmStartAct3Rdr_A2");
                        list.Add("cmStartAct3Rdr_M3");
                        list.Add("cmStartAct3Rdr_A4");
                        list.Add("cmStartAct3Rdr_M5");
                        list.Add("cmStartAct3Rdr_A6");
                        list.Add("cmStartAct3Rdr_M7");
                        list.Add("cmStartAct3Rdr_A8");
                        list.Add("cmStartAct3Rdr_M9");
                        list.Add("cmStartAct3Rdr_A10");
                        list.Add("cmStartAct3Rdr_M11");
                        list.Add("cmStartAct3Rdr_A12");
                        list.Add("cmStartAct3Rdr_M13");
                        list.Add("cmStartAct3Rdr_A14");
                        list.Add("cmStartAct3Rdr_M15");
                        list.Add("cmStartAct3Rdr_A16");

                        break;
                } 
                break;
            case ShipConfig.federation:
            case ShipConfig.mercenary:
            case ShipConfig.krios:
                switch (act)
                { 
                    case 1:
                        list
                            .Add("campStartMercAct2M1");
                        list
                            .Add("campStartMercAct2A1");  
                        list
                            .Add("campStartMercAct2M2");
                        list
                            .Add("campStartMercAct2A2");  
                        list
                            .Add("campStartMercAct2M3");
                        list
                            .Add("campStartMercAct2A3");  
                        list
                            .Add("campStartMercAct2M4");
                        list
                            .Add("campStartMercAct2A4");  
                        list
                            .Add("campStartMercAct2M5");
                        list
                            .Add("campStartMercAct2A5");  
                        list
                            .Add("campStartMercAct2M6");
                        list
                            .Add("campStartMercAct2A6");  
                        list
                            .Add("campStartMercAct2M7");
                        list
                            .Add("campStartMercAct2A7");  
                        list
                            .Add("campStartMercAct2M8");
                        list
                            .Add("DC");  
                        list
                            .Add("campStartMercAct2M9");
                        list
                            .Add("DC");  
                        list
                            .Add("campStartMercAct2M10");
                        list
                            .Add("campStartMercAct2A10");
                        break;
                    case 2:
                        list
                            .Add("campStartMercAct3M1"); 
                        list
                            .Add("campStartMercAct3A1"); 
                        list
                            .Add("campStartMercAct3M2"); 
                        list
                            .Add("campStartMercAct3A2"); 
                        list
                            .Add("campStartMercAct3M3"); 
                        list
                            .Add("campStartMercAct3A3"); 
                        list
                            .Add("campStartMercAct3M4"); 
                        list
                            .Add("campStartMercAct3A4"); 
                        list
                            .Add("campStartMercAct3M5"); 
                        list
                            .Add("campStartMercAct3A5"); 
                        list
                            .Add("campStartMercAct3M6"); 
                        list
                            .Add("campStartMercAct3A6"); 
                        list
                            .Add("campStartMercAct3M7"); 
                        list
                            .Add("campStartMercAct3A7"); 
                        list
                            .Add("campStartMercAct3M8"); 
                        list
                            .Add("DC");    
                        list
                            .Add("campStartMercAct3M9"); 
                        list
                            .Add("DC");    
                        list
                            .Add("campStartMercAct3M10");
                        list
                            .Add("DC");  
                        list
                            .Add("campStartMercAct3M11");
                        list
                            .Add("DC");

                        break;
                }
                break;
            case ShipConfig.ocrons:
                switch (act)
                {
                    case 1:
                        list
                            .Add("campStealOcrM1");
                        list
                            .Add("campStealOcrA1");
                        list
                            .Add("campStealOcrM2");
                        list
                            .Add("campStealOcrA2");
                        list
                            .Add("campStealOcrM3");
                        list
                            .Add("campStealOcrA3");
                        list
                            .Add("campStealOcrM4");
                        list
                            .Add("campStealOcrA4");
                        list
                            .Add("campStealOcrM5");
                        list
                            .Add("campStealOcrA5");
                        list
                            .Add("campStealOcrM6");
                        list
                            .Add("campStealOcrA6");
                        list
                            .Add("campStealOcrM7");
                        list
                            .Add("campStealOcrA7");
                        list
                            .Add("campStealOcrM8");
                        list
                            .Add("DC");
                        list
                            .Add("campStealOcrM9");
                        list
                            .Add("campStealOcrA9");
                        list
                            .Add("campStealOcrM10");
                        list
                            .Add("campStealOcrA10");
                        list
                            .Add("campStealOcrM11");
                        list
                            .Add("campStealOcrA11");
                        list
                            .Add("campStealOcrM12");
                        list
                            .Add("campStealOcrA12");
                        list
                            .Add("campStealOcrM13");
                        list
                            .Add("campStealOcrA13");
                        break;
                    case 2:
                        list
                            .Add("campStealOcr2M1");
                        list
                            .Add("campStealOcr2A1");  
                        list
                            .Add("campStealOcr2M2");
                        list
                            .Add("campStealOcr2A2");  
                        list
                            .Add("campStealOcr2M3");
                        list
                            .Add("campStealOcr2A3");  
                        list
                            .Add("campStealOcr2M4");
                        list
                            .Add("campStealOcr2A4");  
                        list
                            .Add("campStealOcr2M5");
                        list
                            .Add("campStealOcr2A5");  
                        list
                            .Add("campStealOcr2M6");
                        list
                            .Add("campStealOcr2A6");  
                        list
                            .Add("campStealOcr2M7");
                        list
                            .Add("campStealOcr2A7");  
                        list
                            .Add("campStealOcr2M8");
                        list
                            .Add("campStealOcr2A8");
                        break;
                }
                break;
//            case ShipConfig.krios:
//                tags.Add("campStealKrsM1");
//                tags.Add("campStealKrsA1");
//                tags.Add("campStealKrsM2");
//                tags.Add("campStealKrsA2");
//                tags.Add("campStealKrsM3");
//                tags.Add("campStealKrsA3");
//                break;
            default:
                Debug.LogError($"error start dialog  {config.ToString()}  act:{act}");
                return null;
        }


        return GetPairDialogByTag(list
                            , null);
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
