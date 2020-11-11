using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class PlayerArmyUI : MonoBehaviour
{
    public Transform ShipsLayout;
    private RectTransform _shipsLayoutRectTransform;
    public BaseShipInventoryUI ShipBasePrefabUI;
    private BaseShipInventoryUI mainShipInfo;
    public ShipPilotInventoryUI MyPlayerPrefab;
    private List<ShipPilotInventoryUI> playerInfoList;
    private PlayerSafe _player;
    private bool _usable;
    private ConnectInventory _connectedInventory;
    private RectTransform myRectTransform;
    private RectTransform parentRectTransform;
    private IInventory _tradeInventory = null;

    void Awake()
    {
        _shipsLayoutRectTransform = ShipsLayout.GetComponent<RectTransform>();
    }

    public void Init(PlayerSafe player, Transform parent, bool usable, ConnectInventory connectedInventory,IInventory tradeInventory = null)
    {
        _tradeInventory = tradeInventory;
        _usable = usable;
        _player = player;
        _connectedInventory = connectedInventory;
        //        var parentRect = parent.GetComponent<RectTransform>();
        playerInfoList = new List<ShipPilotInventoryUI>();
        myRectTransform = transform.GetComponent<RectTransform>();
        parentRectTransform = parent.GetComponent<RectTransform>();
        var myrect = GetComponent<RectTransform>();
        myrect.sizeDelta = new Vector2(myrect.sizeDelta.x, parentRectTransform.sizeDelta.y);
        myRectTransform.pivot = Vector2.zero;
        transform.SetParent(parent, false);
        myRectTransform.sizeDelta = new Vector2(parentRectTransform.rect.width, parentRectTransform.rect.height);
        //        myRectTransform.transform.
        player.OnAddShip += OnAddShip;
        foreach (var shipPilotData in player.Ships)
        {
            AddShip(shipPilotData, false, tradeInventory);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(_shipsLayoutRectTransform);
        WaitLoadStart();
    }

//    public void TryEnableMainShipInv()
//    {
//        if (mainShipInfo != null)
//        {
//            mainShipInfo.EnableDragZone();
//        }
//    }       
//    public void DisableMainShipInv()
//    {
//        if (mainShipInfo != null)
//        {
//            mainShipInfo.DisableDragZone();
//        }
//    }

    private async void WaitLoadStart()
    {
        await WaitLoadTask();
    }

    private async Task WaitLoadTask()
    {
        await Task.Yield();
        await Task.Yield();
        LayoutRebuilder.ForceRebuildLayoutImmediate(_shipsLayoutRectTransform);

    }

    //    void OnEnable()
    //    {
    //        LayoutRebuilder.ForceRebuildLayoutImmediate(_shipsLayoutRectTransform);
    //    }

    private void AddShip(StartShipPilotData shipPilotData, bool withRebuild,IInventory tradeInventory = null)
    {

        if (shipPilotData.Ship.ShipType == ShipType.Base)
        {
            mainShipInfo = DataBaseController.GetItem(ShipBasePrefabUI);
            mainShipInfo.transform.SetParent(ShipsLayout);
            mainShipInfo.Init(_player,shipPilotData.Ship, true, _connectedInventory, tradeInventory);
        }
        else
        {
            var playerInfo = DataBaseController.GetItem(MyPlayerPrefab);
            playerInfo.transform.SetParent(ShipsLayout);
            playerInfo.Init(shipPilotData, _usable, _connectedInventory, OnToggleSwitched, tradeInventory);
            playerInfoList.Add(playerInfo);
            if (withRebuild)
                LayoutRebuilder.ForceRebuildLayoutImmediate(_shipsLayoutRectTransform);
        }
    }

    private void OnToggleSwitched()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(_shipsLayoutRectTransform);
    }

    private void OnAddShip(StartShipPilotData arg1, bool arg2)
    {
        if (arg2)
        {
            AddShip(arg1, true, _tradeInventory);
        }
        else
        {
            var objToDel = playerInfoList.FirstOrDefault(x => x.ShipData == arg1);
            if (objToDel != null)
            {
                playerInfoList.Remove(objToDel);
                GameObject.Destroy(objToDel.gameObject);
            }
        }
    }

    void Update()
    {

        myRectTransform.sizeDelta = new Vector2(parentRectTransform.rect.width, parentRectTransform.rect.height);
    }

    public void Dispose()
    {
        _player.OnAddShip -= OnAddShip;
        mainShipInfo.Dispose();
        foreach (var inventoryUi in playerInfoList)
        {
            inventoryUi.Dispose();
        }
        playerInfoList.Clear();
    }
    public void SoftRefresh()
    {
        foreach (var inventoryUi in playerInfoList)
        {
            inventoryUi.SoftRefresh();
        }
    }

    public void Disable()
    {
        foreach (var inventoryUi in playerInfoList)
        {
            inventoryUi.Disable();
        }

        if (mainShipInfo != null)
        {
            mainShipInfo.Disable();
        }
    }
//    public void Enable()
//    {
//        foreach (var inventoryUi in playerInfoList)
//        {
//            inventoryUi.Enable();
//        }
//        if (mainShipInfo != null)
//        {
//            mainShipInfo.Enable();
//        }
//    }

}

