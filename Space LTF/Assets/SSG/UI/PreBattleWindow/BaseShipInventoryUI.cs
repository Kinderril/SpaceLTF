using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BaseShipInventoryUI : DragZone
{
    public Image IconType;
    public TextMeshProUGUI ConfigType;
    public TextMeshProUGUI NameField;
    public TextMeshProUGUI MaxShipField;
    public Transform SpellsLayout;
    public Transform PamsLayout;
    public PlayerParameterUI PlayerParameterPrefab;
    public BaseShipSpellContainer ShipSpellContainerPrefab;
    private List<PlayerParameterUI> _curParams = new List<PlayerParameterUI>();

    private ShipInventory _shipInventory;
    private PlayerSafe _player;

    public void Init(PlayerSafe player, ShipInventory shipInventory, bool usable, ConnectInventory connectedInventory, IInventory tradeInventory = null)
    {
        _tradeInventory = tradeInventory;
        _player = player;
        _curParams.Clear();
        _shipInventory = shipInventory;
        SpellsLayout.ClearTransform();
        PamsLayout.ClearTransform();

        var allSlots = InitSpells();
        NameField.text = shipInventory.Name;

        ConfigType.text = Namings.ShipConfig(shipInventory.ShipConfig);
        IconType.sprite = DataBaseController.Instance.DataStructPrefabs.GetShipTypeIcon(shipInventory.ShipType);
        var playerParameters = player.Parameters;
        InitParameter(playerParameters.ChargesCount);
        InitParameter(playerParameters.ChargesSpeed);
        InitParameter(playerParameters.Scouts);
        InitParameter(playerParameters.Repair);
        InitParameter(playerParameters.EnginePower);

        var rect = PamsLayout.GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);


        base.Init(shipInventory, usable, allSlots, connectedInventory);
        InitCurrentItems();
        UpdateArmyCount();
        _player.OnAddShip += OnAddShip;
    }

    private void OnAddShip(StartShipPilotData arg1, bool arg2)
    {
        UpdateArmyCount();
    }

    private void UpdateArmyCount()
    {
        MaxShipField.text = Namings.Format(Namings.Tag("MaxArmyCount"), _player.Ships.Count, (PlayerArmy.MAX_ARMY));
    }

    private void InitParameter(PlayerParameter parameter)
    {
        var itemParameter = DataBaseController.GetItem(PlayerParameterPrefab);
        itemParameter.transform.SetParent(PamsLayout, false);
        itemParameter.Init(parameter);
        _curParams.Add(itemParameter);
    }

    public override void Dispose()
    {
        foreach (var param in _curParams)
        {
            param.Dispose();
        }
        _player.OnAddShip -= OnAddShip;
        _curParams.Clear();
        base.Dispose();
    }

    private HashSet<DragableItemSlot> InitSpells()
    {              
        HashSet<DragableItemSlot> allslots = new HashSet<DragableItemSlot>();
        for (int i = 0; i < _shipInventory.SpellModulsCount; i++)
        {
            var spellSlot = DataBaseController.GetItem(ShipSpellContainerPrefab);
            var inv = _shipInventory.GetModulsInventory(i);
            var slots = spellSlot.InitSpell(i,SpellsLayout, _shipInventory, inv);
            foreach (var dragableItemSlot in slots)
            {
                allslots.Add(dragableItemSlot);
            }
          
        }
        return allslots;
    }

    private void InitCurrentItems()
    {
        for (int i = 0; i < _shipInventory.SpellsModuls.SpellsCount; i++)
        {
            var spell = _shipInventory.SpellsModuls.Get(i);
            if (spell != null)
            {
                var slot = GetFreeSlot(i,ItemType.spell);
                slot.Init(_shipInventory, _usable, i);
                SetStartItem(slot, spell, _tradeInventory);
            }
        }
    }
}

