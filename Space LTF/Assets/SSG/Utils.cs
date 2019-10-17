using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using Random = System.Random;

public class WDictionary<T> : Dictionary<float, T>
{
    private float total = 0;
    private Dictionary<T, float> reversed = new Dictionary<T, float>();

    public WDictionary(Dictionary<T, float> items)
    {
        float curW = 0;
        foreach (var item in items)
        {
            var v = item.Value;
            if (v > 0f)
            {
                curW += v;
                Add(curW, item.Key);
                reversed.Add(item.Key, curW);
            }
        }
        total = curW;
    }

    public void Remove(T type)
    {
        if (reversed.ContainsKey(type))
        {
            var key = reversed[type];
            Remove(key);
        }
    }

    public T Random()
    {
        var res = SMUtils.Range(0, total);
        foreach (var key in Keys)
        {
            if (key >= res)
            {
                return this[key];
            }
        }
        return default(T);
    }

    public override string ToString()
    {
        string ss = "";
        foreach (var v in this)
        {
            ss += "{" + v.Key + "/" + v.Value.GetType() + "}";
        }
        return ss;
    }
}

public static class Utils
{
    public static int LAYER_STATIC_OBJ;

    private static bool haveNextNextGaussian;
    private static float nextNextGaussian;
    private static readonly Random random = new Random();
    private static bool uselast = true;
    private static double next_gaussian;
    private static int groundLayerIndex;
    private static int MaxId;


    public static void SetId(int id)
    {
        if (id > MaxId)
        {
            MaxId = id;
        }
    }

    public static int GetId()
    {
        MaxId++;
        return MaxId;
    }

    public static string FloatToChance(float f)
    {
        return (f*100).ToString("0.0");
    }

    public static void Init(Terrain terrain)
    {
        Utils.groundLayerIndex = 1 << terrain.gameObject.layer;
        Utils.LAYER_STATIC_OBJ = LayerMask.NameToLayer("StaticObjects");
    }
    
    public static T RandomElement<T>(this List<T> list)
    {
        if (list.Count == 0)
            return default(T);
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static float magnitude2d(this Vector3 v)
    {
        return Mathf.Sqrt(v.x*v.x + v.z*v.z);
    }

    public static T RandomElement<T>(this T[] array)
    {
        if (array.Length == 0)
            return default(T);
        return array[UnityEngine.Random.Range(0, array.Length)];
    }

    public static List<T> RandomElement<T>(this List<T> list,int count)
    {
        var listOut = new List<T>();
        if (list.Count == 0)
            return listOut;
        if (count > list.Count)
        {
            return list.ToList();
        }
        var nList = list.Suffle();
        for (int i = 0; i < count; i++)
        {
            listOut.Add(nList[i]);
        }

        return listOut;


//        var toDel = list.Count - count;
//        for (int i = 0; i < toDel; i++)
//        {
//            var rnd = nList.RandomElement();
//            nList.Remove(rnd);
//        }
//
//        return nList;


//        if (list.Count <= count)
//        {
//            foreach (var v in list)
//            {
//                listOut.Add(v);
//            }
//            return listOut;
//        }
        
        for (int i = 0; i < count; i++)
        {
            var a = (list.Count/count);
            var index = UnityEngine.Random.Range(i*a + 1, (i + 1)*a);
            var e = list[Mathf.Clamp(index,0,list.Count)];
                listOut.Add(e);
        }

        return listOut;
    }

    public static void CopyMaterials(Renderer renderer, Color? color = null, string propName = "")
    {
        List<Material> materialsInside = new List<Material>();
        var mat = renderer.materials;
        for (int i = 0; i < mat.Length; i++)
        {
            var newMaterial = GameObject.Instantiate(mat[i]) as Material;
            materialsInside.Add(newMaterial);
            if (color.HasValue)
                newMaterial.SetColor(propName, color.Value);
        }
        renderer.materials = materialsInside.ToArray();
    }

    public static void DeactivateIfNedd(GameObject go,bool val)
    {
        if (!go.activeSelf && val)
        {
            go.SetActive(val);
            return;
        }
        if (go.activeSelf && !val)
        {
            go.SetActive(val);
            return;
        }
    }

    public static Vector3 RotateOnAngUp(Vector3 b, float a)
    {
        var r = a * Mathf.Deg2Rad;
        var sin = Mathf.Sin(r);
        var cos = Mathf.Cos(r);

        var x1 = b.x * cos - b.z * sin;
        var y1 = b.z * cos + b.x * sin;

        return new Vector3(x1, b.y, y1);
    }

    public static Vector3 Rotate90(Vector3 n, SideTurn side)
    {
        if (side == SideTurn.left)
        {
            return new Vector3(-n.z, n.y, n.x);
        }
        else
        {
            return new Vector3(n.z, n.y, -n.x);
        }
    }

    private const float COS45 = 0.707106781f;
    public static Vector3 Rotate45(Vector3 n, SideTurn side)
    {
        var sin = side == SideTurn.left ? COS45 : -COS45;
        var cos = COS45;

        var x1 = n.x * cos - n.z * sin;
        var y1 = n.z * cos + n.x * sin;

        return new Vector3(x1, n.y, y1);
    }

    public static bool IsAngLessNormazied(Vector3 a, Vector3 b, float cos)
    {
        //      Вектора a и b ДОЛЖнЫ БЫТЬ НОРМАЛИЗОВАННЫ
#if UNITY_EDITOR
        var ma = a.magnitude;
        var mb = b.magnitude;
        if (Math.Abs(ma - 1f) > 0.05f || Math.Abs(mb - 1f) > 0.05f)
        {
            Debug.LogError("Wrong vectros in this functions U MUST set only normalized vectors or will be smt bad");
        }
        if (Mathf.Abs(cos) > 1f)
        {
            Debug.LogError("Cosinus can't be more than 1!!!");
        }
    #endif
        var aa = a.x * b.x + a.y * b.y + a.z * b.z;
        return aa > cos;
    }

    public static Vector3 NormalizeFast(Vector3 v)
    {
        var d = (float)Math.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
        return new Vector3(v.x / d, v.y / d, v.z / d);
    }
    public static Vector3 NormalizeFastSelf(Vector3 v)
    {
        var d = (float)Math.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
#if UNITY_EDITOR
        if (d == 0)
        {
            Debug.LogError("Don't normalize zro Vector");
        }
#endif
        v.x = v.x / d;
        v.y = v.y / d;
        v.z = v.z / d;
        return v;
    }


    public static void Sort<T>(List<T> list, Func<T, int> GetPriority) where T : MonoBehaviour
    {
        list.Sort((x, y) =>
        {
            var xPriority = GetPriority(x);
            var yPriority = GetPriority(y);
            if (xPriority > yPriority)
            {
                return 1;
            }

            if (yPriority > xPriority)
            {
                return -1;
            }
            return 0;
        });
        for (int i = list.Count - 1; i >= 0; i--)
        {
            var pe = list[i];
            pe.transform.SetAsLastSibling();
        }
    }
    public static void SetRandomRotation(Transform transform)
    {
        transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(-180, 180), 0);
    }
    public static void SetRandomOffset(Transform transform,float offset = 5f)
    {
        var lp = transform.localPosition;
        var xx = lp.x + UnityEngine.Random.Range(-offset, offset);
        var zz = lp.z + UnityEngine.Random.Range(-offset, offset);
        transform.localPosition = new Vector3(xx,lp.y,zz);
    }

