using System;
using UnityEngine;
using System.Collections;

public class BattleTutorialVideo : VideoTutorialElement
{
    public override void Init()
    {
        base.Init();

//        if (!_isCompleted)
//            WindowManager.Instance.OnWindowSetted += OnWindowSetted;
    }

    public override void Open(Action callback)
    {
        base.Open(callback);
        BattleController.Instance.PauseData.Pause();
    }

    private void OnWindowSetted(BaseWindow obj)
    {
//        var imGameWindow = obj as InGameMainUI;
//        if (imGameWindow != null)
//        {
//            OpenIfNotCompleted();
//            if (!_isCompleted)
//                BattleController.Instance.PauseData.Pause();
//        }
    }

    protected override void OnClose()
    {
        BattleController.Instance.PauseData.Change();
        base.OnClose();
    }

    public override void Dispose()
    {
//        WindowManager.Instance.OnWindowSetted -= OnWindowSetted;
        base.Dispose();
    }
}
