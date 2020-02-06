using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = System.Random;

public enum ParamType
{
    PPower,
    MPower,
    PDef,
    MDef,
    Heath,
    Speed,

}

public static class SMUtils
{
    public const char DELEM_VECTOR = '>';
    public const char DELEM = '|';
    public const char DELEM_BULLET = ']';
    public const char DELEM_EFFECT = ']';
    public const char DELEM_COUNT = '-';

    static Random _r = new Random((int)DateTime.Now.Ticks);

    public static float Range(float a1,float a2)
    {
        var rr =  (float)_r.NextDouble();
        float min;
        float max;
        if (a1 < a2)
        {
            min = a1;
            max = a2;
        }
        else
        {
            min = a2;
            max = a1;
        }

        return min + (max - min) *rr;
    }

    public static int Range(int a1,int a2,bool inclusiveUpper = false)
    {
        var min = Math.Min(a1, a2);
        var max = Math.Max(a1, a2);
        if (inclusiveUpper)
        {
            max += 1;
        }
        return _r.Next(min, max);
    }

    public static float Abs(float power2)
    {
        if (power2 < 0)
        {
            return -power2;
        }
        return power2;
    }

    public static string Vector2String(Vector3 v)
    {
        return v.x.ToString() + DELEM_VECTOR + v.y.ToString() + DELEM_VECTOR + v.z.ToString();
    }

    public static Vector3 String2Vector(string info)
    {
        var ss = info.Split(DELEM_VECTOR);
        var xx = Convert.ToSingle(ss[0]);
        var yy = Convert.ToSingle(ss[1]);
        var zz = Convert.ToSingle(ss[2]);
        return new Vector3(xx,yy,zz);
    }


    public static string Yellow(this string input)
    {
        return $"<color=yellow>{input}</color>";
    }

    public static string Green(this string input)
    {
        return $"<color=green>{input}</color>";
    }

    public static string Red(this string input)
    {
        return Namings.TryFormat("<color=red>{0}</color>", input);
    }

    public static string Blue(this string input)
    {
        return Namings.TryFormat("<color=blue>{0}</color>", input);
    }

}

//public class WDictionary<T> : Dictionary<float, T>
//{
//    private float total = 0;
//    private Dictionary<T, float> reversed = new Dictionary<T, float>();
//
//    public WDictionary(Dictionary<T, float> items)
//    {
//        float curW = 0;
//        foreach (var item in items)
//        {
//            curW += item.Value;
//            Add(curW, item.Key);
//            reversed.Add(item.Key,curW);
//        }
//        total = curW;
//    }
//
//    public void Remove(T type)
//    {
//        var key = reversed[type];
//        Remove(key);
//    }
//
//    public T Random()
//    {
//        var res = SMUtils.Range(0, total);
//        foreach (var key in Keys)
//        {
//            if (key >= res)
//            {
//                return this[key];
//            }
//        }
//        return default(T);
//    }
//}

