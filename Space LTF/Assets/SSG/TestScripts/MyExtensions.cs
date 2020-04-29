using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public static class MyExtensions
{
    private static System.Random rnd;

    static MyExtensions()
    {
        rnd = new System.Random((int)DateTime.Now.Ticks);
    }

    public static string SubstringIfNecessary(this string @string, int count)
    {
        return @string.Length > count ? @string.Substring(0, count - 2) + "..." : @string;
    }

    public static float MinorRandom(float val)
    {
        return val * Random(0.9f, 1.12f);
    }   
    public static float GreateRandom(float val)
    {
        return val * Random(0.8f, 1.2f);
    }

    public static float Random(float a, float b)
    {
        var f = rnd.NextDouble();
        return (float)(a + (b - a) * f);
    }

    public static int Random(int a, int b)
    {
        var f = rnd.NextDouble();
        return (int)((a + (b - a) * f) + .5f);
    }
    public static int RandomSing()
    {
        if (IsTrueEqual())
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    public static bool IsTrue01(float a)
    {
        return Random(0f, 1f) < a;
    }     
    public static bool IsTrueEqual()
    {
        return IsTrue01(.5f);
    }

    public static bool IsPointInside(Vector3 point, Vector3 s, Vector3 e)
    {
        if (point.x < e.x && point.x > s.x &&
   point.y < e.y && point.y > s.y &&
   point.z < e.z && point.z > s.z)
            return true;
        else
            return false;
    }
    public static bool IsPointInsideNoY(Vector3 point, Vector3 s, Vector3 e)
    {
        if (point.x < e.x && point.x > s.x &&
   point.z < e.z && point.z > s.z)
            return true;
        else
            return false;
    }

    public static string SubstringIfNecessary(this int @int)
    {
        return @int > 99 ? "> 99" : @int.ToString();
    }

    public static string RemoveAllNewLines(this string @string)
    {
        return @string.Replace('\n', (char)0);
    }

    public static List<T> Suffle<T>(this List<T> l)
    {
        return l.OrderBy(x => MyExtensions.Random(0f, 5f)).ToList();
    }

    public static void ClearTransform(this Transform t)
    {
        foreach (Transform v in t)
        {
            GameObject.Destroy(v.gameObject);
        }
    }

    public static void ClearTransformImmediate(this Transform t)
    {
        List<Transform> list = new List<Transform>();
        foreach (Transform v in t)
        {
            list.Add(v);
        }
        foreach (var transform in list.ToArray())
        {
            GameObject.DestroyImmediate(transform.gameObject);
        }
    }

    public static void ForceAddValue<T, W>(this Dictionary<T, W> d, T k, W v)
    {
        if (d.ContainsKey(k))
        {
            d[k] = v;
        }
        else
        {
            d.Add(k, v);
        }
    }

    public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
    {
        return source.OrderBy<T, int>((item) => UnityEngine.Random.Range(0, 1000));
    }

//    public static T RandomElement<T>(this List<T> list)
//    {
//        if (list.Count == 0)
//        {
//            return default(T);
//        }
//        return list[UnityEngine.Random.Range(0, list.Count)];
//    }


    public static void ForceAddValue<T>(this List<T> l, T v)
    {
        if (!l.Contains(v))
        {
            l.Add(v);
        }
    }
}