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
          Dictionary<ShipConfig,ExprolerGlobalMapCell> _maxCElls = new Dictionary<ShipConfig, ExprolerGlobalMapCell>();
          foreach (var cell in AllCells)
          {
              cell.Init();
              cell.SetClickCallback(OnClickCell);
              if (slots.ContainsUnblockId(cell.Id))
              {
                cell.Unblock();
                cell.StopAnim();
                if (_maxCElls.TryGetValue(cell.Config, out var maxCell))
                {
                    if (cell.Power > maxCell.Power)
                    {
                        _maxCElls[cell.Config] = cell;
                    }
                }
                else
                {
                    _maxCElls.Add(cell.Config,cell);
                }
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

          foreach (var exprolerGlobalMapCell in _maxCElls)
          {
              exprolerGlobalMapCell.Value.StartAnim();
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
