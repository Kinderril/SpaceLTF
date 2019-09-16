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

        if (obj is InGameMainUI)
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
