using UnityEngine;
using System.Collections;

public class EndBattleTutorial : TutorialElement
{
    public override void Init()
    {        
        if (!_isCompleted)
            WindowManager.Instance.OnWindowSetted += OnWindowSetted;
        base.Init();
    }

    private void OnWindowSetted(BaseWindow obj)
    {
        var imGameWindow = obj as EndGameWindow;
        if (imGameWindow != null)
        {
            OpenIfNotCompleted();
        }
    }

    public override void Dispose()
    {
        WindowManager.Instance.OnWindowSetted -= OnWindowSetted;
        base.Dispose();
    }
}
