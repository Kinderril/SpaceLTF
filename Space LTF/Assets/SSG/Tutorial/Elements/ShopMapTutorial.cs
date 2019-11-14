using UnityEngine;
using System.Collections;

public class ShopMapTutorial : TutorialElement
{
    public override void Init()
    {        
        if (!_isCompleted)
            WindowManager.Instance.OnWindowSetted += OnWindowSetted;
        base.Init();
    }

    private void OnWindowSetted(BaseWindow obj)
    {
        var imGameWindow = obj as WindowShop;
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
