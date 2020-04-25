using System;
using UnityEngine;
using System.Collections;

public class SimpleTutorialVideo : VideoTutorialElement
{
    public override void Init()
    {
        base.Init();
    }

    private void OnWindowSetted(BaseWindow obj)
    {
//        var imGameWindow = obj as InGameMainUI;
//        if (imGameWindow != null)
//        {
//            OpenIfNotCompleted();
//        }
    }

    protected override void OnClose()
    {
//        BattleController.Instance.PauseData.Change();
        base.OnClose();
    }

    public override void Dispose()
    {
//        WindowManager.Instance.OnWindowSetted -= OnWindowSetted;
        base.Dispose();
    }
}
