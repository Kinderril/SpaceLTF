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
        var startParameters = _playerToPlay.Parameters.GetAsDictionary();
        var startGame = new StartNewGameWithSafePlayer(startParameters, Cell.Config, size, 1, null, _playerToPlay, Cell.Power,Cell.MapType);
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
            foreach (var id in nieght)
            {

                if (!slots.ContainsUnblockId(id))
                {
                    Unblocks.Add(id);
                    slots.UnblockId(id);
                }
            }

            CompleteId = Cell.Id;
            slots.CompleteId(Cell.Id, size);
        }
        slots.SaveProfiles();
        Showed = false;
        Win = win;

    }
}
