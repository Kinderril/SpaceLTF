using UnityEngine;
using System.Collections;

public class CampLoadContainer : MonoBehaviour
{

    public Transform Layout;
    public CampLoadSlot PrefabSlot;


    public void Init()
    {
        gameObject.SetActive(true);

        Layout.ClearTransform();

        var campLoader = MainController.Instance.Campaing.CampaingLoader.GetAllSaves();
        if (campLoader == null)
        {
            return;
        }

        foreach (var s in campLoader)
        {
            var nSlot = DataBaseController.GetItem(PrefabSlot);
            nSlot.transform.SetParent(Layout,false);
            nSlot.Init(s);
        }
    }

    public void OnCloseClick()
    {
        gameObject.SetActive(false);
    }
}
