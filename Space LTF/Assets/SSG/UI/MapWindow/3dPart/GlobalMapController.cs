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
    public float Offset;
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
    public GlobalMapCellConnector GlobalMapCellConnectorPrefab;
    public GlobalMapCellConnector GlobalMapCellWayPrefab;
    public GlobalMapCellConnector GlobalMapCellBorderPrefab;
    private List<GlobalMapCellConnector> _connectors = new List<GlobalMapCellConnector>();
    private Dictionary<GlobalMapCell,List<GlobalMapCellConnector>> _cellsWaysObjects 
        = new Dictionary<GlobalMapCell, List<GlobalMapCellConnector>>();
    GlobalMapCellObject myCEll = null;
    private float _mapPressedDownTime;


    public void SingleInit(GalaxyData data,MapWindow mapWindow, Action<GlobalMapCellObject> CallbackNearObjec)
    {
        if (isInited)
        {
            return;
        }

        this.CallbackNearObjec = CallbackNearObjec;
        _mapWindow = mapWindow;
        isInited = true;
        _data = data;
        _allCells = new GlobalMapCellObject[data.Size, data.Size];
        min = Vector3.zero;
        max = new Vector3(Offset * data.Size, 0, Offset * data.Size);

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
//        Draw
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
                    Vector3 v = new Vector3(Offset * i, 0, Offset * j);
                    var cellObj = DataBaseController.GetItem(CellPrefab);
                    cellObj.transform.SetParent(PointsContainer, true);
                    cellObj.transform.position = v;
                    cellObj.Init(cell, Offset);
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
            for (int j = 0; j < _data.Size; j++)
            {
                var cell = _allCells[i, j];
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
        UpdateClosest();
    }


    private void UpdateClosest()
    {

        var ray = GetPointByClick(Input.mousePosition);
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
        var xIndex = (int)((pos.x + Offset/2f) / Offset);
        var zIndex = (int)((pos.z + Offset / 2f) / Offset);
//        Debug.Log($"INDEX: {xIndex} {zIndex}");
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


//        GlobalMapCellObject obj = null;
//        for (int i = 0; i < _data.Size; i++)
//        {
//            for (int j = 0; j < _data.Size; j++)
//            {
//                var a = _allCells[i, j];
//                if (a != null)
//                {
//                    var sDist = (a.transform.position - pos).sqrMagnitude;
//                    if (sDist < vv)
//                    {
//                        vv = sDist;
//                        obj = a;
//                    }
//                }
//            }
//        }
//        dist = vv;
//        return obj;
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

}

