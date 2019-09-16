using UnityEngine;
using System.Collections;

public class TutorialController : Singleton<TutorialController>
{
    public TutorialElement battleSpells;
    public BattleMapTutorial battleStart;
    public LevelUpMapTutorial mapUpgrade;
    public TutorialElement mapInventory;
    public StartMapTutorial mapMain;

    public void Init()
    {
        battleSpells.Init();
        battleStart.Init();
        mapUpgrade.Init();
        mapInventory.Init();
        mapMain.Init();
    }


}
