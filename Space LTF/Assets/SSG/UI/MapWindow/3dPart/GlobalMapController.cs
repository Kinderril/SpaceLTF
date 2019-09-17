using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;


public class GlobalMapController : MonoBehaviour
{
    private GlobalMapCellObject[,] _allCells;
    public GlobalMapCellObject CellPrefab;
    public float OffsetCell;
//    public float OffsetSector;
    private GalaxyData _data;

    private bool IsEnbale
    {
        get { return _isEnable;}
        set
        {
            _isEnable = value;
            CamerasController.Instance.GlobalMapCamera.Enable(_isEnable);

        }
    }
    private bool _isEnable = true;
    private bool isInited = false;
    private bool isBlock = false;
    private Vector3 min;
    private Vector3 max;
    private MapWindow _mapWindow;
    private Action<GlobalMapCellObject> CallbackNearObjec;
    private GlobalMapCellObject _lastNearObject = null;
    public Transform WaysContainer;
    public Transform ConnectionsContainer;
    public Transform PointsContainer;
    public Transform SectorsContainer;
    public GlobalMapCellConnector GlobalMapCellConnectorPrefab;
    public GlobalMapCellConnector GlobalMapCellWayPrefab;
    public GlobalMapMoverObject MapMoverObject;
    public GlobalMapCellConnector GlobalMapCellBorderPrefab;
    private List<GlobalMapCellConnector> _connectors = new List<GlobalMapCellConnector>();
    private Dictionary<GlobalMapCell,List<GlobalMapCellConnector>> _cellsWaysObjects 
        = new Dictionary<GlobalMapCell, List<GlobalMapCellConnector>>();
    GlobalMapCellObject myCEll = null;
    private float _mapPressedDownTime;
    private List<SectorGlobalMapInfo> _allSectros = new List<SectorGlobalMapInfo>();
    private SectorGlobalMapInfo _lastSelectedSector;


    public void SingleInit(GalaxyData data,MapWindow mapWindow, Action<GlobalMapCellObject> CallbackNearObjec)
    {
        if (isInited)
        {
            return;
        }
        SectorsContainer.ClearTransform();
        this.CallbackNearObjec = CallbackNearObjec;
        _mapWindow = mapWindow;
        isInited = true;
        _data = data;
        _allCells = new GlobalMapCellObject[data.Size, data.Size];
        min = Vector3.zero;
        max = new Vector3(OffsetCell * data.Size, 0, OffsetCell * data.Size);

        DrawSectors(_data);
        var startCell = DrawCells(data);
        for (int i = 0; i < 22; i++)
        {
            var con = DataBaseController.GetItem(GlobalMapCellConnectorPrefab);
            _connectors.Add(con);
            con.transform.SetParent(ConnectionsContainer);
            con.gameObject.SetActive(false);
        }
        HashSet<GlobalMapCell> usedCells = new HashSet<GlobalMapCell>();
        if (startCell != null)
            RecursuveDrawWays(data, startCell, usedCells);
        SetBorders();
    }

