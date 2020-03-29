using UnityEngine;

public class DebugAtGlobalMap : MonoBehaviour
{
    //    private MapWindow _inGameWindow;
    //    void Awake()
    //    {
    ////        _inGameWindow = GetComponent<MapWindow>();
    //    }

    void Update()
    {

#if Develop || UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F1))
        {
            OnDebugChangeSound();
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            OnDebugAddRep();
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            OnDebugAddMoney();
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            OnDebugScoutedAll();
        }
#endif
    }

    private void OnDebugChangeSound()
    {
        CamerasController.Instance.MainListenerSwitch();
    }

    public void OnDebugAddRep()
    {
        MainController.Instance.MainPlayer.ReputationData.AddReputation(ShipConfig.ocrons, 10);
        MainController.Instance.MainPlayer.ReputationData.AddReputation(ShipConfig.krios, 10);
        MainController.Instance.MainPlayer.ReputationData.AddReputation(ShipConfig.mercenary, 10);
        MainController.Instance.MainPlayer.ReputationData.AddReputation(ShipConfig.raiders, 10);
        MainController.Instance.MainPlayer.ReputationData.AddReputation(ShipConfig.federation, 10);
    }

    public void OnDebugAddMoney()
    {
        MainController.Instance.MainPlayer.MoneyData.AddMoney(100);
        MainController.Instance.MainPlayer.MoneyData.AddMicrochips(1);

    }

    public void OnDebugScoutedAll()
    {
        var s = MainController.Instance.MainPlayer.MapData.GalaxyData;
        for (int i = 0; i < s.SizeX; i++)
        {
            for (int j = 0; j < s.SizeZ; j++)
            {
                var cell = s.AllCells()[i, j];
                if (cell != null)
                    cell.Scouted();
            }
        }

    }

}
