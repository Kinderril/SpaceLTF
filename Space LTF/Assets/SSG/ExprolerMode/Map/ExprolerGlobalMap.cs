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
          CellsCheck();
            NewBattleInfo.gameObject.SetActive(false);
            _playerToPlay = player;
            var safe = MainController.Instance.SafeContainers;
          SyncWithData(safe);
      }

      private void CellsCheck()
      {
#if UNITY_EDITOR
          foreach (var exprolerGlobalMapCell in AllCells)
          {
              foreach (var neighhood in exprolerGlobalMapCell.Neighhoods)
              {
                  if (neighhood == -1)
                  {
                      Debug.LogError($"Neight is null at {exprolerGlobalMapCell.name}");
                  }
              }
          }
#endif
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
              else
              {
                  cell.Block();
              }

              if (slots.ContainsCompleteId(cell.Id,out var subSet))
              {
                  cell.Complete(subSet);
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
