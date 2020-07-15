public class TutorialController : Singleton<TutorialController>
{
    public LateBattleMapTutorial battleStart;
    public LateBattleMapTutorial battleStartLate;
    public BattleMapTutorial battleStart3;
    public BattleMapTutorial battleStartEnemy;

    public BattleTutorialVideo VideoBattleTutorial;
    public GlobalMapTutorialVideo GlobalMapTutorialVideo;

    public LevelUpMapTutorial mapUpgrade;
    public StartMapTutorial mapMain;
    public StartMapTutorial mapMain2;
    public OpenInventoryTutorial mapInventory;
    public EndBattleTutorial endBattle;
    public ShopMapTutorial shopMain;
    public PreBattleTutorial preBattle;
    public ExprolerMapTutorial exprolerTutor;
    public bool EnableTutor = false;

    public void Init()
    {
        if (EnableTutor)
        {
            #region DISABLED

            if (battleStart != null)
                battleStart.gameObject.SetActive(false);
            if (battleStartLate != null)
                battleStartLate.gameObject.SetActive(false);
            if (battleStart3 != null)
                battleStart3.gameObject.SetActive(false);
            if (battleStartEnemy != null)
                battleStartEnemy.gameObject.SetActive(false);
            if (mapMain != null)
                mapMain.gameObject.SetActive(false);
            if (mapMain2 != null)
                mapMain2.gameObject.SetActive(false);
            if (preBattle != null)
                preBattle.gameObject.SetActive(false);

            GlobalMapTutorialVideo.Init();
            VideoBattleTutorial.Init();
            exprolerTutor.Init();
            #endregion


            if (endBattle != null)
                endBattle.Init();
            if (mapUpgrade != null)
                mapUpgrade.Init();
            if (mapInventory != null)
                mapInventory.Init();
            if (shopMain != null)
                shopMain.Init();
        }
    }
}