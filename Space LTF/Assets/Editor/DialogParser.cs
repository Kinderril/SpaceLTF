using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public  static class DialogParser
{
    private const string KEY_NAME = "Курт:";
    public static Tuple<StringBuilder,StringBuilder> Parse(string strToParse,string mainDialogNmae)
    {
        var parseToStrings = strToParse.Split('\n');
//        Debug.LogError($"rting count:{parseToStrings.Length}");
        var notNull = parseToStrings.Where(x => x.Length > 3).ToList();
        StringBuilder keysToList = new StringBuilder();
        StringBuilder keysToLoc = new StringBuilder();
        bool isFirst = true;
        bool prevIsKurt = false;
        int index = 0;
        foreach (var toString in notNull)
        {
            var isKurt = toString.Contains(KEY_NAME);
            if (isFirst)
            {
                isFirst = false;
                FirstString(mainDialogNmae, isKurt, keysToList, toString, keysToLoc);
            }
            else
            {
                string key;
                if (prevIsKurt)
                {
                    if (isKurt)
                    {
                        var listKey1 = $"list.Add(\"DC\");\n";
                        keysToList.Append(listKey1);
                        key = String.Format("\"{0}_A{1}\"", mainDialogNmae,index);
                        var listKey2 = $"list.Add({key});\n";
                        keysToList.Append(listKey2);
                        //-------------------------------

                    }
                    else
                    {
                        key = String.Format("\"{0}_M{1}\"", mainDialogNmae, index);
                        var listKey2 = $"list.Add({key});\n";
                        keysToList.Append(listKey2);
                        //-------------------------------

                    }
                }
                else
                {
                    if (isKurt)
                    {
                        key = String.Format("\"{0}_A{1}\"", mainDialogNmae, index);
                        var listKey2 = $"list.Add({key});\n";
                        keysToList.Append(listKey2);
                        //-------------------------------
                    }
                    else
                    {
                        var listKey1 = $"list.Add(\"DC\");\n";
                        keysToList.Append(listKey1);
                        key = String.Format("\"{0}_M{1}\"", mainDialogNmae, index);
                        var listKey2 = $"list.Add({key});\n";
                        keysToList.Append(listKey2);
                        //-------------------------------

                    }
                }

                string value;
                if (isKurt)
                {
                    value = toString.Replace(KEY_NAME, "");
                }
                else
                {
                    value = toString;
                }
                value = value.Replace("\n", "");
                value = value.Replace("\r", "");
                var locPh = String.Format("{{{0},\"{1}\"}},\n", key, value);
                keysToLoc.Append(locPh);
            }

            index++;
            prevIsKurt = isKurt;
//            Debug.LogError(toString);
        }
        return             new Tuple<StringBuilder, StringBuilder>(keysToLoc, keysToList);

    }

    private static void FirstString(string mainDialogNmae, bool isKurt, StringBuilder keysToList, string toString,
        StringBuilder keysToLoc)
    {
        string key;
        if (isKurt)
        {
            var listKey1 = $"list.Add(\"DC\");\n";
            keysToList.Append(listKey1);
            key = String.Format("\"{0}_A0\"", mainDialogNmae);
            var listKey2 = $"list.Add({key});\n";
            keysToList.Append(listKey2);
        }
        else
        {
            key = String.Format("\"{0}_M0\"", mainDialogNmae);
            var listKey2 = $"list.Add({key});\n";
            keysToList.Append(listKey2);
        }
        string value;
        if (isKurt)
        {
            value = toString.Replace(KEY_NAME, "");
        }
        else
        {
            value = toString;
        }

        value = value.Replace("\n", "");
        value = value.Replace("\r", "");
        var locPh = String.Format("{{{0},\"{1}\"}},\n", key, value);
        keysToLoc.Append(locPh);
    }
}

