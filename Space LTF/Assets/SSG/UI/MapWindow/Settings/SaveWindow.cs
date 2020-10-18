using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveWindow : MonoBehaviour
{

    public Transform Layout;
    public SaveSlot PrefabSlot;

    private List<SaveSlot> _curSlots = new List<SaveSlot>();

    public void Init()
    {
        gameObject.SetActive(true);
        _curSlots.Clear();
        Layout.ClearTransform();

        var nSlot1 = DataBaseController.GetItem(PrefabSlot);
        nSlot1.transform.SetParent(Layout, false);
        nSlot1.Init(OnClickCallback, SaveComplete);
        _curSlots.Add(nSlot1);

        var campLoader = MainController.Instance.Campaing.CampaingLoader.GetAllSaves();
        if (campLoader == null)
        {
            return;
        }

        foreach (var s in campLoader)
        {
            var nSlot = DataBaseController.GetItem(PrefabSlot);
            nSlot.transform.SetParent(Layout, false);
            nSlot.Init(s, OnClickCallback, SaveComplete);
            _curSlots.Add(nSlot);
        }
    }

    private void SaveComplete(SaveSlot obj)
    {
        OnCloseClick();
    }

    private void OnClickCallback(SaveSlot obj)
    {
        foreach (var saveSlot in _curSlots)
        {
            if (saveSlot != obj)
            {
                saveSlot.OnClickClose();
            }
        }
    }

    public void OnCloseClick()
    {
        gameObject.SetActive(false);
    }
}
