using UnityEngine;
using System.Collections;

public class WindowExprolerGlobalMap : BaseWindow
{
    public ExprolerGlobalMap GlobalMap;
    public ExprolerLastBattleInfo LastBattleInfo;
    public MoneySlotUI CreditsField;
    public MoneySlotUI MicrochipsField;
    private PlayerSafe _player;
    public InventoryUI InventoryUI;
    private Vector3 _stablePos;
    private bool _stablePosCached = false;
    private bool isArmyActive = false;
    public GameObject ArmyInfoContainer;
    private PlayerArmyUI playerArmyUI;
    public Transform PlayerContainer;


    public override void Init()
    {
        
        var exproler = MainController.Instance.Exproler;
        _player = exproler.CurrentPlayer;

        if (!exproler.PlayInfo.Showed)
        {
            exproler.PlayInfo.Showed = true;
            LastBattleInfo.Init(exproler.PlayInfo);
        }
        else
        {
            LastBattleInfo.OnClose();
        }
        InitMyArmy();
        InventoryUI.Init(exproler.CurrentPlayer.Inventory, null, true);
        GlobalMap.Init(_player);
        _player.OnCreditsChange += OnCreditsChange;
        _player.OnMicroChipsChange+= OnMicroChipsChange;
        UpdateFields();
        EnableArmy(false);
        base.Init();
    }

    private void OnMicroChipsChange(int obj)
    {
        UpdateFields();
    }

    private void OnCreditsChange(int obj)
    {
        UpdateFields();
    }
    private void EnableArmy(bool val)
    {
        if (val)
        {
            playerArmyUI.SoftRefresh();
            WindowManager.Instance.UiAudioSource.PlayOneShot(DataBaseController.Instance.AudioDataBase.WindowOpen);
            ArmyInfoContainer.transform.position = _stablePos;
            InventoryUI.RefreshPosition();
        }
        else
        {
            if (!_stablePosCached)
            {
                _stablePosCached = true;
                _stablePos = ArmyInfoContainer.transform.position;
            }
            var v = new Vector3(5000, 0, 0);
            ArmyInfoContainer.transform.position = v;
        }
//        if (OnOpenInventory != null)
//        {
//            OnOpenInventory(val);
//        }
        isArmyActive = val;
    }

    private void UpdateFields()
    {
        CreditsField.Field.Init(_player.Credits);
        MicrochipsField.Field.Init(_player.Microchips);
    }

    public override void Close()
    {
        UnSubscribe();
        MainController.Instance.SafeContainers.SaveProfiles();
        base.Close();
    }
    private void UnSubscribe()
    {

        _player.OnCreditsChange -= OnCreditsChange;
        _player.OnMicroChipsChange -= OnMicroChipsChange;
    }

    public void OnInventory()
    {
        OnArmyShowClick();
    }   
    public void OnSettings()
    {
          WindowManager.Instance.OpenSettingsSettings(EWindowSettingsLauch.exprolerGlobalMap);
    }
    private void InitMyArmy()
    {
        playerArmyUI = DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.PlayerArmyUIPrefab);
        playerArmyUI.Init(_player, PlayerContainer, true, new ConnectInventory(_player.Inventory));
    }
    public void OnArmyShowClick()
    {
        if (isArmyActive)
        {
            EnableArmy(false);
        }
        else
        {
            EnableArmy(true);
        }
    }

    public override void Dispose()
    {
        if (playerArmyUI != null)
        {
            playerArmyUI.Dispose();
            Destroy(playerArmyUI.gameObject);
        }
        UnSubscribe();
        MainController.Instance.SafeContainers.SaveProfiles();
        InventoryUI.Dispose();
        base.Dispose();
    }
}