    public void ClearAll()
    {
        isInited = false;
        Dispsoe();
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
        _allCells = new GlobalMapCellObject[0,0];
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

    private GlobalMapCell DrawCells(GalaxyData data)
    {
    
        var allCells2 = data.AllCells();
#if UNITY_EDITOR
        if (allCells2.Length < 10)
        {
            Debug.LogError("MAP HAVE NO CELLS!!!");
        }

#endif  
        GlobalMapCell startCell = null;
        for (int i = 0; i < data.Size; i++)
        {
            for (int j = 0; j < GalaxyData.VERTICAL_COUNT * data.SizeOfSector - 1; j++)
            {
                var cell = allCells2[i, j];
                if (!(cell is GlobalMapNothing))
                {
                    if (cell is StartGlobalCell)
                    {
                        startCell = cell;
                        //                        haveStartCell = true;
                    }
                    Vector3 v = new Vector3(OffsetCell * i, 0, OffsetCell * j);
                    var cellObj = DataBaseController.GetItem(CellPrefab);
                    cellObj.transform.SetParent(PointsContainer, true);
                    cellObj.transform.position = v;
                    cellObj.Init(cell, OffsetCell);
                    cellObj.Cell.OnDestoyedCell += OnDestoyedCell;
                    _allCells[i, j] = cellObj;
                }
            }
        }

        return startCell;
    }

    private void OnDestoyedCell(GlobalMapCell cell)
    {
        var waysToDestroy = _cellsWaysObjects[cell];
        foreach (var connector in waysToDestroy)
        {
            connector.DestroyWay();
        }
        cell.OnDestoyedCell -= OnDestoyedCell;
    }

    private void RecursuveDrawWays(GalaxyData sector, GlobalMapCell currentCell, HashSet<GlobalMapCell> usedCells)
    {
        if (usedCells.Contains(currentCell))
        {
            return;
        }

        usedCells.Add(currentCell);
        var getAllways = currentCell.GetCurrentPosibleWays();
        foreach (var target in getAllways)
        {
            DrawWays(target, currentCell);
            RecursuveDrawWays(sector, target, usedCells);
        }

//        if (currentCell.ExtraWay != null)
//        {
//            var extra = currentCell.ExtraWay;
//            DrawWays(extra, currentCell);
//            RecursuveDrawWays(sector, extra, usedCells);
//
//        }
    }

    private void DrawWays(GlobalMapCell target, GlobalMapCell currentCell)
    {
        var c1 = _allCells[currentCell.indX, currentCell.indZ];
        var c2 = _allCells[target.indX, target.indZ];
        if (c1 != null && c2 != null)
        {
             var obj = DataBaseController.GetItem(GlobalMapCellWayPrefab);
            obj.gameObject.transform.SetParent(WaysContainer);
            obj.Init(c1.ModifiedPosition, c2.ModifiedPosition);
            AddWayToCell(target,obj);
            AddWayToCell(currentCell, obj);
        }
    }

    private void AddWayToCell(GlobalMapCell target,GlobalMapCellConnector connector)
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
    }

    public void Dispsoe()
    {
        for (int i = 0; i < _data.Size; i++)
        {
            for (int j = 0; j < GalaxyData.VERTICAL_COUNT * _data.SizeOfSector - 1; j++)
            {
                var cell = _allCells[i, j];
                if (cell != null)
                    cell.Cell.OnDestoyedCell -= OnDestoyedCell;
            }
        }
    }

    public void SingleReset(GlobalMapCell currentCell,List<GlobalMapCell> posibleWays)
    {
//        var cells = _data.AllCells();
        for (int i = 0; i < _data.Size; i++)
        {
            for (int j = 0; j < _data.Size; j++)
            {
                var cell = _allCells[i, j];
                if (cell != null)
                {
                    var iamhere = cell.Cell == currentCell;
                    if (iamhere)
                    {
                        myCEll = cell;
                    }
                    cell.SetIAmHere(iamhere);
                }
            }
        }
        MapMoverObject.Init(myCEll);
        int connectd = 0;
        if (myCEll != null)
        {
            foreach (var globalMapCell in posibleWays)
            {
                var canDraw = !(globalMapCell is GlobalMapNothing) && !globalMapCell.IsDestroyed;
                if (canDraw)
                {
                    DrawConnecttion(myCEll, globalMapCell, ref connectd);
                }
            }
        }
    }

    private void DrawConnecttion(GlobalMapCellObject myCEll, GlobalMapCell globalMapCell,ref int connectd)
    {
        for (int i = 0; i < _data.Size; i++)
        {
            for (int j = 0; j < _data.Size; j++)
            {
                var cell = _allCells[i, j];
                if (cell != null)
                {
                    var toConnect = cell.Cell == globalMapCell;
                    if (toConnect)
                    {
                        var c = _connectors[connectd];
                        connectd++;
//                        Debug.Log("Draw connector " + connectd);
                        SetConnectionObject(myCEll, cell, c);
                    }
                }
            }
        }

        for (int i = connectd; i < 8; i++)
        {
            var c = _connectors[i];
            c.gameObject.SetActive(false);
        }
    }

    private void SetConnectionObject(GlobalMapCellObject myCEll, GlobalMapCellObject target,GlobalMapCellConnector connector)
    {
        connector.Init(myCEll.ModifiedPosition, target.ModifiedPosition);
    }

