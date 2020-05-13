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
    private bool _isSimpleTutor;
    private bool _isAdvTutor;
    public VideoTutorialElement PreBattlTutorial; //Базовый тутор
    public VideoTutorialElement AdvBattlTutorial; //Выставить модуль и орудие.

    public ScoutShipInfoUI ScoutShipInfoPrefab;

    //    public ArmyDataInfoUI ArmyDataInfoUIPrefab;

    public override void Init<T>(T obj)
    {
        base.Init(obj);
        ClearTransform(MyPlayersLayout);
        ClearTransform(EnemyPlayersLayout);
        Tuple<Player, Player> data = obj as Tuple<Player, Player>;
        PreBattlTutorial.Init();
        AdvBattlTutorial.Init();
        if (data != null)
        {
            _greenPlayer = data.val1;
            _redPlayer = data.val2;
            _isSimpleTutor = _redPlayer is PlayerAITutor;
            _isAdvTutor = _redPlayer is PlayerAITutorWearModuls;
            if (_isAdvTutor)
            {
                AdvBattlTutorial.Open();
            }
            else if (_isSimpleTutor)
            {
                if (_greenPlayer.Army.Count > 1)
                {
                    PreBattlTutorial.Open();
                }
            }

            if (data.val1 != null)
            {
                _greeArmyUi = DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.PlayerArmyUIPrefab);
                _greeArmyUi.Init(data.val1,MyPlayersLayout,true,new ConnectInventory(_greenPlayer.Inventory));
                PlayersInventory.Init(data.val1.Inventory,null, true);
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
        if (_isSimpleTutor)
        {
            bool isCHeck = CheckGreenWeapons();
            if (!isCHeck)
            {
                PreBattlTutorial.Open();
                return;
            }
        }
        else if (_isAdvTutor)
        {

            bool isCHeck = CheckAdvWeapons();
            if (!isCHeck)
            {
                AdvBattlTutorial.Open();
                return;
            }
        }

        if (_greenPlayer != null && _redPlayer != null)
        {
            MainController.Instance.LaunchBattle(_greenPlayer, _redPlayer);
        }
        else
        {
            Debug.LogError("can't launch battle. No players");
        }
    }

    private bool CheckGreenWeapons()
    {
        bool isMainShipHaveWeapons = false;
        bool isBattleShipHaveWeapons = false;

        if (_greenPlayer.MainShip != null)
        {
            var listSpells = (_greenPlayer.MainShip.Ship.SpellsModuls.Where(x => x != null)).ToList();
            isMainShipHaveWeapons = (listSpells.Count > 0);
        }

        var battleShips = _greenPlayer.Army.Army.Where(x => x.Ship.ShipType != ShipType.Base).ToList();
        if (battleShips.Count == 0)
        {
            isBattleShipHaveWeapons = true;
        }
        else
        {
            var ship = battleShips.First();
            var lisWeapons = ship.Ship.WeaponsModuls.GetNonNullActiveSlots();
            isBattleShipHaveWeapons = lisWeapons.Count > 0;
        }

        return isBattleShipHaveWeapons && isMainShipHaveWeapons;

    }   
    private bool CheckAdvWeapons()
    {
        bool isBattleShipHaveWeapons = false;
        bool isBattleShipModulesWeapons = false;


        var battleShips = _greenPlayer.Army.Army.Where(x => x.Ship.ShipType != ShipType.Base).ToList();
        if (battleShips.Count == 0)
        {
            return true;
        }

        var ship = battleShips.First();
        var lisWeapons = ship.Ship.WeaponsModuls.GetNonNullActiveSlots();
        var lisModuls = ship.Ship.Moduls.GetNonNullActiveSlots();
        isBattleShipHaveWeapons = lisWeapons.Count > 0;
        isBattleShipModulesWeapons = lisModuls.Count > 0;

        return isBattleShipHaveWeapons && isBattleShipModulesWeapons;

    }
}

