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
#if UNITY_EDITOR || Develop
        if (Input.GetKeyDown(KeyCode.F1))
        {
            OnDebugChangeSound();
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (_inGameWindow != null)
                _inGameWindow.OnKillOnEnemiesDebugClick();
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            DebugParamsController.NoMouseMove = !DebugParamsController.NoMouseMove;
        }  
        if (Input.GetKeyDown(KeyCode.F7))
        {
            DebugParamsController.EngineOff = !DebugParamsController.EngineOff;
        }  
        if (Input.GetKeyDown(KeyCode.F8))
        {
            DebugParamsController.NoDamage = !DebugParamsController.NoDamage;
        }
#endif
    }

    private void OnDebugChangeSound()
    {
        CamerasController.Instance.MainListenerSwitch();
    }
}
