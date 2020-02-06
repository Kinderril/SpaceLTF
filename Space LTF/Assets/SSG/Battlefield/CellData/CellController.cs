using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour
{
    [HideInInspector]
    public AICellDataRaound Data;
    public float CellDrawLevel = 10f;
    public float cellSize = 11f;
    public Transform CellsContainer;
    public List<Asteroid> AsteroidsPrefabs;
    // public GameObject PrefabBorder;
    public BattleBorderWall BattleBorderWall;

    public Vector3 Min
    {
        get { return Data.Min; }
    }
    public Vector3 Max
    {
        get { return Data.Max; }
    }

    public void Init(int coef = 0)
    {
        gameObject.SetActive(true);
        var size = 2 + MyExtensions.Random(5 + coef / 2, 7 + coef);
        //        var sizeX = MyExtensions.Random(7 + coef/2, 8 + coef);
        //        var sizeZ = MyExtensions.Random(7 + coef/2, 8 + coef);
        Data.Init(transform.position, size, cellSize);
        InstantiatePrefabs();
        InstantiateBorders(Data.CenterZone);
    }

    private void InstantiateBorders(Vector3 center)
    {
        Vector3 dir = new Vector3(0, 0, 1);
        List<Vector3> poitns = new List<Vector3>();
        for (int i = 0; i < 8; i++)
        {
            dir = Utils.Rotate45(dir, SideTurn.left);
            var pos = center + dir * Data.Radius;
            poitns.Add(pos);
        }

        var p01 = poitns[7];
        var p02 = poitns[0];
        CreateBordeElement(p01, p02, center);
        for (int i = 0; i < 8 - 1; i++)
        {
            var p1 = poitns[i];
            var p2 = poitns[i + 1];
            CreateBordeElement(p1, p2, center);
        }

    }

    private void CreateBordeElement(Vector3 p1, Vector3 p2, Vector3 worldCenter)
    {
        var go = DataBaseController.GetItem(BattleBorderWall);
        var camera = CamerasController.Instance.GameCamera;
        go.Init(worldCenter,p1, p2, CellsContainer, camera);
    }

    private void InstantiatePrefabs()
    {
        Utils.ClearTransform(CellsContainer);
        int index = 0;
        Debug.Log($"Asteroids count: {Data.Asteroids.Count}.  FieldRadius:{Data.Radius}");
        foreach (var aiAsteroidPredata in Data.Asteroids)
        {
            index++;
            var pref = AsteroidsPrefabs.RandomElement();
            var astreroid = DataBaseController.GetItem(pref, aiAsteroidPredata.Position);
            astreroid.Init(aiAsteroidPredata);
            astreroid.name = $"Asteroids I:{index}  ";
            astreroid.transform.SetParent(CellsContainer);
        }
    }

    public AICell GetCell(Vector3 pos)
    {
        var xIndex = IndexByFloat(pos.x, Data.StartX);
        var zIndex = IndexByFloat(pos.z, Data.StartZ);
        var cell = Data.GetCell(xIndex, zIndex);
        //        Debug.Log("cell count:" + cell.Links.Length);
        return cell;
    }

    private int IndexByFloat(float f, float startCoef)
    {
        var delta = f - startCoef;
        int indexPre = (int)(delta / Data.CellSize);
        return indexPre;
    }
    public void AddShip(ShipBase shipBase)
    {
        Data.AddShip(shipBase);
    }
    public void ClearPosition(Vector3 vector3)
    {
        Data.ClearPosition(vector3);
    }

    public AICell GetCellByDir(AICell curCell, Vector3 dir)
    {
        int xx, zz;
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
        {
            zz = curCell.Zindex;
            if (dir.x > 0)
            {
                xx = curCell.Xindex + 1;
            }
            else
            {
                xx = curCell.Xindex - 1;
            }
        }
        else
        {
            xx = curCell.Xindex;
            if (dir.x > 0)
            {
                zz = curCell.Zindex + 1;
            }
            else
            {
                zz = curCell.Zindex - 1;
            }
        }
        return Data.GetCell(xx, zz);
    }

    void OnDrawGizmosSelected()
    {
        if (Data == null)
        {
            return;
        }

        var RadiusToBeSafe = Data.Radius - AICellDataRaound.SafeRadius;
        DrawUtils.DrawCircle(Data.CenterZone, Vector3.up, Color.green, RadiusToBeSafe);
        DrawUtils.DrawCircle(Data.CenterZone, Vector3.up, Color.red, Data.Radius);
        //        Gizmos.DrawWireCube(new Vector3(), );

        for (int i = 0; i < Data.MaxIx; i++)
        {
            for (int j = 0; j < Data.MaxIz; j++)
            {
                var cell = Data.List[i, j];
                cell.DrawGizmosSelected();
            }
        }
    }

    public void Dispose()
    {
        Utils.ClearTransform(CellsContainer);
    }

}

