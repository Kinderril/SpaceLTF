using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MinCutTest : MonoBehaviour
{
    private const int PointsCount = 3;
    private int[,] Ways = new int[PointsCount, PointsCount];
    private List<Dictionary<int,bool>> variants = new List<Dictionary<int, bool>>();

    void Awake()
    {
        GetList();
    }

    private void GetList()
    {
        for (int i = 0; i < PointsCount; i++)
        {
            for (int j = 0; j < PointsCount; j++)
            {
                Ways[i, j] = i + j;
            }
        }


        int curIndex = 0;
        Dictionary<int, bool> list = new Dictionary<int, bool>();
        for (int i = 0; i < PointsCount; i++)
        {
            list[i] = false;
        }

        Debug.Log($"Start list count  {list.Count}");
        //Тщем все возможные варианты вершин
        DoRecutsive(list, -1);


        //Выводим все возможные списки вершин на экран
        foreach (var variant in variants)
        {
            DrawList(variant);
        }

        //Проходим по всем спискам вершин и ищем минимальную сумму пути
        int tmpMin = Int32.MaxValue;
        Dictionary<int,bool > tmpList = null;
        Dictionary<int,bool > tmpListOpposit = null;
        foreach (var variant in variants)
        {
            int varSum = 0;
            //Берем остальной список веришн для путей
            var opposit = GetOpposite(variant);
            foreach (var vertex in variant)
            {
                if (vertex.Value)
                {
                    foreach (var opVertex in opposit)
                    {
                        if (opVertex.Value)
                        {
                            var val = Ways[vertex.Key, opVertex.Key];
                            varSum += val;
                        }
                    }
                }
            }

            if (varSum < tmpMin)
            {
                tmpMin = varSum;
                tmpList = variant;
                tmpListOpposit = opposit;
            }
        }

        Debug.Log($"Ответ сумма : {tmpMin}   Списки:");
        if (tmpList != null)
        {
            DrawList(tmpList);
            DrawList(tmpListOpposit);
        }

    }

    private Dictionary<int, bool> GetOpposite(Dictionary<int, bool> variant)
    {
        Dictionary<int, bool> list = new Dictionary<int, bool>();
        foreach (var v in variant)
        {
            list.Add(v.Key,!v.Value);
        }


        return list;
    }

    private void DoRecutsive(Dictionary<int, bool> list,int index)
    {
        var nextIndex = index + 1;
        if (nextIndex >= PointsCount)
        {
            return;
        }

        var copy = Copy(list);
        copy[nextIndex] = false;
        TryAdd(copy);
        DoRecutsive(copy, nextIndex);


        var copy2 = Copy(list);
        copy2[nextIndex] = true;
//        Debug.Log($"dd");
        TryAdd(copy2);
        DoRecutsive(copy2, nextIndex);
    }

    private Dictionary<int, bool> Copy(Dictionary<int, bool> list)
    {
        Dictionary<int, bool> copy = new Dictionary<int, bool>();
        foreach (var b in list)
        {
            copy.Add(b.Key,b.Value);
        }
//        Debug.Log($"Copy : {copy.Count}");
        return copy;

    }

    private void DrawList(Dictionary<int, bool> list)
    {
        string ss = $"{list.Count}===";
        foreach (var b in list)
        {
            if (b.Value)
            {
                ss = $"{ss}-{b.Key}";
            }
            else
            {

                ss = $"{ss}-_";
            }
        }
        Debug.Log(ss);
    }

    private int countTryAdd = 0;

    private void TryAdd(Dictionary<int, bool> list)
    {
        countTryAdd++;
        bool canAdd = true;
        if (variants.Count > 0)
        {
            foreach (var variant in variants)
            {
                bool isAllTheSame = true;
                for (int i = 0; i < PointsCount; i++)
                {
                    if (variant[i] != list[i])
                    {
                        isAllTheSame = false;
                        break;
                    }
                }

                if (isAllTheSame)
                {
                    canAdd = false;
                    break;
                }
            }
        }
        else
        {
            canAdd = true;
        }

        if (canAdd)
        {
            canAdd = list.Any(x => x.Value);
            if (canAdd)
            {
                canAdd = list.Any(x => !x.Value);
            }
        }

//        Debug.Log($"try {countTryAdd} add: {canAdd}  variant.Count:{variants.Count}");
//        DrawList(list);
        if (canAdd)
        {
            variants.Add(list);
        }
    }
}
