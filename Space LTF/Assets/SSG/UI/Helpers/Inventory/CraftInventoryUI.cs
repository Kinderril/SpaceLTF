using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class CraftInventoryUI : InventoryUI
{
    public TextMeshProUGUI InfoField;
    public Button CraftButton;
    private PlayerInventory _targetCraftInventory;
    public Transform Result;
    public UIElementWithTooltipCache Tooltip;
    private GameObject _prevObject;
    public Animator CraftAnim;
    public InventoryUI ResultCraftPlace;
    public Transform CraftResultContainer;

    public void Init(PlayerInventory craftInventory, PlayerInventory targetInv)
    {
        _targetCraftInventory = targetInv;
        base.Init(craftInventory,null,true);
        ResultCraftPlace.Init(targetInv,null,true);
        craftInventory.OnItemAdded += OnItemAdded;
        targetInv.OnItemAdded += OnItemAdded;
        OnItemAdded(null,false);

    }

    private void NoPattern()
    {

        InfoField.text = Namings.Tag("NoPattern");
    }

    private bool HaveFreeSlot()
    {
        var slots = _targetCraftInventory.GetFreeSlotsCount();
        return slots > 0;
    }

    private void OnItemAdded(IItemInv item, bool val)
    {
        if (HaveFreeSlot())
        {
//            CraftResultContainer.gameObject.SetActive(false);
            var items = _inventory.GetAllItems();
            var result = CraftLibrary.CanUsePatter(items, out var pattern);
            if (result)
            {
                InfoField.text = Namings.Tag("ApplyPattern");
                Result.gameObject.SetActive(true);
                if (_prevObject != null)
                {
                    GameObject.Destroy(_prevObject);
                }
                _prevObject = pattern.ResultIcon(items, out var tooltipInfo);
                Tooltip.Cache = tooltipInfo;
                _prevObject.transform.SetParent(Result, false);
            }
            else
            {
                NoPattern();
                Result.gameObject.SetActive(false);
            }

            CraftButton.interactable = result;
        }
        else
        {
//            CraftResultContainer.gameObject.SetActive(true);
            InfoField.text = Namings.Tag("CraftReady");
            Result.gameObject.SetActive(false);
            CraftButton.interactable = false;
        }

    }


    public void OnClickTryCraft()
    {

        var items = _inventory.GetAllItems();
        if (CraftLibrary.CanUsePatter(items, out var pattern))
        {

            if (CraftLibrary.TryApplyPatter(items, _targetCraftInventory, pattern))
            {
                CraftAnim.SetTrigger("play");
            }
            else
            {
                WindowManager.Instance.InfoWindow.Init(null,Namings.Tag("CraftError"));
            }
        }
    }

    public override void Dispose()
    {
        if (_prevObject != null)
        {
            GameObject.Destroy(_prevObject);
        }
        _inventory.OnItemAdded -= OnItemAdded;
        _targetCraftInventory.OnItemAdded -= OnItemAdded;
        ResultCraftPlace.Dispose();
        base.Dispose();
    }
}
