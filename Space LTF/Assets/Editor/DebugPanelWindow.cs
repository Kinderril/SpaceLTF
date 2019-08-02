using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class DebugPanelWindow : EditorWindow
{
    
    public static List<NavMeshSurface> navMeshSurfaceSelected = new List<NavMeshSurface>();
    public static ShipBase SelectedShip;

    [MenuItem("Tools/Debug Panel")]
    static void Init()
    {
        var w = GetWindow<DebugPanelWindow>();
        w.Show();
        
    }
    
    private Editor _e;
    public const float DIST_CHECK_EARTH = 10f;
    private int Hour;
    private bool SetCarve = false;
    int selected = 0;

    public void OnGUI()
    {
        if (!Application.isPlaying)
        {
            NoInGame();
        }
//        EditorGUILayout.BeginHorizontal();
//        
//        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Find first AI"))
        {
            FindAnyAI();
        }
        if (GUILayout.Button("Kill all enemies"))
        {
            DebugUtils.KillAllEnemies();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("EngineOff:" + DebugParamsController.EngineOff))
        {
            DebugParamsController.SwitchEngine();
        }       
        if (GUILayout.Button("NoDamage:" + DebugParamsController.NoDamage))
        {
            DebugParamsController.SwitchNoDamage();
        }        
        if (GUILayout.Button("NoMouseMove." + DebugParamsController.NoMouseMove))
        {
            DebugParamsController.SwitchNoMouseMove();
        }
        
        EditorGUILayout.EndHorizontal();
//        SelectedShip = (ShipBase)EditorGUILayout.ObjectField("Selected ship ", SelectedShip, typeof(ShipBase), true);
        if (BattleController.Instance.InGameMainUI != null)
        {
            var ss = BattleController.Instance.InGameMainUI.SelectedShip;
            if (ss != null)
            {
                EditorGUILayout.BeginHorizontal();
                float externalPower = 14;
                float externalTime = 0.4f;
                if (GUILayout.Button("External force"))
                {
                    SelectedShip.ExternalForce.Init(externalPower, externalTime, SelectedShip.LookDirection);
                }
                if (GUILayout.Button("External left"))
                {
                    SelectedShip.ExternalForce.Init(externalPower, externalTime, SelectedShip.LookLeft);
                }
                if (GUILayout.Button("Do damage"))
                {
                    DoDamageToSelected();
                }
                if (GUILayout.Button("Do fire"))
                {
                    DoFireToSelected();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }


    private void NoInGame()
    {
        if (GUILayout.Button("Recalc bullets IDs"))
        {
            ResetBulletsIds2();
            Repaint();
        }
    }

    private void FindAnyAI()
    {
        var bots = GameObject.FindObjectsOfType<ShipBase>();
        if (bots != null)
        {
            foreach (var shipBase in bots)
            {
                if (!shipBase.IsDead && shipBase.ShipParameters.StartParams.ShipType != ShipType.Base)
                {
                    SelectedShip = shipBase;
                    return;
                }
            }
        }
    }

    private void ResetBulletsIds()
    {
        int bulletIndex = 1000;
        var db = GameObject.FindObjectOfType<DataBaseController>();
        int lastINdex = 0;
        foreach (var b in db.DataStructPrefabs.Bullets)
        {
            Debug.Log(b.ToString() + "  set id: " + bulletIndex);
            b.ID = bulletIndex;
            bulletIndex++;
           
            lastINdex = bulletIndex;
        }
        Debug.Log("Bullets ids reclcs last index:" + lastINdex);
    }
    public static string[] GetAllPrefabs()
    {
        string[] temp = AssetDatabase.GetAllAssetPaths();
        List<string> result = new List<string>();
        foreach (string s in temp)
        {
            if (s.Contains(".prefab")) result.Add(s);
        }
        return result.ToArray();
    }

    private static void ResetBulletsIds2()
    {
        int bulletIndex = 1000;
        string[] allPrefabs = GetAllPrefabs();
        //        var listResult = new List<string>();
        int errorsCount = 0;
        foreach (string prefab in allPrefabs)
        {
            UnityEngine.Object o = AssetDatabase.LoadMainAssetAtPath(prefab);
            GameObject go;
            try
            {
                go = (GameObject)o;
                var components = go.GetComponentsInChildren<Bullet>(true);
                foreach (Bullet c in components)
                {
                    bulletIndex++;
                    c.ID = bulletIndex;
                }
            }
            catch
            {
                errorsCount++;
            }
        }
        Debug.Log("errorsCount:" + errorsCount);
    }

    private void DoDamageToSelected()
    {
        var selected = BattleController.Instance.InGameMainUI.SelectedShip;
        if (selected != null)
        {
            selected.ShipParameters.Damage(25, 15,null,null);
        }

    }
    private void DoFireToSelected()
    {
        var selected = BattleController.Instance.InGameMainUI.SelectedShip;
        if (selected != null)
        {
            selected.DamageData.ApplyEffect(ShipDamageType.fire,10,true);
        }

    }

    private void CalcAllNavMesh()
    {
        CalcAllNav();
    }

    private static void CalcAllNav()
    {
        foreach (var navMeshSurface in navMeshSurfaceSelected)
        {
            navMeshSurface.BuildNavMesh();
        }
    }
    
    private static void FindAllNavMesh()
    {
        var zones = FindObjectsOfType<NavMeshSurface>();
        if (zones.Any())
        {
            navMeshSurfaceSelected = new List<NavMeshSurface>();
        }
        foreach (var botZone in zones)
        {
            var navMeshSurf = botZone.GetComponent<NavMeshSurface>();
            if (navMeshSurf != null)
            {
                navMeshSurf.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
                navMeshSurfaceSelected.Add(navMeshSurf);
            }
        }

    }
    
    private void SetNavMeshes()
    {
        int newCount = Mathf.Max(0, EditorGUILayout.IntField("NavMeshSurfaces:", navMeshSurfaceSelected.Count));
        while (newCount < navMeshSurfaceSelected.Count)
            navMeshSurfaceSelected.RemoveAt(navMeshSurfaceSelected.Count - 1);
        while (newCount > navMeshSurfaceSelected.Count)
            navMeshSurfaceSelected.Add(null);

        for (int i = 0; i < navMeshSurfaceSelected.Count; i++)
            navMeshSurfaceSelected[i] =
                (NavMeshSurface)EditorGUILayout.ObjectField(navMeshSurfaceSelected[i], typeof(NavMeshSurface), allowSceneObjects: true);
    }
    
}