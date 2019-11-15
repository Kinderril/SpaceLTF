using UnityEngine;
using System.Collections;

public class OpenInventoryTutorial : TutorialElement
{
    private MapWindow _mapWindow;
    public override void Init()
    {
        if (!_isCompleted)
            WindowManager.Instance.OnWindowSetted += OnWindowSetted;
        base.Init();
    }

    private void OnWindowSetted(BaseWindow obj)
    {

        if (obj is MapWindow)
        {
            WindowManager.Instance.OnWindowSetted -= OnWindowSetted;
            _mapWindow = obj as MapWindow;
            _mapWindow.OnOpenInventory += OnOpenInventory;

        }
    }

    private void OnOpenInventory(bool obj)
    {
        if (obj)
        {
            OpenIfNotCompleted();
        }
    }

    public override void Dispose()
    {
        if (_mapWindow != null)
        {
            _mapWindow.OnOpenInventory -= OnOpenInventory;
        }
        WindowManager.Instance.OnWindowSetted -= OnWindowSetted;
        base.Dispose();
    }
}
