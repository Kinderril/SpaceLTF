using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

public class NavPoint
{
    public NavPoint(int index, Vector3 pos)
    {
        Index = index;
        Pos = pos;
    }

    public int Index;
    public bool IsValid = true;
    public Vector3 Pos;
}

public class TestWindowEd : EditorWindow
{

    public Transform Container;
    [MenuItem("Test/WndEnd")]
    static void Init()
    {
        var w = GetWindow<TestWindowEd>();
        w.Show();
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Do Test"))
        {
//            DoTest1();
        }
        Container = (Transform)EditorGUILayout.ObjectField("Container", Container, typeof(Transform), true);
    }

//    private void DoTest1()
//    {
//        if (Container != null)
//            MyExtensions.ClearTransformImmediate(Container);
//        var triangulation = UnityEngine.AI.NavMesh.CalculateTriangulation();
//        var ind = triangulation.indices;
//        var vert = new List<NavPoint>();
//        for (int i = 0; i < triangulation.vertices.Length; i++)
//        {
//            var navPoint = new NavPoint(i,triangulation.vertices[i]);
//            vert.Add(navPoint);
//        }
//        vert.Add(new NavPoint(triangulation.vertices.Length,vert[0].Pos));//TEST
//        Debug.Log("Cnt_vertices on start:" + vert.Count);
//        Dictionary<int,int> indexes2Remove = new Dictionary<int, int>();
//        for (int i = 0; i < vert.Count; i++)
//        {
//            if (indexes2Remove.ContainsKey(i))
//            {
//                continue;
//            }
//            var a = vert[i];
//            for (int j = i + 1; j < vert.Count; j++)
//            {
//                var b = vert[j];
//                var sqrDist = (a.Pos - b.Pos).sqrMagnitude == 0f;
//                if (sqrDist)
//                {
//                    indexes2Remove.Add(j,i);
//                }
//            }
//        }
//        //>>>>>>>>>>>>>>>>>>>>>>>>>>
////        foreach (var i in indexes2Remove)
////        {
////            Debug.Log("indexes : " + i.Key + "   to   " + i.Value);
////        }
//        //>>>>>>>>>>>>>>>>>>>>>>>>>>
//
//
//        foreach (var val in indexes2Remove)
//        {
//            vert[val.Key].IsValid = false;
//            for (int j = 0; j < ind.Length; j++)
//            {
//                var index = ind[j];
//                if (index == val.Key)
//                {
//                    ind[j] = val.Value;
//                }
//            }
//        }
//        
//        Debug.Log("Cnt_vertices after optimiztion:" + vert.Count);
//
/////////>>>>>>>>>>>>>>>>>>>>>>>>>
//        for (int i = 0; i < ind.Length/3; i++)
//        {
//            var b = i*3;
//            var a = ind[b];
//            var a1 = ind[b + 1];
//            var a2 = ind[b + 2];
//            Debug.Log("Triangle: " + a + "   " + a1 + "    " + a2);
//        }
//        foreach (var v in vert)
//        {
//            Debug.Log("Point: " + v.Pos + "  " + v.IsValid);
//            if (v.IsValid)
//            {
//                DrawPoint(1, v.Pos, 0);
//            }
//        }
//        ///////>>>>>>>>>>>>>>>>>>>>>>>>>
//
//
//        if (Container != null)
//            MyExtensions.ClearTransformImmediate(Container);
//        var index2test = vert.Where(x => x.IsValid).ToList().RandomElement();
//        var indexsesOfClose = DoTest(index2test, ind);
//        indexsesOfClose.Remove(index2test);
//        Draw(indexsesOfClose,vert);
//        DrawPoint(2, vert[index2test].Pos, index2test);
//        Debug.Log("Tested index" + index2test);
//        Debug.Log("Cnt_vertices:" + vert.Count);
//        Debug.Log("Cnt_indices:" + ind.Length);
//
//    }

    private List<int> DoTest(int index, int[] indices)
    {
        List<int> IndexesOfTriangles = new List<int>();
        for (int i = 0; i < indices.Length; i++)
        {
            var val = indices[i];
            if (val == index)
            {
                var isOK = i%3 == 0;
                Action<int> action = startIndex =>
                {
                    var t = indices[startIndex];
                    var t1 = indices[startIndex + 1];
                    var t2 = indices[startIndex + 2];
                    AddIfCan(t, IndexesOfTriangles);
                    AddIfCan(t1, IndexesOfTriangles);
                    AddIfCan(t2, IndexesOfTriangles);
                };
                if (isOK)
                {
                    action(i);
                }
                else if ((i-1) % 3 == 0)
                {
                    action(i-1);
                }
                else if ((i-2) % 3 == 0)
                {

                    action(i-2);
                }


            }
        }
        return IndexesOfTriangles;
    }

    private static void AddIfCan<T>(T index, List<T> list)
    {
        if (list.Contains(index))
        {
            return;
        }
        list.Add(index);
    }


    private void Draw(List<int> IndexesOfTriangles, List<NavPoint> list )
    {
        if (Container == null)
        {
            return;
        }

        foreach (var index0 in IndexesOfTriangles)
        {
            var pos = list[index0];
            DrawPoint(1, pos.Pos, index0);
        }
    }

    private void DrawPoint(int index,Vector3 pos ,int nameIndex)
    {
        var g = new GameObject("trianglepoint" + nameIndex);
        g.transform.position = pos;
        g.transform.SetParent(Container, true);
        SetIcon(g, index);
    }

    private static GUIContent[] largeIcons;

    public static void SetIcon(GameObject gObj,int img)
    {
        if (largeIcons == null)
        {
            largeIcons = GetTextures("sv_icon_dot", "_pix16_gizmo", 0, 16);
        }

        SetIcon(gObj, largeIcons[img].image as Texture2D);
    }

    private static GUIContent[] GetTextures(string baseName, string postFix, int startIndex, int count)
    {
        GUIContent[] guiContentArray = new GUIContent[count];
        for (int index = 0; index < count; ++index)
        {
            GUIContent cui = EditorGUIUtility.IconContent(baseName + (object)(startIndex + index) + postFix);
            guiContentArray[index] = cui;
        }

        return guiContentArray;
    }

    private static void SetIcon(GameObject gObj, Texture2D texture)
    {
        var ty = typeof(EditorGUIUtility);
        var mi = ty.GetMethod("SetIconForObject", BindingFlags.NonPublic | BindingFlags.Static);
        mi.Invoke(null, new object[] { gObj, texture });
    }
}
