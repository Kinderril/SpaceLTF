using System;
using UnityEngine;


[System.Serializable]
public class LastScoutsData
{
    public int intX { get; private set; }
    public int intZ { get; private set; }
    public GlobalMapCell Cell { get; set; }

    public bool HaveScouted;

    [field: NonSerialized]
    public event Action OnLastScouts;

    public void SetLastScout(GlobalMapCell cell)
    {
        Cell = cell;
        HaveScouted = true;
        intX = cell.indX;
        intZ = cell.indZ;
        MainController.Instance.MainPlayer.MessagesToConsole.AddMsg(Namings.TryFormat(Namings.Tag("CellScouted"), intX, intZ));
        if (OnLastScouts != null)
        {
            OnLastScouts();
        }
#if UNITY_EDITOR
        if (cell == MainController.Instance.MainPlayer.MapData.CurrentCell)
        {
            Debug.LogError("Wrong scouted cell");
        }
#endif
    }
}

