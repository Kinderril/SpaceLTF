using UnityEngine;
using System.Collections;

public class DebugAtStartNewGame : MonoBehaviour
{
//    private MapWindow _inGameWindow;
//    void Awake()
//    {
////        _inGameWindow = GetComponent<MapWindow>();
//    }

    void Update()
    {
//#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.F5))
        {
            OnDebugOpenAll();
        }
//#endif
    }

    private void OnDebugOpenAll()
    {
        var startNewGameWindow = WindowManager.Instance.CurrentWindow as WindowNewGame;
        if (startNewGameWindow != null)
        {
            startNewGameWindow.DebugOpenAll();
        }
    }
}
