using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Toggle = UnityEngine.UI.Toggle;

public class WindowKeys : MonoBehaviour
{
    public Transform Layout;
    public HotkeyElement HotkeyElementPrefab;
    public bool IsInited { get; set; }
    public CanvasGroup Canvas;
    private Action _closeCallback;
    public Toggle SoundToggle;

    public void Init()
    {
        SoundToggle.isOn = (CamerasController.Instance.IsAudioEnable);
        if (!IsInited)
        {
            Canvas.ignoreParentGroups = true;
            IsInited = true;
            var list = new List<KeyCode>();
            list.Add(KeyCode.Q);
            list.Add(KeyCode.Alpha1);
            list.Add(KeyCode.Alpha2);
            list.Add(KeyCode.Alpha3);
            list.Add(KeyCode.Alpha4);
            list.Add(KeyCode.Alpha5);
            list.Add(KeyCode.Alpha6);
            list.Add(KeyCode.LeftShift);
            list.Add(KeyCode.Space);
            list.Add(KeyCode.Tab);
            list.Add(KeyCode.W);
            list.Add(KeyCode.S);
            list.Add(KeyCode.A);
            list.Add(KeyCode.D);
            foreach (var keyCode in list)
            {
                var elemnt = DataBaseController.GetItem(HotkeyElementPrefab);
                elemnt.gameObject.transform.SetParent(Layout, false);
                elemnt.Init(keyCode);
            }
        }
    }

    public void Open(Action closeCallback)
    {
        _closeCallback = closeCallback;
        gameObject.SetActive(true);
    }

    public void OnClickClose()
    {
        gameObject.SetActive(false);
        _closeCallback();
    }

    public void OnSound()
    {
        CamerasController.Instance.MainListenerSwitch();
    }

}
