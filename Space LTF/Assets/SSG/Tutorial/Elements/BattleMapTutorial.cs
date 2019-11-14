using UnityEngine;
using System.Collections;

public class BattleMapTutorial : TutorialElement
{
    public override void Init()
    {                                                                                           
        if (!_isCompleted)
            WindowManager.Instance.OnWindowSetted += OnWindowSetted;
        base.Init();
    }

    private void OnWindowSetted(BaseWindow obj)
    {
        var imGameWindow = obj as InGameMainUI;
        if (imGameWindow != null)
        {
            Open();
            if (!_isCompleted)
                BattleController.Instance.PauseData.Pause();
        }
    }

    public override void Dispose()
    {
        WindowManager.Instance.OnWindowSetted -= OnWindowSetted;
        base.Dispose();
    }
}
