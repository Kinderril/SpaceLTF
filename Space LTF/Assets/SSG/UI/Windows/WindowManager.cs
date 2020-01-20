using System;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public enum MainState
{
    start = 1,
    play = 2,
    pay = 3,
    debugStart = 4,
    endBattle = 5,
    preBattle = 6,
    map = 7,
    shop = 8,
    modif = 9,
    repair = 10,
    loseBattle = 11,
    startNewGame = 12,
    endGame = 13,
    runAwayBattle = 14,
    loadinbg = 15,
    achievements = 16,
    //    loading,
}

[Serializable]
public struct WindowT
{
    public MainState state;
    public BaseWindow window;
}

public class WindowManager : Singleton<WindowManager>
{
    public WindowT[] windows;
    private BaseWindow currentWindow;
    private BaseWindow nextWindow;

    public Camera MainCamera;
    public Camera SubCamera;
    public ConfirmWindow ConfirmWindow;
    public WaitingWindow WaitingWindow;
    //    public WindowSellWithCount ConfirmWindowWithCount;
    public InfoWindow InfoWindow;
    public InfoWindowWithShop InfoWindowWithShop;
    public ItemInfosController ItemInfosController;
    private GameObject planeX;
    public GameObject MainBack;
    public GameObject LoadingScreen;
    public event Action<BaseWindow> OnWindowSetted;
    //    public event Action OnInfoWindowWithShopClose;
    public Action OnItemWindowClose;
    public TextMeshProUGUI VersionField;

    public CanvasGroup WindowMainCanvas;
    public CanvasGroup WindowSubCanvas;
    //    public ItemWindow ItemWindow;

    public Transform UIPool;
    public Transform TopPanel;
    public MapSettingsWindow WindowSettings;
    public AudioSource UiAudioSource;

    public BaseWindow CurrentWindow => currentWindow;

    public void Init()
    {
        WindowSettings.gameObject.SetActive(false);
        LoadingScreen.gameObject.SetActive(false);
        foreach (var window in windows)
        {
            window.window.gameObject.SetActive(false);
            window.window.Activate();
        }
        ItemInfosController.InitSelf();
        if (ConfirmWindow != null)
            ConfirmWindow.gameObject.SetActive(false);
        if (WaitingWindow != null)
            WaitingWindow.gameObject.SetActive(false);
        //        ConfirmWindowWithCount.gameObject.SetActive(false);
        if (InfoWindow != null)
            InfoWindow.gameObject.SetActive(false);

        MainController.CheckVersion();
        VersionField.text = MainController.VERSION;
    }

    public void OpenWindow(MainState state)
    {
        OpenWindow<object>(state, null);
    }

    public void OpenSettingsSettings(bool withCloseButtons, Action closeCallback = null)
    {
        WindowManager.Instance.UiAudioSource.PlayOneShot(DataBaseController.Instance.AudioDataBase.WindowOpen);
        CurrentWindow.CanvasGroup.interactable = false;
        WindowSettings.Init(withCloseButtons);
        WindowSettings.Open(() =>
        {
            closeCallback?.Invoke();
            CurrentWindow.CanvasGroup.interactable = true;
        });
    }
    private void MakeScreenShot()
    {
        var cam = MainCamera;
        planeX = new GameObject("planeX");
        var mr = planeX.AddComponent<MeshRenderer>();
        var mf = planeX.AddComponent<MeshFilter>();
        var mesh = new Mesh();
        cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3[] vertices =
        {
            cam.ViewportToWorldPoint(new Vector3(0, 0, 0)),
            cam.ViewportToWorldPoint(new Vector3(1, 0, 0)),
            cam.ViewportToWorldPoint(new Vector3(0, 1, 0)),
            cam.ViewportToWorldPoint(new Vector3(1, 1, 0))
        };
        mesh.vertices = vertices;
        Vector2[] uv = { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) };
        mesh.uv = uv;
        int[] triangles = { 2, 1, 0, 2, 3, 1 };
        mesh.triangles = triangles;
        mf.mesh = mesh;
        //        mr.material = new Material(Shader.Find("Custom/BackgroundNoAlpha"));
        var rt = new RenderTexture(Screen.width, Screen.height, 24);
        RenderTexture renderTextureTmp = null;
        if (cam.targetTexture != null)
        {
            renderTextureTmp = cam.targetTexture;
        }
        cam.targetTexture = rt;
        cam.Render();
        RenderTexture.active = rt;
        if (renderTextureTmp != null)
        {
            cam.targetTexture = renderTextureTmp;
        }
        else
        {
            cam.targetTexture = null;
        }
        mr.material.mainTexture = rt;
    }

    public void OpenWindow<T>(MainState state, T obj)
    {

        if (planeX != null)
        {
            Destroy(planeX.gameObject);
        }
        UiAudioSource.PlayOneShot(DataBaseController.Instance.AudioDataBase.WindowOpen);
        WindowSettings.gameObject.SetActive(false);
        Debug.Log("OpenWindow " + state);
        var isInGame = state == MainState.play;
        MainCamera.enabled = isInGame;
        SubCamera.enabled = !isInGame;
        MainController.Instance.InputManager.SetEnable(isInGame);
        var nextWindow = windows.FirstOrDefault(x => x.state == state).window;
        if (nextWindow == null)
        {
            Debug.LogError("can't find window" + state.ToString());
            return;
        }


        //window.StartAnimation();
        try
        {
            OpenWindowPreTask(obj, nextWindow);
        }
        catch (Exception ex)
        {
            var t = nextWindow.name + " error " + ex;
            //            DebugController.Instance.SetInfo2(t);
            Debug.LogError(ex);
        }
    }

    private async void OpenWindowPreTask<T>(T obj, BaseWindow windowToLoad)
    {
        await OpenWindowTask(obj, windowToLoad);
    }

    private async Task OpenWindowTask<T>(T obj, BaseWindow windowToLoad)
    {
        bool withLOadWindow = windowToLoad.WithLoadWindow;
        if (withLOadWindow)
        {

            LoadingScreen.gameObject.SetActive(true);
            await Task.Yield();
            await Task.Delay(100);
        }
        if (currentWindow != null)
        {
            CurrentWindow.Dispose();
            if (windowToLoad.Animator == null)
            {
                CurrentWindow.EndClose();
            }
            else
            {
                CurrentWindow.Close();
            }
            //            Debug.LogError($"CurrentWindow.gameObject.SetActive(false) {CurrentWindow.name}");
            CurrentWindow.gameObject.SetActive(false);
            //            var sIndex = currentWindow.transform.GetSiblingIndex();
            //            nextWindow.transform.SetSiblingIndex(sIndex + 1);

            //            TopPanel.transform.SetAsLastSibling();
        }
        //        Debug.LogError($"$Window to load {windowToLoad.name}");
        if (obj != null)
        {
            windowToLoad.Init<T>(obj);
        }
        else
        {
            windowToLoad.Init();
        }

        if (withLOadWindow)
        {
            await Task.Yield();
            LoadingScreen.gameObject.SetActive(false);
        }

        currentWindow = windowToLoad;
        //        Debug.LogError($"set currentWindow {currentWindow.name}");
        if (OnWindowSetted != null)
        {
            OnWindowSetted(currentWindow);
        }
    }

    public void NotEnoughtMoney(int money)
    {
        WindowManager.Instance.InfoWindow.Init(null, String.Format("Not enought credits {0}.", money));
    }

    public void ItemWindowClose()
    {
        if (OnItemWindowClose != null)
        {
            OnItemWindowClose();
        }
    }
}

