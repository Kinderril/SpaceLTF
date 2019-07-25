using System;
using UnityEngine;
using UnityEngine.UI;

public class TacticChooseButton : MonoBehaviour
{
    public Image TacticImage;
    public PilotTcatic Tcatic { get; set; }
    public Action<TacticChooseButton> OnClickCallback;

    public void Init(PilotTcatic tcatic, Action<TacticChooseButton> OnClick)
    {
        Tcatic = tcatic;
        OnClickCallback = OnClick;
        TacticImage.sprite = DataBaseController.Instance.DataStructPrefabs.GetTacticIcon(tcatic);
    }

    public void OnClick()
    {
        OnClickCallback(this);
    }
}

