using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class PreBattleWindow : BaseWindow
{
    public Transform MyPlayersLayout;
    public Transform EnemyPlayersLayout;
    public InventoryUI PlayersInventory;
    private Player _greenPlayer;
    private Player _redPlayer;
    private PlayerArmyUI _greeArmyUi;
    private PlayerArmyUI _redArmyUi;

    public ScoutShipInfoUI ScoutShipInfoPrefab;

    //    public ArmyDataInfoUI ArmyDataInfoUIPrefab;

    public override void Init<T>(T obj)
    {
        base.Init(obj);
        ClearTransform(MyPlayersLayout);
        ClearTransform(EnemyPlayersLayout);
        Tuple<Player, Player> data = obj as Tuple<Player, Player>;
        if (data != null)
        {
            _greenPlayer = data.val1;
            _redPlayer = data.val2;
            if (data.val1 != null)
            {
                _greeArmyUi = DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.PlayerArmyUIPrefab);
                _greeArmyUi.Init(data.val1,MyPlayersLayout,true,new ConnectInventory(_greenPlayer.Inventory));
                PlayersInventory.Init(data.val1.Inventory,null);
            }
            if (data.val2 != null)
            {
                if (_greenPlayer.Parameters.ScoutsIsMax())
                {
                    _redArmyUi = DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.PlayerArmyUIPrefab);
                    _redArmyUi.Init(data.val2, EnemyPlayersLayout, false, null);
                }
                else
                {
                    var scouts = _redPlayer.ScoutData.GetShipScouts(_greenPlayer.Parameters.Scouts.Level);
                    foreach (var shipScoutData in scouts)
                    {
                        var scoutInfo = DataBaseController.GetItem(ScoutShipInfoPrefab);
                        scoutInfo.transform.SetParent(EnemyPlayersLayout,false);
                        scoutInfo.Init(shipScoutData);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("wrong init data PreBattleWindow");
        }
    }

    public override void Dispose()
    {
        _greeArmyUi.Dispose();
        if (_redArmyUi != null)
        {
            _redArmyUi.Dispose();
        }
        else
        {
            
        }
        PlayersInventory.Dispose();
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnClickStart();
        }
    }

    public void OnClickStart()
    {
        if (_greenPlayer != null && _redPlayer != null)
        {
            MainController.Instance.LaunchBattle(_greenPlayer, _redPlayer);
        }
        else
        {
            Debug.LogError("can't launch battle. No players");
        }
    }
}

