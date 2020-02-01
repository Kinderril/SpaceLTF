using UnityEngine;


public class AudioSourceMusicControl : MonoBehaviour
{
    public AudioSource GlobalMapSource;
    public AudioSource MenuSource;

    private bool _fadeInGlobal;
    private float _fadeInGlobalStatus;
    private bool _fadeInMenu;
    private float _fadeInGlobalMenu;

    private const float FADE_PERIOD_SEC = 0.7f;

    public void StopMenuAudio()
    {
        MenuSource.enabled = false;
    }
    public void StartMenuAudio()
    {
        MenuSource.enabled = true;
        MenuSource.volume = 0f;
        _fadeInGlobalMenu = 0f;
        _fadeInMenu = true;
    }
    public void StartGlobalAudio()
    {
        GlobalMapSource.enabled = true;
        GlobalMapSource.volume = 0f;
        _fadeInGlobalStatus = 0f;
        _fadeInGlobal = true;
    }
    public void StopGlobalAudio()
    {
        GlobalMapSource.enabled = false;
    }

    void Update()
    {
        FadeIn(ref _fadeInGlobal, ref _fadeInGlobalStatus, ref GlobalMapSource);
        FadeIn(ref _fadeInMenu, ref _fadeInGlobalMenu, ref MenuSource);
    }

    private void FadeIn(ref bool shallUse, ref float status, ref AudioSource source)
    {
        if (shallUse)
        {
            status += Time.deltaTime;
            var volume = status / FADE_PERIOD_SEC;
            if (volume > 1)
            {
                source.volume = 1f;
                shallUse = false;
            }
            else
            {
                source.volume = volume;
            }
        }
    }
}

