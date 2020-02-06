using UnityEngine;

public class DebugAtStartNewGame : MonoBehaviour
{
    private string curMasg = "";
    //    private MapWindow _inGameWindow;
    //    void Awake()
    //    {
    ////        _inGameWindow = GetComponent<MapWindow>();
    //    }

    void Update()
    {
        //#if UNITY_EDITOR

        CheatsInput();
        // if (Input.GetKeyDown(KeyCode.F5))
        // {
        //     OnDebugOpenAll();
        // }
        //#endif
    }

    private float _lastKeyDown;

    private void CheatsInput()
    {
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(vKey))
            {
                var delta = Time.time - _lastKeyDown;
                _lastKeyDown = Time.time;
                var k = vKey.ToString();
                if (delta < 1f)
                {
                    curMasg = $"{curMasg}{k}";
                    if (curMasg == "IDKAFA")
                    {
                        Debug.Log($"Cheat activated OnDebugOpenAll");
                        OnDebugOpenAll();
                    }
                    // Debug.Log("Current Key is : " + curMasg);
                }
                else
                {
                    curMasg = k;
                    // Debug.Log("Start Key is : " + k);

                }

            }
        }

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
