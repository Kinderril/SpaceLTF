using UnityEngine;
using System.Collections;

public class TutorialController : Singleton<TutorialController>
{
    public BattleMapTutorial battleStart;
    public LevelUpMapTutorial mapUpgrade;
    public StartMapTutorial mapMain;
    public OpenInventoryTutorial mapInventory;

    public TutorialElement battleSpells;

    public void Init()
    {
        battleSpells.Init();
        battleStart.Init();
        mapUpgrade.Init();
        mapInventory.Init();
        mapMain.Init();
    }


}
