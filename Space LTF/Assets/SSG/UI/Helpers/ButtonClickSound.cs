using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSound : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip Clicp;

    void Awake()
    {
        var btn = GetComponent<Button>();
        if (btn != null)
        {
            audioSource = WindowManager.Instance.UiAudioSource;
            Clicp = DataBaseController.Instance.AudioDataBase.ButtonClick;
            btn.onClick.AddListener(onClick);
        }
    }

    private void onClick()
    {
        audioSource.PlayOneShot(Clicp);
    }
}
