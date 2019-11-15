using UnityEngine;
using System.Collections;

public class StartMapTutorial : TutorialElement
{
    public override void Init()
    {
        if (!_isCompleted)
            WindowManager.Instance.OnWindowSetted += OnWindowSetted;
        base.Init();
    }

    private void OnWindowSetted(BaseWindow obj)
    {

        var mapWindow = obj as MapWindow;
        if (mapWindow != null)
        {
            if (mapWindow.StartInfo.activeInHierarchy)
            {
                void OnStartInfoClose()
                {
                    OpenIfNotCompleted();
                    mapWindow.OnStartInfoClose -= OnStartInfoClose;
                }

                mapWindow.OnStartInfoClose += OnStartInfoClose;
            }
            else
            {
                OpenIfNotCompleted();
            }
        }
    }

    public override void Dispose()
    {
        WindowManager.Instance.OnWindowSetted -= OnWindowSetted;
        base.Dispose();
    }
}
