using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class GlobalMapController : MonoBehaviour
{
    private GlobalMapCellObject[,] _allCells;
    private readonly List<SectorGlobalMapInfo> _allSectros = new List<SectorGlobalMapInfo>();

    private readonly Dictionary<GlobalMapCell, List<GlobalMapCellConnector>> _cellsWaysObjects
        = new Dictionary<GlobalMapCell, List<GlobalMapCellConnector>>();

    private readonly List<GlobalMapCellConnector> _connectors = new List<GlobalMapCellConnector>();
    private readonly List<GameObject> _pointers = new List<GameObject>();
    List<GlobalMapCellConnector> _waysList = new List<GlobalMapCellConnector>();

    //    public float OffsetSector;
    private GalaxyData _data;
    private readonly Dictionary<MovingArmy,EnemyGlobalMapMoverObjet> _enemiesObjects = new Dictionary<MovingArmy,EnemyGlobalMapMoverObjet>();
    private bool _isEnable = true;
    private GlobalMapCellObject _lastNearObject;
    private SectorGlobalMapInfo _lastSelectedSector;

    private readonly Dictionary<SectorCellContainer, GlobalMapCellObject> _logicToVisualObjects =
        new Dictionary<SectorCellContainer, GlobalMapCellObject>();

    private float _mapPressedDownTime;
    private MapWindow _mapWindow;
    private Action<GlobalMapCellObject> CallbackNearObjec;
    public GlobalMapCellObject CellPrefab;
    public Transform ConnectionsContainer;
//    public GlobalMapCellConnector GlobalMapCellBorderPrefab;
    public GlobalMapCellConnector GlobalMapCellConnectorPrefab;
    public GameObject GlobalMapCellPointerPrefab;
    public GlobalMapCellConnector GlobalMapCellWayPrefab;
    private bool isBlock = false;
    private bool isInited;
    public GlobalMapMoverObject MapMoverObject;
    private Vector3 max;
    private Vector3 min;
    public Transform MovingArmyContainer;
    private GlobalMapCellObject myCEll;
    public GlobalMapLighterUpCells LighterUpCells;
    public float OffsetCell;
    public Transform PointsContainer;
    public Transform SectorsContainer;
    public Transform WaysContainer;

    private bool IsEnbale
    {
        get => _isEnable;
        set
        {
            _isEnable = value;
            CamerasController.Instance.GlobalMapCamera.Enable(_isEnable);
        }
    }


    public void SingleInit(GalaxyData data, MapWindow mapWindow, Action<GlobalMapCellObject> CallbackNearObjec)
    {
        if (isInited)
            return;
        LighterUpCells.Init(this);
        _waysList.Clear();
        _logicToVisualObjects.Clear();
        SectorsContainer.ClearTransform();
        this.CallbackNearObjec = CallbackNearObjec;
        _mapWindow = mapWindow;
        isInited = true;
        _data = data;
        var player = MainController.Instance.MainPlayer;
        player.Parameters.Scouts.OnUpgrade += OnUpgrade;
        _data.GalaxyEnemiesArmyController.OnAddMovingArmy += OnAddMovingArmy;
        _allCells = new GlobalMapCellObject[data.SizeX, data.SizeZ];
        min = Vector3.zero;
        max = new Vector3(OffsetCell * data.SizeX, 0, OffsetCell * data.SizeZ);
        _data.OnWayDelete += OnWayDelete;
        DrawSectors(_data);
        DrawCells(data);
        Debug.Log($"cells drawed size: SizeX:{data.SizeX}   SizeZ:{data.SizeZ}     SizeOfSector:{data.SizeOfSector}");
        for (var i = 0; i < 22; i++)
        {
            var con = DataBaseController.GetItem(GlobalMapCellConnectorPrefab);
            _connectors.Add(con);
            con.transform.SetParent(ConnectionsContainer);
            con.gameObject.SetActive(false);
            
            var pointer = DataBaseController.GetItem(GlobalMapCellPointerPrefab);
            _pointers.Add(pointer);
            pointer.transform.SetParent(ConnectionsContainer);
            pointer.gameObject.SetActive(false);
        }
//var usedCells = new HashSet<GlobalMapCell>();
//        if (startCell != null)
//            RecursuveDrawWays(data, startCell, usedCells);
        DrawAllWays();
        SetBorders();
        Debug.Log($"cells armies:{Time.frameCount}  {_logicToVisualObjects.Count}");
//        await Task.Yield();
        Debug.Log($"Task.Delay(2000) armies:{Time.frameCount}  {_logicToVisualObjects.Count}");
        DrawCurrentArmies(_data.GalaxyEnemiesArmyController);

        int list = data.GetAllWaysCount();
        if (_waysList.Count != list)
        {
            Debug.LogError($" _waysList visual :{_waysList.Count}    waysLogic:{list}");
        }

        CheckObjectsOnHide();
    }

    private void CheckObjectsOnHide()
    {

        foreach (var logicToVisualObject in _logicToVisualObjects)
        {
            OnHide(logicToVisualObject.Key.Data, !logicToVisualObject.Key.Data.IsHide);
        }
    }

    private void OnUpgrade(PlayerParameter obj)
    {
        UpdateLookAtAllMoverObjects();
        UpdateHideAllMoverObjects();
        foreach (var logicToVisualObject in _logicToVisualObjects)
        {
            logicToVisualObject.Value.TryOpenBattleEvent();
        }
    }

    private void DrawCurrentArmies(GalaxyEnemiesArmyController dataGalaxyEnemiesArmyController)
    {
        foreach (var currentArmy in dataGalaxyEnemiesArmyController.GetCurrentArmies())
        {
            CreateMovingArmy(currentArmy);
        }
    }

    private void DrawAllWays()
    {
        foreach (var globalMapCell in _data.GetAllContainersNotNull())
        {
            var ways = globalMapCell.GetAllPosibleWays();
            foreach (var target in ways)
            {
                DrawWays(target.Data, globalMapCell.Data);
            }
        }

    }

    private void OnWayDelete(int id1, int id2)
    {                                                                
        Debug.LogError($"Tru delete id1:{id1}  id2:{id2}");
        foreach (var connector in _waysList)
        {
            if ((connector.FromId == id1 && connector.ToId == id2) || (connector.FromId == id2 && connector.ToId == id1))
            {
                connector.DestroyWay();
                return;
            }
        }
        Debug.LogError($"Error delted  id1:{id1}  id2:{id2}");
    }

    public void CreateMovingArmy(MovingArmy arg1)
    {
        var start = GetCellObjectByCell(arg1.CurCell.Data);
        if (start == null)
        {
            Debug.LogError($"CreateMovingArmy have null start object:{arg1.CurCell.Data.Id}  indX:{arg1.CurCell.indX}   indZ:{arg1.CurCell.indZ}");
            return;
        }
        EnemyGlobalMapMoverObjet prefab;
        if (arg1 is SpecOpsMovingArmy)
        {
            prefab = DataBaseController.Instance.DataStructPrefabs.SpecOpsMovingArmyObject;
        }
        else
        {
            prefab = DataBaseController.Instance.DataStructPrefabs.SimpleMovingArmyObject;
        }

        var army = DataBaseController.GetItem(prefab);
        army.transform.SetParent(MovingArmyContainer);
        _enemiesObjects.Add(arg1, army);
        var player = MainController.Instance.MainPlayer;
        army.Init(this, start, arg1);
        if (player.ReputationData.Allies.HasValue)
        {
            if (arg1.StartConfig == player.ReputationData.Allies.Value)
            {
                army.SetAllies();
            }
        }

    }

    private void OnAddMovingArmy(MovingArmy arg1, bool arg2)
    {
        if (arg2)
        {
            CreateMovingArmy(arg1);
        }
        else
        {

            if (_enemiesObjects.ContainsKey(arg1))
            {
                var objToRemove = _enemiesObjects[arg1];
                var destroyedArmy = _enemiesObjects.Remove(arg1);
                if (destroyedArmy)
                    GameObject.Destroy(objToRemove.gameObject);
            }
        }
    }

    public void ClearAll()
    {
        isInited = false;
        Dispose();
        CallbackNearObjec = null;
        _lastNearObject = null;
        _lastSelectedSector = null;
        SectorsContainer.ClearTransform();
        PointsContainer.ClearTransform();
        ConnectionsContainer.ClearTransform();
        WaysContainer.ClearTransform();
        _cellsWaysObjects.Clear();
        _allSectros.Clear();
        _connectors.Clear();
        _pointers.Clear();
        _allCells = new GlobalMapCellObject[0, 0];
        if (_data != null)
            _data.OnWayDelete -= OnWayDelete;
        _data = null;
    }

    private void DrawSectors(GalaxyData data)
    {
        foreach (var sector in data.AllSectors)
        {
            var sectorPlace =
                DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.SectorGlobalMapInfo);
            sectorPlace.transform.SetParent(SectorsContainer);
            sectorPlace.Init(sector, data, OffsetCell);
            sectorPlace.name = $"Sector {sector.Id}";
            _allSectros.Add(sectorPlace);
        }
    }

    private void DrawCells(GalaxyData data)
    {
        var allCells2 = data.AllCells();
#if UNITY_EDITOR
        if (allCells2.Length < 10) Debug.LogError("MAP HAVE NO CELLS!!!");

#endif
        GlobalMapCell startCell = null;
        int cellDrawed = 0;
        

//        var vertical = _data.VerticalCount * data.SizeOfSector - 1;gg
        var vertical = _data.SizeZ;
        Debug.Log($"DrawCells Visual: SizeX:{data.SizeX}  vertical:{vertical}");

        for (var i = 0; i < data.SizeX; i++)
        {
            for (var j = 0; j < vertical; j++)
            {
                var cell = allCells2[i, j];
                if (cell == null) continue;

                if (!(cell is GlobalMapNothing))
                {
                    cellDrawed++;
//                    if (cell is StartGlobalCell)
//                        startCell = cell;
                    var v = new Vector3(OffsetCell * i, 0, OffsetCell * j);
                    var cellObj = DataBaseController.GetItem(CellPrefab);
                    cellObj.transform.SetParent(PointsContainer, true);
                    cellObj.transform.position = v;
                    cellObj.name = $"Cell:{cell.ToString()}";
                    cellObj.Init(cell, OffsetCell);
                    _logicToVisualObjects.Add(cell.Container, cellObj);
                    cellObj.Cell.OnDestoyedCell += OnDestoyedCell;
                    cellObj.Cell.OnHide += OnHide;
                    //                    Debug.Log($"Add cell:{Time.frameCount}  {_logicToVisualObjects.Count}");
                    _allCells[i, j] = cellObj;
                }
            }
        }

        if (cellDrawed == 0)
        {
            Debug.LogError("No cell drawed");
        }
    }

    private void OnDestoyedCell(GlobalMapCell cell)
    {
        var waysToDestroy = _cellsWaysObjects[cell];
        foreach (var connector in waysToDestroy) connector.DestroyWay();
        cell.OnDestoyedCell -= OnDestoyedCell;
        cell.OnHide -= OnHide;
    }

    private void HideWays(GlobalMapCell cell)
    {
//        Debug.LogError($"HideWays1:{cell.IsHide} ");
        if (_cellsWaysObjects.TryGetValue(cell, out var waysToDestroy))
        {
//            Debug.LogError($"HideWays2:{cell.IsHide} ");
            foreach (var connector in waysToDestroy)
            {
                connector.Hide();
            }
        }
    }   
    private void ShowWays(GlobalMapCell cell)
    {
//        Debug.LogError($"ShowWays1:{cell.IsHide} ");
        if (_cellsWaysObjects.TryGetValue(cell, out var waysToDestroy))
        {
//            Debug.LogError($"ShowWays2:{cell.IsHide} ");
            foreach (var connector in waysToDestroy)
            {
                connector.Show();
            }
        }
    }

    private void OnHide(GlobalMapCell cell,bool enable)
    {
        if (cell.IsHide)
        {
            HideWays(cell);
        }
        else
        {
            UpdateHideAllMoverObjects();
            ShowWays(cell);
        }
    }

