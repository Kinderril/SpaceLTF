using UnityEngine;
using System.Collections;

public class CampaingController
{
    public PlayerChampaing PlayerChampaing;

    public void Load(int field)
    {

    }

    public void PlayerNewGame()
    {
        var playerChampaing = new PlayerChampaing();
        PlayerChampaing = playerChampaing;
        WindowManager.Instance.OpenWindow(MainState.startNewChampaing, PlayerChampaing);
    }

    public void DebugNewChamp(int act)
    {
        ShipConfig cfg = ShipConfig.ocrons;
        var playerChampaing = new PlayerChampaing();
        PlayerChampaing = playerChampaing;
        PlayerChampaing.StartNewGame(cfg, EStartGameDifficulty.Normal);
        PlayerChampaing.DebugSetAct(act);
        PlayerChampaing.PlayNextAct();
        PlayerChampaing.Player.ReputationData.SetAllies(cfg);
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
