using UnityEngine;
using System.Collections;

public class PreBattleTutorial : TutorialElement
{
    public override void Init()
    {                                                                                           
        if (!_isCompleted)
            WindowManager.Instance.OnWindowSetted += OnWindowSetted;
        base.Init();
    }

    private void OnWindowSetted(BaseWindow obj)
    {
        var imGameWindow = obj as PreBattleWindow;
        if (imGameWindow != null)
        {
            Open();                                      
        }
    }

    public override void Dispose()
    {
        WindowManager.Instance.OnWindowSetted -= OnWindowSetted;
        base.Dispose();
    }
}
