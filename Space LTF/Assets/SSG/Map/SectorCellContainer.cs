using UnityEngine;
using System.Collections;

[System.Serializable]
public class SectorCellContainer
{
    public int indX;
    public int indZ;
    public GlobalMapCell Data { get; private set; }

    public SectorCellContainer(int x,int z)
    {
        Data = null;
        indX = x;
        indZ = z;
    }

    public void SetData(GlobalMapCell data)
    {
        Data = data;
    }


    public void SetConnectedCell(int coreId)
    {
        if (Data != null)
            Data.SetConnectedCell(coreId);
    }

    public bool IsFreeToPopulate()
    {
        return Data == null;
    }
}
