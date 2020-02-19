public class BattleMapTutorial : TutorialElement
{
    public bool ThisLauchOpened { get; private set; }
    public override void Init()
    {
        if (!_isCompleted)
            WindowManager.Instance.OnWindowSetted += OnWindowSetted;
        base.Init();
    }

    private void OnWindowSetted(BaseWindow obj)
    {
        var imGameWindow = obj as InGameMainUI;
        if (imGameWindow != null)
        {
            OpenIfNotCompleted();
            if (!_isCompleted)
                BattleController.Instance.PauseData.Pause();
        }
    }

    protected override void subClose()
    {
        ThisLauchOpened = true;
    }

    public override void Dispose()
    {
        WindowManager.Instance.OnWindowSetted -= OnWindowSetted;
        base.Dispose();
    }
}
