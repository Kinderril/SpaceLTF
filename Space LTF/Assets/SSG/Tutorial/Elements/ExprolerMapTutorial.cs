using UnityEngine;
using System.Collections;

public class ExprolerMapTutorial : TutorialElement
{
    public override void Init()
    {
        if (!_isCompleted)
            WindowManager.Instance.OnWindowSetted += OnWindowSetted;
        base.Init();
    }

    private void OnWindowSetted(BaseWindow obj)
    {

        var mapWindow = obj as WindowExprolerGlobalMap;
        if (mapWindow != null)
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
