using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public class DebugPanelWindow : EditorWindow
{
    
    public static List<NavMeshSurface> navMeshSurfaceSelected = new List<NavMeshSurface>();
//    public static ShipBase SelectedShip => BattleController.Instance.
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
//            if (GUILayout.Button("Find first AI"))
//            {
//                FindAnyAI();
//            }
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
            
            if (GUILayout.Button("FastRecharge." + FastRecharge))
            {
                SwitchFastRecharge();
            }  
            if (GUILayout.Button("AnyWay." + AnyWay))
            {
                SwitchAnyWay();
            }
            if (GUILayout.Button("AllModuls." + AllModuls))
            {
                AllModuls = !AllModuls;
                DebugParamsController.AllModuls = AllModuls;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("1 act" ))
            {
                if (MainController.Instance.Campaing == null)
                {
                    Debug.LogError("MainController.Instance.Campaing is NULL!");
                    return;
                }
                MainController.Instance.Campaing.DebugNewChamp(0);
            }   
            if (GUILayout.Button("2 act" ))
            {
                if (MainController.Instance.Campaing == null)
                {
                    Debug.LogError("MainController.Instance.Campaing is NULL!");
                    return;
                }
                MainController.Instance.Campaing.DebugNewChamp(1);
            } 
            if (GUILayout.Button("3 act" ))
            {
                if (MainController.Instance.Campaing == null)
                {
                    Debug.LogError("MainController.Instance.Campaing is NULL!");
                    return;
                }
                MainController.Instance.Campaing.DebugNewChamp(2);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
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
            if (GUILayout.Button("Modules"))
            {
                AddDebugModules();
            }
            if (GUILayout.Button("MovArmy"))
            {
                BornMovingArmy();
            }  
            if (GUILayout.Button("CompQues"))
            {
                DebugCompleteQuest();
            }
            if (GUILayout.Button("AddFinalQuest"))
            {
                AddFinalQuest();
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
                if (ss != null )
                {
                    EditorGUILayout.BeginHorizontal();
                    float externalPower = 14;
                    float externalTime = 0.4f;
                    var selectedShip = ss;
                    if (GUILayout.Button("Twist"))
                    {
                        selectedShip.Boost.BoostTwist.Activate();
                    }  
                    if (GUILayout.Button("Ram"))
                    {
                        selectedShip.Boost.BoostRam.Activate();
                    }

//                    if (GUILayout.Button("External force"))
//                    {
//                        SelectedShip.ExternalForce.Init(externalPower, externalTime, SelectedShip.LookDirection);
//                    }
//                    if (GUILayout.Button("External left"))
//                    {
//                        SelectedShip.ExternalForce.Init(externalPower, externalTime, SelectedShip.LookLeft);
//                    }
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

    private void DebugCompleteQuest()
    {
        MainController.Instance.MainPlayer.QuestData.DebugCompleteRndQuest();

    }

    private void AddFinalQuest()
    {
        MainController.Instance.MainPlayer.QuestData.AddFinalQuests();
    }

    private void CalcMyPower()
    {
        var army = MainController.Instance.MainPlayer.Army;
        var power = ArmyCreator.CalcArmyPower(army);
        Debug.Log($"YOU ARMY POWER:{power}".Green());
    }


    private void AddDebugModules()
    {
        var player = MainController.Instance.MainPlayer;
        if (player == null)
        {
            return;
        }

        var inventory = player.Inventory;
        if (inventory == null)
        {
            return;
        }

        var allVals = (SimpleModulType[])Enum.GetValues(typeof(SimpleModulType));
        foreach (var type in allVals)
        {
            if (inventory.GetFreeSimpleSlot(out var index1))
            {
                var modul = Library.CreatSimpleModul(type, 1);
                inventory.TryAddSimpleModul(modul, index1);
            }
        }

        var allSpellType = (SpellType[])Enum.GetValues(typeof(SpellType));
        foreach (var type in allSpellType)
        {
            if (type != SpellType.BaitPriorityTarget && type != SpellType.priorityTarget && inventory.GetFreeSpellSlot(out var index1))
            {
                var modul = Library.CreateSpell(type);
                inventory.TryAddSpellModul(modul, index1);
            }

        }
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
        DebugParamsController.LevelUpRandom(MainController.Instance.MainPlayer);
    }

    private void ExpAll()
    {
       MainController.Instance.MainPlayer.Army.DebugAddAllExp();
    }

//    private void AddCore()
//    {
////        MainController.Instance.MainPlayer.QuestData.AddElement();
//    }

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

    private GameObject _cell1;
    private GameObject _cell2;
    private GameObject _targetTransform;
    private GameObject _targetParent;
    private GameObject _targetPrefab;
    private GameObject shipToFindRenderer;
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
        shipToFindRenderer = EditorGUILayout.ObjectField(shipToFindRenderer, typeof(GameObject), true) as GameObject;
        if (GUILayout.Button("CacheRenderers"))
        {
//            List<GameObject> prefabs = new List<GameObject>();
//            LoadAllPrefabsAt("Assets/Resources/Prefabs", prefabs);
            var shaderToFind = Shader.Find("Custom/HeroShader");
            if (shaderToFind == null)
            {
                Debug.LogError($"Can't find shader to cache");
                return;
            }

            if (shipToFindRenderer != null)
            {
                CheckRenderers(shipToFindRenderer, shaderToFind.name);

            }
            else
            {
                var allSHips = GameObject.FindObjectsOfType<ShipBase>();
                foreach (var shipBase in allSHips)
                {
                    CheckRenderers(shipBase.gameObject, shaderToFind.name);
                }
            }

        }

        if (GUILayout.Button("Create localization"))
        {
            var locEng = EngLocalization._locals;
            var locRus = RusLocalization._locals;

//            Debug.Log($"locEng:{locEng.Count} locRus:{locRus.Count} ");

             Dictionary<string,string> nextLoc = new Dictionary<string, string>();
            foreach (var upload in locEng)
            {
                var key = upload.Key;
                var isGood= GetTranslate( upload.Value,out var translation);
                if (isGood)
                {
                    nextLoc.Add(key,translation);
                }
                else
                {
                    Debug.LogError($"can't translate:{upload.Key}  {upload.Value}");
                }
            }
            CreateTxtFile(nextLoc, "esp");
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Set Datas"))
        {
            if (_targetTransform != null)
            {
                SetDatasToCells(_targetTransform);
            }
        }

        if (GUILayout.Button("DrawConnectors"))
        {
            if (_targetTransform != null && _targetParent != null  && _targetPrefab!=null)
            {
                Draw2dConnectors(_targetTransform, _targetParent.transform, _targetPrefab);
            }
        }

        EditorGUILayout.EndHorizontal();
        _targetTransform = EditorGUILayout.ObjectField("Begin", _targetTransform, typeof(GameObject), true) as GameObject;
        _targetParent = EditorGUILayout.ObjectField("Parent", _targetParent, typeof(GameObject), true) as GameObject;
        _targetPrefab = EditorGUILayout.ObjectField("Prefab",_targetPrefab, typeof(GameObject), true) as GameObject;

        if (GUILayout.Button("Upload all cells"))
        {
            UploadAllCells();
        }
        _cell1 = EditorGUILayout.ObjectField("Cell1", _cell1, typeof(GameObject), true) as GameObject;
        _cell2 = EditorGUILayout.ObjectField("Cell12", _cell2, typeof(GameObject), true) as GameObject;
        if (GUILayout.Button("Try Link"))
        {
            TryLink(_cell1,_cell2);
        }
        if (GUILayout.Button("GlowUpConnections"))
        {
            GlowUpConnections();
        }
        //        EditorGUILayout.EndHorizontal();
    }

    private void TryLink(GameObject cell1, GameObject cell2)
    {
        if (cell1 == null || cell2 == null)
        {
            return;
        }
        var c1 = cell1.GetComponent<ExprolerGlobalMapCell>();
        var c2 = cell2.GetComponent<ExprolerGlobalMapCell>();
        if (c1 == null || c2 == null)
        {
            return;
        }

        c1.Neighhoods.Add(c2.Id);
        c1.ids = $"{c1.ids}_{c2.Id}";
        c2.Neighhoods.Add(c1.Id);
        c2.ids = $"{c2.ids}_{c1.Id}";

    }

    private void UploadAllCells()
    {
        var mapObject = GameObject.FindObjectOfType<ExprolerGlobalMap>();
        if (mapObject != null)
        {
            Undo.RecordObject(mapObject, "Record all2");
            var findCElls = GameObject.FindObjectsOfType<ExprolerGlobalMapCell>();
            HashSet<int> ids = new HashSet<int>();
            foreach (var cell in findCElls)
            {
                if (ids.Contains(cell.Id))
                {
                    Debug.LogError($"Doubled ids:{cell.Id}");
                }

                ids.Add(cell.Id);
            }
            mapObject.AllCells = findCElls.ToList();
            Debug.Log($"Cells linked: {mapObject.AllCells.Count} ");
        }
        else
        {
            Debug.LogError("can't find ExprolerGlobalMap. maybe this GameObject turned off?");
        }

    }

    public void GlowUpConnections()
    {
        var findCElls = GameObject.FindObjectsOfType<ExprolerGlobalMapCell>();

        foreach (var exprolerGlobalMapCell in findCElls)
        {
            var split = exprolerGlobalMapCell.ids.Split('_');
            foreach (var s in split)
            {
                if (s.Length > 0)
                {
                    if (Int32.TryParse(s, out var nID))
                    {
                        var trg = findCElls.FirstOrDefault(x => x.Id == nID);
                        if (trg != null)
                        {
                            var start = exprolerGlobalMapCell.transform.position;
                            var end = trg.transform.position;

                            var dir = end - start;


                            Debug.DrawLine(start, start + dir/2,Color.red,5f);
                        }
                    }
                }

            }
        }
    }

    private void SetDatasToCells(GameObject targetTransform)
    {
        EditorSceneManager.MarkAllScenesDirty();
        Undo.RecordObject(targetTransform, "Record all");
        bool ConfigSetter = false;
        ShipConfig config = ShipConfig.droid;
        int curId = 0;
        int powerIndex = 0;
        int lastPower = 12;
        foreach (Transform transform in targetTransform.transform)
        {
            Undo.RecordObject(transform, "Record all");
            var cell = transform.GetComponent<ExprolerGlobalMapCell>();
            if (cell != null)
            {
                cell.Neighhoods.Clear();
                cell.ids = "";
            }
        }

        //Set ids
        foreach (Transform transform in targetTransform.transform)
        {
            var cell = transform.GetComponent<ExprolerGlobalMapCell>();
            if (cell != null)
            {
                Undo.RecordObject(cell, "Record all");
                if (!ConfigSetter)
                {
                    ConfigSetter = true;
                    config = cell.Config;
                    switch (config)
                    {
                        case ShipConfig.raiders:
                            curId = 600;
                            break;
                        case ShipConfig.federation:
                            curId = 300;
                            break;
                        case ShipConfig.mercenary:
                            curId = 200;
                            break;
                        case ShipConfig.ocrons:
                            curId = 400;
                            break;
                        case ShipConfig.krios:
                            curId = 500;
                            break;
                        default:
                        case ShipConfig.droid:
                            curId = 100;
                            break;
                    }
                }

                cell.Id = curId;
                curId++;
                cell.Config = config;
                cell.Power = lastPower + powerIndex * 2;
                cell.name = $"Cell_{config.ToString()}_{cell.Id}";
                powerIndex++;
                var field = transform.GetComponentInChildren<TextMeshProUGUI>();
                if (field != null)
                {
                    field.text = cell.Power.ToString();
                }
            }
        }

        ExprolerGlobalMapCell lastCell = null;
        foreach (Transform transform1 in targetTransform.transform)
        {
            var cell2 = transform1.GetComponent<ExprolerGlobalMapCell>();
            if (cell2 != null)
            {
                if (lastCell != null)
                {
                    Undo.RecordObject(cell2.gameObject, "Record all");
                    Undo.RecordObject(lastCell.gameObject, "Record all");
                    cell2.Neighhoods.Add(lastCell.Id);
                    lastCell.Neighhoods.Add(cell2.Id);

                    cell2.ids = $"{cell2.ids}_{lastCell.Id}";
                    lastCell.ids = $"{lastCell.ids}_{cell2.Id}";

                    Undo.RecordObject(lastCell.gameObject, "Record all");
                }

                lastCell = cell2;
            }
        }
        Undo.RecordObject(targetTransform, "Record all3");

    }

    private void Draw2dConnectors(GameObject targetTransform,Transform trParent,GameObject prefab)
    {
        foreach (Transform trToDel in trParent)
        {
            GameObject.DestroyImmediate(trToDel.gameObject);
        }

        var list = new List<ExprolerGlobalMapCell>();
        foreach (Transform transform in targetTransform.transform)
        {
            var cellData = transform.GetComponent<ExprolerGlobalMapCell>();
            if (cellData != null)
            {
                list.Add(cellData);
            }
        }
        HashSet<int> createdWays = new HashSet<int>();

        foreach (var exprolerGlobalMapCell in list)
        {
            foreach (var globalMapCell in exprolerGlobalMapCell.Neighhoods)
            {
                var sum = globalMapCell + exprolerGlobalMapCell.Id;
                if (!createdWays.Contains(sum))
                {
                    createdWays.Add(sum);
                    var tr2 = list.FirstOrDefault(x => x.Id == globalMapCell);
                    if (tr2 != null)
                    {
                        Draw2dConnector(exprolerGlobalMapCell.transform, tr2.transform, trParent, prefab);
                    }
                }
            }
        }
//        for (int i = 0; i < list.Count-1; i++)
//        {
//            var tr1 = list[i];
//            var tr2 = list[i+1];
//            Draw2dConnector(tr1,tr2, trParent, prefab);
//        }
    }
    
    private void Draw2dConnector(Transform tr1, Transform tr2, Transform trParent, GameObject prefab)
    {
        var obj = DataBaseController.GetItem(prefab);
        obj.transform.SetParent(trParent);

        var midPos = (tr1.position + tr2.position) / 2f;
        var rectTr = obj.GetComponent<RectTransform>();

        var dir = (tr1.position - tr2.position);
        var dist = dir.magnitude;

        rectTr.localScale = new Vector3(dist/100f,1,1);

        var ang = Vector3.Angle(dir, new Vector3(1, 0, 0));
        if (tr2.position.y < tr1.position.y)
        {
            ang = -ang;
        }

        rectTr.rotation = Quaternion.Euler(new Vector3(0,0,-ang)) ;

        obj.name = $"CON {tr1.name}_{tr2.name}";
        rectTr.position = midPos;


    }

    private void CheckRenderers(GameObject gameObject,string nameShader)
    {
//        var asset_path = AssetDatabase.GetAssetPath(gameObject);
//        var editable_prefab = PrefabUtility.LoadPrefabContents(asset_path);

        var ShipBase = gameObject.GetComponent<ShipBase>();
        if (ShipBase == null)
        {
            return;
        }
        HashSet<Renderer> renderers = new HashSet<Renderer>();

        GetRenderers(ShipBase.transform, renderers, nameShader);
        ShipBase.Renderers = renderers.ToList();
        Debug.Log($"Shp ready:{ShipBase.name}  renderers:{renderers.Count}");

//        PrefabUtility.SaveAsPrefabAsset(editable_prefab, asset_path);
//        PrefabUtility.UnloadPrefabContents(editable_prefab);
    }

    private void GetRenderers(Transform tr1, HashSet<Renderer> renderers,string nameShader)
    {
        if (tr1 == null)
        {
            return;
        }
        foreach (Transform tr in tr1)
        {
            var renderer = tr.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                if (renderer.sharedMaterial != null && renderer.sharedMaterial.shader.name == nameShader)
                {
                    renderers.Add(renderer);
                }
            }
            GetRenderers(tr,renderers,nameShader);
        }
    }

    private void CreateTxtFile(Dictionary<string, string> locEng,string fileSubName)
    {
        string fileName = $"{fileSubName}locCreated.txt";
        StringBuilder builder = new StringBuilder();
        foreach (var d in locEng)
        {
            builder.Append("{\"");
            builder.Append(d.Key);
            builder.Append("\",\"");
            var replaces = Regex.Replace(d.Value, "\n", "");
            builder.Append(replaces);
            builder.Append("\"},\n");
        }

        var fullPath = Application.persistentDataPath + "/" + fileName;
        Debug.LogError($"Path: {fullPath}");

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        var result = builder.ToString();
        File.Create(fullPath).Dispose();
        File.AppendAllText(fullPath,result);

    }

    bool GetTranslate(string word,out string translate)
    {
//        var result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Translation code =\"200\" lang=\"en-ru\" ><text> название </text></Translation>";

        try
        {

            var webClient = new WebClient();
            var result = webClient.DownloadString("https://translate.yandex.net/api/v1.5/tr/translate?" +
                                                  "key=trnsl.1.1.20200415T103235Z.360c509a1082cd01.a1a4ec75dc21cc1c43154616c4f401618af8ebec" +
                                                  $"&text={word}" +
                                                  "&lang=en-es");
//            Debug.Log(result);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            translate = doc.InnerText;
            return true;
        }
        catch (Exception e)
        {
            translate = "";
            return false;
        }
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
//    private void FindAnyAI()
//    {
//        var bots = GameObject.FindObjectsOfType<ShipBase>();
//        if (bots != null)
//        {
//            foreach (var shipBase in bots)
//            {
//                if (!shipBase.IsDead && shipBase.ShipParameters.StartParams.ShipType != ShipType.Base)
//                {
//                    SelectedShip = shipBase;
//                    return;
//                }
//            }
//        }
//    }

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