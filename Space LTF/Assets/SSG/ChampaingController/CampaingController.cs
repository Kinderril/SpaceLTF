using UnityEngine;
using System.Collections;

public class CampaingController
{
    public PlayerChampaingContainer PlayerChampaingContainer;
    public CampaingLoader CampaingLoader = new CampaingLoader();

    public void Load(PlayerChampaingContainer loadPlayer)
    {
        PlayerChampaingContainer = loadPlayer;
        MainController.Instance.TryLoadCamp(loadPlayer);
    }

    public bool SaveGame(string name,bool autosave)
    {
        if (PlayerChampaingContainer == null)
        {
            Debug.LogError("can't save null profile");
            return false;
        }
        return CampaingLoader.SaveTo(name, PlayerChampaingContainer, autosave);
    }

    public void DeleteSave(string name)
    {
        CampaingLoader.DeleteSave(name);

    }
    public void PlayerNewGame()
    {
        var playerChampaing = new PlayerChampaingContainer();
        PlayerChampaingContainer = playerChampaing;
        WindowManager.Instance.OpenWindow(MainState.startNewChampaing, PlayerChampaingContainer);
    }

    public void DebugNewChamp(int act)
    {
        ShipConfig cfg = ShipConfig.raiders;
        var playerChampaing = new PlayerChampaingContainer();
        PlayerChampaingContainer = playerChampaing;
        PlayerChampaingContainer.StartNewGame(cfg, EStartGameDifficulty.Normal);
        PlayerChampaingContainer.DebugSetAct(act);
        PlayerChampaingContainer.PlayNextAct();
        PlayerChampaingContainer.Player.ReputationData.SetAllies(cfg);
        switch (act)
        {
//            case 1:
//                break;
            case 1:
                DebugParamsController.HireAction(playerChampaing.Player, 4);
                for (int i = 0; i < 10; i++)
                {
                    DebugParamsController.LevelUpRandom(playerChampaing.Player);
                }
                break;
            case 2:
                DebugParamsController.HireAction(playerChampaing.Player, 4);
                DebugParamsController.HireAction(playerChampaing.Player, 4);
                for (int i = 0; i < 20; i++)
                {
                    DebugParamsController.LevelUpRandom(playerChampaing.Player);
                }
                break;
        }
    }


}
