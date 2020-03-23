using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public class DebugPanelWindow : EditorWindow
{
    
    public static List<NavMeshSurface> navMeshSurfaceSelected = new List<NavMeshSurface>();
    public static ShipBase SelectedShip;
    public static bool EngineOff;
    public static bool NoDamage;
//    public static bool NoMouseMove;
    public static bool FastRecharge;
    public static bool AnyWay;
    public static bool AllModuls;

    [MenuItem("Tools/Debug Panel")]
    static void Init()
    {
        var w = GetWindow<DebugPanelWindow>();
        w.Show();
        
    }
    
    private Editor _e;
    public const float DIST_CHECK_EARTH = 10f;
    private int Hour;
    private int EnemyPower;
    private int MyPower;
    private bool SetCarve = false;
    int selected = 0;

    public void OnGUI()
    {
        if (!Application.isPlaying)
        {
            NoInGame();
        }
        else
        {

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
            if (GUILayout.Button("EngineOff:" + EngineOff))
            {
                SwitchEngine();
            }
            if (GUILayout.Button("NoDamage:" + NoDamage))
            {
                SwitchNoDamage();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
//            if (GUILayout.Button("NoMouseMove." + NoMouseMove))
//            {
//                SwitchNoMouseMove();
//            }

            if (GUILayout.Button("FastRecharge." + FastRecharge))
            {
                SwitchFastRecharge();
            }  
            if (GUILayout.Button("AnyWay." + AnyWay))
            {
                SwitchAnyWay();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("AllModuls." + AllModuls))
            {
                AllModuls = !AllModuls;
                if (AllModuls)
                {
                    PlayerInventory.MAX_SLOTS = 150;
                }
                DebugParamsController.AllModuls = AllModuls;
            }
            if (GUILayout.Button("Hire test."))
            {
                DebugParamsController.TestHire();
            }
            if (GUILayout.Button("LevelUp."))
            {
                LevelUpRandom();
            }     
            if (GUILayout.Button("Exp."))
            {
                ExpAll();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Go End"))
            {
                GoToEnd();
            } 
//            if (GUILayout.Button("Anomaly"))
//            {
//                DebugEventStart.AcitvateDialog(GlobalMapEventType.anomaly);
//            }
            if (GUILayout.Button("MovArmy"))
            {
                BornMovingArmy();
            }
            if (GUILayout.Button("Add core"))
            {
                AddCore();
            }
            EditorGUILayout.EndHorizontal();
            EnemyPower = EditorGUILayout.IntField("EP", EnemyPower);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Fight"))
            {
                FightDebug();
            }
            if (GUILayout.Button("CalcMy"))
            {
                CalcMyPower();
            }
            EditorGUILayout.EndHorizontal();
            if (BattleController.Instance != null && BattleController.Instance.InGameMainUI != null)
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
    }

    private void CalcMyPower()
    {
        var army = MainController.Instance.MainPlayer.Army;
        var power = ArmyCreator.CalcArmyPower(army);
        Debug.Log($"YOU ARMY POWER:{power}".Green());
    }  

    private void BornMovingArmy()
    {                                 
        MainController.Instance.MainPlayer.MapData.GalaxyData.GalaxyEnemiesArmyController.DebugTryBornArmy();
    }

    private void FightDebug()
    {
        ArmyCreatorDebug.DoFight(EnemyPower);
    }

    private void LevelUpRandom()
    {
        var army = MainController.Instance.MainPlayer.Army.Army.Suffle();
        var points = 1000f;
        foreach (var pilotData in army)
        {
            if (pilotData.Ship.ShipType != ShipType.Base)
            {
                if (ArmyCreator.TryUpgradePilot(new ArmyRemainPoints(points), pilotData.Pilot, new ArmyCreatorLogs()))
                {
                    Debug.Log("LevelUpRandom complete");
                    return;
                }
            }
        }
        Debug.LogError("can't upgrade");

    }

    private void ExpAll()
    {
       MainController.Instance.MainPlayer.Army.DebugAddAllExp();
    }

    private void AddCore()
    {
        MainController.Instance.MainPlayer.QuestData.AddElement();
    }

    private void GoToEnd()
    {
        var isCurMapWindow = WindowManager.Instance.CurrentWindow as MapWindow;
        if (isCurMapWindow != null)
        {
            isCurMapWindow.DebugActivateEndDialog();
        }
        else
        {
            Debug.LogError("Current window is not Map Window");
        }

    }

    public static void SwitchEngine()
    {
        EngineOff = !EngineOff;
        DebugParamsController.EngineOff = EngineOff;
    }
    public static void SwitchNoDamage()
    {
        NoDamage = !NoDamage;
        DebugParamsController.NoDamage = NoDamage;
    }
//    public static void SwitchNoMouseMove()
//    {
//        NoMouseMove = !NoMouseMove;
////        DebugParamsController.NoMouseMove = NoMouseMove;
//    }

    public static void SwitchFastRecharge()
    {
        FastRecharge = !FastRecharge;
        DebugParamsController.FastRecharge = FastRecharge;
    }

    public static void SwitchAnyWay()
    {
        AnyWay = !AnyWay;
        DebugParamsController.AnyWay = AnyWay;
    }

    private GameObject _aimingBox;
    private void NoInGame()
    {
        if (GUILayout.Button("Recalc bullets IDs"))
        {
            ResetBulletsIds2();
            Repaint();
        }   
        if (GUILayout.Button("Set Audio"))
        {
            List<GameObject> prefabs = new List<GameObject>();
            LoadAllPrefabsAt("Assets/Resources/Prefabs", prefabs);
            foreach (var gameObject in prefabs)
            {
                AddAudioTest(gameObject);
            }

        }

        _aimingBox = EditorGUILayout.ObjectField(_aimingBox, typeof(GameObject), true) as GameObject;
//        if (GUILayout.Button("Update aiming Box"))
//        {
//            List<GameObject> prefabs = new List<GameObject>();
//            LoadAllPrefabsAt("Assets/Resources/Prefabs", prefabs);
//            var aimingBox = _aimingBox.GetComponent<AimingBox>();
//            if (aimingBox != null)
//            {
//                foreach (var gameObject in prefabs)
//                {
//                    AddAimingBox(gameObject, aimingBox);
//                }
//            }
//
//        }
    }

    public static void LoadAllPrefabsAt(string path, List<GameObject> prefabs)
    {
        if (path != "")
        {
            if (path.EndsWith("/"))
            {
                path = path.TrimEnd('/');
            }
        }

        DirectoryInfo dirInfo = new DirectoryInfo(path);
        var drer = dirInfo.GetDirectories();
        foreach (var directoryInfo in drer)
        {
            LoadAllPrefabsAt(path +"/" + directoryInfo.Name, prefabs);
        }

       FileInfo[] fileInf = dirInfo.GetFiles("*.prefab");

        //loop through directory loading the game object and checking if it has the component you want
        foreach (FileInfo fileInfo in fileInf)
        {
            string fullPath = fileInfo.FullName.Replace(@"\", "/");
            string assetPath = "Assets" + fullPath.Replace(Application.dataPath, "");
            GameObject prefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;

            if (prefab != null)
            {
                prefabs.Add(prefab);
            }
        }
    }

    private void AddAudioTest( GameObject PrefabAsset)
    {
//        PrefabAsset = EditorGUILayout.ObjectField("Prefab", PrefabAsset, typeof(GameObject), allowSceneObjects: false);
        var asset_path = AssetDatabase.GetAssetPath(PrefabAsset);
        var editable_prefab = PrefabUtility.LoadPrefabContents(asset_path);

        // Do your changes here.
        var ShipBase = editable_prefab.GetComponent<ShipBase>();
        if (ShipBase == null)
        {
            return;
        }
        Debug.Log($"{ShipBase.gameObject.name}  SHIP audio COMPLETE");
        if (ShipBase.ShipVisual.EngineEffect != null)
        {
            var source = ShipBase.ShipVisual.EngineEffect.SourceEngine;
            if (source == null)
            {
                //                            PrefabUtility.
                source = ShipBase.ShipVisual.EngineEffect.gameObject.AddComponent<AudioSource>();
                ShipBase.ShipVisual.EngineEffect.SourceEngine = source;
                source.clip = DataBaseController.Instance.AudioDataBase.EngineDefault;
            }

            foreach (var positon in ShipBase.WeaponPosition)
            {
                if (positon.Source == null)
                {
                    positon.Source = positon.GetComponent<AudioSource>();
                }

                if (positon.Source == null)
                {
                    positon.Source = positon.gameObject.AddComponent<AudioSource>();
                }
            }

            ShipType _shipType = ShipType.Base;
            if (PrefabAsset.name.Contains("Heavy"))
            {
                _shipType = ShipType.Heavy;
            }
            else if (PrefabAsset.name.Contains("Mid"))
            {
                _shipType = ShipType.Middle;
            }
            else if (PrefabAsset.name.Contains("Lig"))
            {
                _shipType = ShipType.Light;
            }

            source.clip = DataBaseController.Instance.AudioDataBase.GetEngine(_shipType);
        }

        var mainSource = ShipBase.Audio;
        if (mainSource == null)
        {
            ShipBase.Audio = ShipBase.gameObject.AddComponent<AudioSource>();
        }



        Undo.RecordObject(ShipBase, "Audio FIX");
        ShipBase.enabled = false;

        // NO SAVE - ArgumentException: Can't save a Prefab instance.
        //~ PrefabUtility.SavePrefabAsset(editable_prefab);
        // NO SAVE - Prefab still has MeshRenderer enabled.
        //~ PrefabUtility.SavePrefabAsset(PrefabAsset);
        
        // This save method works.
        PrefabUtility.SaveAsPrefabAsset(editable_prefab, asset_path);
        PrefabUtility.UnloadPrefabContents(editable_prefab);
    }   
    /*
    private void AddAimingBox( GameObject PrefabAsset,AimingBox box)
    {
//        PrefabAsset = EditorGUILayout.ObjectField("Prefab", PrefabAsset, typeof(GameObject), allowSceneObjects: false);
        var asset_path = AssetDatabase.GetAssetPath(PrefabAsset);
        var editable_prefab = PrefabUtility.LoadPrefabContents(asset_path);

        // Do your changes here.
        var ShipBase = editable_prefab.GetComponent<ShipBase>();
        if (ShipBase == null)
        {
            return;
        }

        var par = ShipBase.ShipVisual;
        var item = DataBaseController.GetItem(box);
        item.transform.SetParent(par.transform);
//        ShipBase.AimingBox = item;
        item.transform.localPosition = new Vector3(0,0,0.64f);





        Undo.RecordObject(ShipBase, "Aimng box FIX");
        ShipBase.enabled = false;
        // This save method works.
        PrefabUtility.SaveAsPrefabAsset(editable_prefab, asset_path);
        PrefabUtility.UnloadPrefabContents(editable_prefab);
    }
              */
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
            selected.DamageData.ApplyEffect(ShipDamageType.fire,10);
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