//    private void RecursuveDrawWays(GalaxyData sector, GlobalMapCell currentCell, HashSet<GlobalMapCell> usedCells)
//    {
//        if (usedCells.Contains(currentCell)) return;
//
//        usedCells.Add(currentCell);
//        var getAllways = currentCell.GetCurrentPosibleWays();
//        foreach (var target in getAllways)
//        {
//            DrawWays(target, currentCell);
//            RecursuveDrawWays(sector, target, usedCells);
//        }
//    }

    private Dictionary<GlobalMapCell, HashSet<GlobalMapCell>> _drawedCells = new Dictionary<GlobalMapCell, HashSet<GlobalMapCell>>();

    private void DrawWays(GlobalMapCell target, GlobalMapCell currentCell)
    {
        var c1 = _allCells[currentCell.indX, currentCell.indZ];
        var c2 = _allCells[target.indX, target.indZ];
        if (c1 != null && c2 != null)
        {
            var shallAdd = AddConnectToCheck(target, currentCell);
            if (shallAdd)
            {
                var connector = DataBaseController.GetItem(GlobalMapCellWayPrefab);
                connector.gameObject.transform.SetParent(WaysContainer);
                connector.Init(c1, c2);

                AddWayToCell(target, connector);
            }

//            AddWayToCell(currentCell, connector);
        }
        else
        {
            Debug.LogError($"Can't draw way target:{target.Id}  currentCell:{currentCell.Id}   target:{target.GetType()}   currentCell:{currentCell.GetType()} ");
            Debug.LogError($"currentCell  indX:{currentCell.indX}  indZ:{currentCell.indZ}   target indX:{target.indX}  indZ:{currentCell.indZ} ");
        }

    }

    private bool AddConnectToCheck(GlobalMapCell from, GlobalMapCell to)
    {
        HashSet<GlobalMapCell> cells;
        if (!_drawedCells.TryGetValue(from, out cells))
        {
            cells = new HashSet<GlobalMapCell>();
            if (cells.Contains(to))
            {
//                Debug.LogError($"draw second time {from.Id} to {to.Id}");
                return false;
            }
            cells.Add(to);
            _drawedCells.Add(from, cells);
        }
        else
        {
            if (cells.Contains(to))
            {
//                Debug.LogError($"draw second time {from.Id} to {to.Id}");
                return false;
            }
            cells.Add(to);
        }

        return true;
    }

    private void AddWayToCell(GlobalMapCell target, GlobalMapCellConnector connector)
    {
        List<GlobalMapCellConnector> list;
        if (!_cellsWaysObjects.ContainsKey(target))
        {
            list = new List<GlobalMapCellConnector>();
            _cellsWaysObjects.Add(target, list);
        }
        else
        {
            list = _cellsWaysObjects[target];
        }

        list.Add(connector);
        _waysList.Add(connector);
    }

    public void Dispose()
    {
        var player = MainController.Instance.MainPlayer;
        player.Parameters.Scouts.OnUpgrade -= OnUpgrade;
        if (_data != null)
        {
            _data.GalaxyEnemiesArmyController.OnAddMovingArmy -= OnAddMovingArmy;
            for (var i = 0; i < _data.SizeX; i++)
            { 
                for (var j = 0; j < _data.VerticalCount * _data.SizeOfSector - 1; j++)
                {
                    var cell = _allCells[i, j];
                    if (cell != null)
                    {
                        cell.Cell.OnHide -= OnHide;
                        cell.Cell.OnDestoyedCell -= OnDestoyedCell;
                    }
                }
            }
        }

        LighterUpCells.Dispose();
        foreach (var moverObject in _enemiesObjects)
            DestroyImmediate(moverObject.Value.gameObject);

        _enemiesObjects.Clear();
    }

    public void AfterMoveReset()
    {
        var player = MainController.Instance.MainPlayer;
        SingleReset(player.MapData.CurrentCell, player.MapData.ConnectedCellsToCurrent());

        var armies = player.MapData.GalaxyData.GalaxyEnemiesArmyController;
        armies.CacheTargets(CellHaveObject);
        UpdateLookAtAllMoverObjects();
        UpdateHideAllMoverObjects();
    }

    private void UpdateLookAtAllMoverObjects()
    {
        foreach (var moverObjet in _enemiesObjects)
        {
            moverObjet.Value.UpdateLookDirection();
        }
    }

    private void UpdateHideAllMoverObjects()
    {
        foreach (var moverObjet in _enemiesObjects)
        {
            moverObjet.Value.UpdateCurHideCell();
        }
    }

    public void SingleReset(GlobalMapCell currentCell, HashSet<SectorCellContainer> posibleWays)
    {
        if (_data == null) return;
        //        var cells = _data.AllCells();
        for (var i = 0; i < _data.SizeX; i++)
        {
            for (var j = 0; j < _data.SizeZ; j++)
            {
                var cell = _allCells[i, j];
                if (cell != null)
                {
                    var iamhere = cell.Cell == currentCell;
                    if (iamhere)
                        myCEll = cell;
                    cell.SetIAmHere(iamhere);
                }
            }
        }

        MapMoverObject.Init(myCEll);
        var connectd = 0;
        if (myCEll != null)
        {
            foreach (var globalMapCell in posibleWays)
            {
                if (globalMapCell != null && globalMapCell.Data != null)
                {
                    var data = globalMapCell.Data;
                    var canDraw = !(data is GlobalMapNothing) && !data.IsDestroyed;
                    if (canDraw)
                        DrawConnecttion(myCEll, data, ref connectd);
                }
            }
        }
    }

    private void DrawConnecttion(GlobalMapCellObject myCEll, GlobalMapCell globalMapCell, ref int connectd)
    {
        for (var i = 0; i < _data.SizeX; i++)
            for (var j = 0; j < _data.SizeZ; j++)
            {
                var cell = _allCells[i, j];
                if (cell != null)
                {
                    var toConnect = cell.Cell == globalMapCell;
                    if (toConnect)
                    {
                        var connector = _connectors[connectd];
                        var pointer = _pointers[connectd];
                        connectd++;
                        //                        Debug.Log("Draw connector " + connectd);
                        SetConnectionObject(myCEll, cell, connector, pointer);
                    }
                }
            }

        for (var i = connectd; i < 8; i++)
        {
            var c = _connectors[i];
            c.gameObject.SetActive(false);
            var pointer = _pointers[i];
            pointer.gameObject.SetActive(false);
        }  
    }

    private void SetConnectionObject(GlobalMapCellObject myCEll, GlobalMapCellObject target,
        GlobalMapCellConnector connector,GameObject pointer)
    {
        connector.Init(myCEll, target);
        pointer.SetActive(true);
        pointer.transform.position = target.ModifiedPosition;
    }

    public void Open()
    {
        IsEnbale = true;
        CamerasController.Instance.OpenGlobalCamera();
    }

    private void SetBorders()
    {
        float offsetBorders = 10;
        var offset = new Vector3(offsetBorders, 0, offsetBorders);
        min = min - offset;
        max = max - offset;
        CamerasController.Instance.GlobalMapCamera.InitBorders(min, max);
    }

    public void Close()
    {
        IsEnbale = false;
        gameObject.SetActive(false);
        CamerasController.Instance.CloseGlobalCamera();
    }


    public void Clicked(Vector3 pos, bool left)
    {
        if (!IsEnbale) return;
        if (left)
        {
            var isOverUI = EventSystem.current.IsPointerOverGameObject();
            if (isOverUI) return;

            var ray = GetPointByClick(pos);
            if (ray.HasValue)
            {
                float dist;
                var closeObject = ClosestObject(ray.Value, out dist);
                if (closeObject != null) 
                    _mapWindow.ClickCell(closeObject.Cell);
            }
        }
    }

    private void LateUpdate()
    {
        if (!IsEnbale || !isInited) return;

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            _mapPressedDownTime = Time.time;
            return;
        }

        if (Input.GetMouseButtonUp(0) && Time.time - _mapPressedDownTime < 0.5f &&
            !EventSystem.current.IsPointerOverGameObject())
        {
            Clicked(Input.mousePosition, true);
            return;
        }

        var ray = GetPointByClick(Input.mousePosition);
        UpdateClosetsSector(ray);
        UpdateClosest(ray);
    }

    private void UpdateClosetsSector(Vector3? point)
    {
        if (point.HasValue)
        {
            var secot = ClosestSector(point.Value, out var dist);
            if (secot != null)
            {
                //                Debug.LogError($"closest {secot.transform.position}   point:{point}  name:{secot.name}");
            }

            if (secot != _lastSelectedSector)
            {
                if (_lastSelectedSector != null)
                    _lastSelectedSector.UnSelect();
                _lastSelectedSector = secot;
                _lastSelectedSector.Select();
            }
        }
    }

    private void UpdateClosest(Vector3? ray)
    {
        if (ray.HasValue)
        {
            float dist;
            var nearObject = ClosestObject(ray.Value, out dist);
            if (nearObject != null)
                //                Debug.Log("dist " + Mathf.Sqrt(dist) + "  " + nearObject.Cell.InfoOpen);
                if (dist < 13)
                {
                    SetClosetsObject(nearObject);
                    CallbackNearObjec(nearObject);
                    return;
                }
        }

        SetClosetsObject(null);
        CallbackNearObjec(null);
    }

    private void SetClosetsObject(GlobalMapCellObject nearObject)
    {
        if (_lastNearObject != null) _lastNearObject.UnSelected();

        _lastNearObject = nearObject;

        if (_lastNearObject != null)
            _lastNearObject.Selected();
    }

    [CanBeNull]
    private GlobalMapCellObject ClosestObject(Vector3 pos, out float dist)
    {
        var vv = float.MaxValue;
        var xIndex = (int)((pos.x + OffsetCell / 2f) / OffsetCell);
        var zIndex = (int)((pos.z + OffsetCell / 2f) / OffsetCell);
        if (xIndex >= 0 && xIndex < _data.SizeX && zIndex >= 0 && zIndex < _data.SizeZ)
        {
            var a = _allCells[xIndex, zIndex];
            if (a != null)
            {
                dist = (a.transform.position - pos).sqrMagnitude;
                return a;
            }
        }

        dist = 0f;
        return null;
    }

    private SectorGlobalMapInfo ClosestSector(Vector3 pos, out float dist)
    {
        var startDist = float.MaxValue;
        dist = 0f;
        SectorGlobalMapInfo secotr = null;
        foreach (var allSectro in _allSectros)
        {
            dist = (allSectro.transform.position - pos).sqrMagnitude;
            if (dist < startDist)
            {
                //                Debug.LogError($"dist to secv {dist}   name:{allSectro.name}");
                startDist = dist;
                secotr = allSectro;
            }
        }

        return secotr;
    }

    private Vector3? GetPointByClick(Vector3 pos)
    {
        if (CamerasController.Instance.GlobalMapCamera == null)
            return null;
        var ray = CamerasController.Instance.GlobalMapCamera.ScreenPointToRay(pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 9999999, 1))
            return hit.point;
        return null;
    }

    public void SetCameraHome()
    {
        if (myCEll != null)
            CamerasController.Instance.SetCameraTo(myCEll.ModifiedPosition + new Vector3(0,0,-10));
    }

    public void SetCameraToCellHome(GlobalMapCell cell)
    {
        var cellToNavigate = GetCellObjectByCell(cell);
        if (cellToNavigate != null)
            CamerasController.Instance.SetCameraTo(cellToNavigate.ModifiedPosition + new Vector3(0, 0, -10));
    }   
    public void SetCameraToPosition(Vector3 cell)
    {
            CamerasController.Instance.SetCameraTo(cell + new Vector3(0, 0, -10));
    }

    public void SetCameraToPosition(HashSet<GlobalMapCell> cell)
    {
        Vector3 midPos = Vector3.zero;
        int cnt = 0;
        foreach (var globalMapCell in cell)
        {

            var cellToNavigate = GetCellObjectByCell(globalMapCell);
            if (cellToNavigate != null)
            {
                cnt++;
                midPos += cellToNavigate.ModifiedPosition;
            }
        }

        if (cnt > 0)
        {
            var center = midPos / cnt;
            CamerasController.Instance.SetCameraTo(center + new Vector3(0, 0, -10));
        }
    }

    [CanBeNull]
    public GlobalMapCellObject GetCellObjectByCell(GlobalMapCell cell)
    {
        if (_logicToVisualObjects.TryGetValue(cell.Container, out var cellObj))
            return cellObj;
        Debug.LogError($"Can't find cell {cell}  _logicToVisualObjects.count:{_logicToVisualObjects.Count}");
        return null;
    }

    public void UnBlock()
    {
        IsEnbale = true;
        SetCameraHome();
    }

    public void Block()
    {
        IsEnbale = false;
    }

    public void MoveToCell(GlobalMapCell target, bool armiesCanMove, Action callback)
    {
        MainController.Instance.MainPlayer.SaveOnMoveGame();
        Block();
        var targetCell = GetCellObjectByCell(target);

        var shallChange2 = WindowManager.Instance.WindowSubCanvas.interactable;
        var shallChange = WindowManager.Instance.WindowMainCanvas.interactable;
        if (shallChange)
            WindowManager.Instance.WindowMainCanvas.interactable = false;
        if (shallChange2)
            WindowManager.Instance.WindowSubCanvas.interactable = false;

        bool isMainReady = false, isCallback = false, isEnemiesReady = false;

        void CheckIsAllReady()
        {
            if (isCallback) return;
            if (isMainReady)
            {
                isCallback = true;
                callback();
            }
        }

        if (targetCell != null)
        {
            var timeToMove = MapMoverObject.MoveTo(targetCell, () =>
            {
                CheeckAllArmiesToFight();
                CheeckAllArmiesToDestroy(targetCell.Cell.Container);
                AfterAction(shallChange, shallChange2);
                isMainReady = true;
                CheckIsAllReady();
                AfterMoveReset();
                CamerasController.Instance.SetCameraTo(targetCell.ModifiedPosition);
                Debug.Log(
                    $"My object come to: {targetCell.Cell}.   CurMovingArmy:{targetCell.Cell.CurMovingArmy.HaveArmy()}");
            });
            if (armiesCanMove)
            {
                MoveEnemies(() =>
                {
                    isEnemiesReady = true;
                }, target, timeToMove);
            }
        }
        else
        {
            Debug.LogError("can't find object cell by target cell");
            AfterAction(shallChange, shallChange2);
        }
    }

    private void CheeckAllArmiesToDestroy(SectorCellContainer playerTrg)
    {
        foreach (var currentArmy in _data.GalaxyEnemiesArmyController.GetCurrentArmies())
        {
            currentArmy.AfterStepAction(playerTrg);
        }

    }

    // private void MainObjectMoveTo(Vector3 obj)
    // {
    //     CamerasController.Instance.SetCameraTo(obj);
    // }

    private void CheeckAllArmiesToFight()
    {
        foreach (var globalMapCell in _data.GetAllContainersNotNull())
        {
            if (!(globalMapCell.Data is GlobalMapNothing))
                globalMapCell.CurMovingArmy.CheckIfHaveFight();
        }
    }

    private void MoveEnemies(Action callback, GlobalMapCell playersCell, float timeToMove)
    {
        var completed = 0;
        var targetCallbacks = 0;
        var isCallbackComlete = false;

//        void CheckIsAllComplete()
//        {
//            if (isCallbackComlete) return;
//            if (completed >= targetCallbacks)
//            {
//                isCallbackComlete = true;
//                callback();
//            }
//        }

        timeToMove = timeToMove * 0.99f;


        var armies = MainController.Instance.MainPlayer.MapData.GalaxyData.GalaxyEnemiesArmyController;

        var targets = armies.GetTargets(CellHaveObject);
        armies.LeaveCells(targets);
        foreach (var globalMapCell in targets)
        {
            var army = globalMapCell.Key;
//            army.DoMove();
            if (_enemiesObjects.TryGetValue(army, out var obj))
            {
                obj.AndGo(globalMapCell.Value, timeToMove);
            }
        }


//        var objectsCels = new Dictionary<GlobalMapCell, EnemyGlobalMapMoverObjet>();
//        foreach (var globalMapMoverObject in _enemiesObjects)
//        {
//            var place = globalMapMoverObject.FindPlace(playersCell);
//            if (place != null && !objectsCels.ContainsKey(place))
//                objectsCels.Add(place, globalMapMoverObject);
//        }

//        foreach (var obj in objectsCels)
//        {
//            if (FindAndGo(timeToMove, obj.Value, obj.Key, () =>
//            {
//                completed++;
//                CheckIsAllComplete();
//            }))
//                targetCallbacks++;
//        }

//        if (targetCallbacks == 0)
//        {
//            isCallbackComlete = true;
//            callback();
//        }
    }


    private bool CellHaveObject(SectorCellContainer cell)
    {
        return GetCellObjectByCell(cell.Data) != null;
    }

//    private bool FindAndGo(float timeToMove, EnemyGlobalMapMoverObjet obj, GlobalMapCell cell, Action callback)
//    {
//        if (obj.AndGo(callback, cell, timeToMove))
//            return true;
//
//        return false;
//    }

    private void AfterAction(bool shallChange, bool shallChange2)
    {
        if (shallChange)
            WindowManager.Instance.WindowMainCanvas.interactable = true;
        if (shallChange2)
            WindowManager.Instance.WindowSubCanvas.interactable = true;
        UnBlock();
    }

}