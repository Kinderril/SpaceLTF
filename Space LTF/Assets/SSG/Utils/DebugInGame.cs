using UnityEngine;
using System.Collections;

public class DebugInGame : MonoBehaviour
{
    private InGameMainUI _inGameWindow;
    void Awake()
    {
        _inGameWindow = GetComponent<InGameMainUI>();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (_inGameWindow != null)
                _inGameWindow.OnKillOnEnemiesDebugClick();
        }
#endif
    }


}
