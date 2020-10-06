using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class WindowChampaing : BaseWindow
{
    public DialogWindow Dialog;
    private PlayerChampaing _playerChampaing;
    public TextMeshProUGUI Field;
    public GameObject ScrollContent;
    public GameObject ScrollContainer;
//    public bool ShallRefreshPos = true;
//    public bool ShallRefreshPosWithZero = true;

    public override void Init()
    {
        Debug.LogError("Wrong");
        base.Init();
    }

    public override void Init<T>(T obj)
    {
        base.Init(obj);
        Dialog.gameObject.SetActive(false);
        ScrollContent.gameObject.SetActive(true);
        _playerChampaing = obj as PlayerChampaing;
        Field.text = _playerChampaing.GetActDesc();
    }

//    public void RefreshPosition()
//    {
//        if (!ShallRefreshPos)
//        {
//            return;
//        }
//        var rect = ScrollContainer.GetComponent<RectTransform>();
//        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
//        if (ShallRefreshPosWithZero)
//        {
//            ScrollContainer.gameObject.transform.localPosition = new Vector3(0, -1000, 0);
//        }
//        else
//        {
//            var localPos = ScrollContainer.gameObject.transform.localPosition;
//            ScrollContainer.gameObject.transform.localPosition = new Vector3(localPos.x, -1000, localPos.z);
//
//        }
//        //        Debug.LogError($"RefreshPosition:{ Layout.gameObject.transform.localPosition}");
//    }

    public void OnClickClose()
    {
        var startDialog = _playerChampaing.Dialog();
        if (startDialog != null)
        {
            ScrollContent.gameObject.SetActive(false);
            Dialog.Init(startDialog,DialogEnds);
        }
        else
        {
            ContinueAct();
        }

    }

    private void ContinueAct()
    {

        _playerChampaing.PlayNextAct();
    }

    private void DialogEnds(bool shallcompletecell, bool shallreturntolastcell)
    {
        ContinueAct();

    }
}
