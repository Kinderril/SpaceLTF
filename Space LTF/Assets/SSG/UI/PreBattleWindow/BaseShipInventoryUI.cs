using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public  class BaseShipInventoryUI : DragZone
{
    public Image IconType;
    public TextMeshProUGUI ConfigType;
    public TextMeshProUGUI NameField;
    public Transform SpellsLayout;
    public Transform PamsLayout;
    public PlayerParameterUI PlayerParameterPrefab;
    private List<PlayerParameterUI> _curParams = new List<PlayerParameterUI>();

    private ShipInventory _shipInventory;

    public void Init(PlayerParameters playerParameters,ShipInventory shipInventory, bool usable, ConnectInventory connectedInventory)
    {
        _curParams.Clear();
        _shipInventory = shipInventory;
        SpellsLayout.ClearTransform();
        PamsLayout.ClearTransform();

        var allSlots = InitSpells();
        NameField.text = shipInventory.Name;

        ConfigType.text = Namings.ShipConfig(shipInventory.ShipConfig);
        IconType.sprite = DataBaseController.Instance.DataStructPrefabs.GetShipTypeIcon(shipInventory.ShipType);

        InitParameter(playerParameters.ChargesCount);
        InitParameter(playerParameters.ChargesSpeed);
        InitParameter(playerParameters.Scouts);
        InitParameter(playerParameters.Diplomaty);
        InitParameter(playerParameters.Repair);
        var rect = PamsLayout.GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);


        base.Init(shipInventory, usable, allSlots, connectedInventory);
        InitCurrentItems();
    }

    private void InitParameter(PlayerParameter parameter)
    {
        var itemParameter = DataBaseController.GetItem(PlayerParameterPrefab);
        itemParameter.transform.SetParent(PamsLayout,false);
        itemParameter.Init(parameter);
        _curParams.Add(itemParameter);
    }

    public override void Dispose()
    {
        foreach (var param in _curParams)
        {
            param.Dispose();
        }
        _curParams.Clear();
        base.Dispose();
    }

    private List<DragableItemSlot> InitSpells()
    {
        List<DragableItemSlot> allslots = new List<DragableItemSlot>();
        for (int i = 0; i < _shipInventory.SpellModulsCount; i++)
        {
            var spellSlot = InventoryOperation.GetDragableItemSlot();
            allslots.Add(spellSlot);
            spellSlot.Init(_shipInventory,true);
            spellSlot.transform.SetParent(SpellsLayout, false);
            spellSlot.DragItemType = DragItemType.spell;
        }
        return allslots;
    }

    private void InitCurrentItems()
    {
        for (int i = 0; i < _shipInventory.SpellsModuls.Length; i++)
        {
            var spell = _shipInventory.SpellsModuls[i];
            var slot = GetFreeSlot(ItemType.spell);
            slot.Init(_shipInventory, _usable);
            SetStartItem(slot, spell);
        }
    }
}