    public void Open()
    {
        IsEnbale = true;

//        CamerasController.Instance.SetCameraTo((min+max)/2f);
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
        if (!IsEnbale)
        {
            return;
        }
        if (left)
        {
            var isOverUI = EventSystem.current.IsPointerOverGameObject();
            if (isOverUI)
            {
                return;
            }

            var ray = GetPointByClick(pos);
            if (ray.HasValue)
            {
                float dist;
                var closeObject = ClosestObject(ray.Value, out dist);
                if (closeObject != null)
                {
                    _mapWindow.ClickCell(closeObject.Cell);
                }
            }
        }
    }

    void LateUpdate()
    {
        if (!IsEnbale || !isInited)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            _mapPressedDownTime = Time.time;
            return;
        }

        if (Input.GetMouseButtonUp(0) && Time.time - _mapPressedDownTime < 0.5f && !EventSystem.current.IsPointerOverGameObject())
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
                {
                    _lastSelectedSector.UnSelect();
                }
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
            if (nearObject != null )
            {
//                Debug.Log("dist " + Mathf.Sqrt(dist) + "  " + nearObject.Cell.InfoOpen);
                if (dist < 13 )
                {
                    SetClosetsObject(nearObject);
                    CallbackNearObjec(nearObject);
                    return;
                }
            }
        }
        SetClosetsObject(null);
        CallbackNearObjec(null);

    }
     
    private void SetClosetsObject(GlobalMapCellObject nearObject)
    {

        if (_lastNearObject != null)
        {
            _lastNearObject.UnSelected();
        }

        _lastNearObject = nearObject;

        if (_lastNearObject!=null)
            _lastNearObject.Selected();
    }
     
    [CanBeNull]
    private GlobalMapCellObject ClosestObject(Vector3 pos,out float dist)
    {
        float vv = Single.MaxValue;
        var xIndex = (int)((pos.x + OffsetCell/2f) / OffsetCell);
        var zIndex = (int)((pos.z + OffsetCell / 2f) / OffsetCell);
        if (xIndex >= 0 && xIndex < _data.Size && zIndex >= 0 && zIndex < _data.Size)
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

    private SectorGlobalMapInfo ClosestSector(Vector3 pos,out float dist)
    {
        var startDist = Single.MaxValue;
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
        {
            return null;
        }
        var ray = CamerasController.Instance.GlobalMapCamera.ScreenPointToRay(pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 9999999, 1))
        {
            return hit.point;
        }
        return null;
    }

    public void SetCameraHome()
    {
        if (myCEll != null)
        {
            CamerasController.Instance.SetCameraTo(myCEll.ModifiedPosition);
        }
    }

    public void SetCameraToCellHome(GlobalMapCell cell)
    {
        GlobalMapCellObject cellToNavigate = GetCellObjectByCell(cell);
        if (cellToNavigate != null)
        {
            CamerasController.Instance.SetCameraTo(cellToNavigate.ModifiedPosition);
        }
    }

     [CanBeNull]
    private GlobalMapCellObject GetCellObjectByCell(GlobalMapCell cell)
    {
//        GlobalMapCellObject cellToNavigate = null;
        for (int i = 0; i < _data.Size; i++)
        {
            for (int j = 0; j < _data.Size; j++)
            {
                var a = _allCells[i, j];
                if (a != null)
                {
                    if (a.Cell == cell)
                    {
                        return a;

                    }
                }
            }
        }
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

    public void MoveToCell(GlobalMapCell target,Action callback)
    {
        Block();
        var targetCell = GetCellObjectByCell(target);
        
        var shallChange2 = WindowManager.Instance.WindowSubCanvas.interactable;
        var shallChange = WindowManager.Instance.WindowMainCanvas.interactable;
        if (shallChange)
            WindowManager.Instance.WindowMainCanvas.interactable = false;
        if (shallChange2)
            WindowManager.Instance.WindowSubCanvas.interactable = false;

        void AfterAction()
        {
            if (shallChange) WindowManager.Instance.WindowMainCanvas.interactable = true;
            if (shallChange2) WindowManager.Instance.WindowSubCanvas.interactable = true;
            UnBlock();
        }

        if (targetCell != null)
        {
            MapMoverObject.MoveTo(targetCell, () =>
            {
                AfterAction();
                callback();
            });

        }
        else
        {
            Debug.LogError("can't find object cell by target cell");
            AfterAction();
        }



    }
}

