using UnityEngine;
using System.Collections;

public class ExprolerController
{
    public PlayerSafe CurrentPlayer;
    public ExprolerStartPlayInfo PlayInfo;
    private int _size;


    public void StartGame(PlayerSafe playerToPlay)
    {
        PlayInfo = new ExprolerStartPlayInfo();
        CurrentPlayer = playerToPlay;
        WindowManager.Instance.OpenWindow(MainState.exprolerModeGlobalMap);
    }

    public void PlaySector(ExprolerGlobalMapCell cell, int size)
    {
        _size = size;
        PlayInfo.Init(cell,CurrentPlayer, _size);
    }


    public void SetLastBalleData(bool win)
    {
        PlayInfo.SetSectorResult(win,_size);
//        WindowManager.Instance.OpenWindow(MainState.exprolerModeGlobalMap);
    }
}
