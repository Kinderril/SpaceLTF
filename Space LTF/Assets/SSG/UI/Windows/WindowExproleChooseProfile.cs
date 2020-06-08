using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class WindowExproleChooseProfile : BaseWindow
{
    private bool _isSingleInit;
    public RectTransform Layout;
    public ExprolerEmptyProfile EmptyProfile;
    private  List<ExprolerProfileElement> _profiles = new List<ExprolerProfileElement>();
    public override void Init()
    {
        base.Init();
        EmptyProfile.Init();
        if (!_isSingleInit)
        {
            SingleInit();
        }
    }

    private void SingleInit()
    {
        _isSingleInit = true;
//        EmptyProfile.Init();
        var data = MainController.Instance.SafeContainers;
        foreach (var playersProfile in data.PlayersProfiles())
        {
            OnAddSafeContainer(playersProfile);
        }
        data.OnAddSafeContainer += OnAddSafeContainer;
        data.OnRemoveSafeContainer += OnRemoveSafeContainer;
    }

    private void OnRemoveSafeContainer(PlayerSafe obj)
    {
        var find = _profiles.FirstOrDefault(x => x.PlayerSafe == obj);
        if (find == null)
        {
            Debug.LogError("can't remove player visual");
            return;
        }  
        GameObject.Destroy(find.gameObject);
    }

    private void OnAddSafeContainer(PlayerSafe obj)
    {
        var element =
            DataBaseController.GetItem(DataBaseController.Instance.ExprolerDataBase.ExprolerProfileElement);
        element.Init(obj, ProfileRemove);
        element.transform.SetParent(Layout);
        element.transform.SetAsFirstSibling();
        LayoutRebuilder.ForceRebuildLayoutImmediate(Layout);
        _profiles.Add(element);
    }

    private void ProfileRemove(PlayerSafe obj)
    {
        var data = MainController.Instance.SafeContainers;
        data.RemoveNewContainer(obj);
    }

    public void OnClickBack()
    {
        WindowManager.Instance.OpenWindow(MainState.start);
    }


    public override void Dispose()
    {
        base.Dispose();
    }

}
