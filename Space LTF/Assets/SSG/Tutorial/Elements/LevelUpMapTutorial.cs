using UnityEngine;
using System.Collections;

public class LevelUpMapTutorial : TutorialElement
{
    public override void Init()
    {
        if (!_isCompleted)
            WindowManager.Instance.OnWindowSetted += OnWindowSetted;
        base.Init();
    }

    private void OnWindowSetted(BaseWindow obj)
    {

        if (obj is MapWindow)
        {
            var player = MainController.Instance.MainPlayer;
            var army = player.Army;
            foreach (var data in army)
            {
                if (data.Pilot.CanUpgradeAnyParameter(0))
                {
                    OpenIfNotCompleted();
                    return;
                }
            }
        }
    }

    public override void Dispose()
    {
        WindowManager.Instance.OnWindowSetted -= OnWindowSetted;
        base.Dispose();
    }
}
