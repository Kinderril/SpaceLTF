using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CellController : MonoBehaviour
{
    [HideInInspector]
    public AICellData Data;
    public float CellDrawLevel = 10f;
    public float cellSize = 11f;

//    public Transform Start;
//    public Transform End;

    public GameObject CloudsPrefab;
    public AsteroidField AsteroidFieldPrefab;
    public DeepSpaceField DeepSpace;
    public Transform CellsContainer;
    

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
        var sizeX = MyExtensions.Random(7 + coef/2, 8 + coef);
        var sizeZ = MyExtensions.Random(7 + coef/2, 8 + coef);
        Data.Init(transform.position,sizeX,sizeZ, cellSize);
        InstantiatePrefabs();
    }

    private void InstantiatePrefabs()
    {
        Utils.ClearTransform(CellsContainer);
        for (int i = 0; i < Data.MaxIx; i++)
        {
            for (int j = 0; j < Data.MaxIz; j++)
            {
                var cell1 = Data.GetCell(i, j);
//                var xx = Data.StartX + (i + 0.5f)*Data.CellSize;
//                var zz = Data.StartX + (j + 0.5f)*Data.CellSize;
                var p = cell1.Center;
                switch (cell1.CellType)
                {
                    case CellType.Asteroids:
                        var ast = DataBaseController.GetItem(AsteroidFieldPrefab);
                        ast.Init(cellSize);
                        ast.transform.SetParent(CellsContainer);
                        ast.transform.position = p;
                        ast.name = String.Format("Asteroids I:{0}  J:{1}",i,j);
                        break;
                    case CellType.DeepSpace:
                        var ds = DataBaseController.GetItem(DeepSpace);
                        ds.transform.SetParent(CellsContainer);
                        ds.transform.position = p;
                        ds.name = String.Format("DeepSpace I:{0}  J:{1}", i, j);
                        break;
                    case CellType.Clouds:
                        var cs = DataBaseController.GetItem(CloudsPrefab);
                        cs.transform.SetParent(CellsContainer);
                        cs.transform.position = p;
                        cs.name = String.Format("Clouds I:{0}  J:{1}", i, j);
                        break;
                }
            }
        }
    }

    public AICell FindCell(Vector3 pos)
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
        int indexPre = (int) (delta/Data.CellSize);
        return indexPre;
    }

    public AICell GetCellByDir(AICell curCell, Vector3 dir)
    {
        int xx,zz;
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
        for (int i = 0; i < Data.MaxIx; i++)
        {
            for (int j = 0; j < Data.MaxIz; j++)
            {
//                var p = new Vector3(Data.StartX + Data.CellSize * i, CellDrawLevel, Data.StartZ + Data.CellSize * j);

                var cell = Data.List[i, j];
                cell.DrawGizmosSelected();
//                var p1 = p + new Vector3(Data.CellSize, 0, 0);
//                var p2 = p + new Vector3(0, 0, Data.CellSize);
//                var c = p + new Vector3(Data.CellSize / 2f, 0, Data.CellSize / 2f);
//                Gizmos.color = Color.green;
//                if (j != Data.MaxIz)
//                {
//                    Gizmos.DrawLine(p, p2);
//                }
//                if (i != Data.MaxIx)
//                {
//                    Gizmos.DrawLine(p, p1);
//                }
            }
        }
    }

    public void Dispose()
    {
        Utils.ClearTransform(CellsContainer);
    }
}

