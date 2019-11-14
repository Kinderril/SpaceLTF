using UnityEngine;
using System.Collections;

public class TutorialController : Singleton<TutorialController>
{
    public BattleMapTutorial battleStart;
    public BattleMapTutorial battleStart2;
    public BattleMapTutorial battleStart3;
    public LevelUpMapTutorial mapUpgrade;
    public StartMapTutorial mapMain;
    public StartMapTutorial mapMain2;
    public OpenInventoryTutorial mapInventory;
    public EndBattleTutorial endBattle;
    public ShopMapTutorial shopMain;
    public PreBattleTutorial preBattle;
    public bool EnableTutor = false;

    public void Init()
    {
        if (EnableTutor)
        {
            if (preBattle != null)
                preBattle.Init();  
            if (endBattle != null)
                endBattle.Init();  
            if (battleStart != null)
                battleStart.Init();
            if (battleStart2 != null)
                battleStart2.Init();
            if (battleStart3 != null)
                battleStart3.Init();

            if (mapMain != null)
                mapMain.Init();
            if (mapMain2 != null)
                mapMain2.Init();

            if (mapUpgrade != null)
                mapUpgrade.Init();
            if (mapInventory != null)
                mapInventory.Init();
            if (shopMain != null)
                shopMain.Init();
        }
    }
}
