using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum DragItemType
{
    free,
    weapon,
    modul,
    spell,
    cocpit,
    engine,
    wings,
}

[RequireComponent(typeof(GraphicRaycaster))]
public abstract class DragableItem : UIElementWithTooltip, IDropHandler,
    IEndDragHandler, IDragHandler, IBeginDragHandler,
    IPointerDownHandler, IPointerUpHandler
{
    public DragItemType DragItemType;
    public DragableItemSlot Slot;
    private float _startDrag = 0f;
    private GraphicRaycaster m_Raycaster;
    public IItemInv ContainerItem;
    public TextMeshProUGUI Lable;
    public Image Icon;
    public bool Usable { get; set; }
    private Transform _prevTransform = null;
    public int id;
    private float _clickTime;

    public static DragableItem Create(IItemInv item, bool usable)
    {
        DragableItem itemPrefab;
        switch (item.ItemType)
        {
            case ItemType.weapon:
                itemPrefab = DataBaseController.Instance.DataStructPrefabs.DragableItemWeaponPrefab;
                break;
            case ItemType.modul:
                itemPrefab = DataBaseController.Instance.DataStructPrefabs.DragableItemModulPrefab;
                break;
            case ItemType.spell:
                itemPrefab = DataBaseController.Instance.DataStructPrefabs.DragableItemSpellPrefab;
                break;   
            case ItemType.engine:
            case ItemType.wings:
            case ItemType.cocpit:
                itemPrefab = DataBaseController.Instance.DataStructPrefabs.DragableItemParameterPrefab;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        var daragbleElement = DataBaseController.GetItem(itemPrefab);
        switch (item.ItemType)
        {
            case ItemType.weapon:
                daragbleElement.DragItemType = DragItemType.weapon;
                break;
            case ItemType.modul:
                daragbleElement.DragItemType = DragItemType.modul;
                break;
            case ItemType.spell:
                daragbleElement.DragItemType = DragItemType.spell;
                break; 
            case ItemType.cocpit:
                daragbleElement.DragItemType = DragItemType.cocpit;
                break;
            case ItemType.engine:
                daragbleElement.DragItemType = DragItemType.engine;
                break;
            case ItemType.wings:
                daragbleElement.DragItemType = DragItemType.wings;
                break;
        }

        daragbleElement.ContainerItem = item;
        daragbleElement.Usable = usable;
        daragbleElement.id = Utils.GetId();
        daragbleElement.Icon.sprite = daragbleElement.GetIcon();
        //        Debug.Log("Dragabale element created:" + daragbleElement.id);
        daragbleElement.Lable.text = item.GetInfo();
        daragbleElement.Init();
        return daragbleElement;
    }

    protected virtual void Init()
    {

    }

    protected override string TextToTooltip()
    {
        return ContainerItem.GetInfo();
    }

    void Awake()
    {
        m_Raycaster = this.GetComponent<GraphicRaycaster>();
    }

    public abstract Sprite GetIcon();

    public virtual string GetInfo()
    {
        return ContainerItem == null ? "" : ContainerItem.GetInfo();
    }

    protected void TestSlot(DragableItemSlot findedSlot, Action<bool> callback)
    {
        if (findedSlot != null)
        {
            if (findedSlot != Slot)
            {
                //                var slotInventory = findedSlot._inventory;

                if (findedSlot.CanPutHere(this))
                {

                    WindowManager.Instance.UiAudioSource.PlayOneShot(DataBaseController.Instance.AudioDataBase.InventoryMove, 1f);
                    findedSlot.ImplimentItem(this, callback);
                    return;
                }
            }
        }
        callback(false);

    }

    public void OnDrop(PointerEventData eventData)
    {
        //        Debug.Log("ON DROP DRAG");
        //        Release();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //                Debug.Log("END DRAG");
        Release();
        //        Release();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Usable)
        {
            //            Debug.Log(" OnDrag Input");
            transform.position = Input.mousePosition;
        }
    }

    private void Release()
    {
        if (Usable)
        {
            WindowManager.Instance.UiAudioSource.PlayOneShot(DataBaseController.Instance.AudioDataBase.InventoryMove);
            var p = transform.position;
            DragableItemSlot findedSlot = TryFindSlot(p);
//            Debug.Log($"Release findedSlot:{findedSlot != null}");
//if (findedSlot.CanPutHere())

            TestSlot(findedSlot, b =>
            {
                if (!b)
                {
#if UNITY_EDITOR
                    if (_prevTransform == null)
                    {
                        Debug.LogError("prev transform is null on end drag");
                    }
#endif
                    ReturnToLastParent();
                }
            });
            transform.localPosition = Vector3.zero;
            return;
            //            transform.SetParent(topPanel, false);
        }
        //        Debug.Log(" Release SET ZEREO");
        transform.localPosition = Vector3.zero;
    }

    public void ReturnToLastParent()
    {
        if (_prevTransform != null)
        {
            transform.SetParent(_prevTransform);
            transform.localPosition = Vector3.zero;
            _prevTransform = null;
        }
    }

    private DragableItemSlot TryFindSlot(Vector3 vector3)
    {

        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = vector3;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        foreach (RaycastResult result in raycastResults)
        {
            var tmpSlot = result.gameObject.GetComponent<DragableItemSlot>();
            if (tmpSlot != null)
            {
                return tmpSlot;
            }
        }
        return null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Usable)
        {
            var topPanel = WindowManager.Instance.TopPanel;
            _prevTransform = transform.parent;
#if UNITY_EDITOR
            if (_prevTransform == null)
            {
                Debug.LogError("prev transform is null on begin drag");
            }
#endif
            transform.SetParent(topPanel, false);
            transform.localPosition = Vector3.zero;
        }
    }

    //    public void OnPointerClick(PointerEventData eventData)
    //    {
    //
    //    }

    void Update()
    {
#if UNITY_EDITOR
        if (ContainerItem != null && Usable)
        {
            if (ContainerItem.CurrentInventory != Slot._inventory)
            {
                Debug.LogError("item not on place " + id);
            }
        }
#endif
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _clickTime = Time.time;
        if (eventData.clickCount == 2)
        {
            Slot.DoubleClick(this);
            eventData.clickCount = 0;
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var delta = Time.time - _clickTime;
        if (delta < 0.2f)
        {
            OnClickComplete();
        }

    }

    protected abstract void OnClickComplete();

    protected bool CanShowWindow()
    {
        if (Usable)
        {
            if (ContainerItem.CurrentInventory == MainController.Instance.MainPlayer.Inventory)
            {
                return true;
            }

            foreach (var startShipPilotData in MainController.Instance.MainPlayer.Army.Army)
            {
                if (startShipPilotData.Ship == ContainerItem.CurrentInventory)
                {
                    return true;
                }
            }
        }

        return false;
    }

    protected virtual void Dispose()
    {

    }
    protected virtual void Refresh()
    {

    }

    void OnEnable()
    {
        Refresh();
    }

    void OnDestroy()
    {
        Dispose();
    }
}

