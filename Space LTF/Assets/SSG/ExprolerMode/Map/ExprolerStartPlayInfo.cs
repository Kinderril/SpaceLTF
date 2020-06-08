using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExprolerStartPlayInfo 
{
    private PlayerSafe _playerToPlay;
    public ExprolerGlobalMapCell Cell;
    public bool Win;
    public bool Showed = true;
    public HashSet<int> Unblocks = new HashSet<int>();
    public int CompleteId = -1;

    public void Init(ExprolerGlobalMapCell cell, PlayerSafe playerToPlay, int size)
    {
        _playerToPlay = playerToPlay;
        Cell = cell;
        var startParameters = _playerToPlay.StartParametersLevels;
        var startGame = new StartNewGameWithSafePlayer(startParameters, Cell.Config, size, 1, null, _playerToPlay, Cell.Power);
        MainController.Instance.CreateNewPlayerAndStartGame(startGame);
    }


    public void RepairAllShips()
    {
        foreach (var startShipPilotData in _playerToPlay.Ships)
        {
            startShipPilotData.Ship.SetRepairPercent(1f);
            startShipPilotData.Ship.RestoreAllCriticalDamages();
        }
    }

    

    public void SetSectorResult(bool win,int size)
    {
        RepairAllShips();

        var slots = MainController.Instance.SafeContainers;
        if (win)
        {
            var nieght = Cell.Neighhoods;
            foreach (var cell in nieght)
            {
                if (!slots.ContainsUnblockId(cell.Id))
                {
                    Unblocks.Add(cell.Id);
                    slots.UnblockId(cell.Id);
                }
            }

            if (!slots.ContainsCompleteId(Cell.Id))
            {
                CompleteId = Cell.Id;
                slots.CompleteId(Cell.Id);
            }
        }
        slots.SaveProfiles();
        Showed = false;
        Win = win;

    }
}