    public static void GroundTransform(Transform transform, float checkDist = 9999f)
    {
        RaycastHit hitInfo;
        var p = new Vector3(transform.position.x, 100, transform.position.z);
        Ray ray = new Ray(p, Vector3.down);
//        Debug.DrawRay(p, Vector3.down * 100, Color.yellow, 20);

        

        if (Physics.Raycast(ray, out hitInfo, checkDist,groundLayerIndex))
        {
            var t = transform.position;
//            var groundOffset = hitInfo.distance;
            transform.position = new Vector3(t.x, hitInfo.point.y, t.z);
        }
    }

    public static Material[] CopyMaterials(Renderer renderer,Color? color = null)
    {
        List<Material> materialsInside = new List<Material>();
        var mat = renderer.materials;
        for (int i = 0; i < mat.Length; i++)
        {
            var newMaterial = GameObject.Instantiate(mat[i]) as Material;
            materialsInside.Add(newMaterial);
            if (color.HasValue)
                newMaterial.color = color.Value;
        }
        renderer.materials = materialsInside.ToArray();
        return renderer.materials;
    }

    public static void LoadTexture(string icon,out Sprite IconSprite)
    {
        if (File.Exists(icon))
        {
            var bytes = System.IO.File.ReadAllBytes(icon);
            var texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            texture.filterMode = FilterMode.Bilinear;
            texture.Apply();
            IconSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            IconSprite = null;
        }
    }

    public static float RandomNormal(float min, float max)
    {
        var deviations = 3.5;
        double r;
        while ((r = BoxMuller(min + (max - min)/2.0, (max - min)/2.0/deviations)) > max || r < min)
        {
        }

        return (float)r;
    }

    public static double BoxMuller(double mean, double standard_deviation)
    {
        return mean + BoxMuller()*standard_deviation;
    }

    public static double BoxMuller()
    {
        if (uselast)
        {
            uselast = false;
            return next_gaussian;
        }
        double v1, v2, s;
        do
        {
            v1 = 2.0*random.NextDouble() - 1.0;
            v2 = 2.0*random.NextDouble() - 1.0;
            s = v1*v1 + v2*v2;
        } while (s >= 1.0 || s == 0);

        s = Math.Sqrt((-2.0*Math.Log(s))/s);

        next_gaussian = v2*s;
        uselast = true;
        return v1*s;
    }

    public static void ClearTransform(Transform t)
    {
        foreach (Transform v in t)
        {
            GameObject.Destroy(v.gameObject);
        }
    }

    public static void ClearTransformImmediate(Transform t)
    {
        List<GameObject> obj = new List<GameObject>();
        foreach (Transform v in t)
        {
            obj.Add(v.gameObject);
        }
        while (obj.Count > 0)
        {
            var o = obj[0];
            obj.Remove(o);
            GameObject.DestroyImmediate(o.gameObject);
        }
    }

    public static void ForceAddValue<T>(this List<T> l, T v)
    {
        if (!l.Contains(v))
        {
            l.Add(v);
        }
    }

    public static float FastDot(Vector3 lhs, Vector3 rhs)
    {
        return (float)((double)lhs.x * (double)rhs.x + (double)lhs.z * (double)rhs.z);
    }

    public static float VectorMultY(Vector3 a, Vector3 b)
    {
        return a.z*b.x - a.x*b.z;
    }

    public static float GreateRandom(float ang)
    {
        var a = ang * 0.8f;
        var b = ang * 1.2f;
        return RandomNormal(a, b);
    }
}



public class Tuple<T, W>
{
    public T val1;
    public W val2;

    public Tuple(T t, W w)
    {
        val1 = t;
        val2 = w;
    }
}
