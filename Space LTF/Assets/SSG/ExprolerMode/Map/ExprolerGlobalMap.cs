using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExprolerGlobalMap : MonoBehaviour
{
      public List<ExprolerGlobalMapCell> AllCells = new List<ExprolerGlobalMapCell>();
      public PlayerSafe _playerToPlay;
//      public ShipConfig Config = ShipConfig.droid;
//      public int Power = 10;
      public ExprolerNewBattleInfo NewBattleInfo;

      public void Init(PlayerSafe player)
      {
        NewBattleInfo.gameObject.SetActive(false);
        _playerToPlay = player;
        var safe = MainController.Instance.SafeContainers;
          SyncWithData(safe);
      }

      public void SyncWithData(PlayerSlotsContainer slots)
      {
          foreach (var cell in AllCells)
          {
              cell.Init();
              cell.SetClickCallback(OnClickCell);
              if (slots.ContainsUnblockId(cell.Id))
              {
                cell.Unblock();
//                UnblockAndAllNear(cell);
              }

              if (slots.ContainsCompleteId(cell.Id))
              {
                  cell.Complete();
              }
          }
      }

//      private void UnblockAndAllNear(ExprolerGlobalMapCell cell)
//      {
//          foreach (var exprolerGlobalMapCell in cell.Neighhoods)
//          {
//              exprolerGlobalMapCell.Unblock();
//          }
//      }

      private void OnClickCell(ExprolerGlobalMapCell cell)
      {
          if (cell.IsOpen)
          {
              NewBattleInfo.Init(cell);
//              WindowManager.Instance.
          }
      }


}
