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
                    Open();
                    mapWindow.OnStartInfoClose -= OnStartInfoClose;
                }

                mapWindow.OnStartInfoClose += OnStartInfoClose;
            }
            else
            {
                Open();
            }
        }
    }

    public override void Dispose()
    {
        WindowManager.Instance.OnWindowSetted -= OnWindowSetted;
        base.Dispose();
    }
}
