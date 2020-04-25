using UnityEngine;
using System.Collections;

public class GlobalMapTutorialVideo : VideoTutorialElement
{
    public override void Init()
    {
        base.Init();

//        if (!_isCompleted)
//            WindowManager.Instance.OnWindowSetted += OnWindowSetted;
    }

    private void OnWindowSetted(BaseWindow obj)
    {
//        var imGameWindow = obj as MapWindow;
//        if (imGameWindow != null)
//        {
//            OpenIfNotCompleted();
//        }
    }

    protected override void OnClose()
    {
        base.OnClose();
    }

    public override void Dispose()
    {
//        WindowManager.Instance.OnWindowSetted -= OnWindowSetted;
        base.Dispose();
    }
}
