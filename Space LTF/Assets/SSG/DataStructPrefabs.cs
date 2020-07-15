using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct ActionIcon
{
    public Sprite Icon;
    public ActionType ActionType;
}

[Serializable]
public struct ShipTypeIcon
{
    public Sprite Icon;
    public ShipType ShipType;
}

//[Serializable]
//public struct BaseModulIcon
//{
//    public Sprite Icon;
//    public SimpleModulType SimpleModulType;
//}

[Serializable]
public struct ParameterItemIcon
{
    public Sprite Icon;
    public EParameterItemSubType SubType;
    public ItemType ItemType;
}
[Serializable]
public struct ParameterItemIconNoSubtype
{
    public Sprite Icon;
    public ItemType ItemType;
}

[Serializable]
public struct WeaponIcon
{
    public Sprite Icon;
    public WeaponType WeaponType;
}
[Serializable]
public struct SpellIcon
{
    public Sprite Icon;
    public SpellType SpellType;
}
[Serializable]
public struct PilotTacticIconPriority
{
    public Sprite Icon;
    public ECommanderPriority1 Tactic;
}
[Serializable]
public struct PilotTacticIconSide
{
    public Sprite Icon;
    public ESideAttack Tactic;
}

[Serializable]
public struct PilotGlobalTacticIconSide
{
    public Sprite Icon;
    public EGlobalTactics Tactic;
}
[Serializable]
public struct PilotTrickIcon
{
    public Sprite Icon;
    public EPilotTricks Trick;
}

public class DataStructPrefabs : MonoBehaviour
{
    public List<Bullet> Bullets = new List<Bullet>();
    public List<ShipStruct> Ships = new List<ShipStruct>();
    public List<ShipStruct> Turrets = new List<ShipStruct>();

    public List<PoolElement> AsteroidParts = new List<PoolElement>();
    public List<PoolElementRigitBody> DestroyedShipParts = new List<PoolElementRigitBody>();

    public DebugRationInfo DebugRationInfo;
    public ShipUIOnMap ShipUIOnMap;
    public ShipUIOnMap ShipUIOnMapMini;
    public ModulUI ModulUI;
    public SupportModulUI SupportModulUI;
    public WeaponModulUI WeaponModulUI;
    public CircleShader CircleShader;
    public FlyNumberWithDependence FlyNumberWithDependence;
    public FlyingNumbers FlyingNumber;
    public CoinUI CoinPrefab;
    public SideShipInfo SideShipInfoLeft;
    public SideShipMiniInfo SideShipInfoRight;
    public SectorGlobalMapInfo SectorGlobalMapInfo;

    public DragableWeaponItem DragableItemWeaponPrefab;
    public DragableModulItem DragableItemModulPrefab;
    public DragableSpellItem DragableItemSpellPrefab;
    public DragableItemSlot DragableItemSlotPrefab;
    public DragableParameterItem DragableItemParameterPrefab;

    public LightUpObject LightUpObject;

    //    public BaseEffectAbsorber ModulEffectDestroyPrefab;
    //    public BaseEffectAbsorber ShipEngineStopPrefab;
    //    public BaseEffectAbsorber RepairEffectPrefab;
    //    public BaseEffectAbsorber PeriodDamageEffectPrefab;
    public BaseEffectAbsorber OnShipDeathEffect;
    public BaseEffectAbsorber ShieldChagedEffect;
    public BaseEffectAbsorber EngineLockerEffect;
    public BaseEffectAbsorber WeaponWaveStrike;

    //    public List<ActionIcon> ActionIcons = new List<ActionIcon>();
    //    private Dictionary<ActionType,Sprite> ActionIconsDic = new Dictionary<ActionType, Sprite>(); 

    public List<PilotTacticIconPriority> ShipTacticIcons = new List<PilotTacticIconPriority>();
    private Dictionary<ECommanderPriority1, Sprite> ShipTacticIconsDic = new Dictionary<ECommanderPriority1, Sprite>();
    public List<PilotTacticIconSide> ShipTacticSideIcons = new List<PilotTacticIconSide>();
    public List<PilotGlobalTacticIconSide> GlobalTacticsIconsSide = new List<PilotGlobalTacticIconSide>();
    public List<PilotTrickIcon> PilotTricksIconsSide = new List<PilotTrickIcon>();
    private Dictionary<ESideAttack, Sprite> ShipTacticIconsSideDic = new Dictionary<ESideAttack, Sprite>();
    private Dictionary<EGlobalTactics, Sprite> GlobalTacticsIconsSideDic = new Dictionary<EGlobalTactics, Sprite>();
    private Dictionary<EPilotTricks, Sprite> PilotTricksIconsSideDic = new Dictionary<EPilotTricks, Sprite>();

    public List<ShipTypeIcon> ShipTypeIcons = new List<ShipTypeIcon>();
    private Dictionary<ShipType, Sprite> ShipTypeIconsDic = new Dictionary<ShipType, Sprite>();

    //    public List<BaseModulIcon> ModulsIcons = new List<BaseModulIcon>();
    private Dictionary<SimpleModulType, Sprite> ModulsIconsDic = new Dictionary<SimpleModulType, Sprite>();
    private Dictionary<PilotRank, Sprite> PilotRankIconsDic = new Dictionary<PilotRank, Sprite>();

    public List<ParameterItemIcon> ParametersItemsIcons = new List<ParameterItemIcon>();
    public List<ParameterItemIconNoSubtype> ParametersItemsIconsNoSubType = new List<ParameterItemIconNoSubtype>();
    public List<WeaponIcon> WeaponTypeIcons = new List<WeaponIcon>();
    private Dictionary<WeaponType, Sprite> WeaponTypeIconsDic = new Dictionary<WeaponType, Sprite>();
    private Dictionary<string, Sprite> SpritesByName = new Dictionary<string, Sprite>();

    public List<SpellIcon> SpellTypeIcons = new List<SpellIcon>();
    private Dictionary<SpellType, Sprite> SpellTypeIconsDic = new Dictionary<SpellType, Sprite>();
    public PlayerArmyUI PlayerArmyUIPrefab;
    public MoneySlotUI MoneySlotUIPrefab;
    public Tooltip TooltipPrefab;
    public SpellBigTooltip SpellBigTooltip;
    public WeaponBigTooltip WeaponBigTooltip;
    public ModulBigTooltip ModulBigTooltip;
    public ParamItemBigTooltip ParamItemBigTooltip;
    public FlyingNumberWithDependencesHoldin FlyingNumberWithDependencesHoldinPrefab;
    public SelfCamera SelfCameraPrefab;
    public ArrowTarget ArrowTargetPersonal;
    public GameObject ShipSelectedObject;
    public List<FlyingAsteroid> FlyingAsteroids;
    public Sprite MoneyIcon;
//    public QuestOnStartElement QuestOnStartElement;
    public EnemyGlobalMapMoverObjet SpecOpsMovingArmyObject;
    public EnemyGlobalMapMoverObjet SimpleMovingArmyObject;
    public ImageWithTooltip NoScoutData;
    public Material GreenFlare;
    public Material RedFlare;
    public RoadMeshCreator ConnectorTurrets;
    public TurretConnectorContainer TurretConnector;
    public List<GameObject> FireVortexs;
    public List<GameObject> Vortexs;
    public List<GameObject> IceZone;
    public List<GameObject> BlackHole;
    public QuestStageElement QuestStageElement;
    public QuestContainerElement QuestContainerElement;
    public CraftDataBase Craft;
    public ShipWayDrawler WayDrawler;


    public void Init()
    {
        //----------
        foreach (var actionIcon in SpellTypeIcons)
        {
            SpellTypeIconsDic.Add(actionIcon.SpellType, actionIcon.Icon);
        }
        var values6 = Enum.GetValues(typeof(SpellType)).Length;
        if (values6 != SpellTypeIconsDic.Count)
        {
            Debug.LogError($"not enought icons SpellType {values6}/{SpellTypeIconsDic.Count}");
        }

        //----------
        foreach (var actionIcon in WeaponTypeIcons)
        {
            WeaponTypeIconsDic.Add(actionIcon.WeaponType, actionIcon.Icon);
        }
        //        var values4 = Enum.GetValues(typeof(WeaponType)).Length;
        if (8 != WeaponTypeIcons.Count)
        {
            Debug.LogError("not enought icons WeaponTypeIconsDic: 6");
        }
        //----------

        LoadModulsIcons();
        LoadRankIcons();
        LoadAchievementsIcon();

        //----------
        //        foreach (var actionIcon in ActionIcons)
        //        {
        //            ActionIconsDic.Add(actionIcon.ActionType,actionIcon.Icon);
        //        }
        //        var values = Enum.GetValues(typeof(ActionType)).Length;
        //        if (values != ActionIcons.Count)
        //        {
        //            Debug.LogError("not enought icons actions");
        //        }

        //----------
        foreach (var actionIcon in ShipTypeIcons)
        {
            ShipTypeIconsDic.Add(actionIcon.ShipType, actionIcon.Icon);
        }
        var values2 = Enum.GetValues(typeof(ShipType)).Length;
        if (values2 != ShipTypeIcons.Count)
        {
            Debug.LogError("not enought icons ships");
        }

        //----------
        foreach (var actionIcon in ShipTacticIcons)
        {
            ShipTacticIconsDic.Add(actionIcon.Tactic, actionIcon.Icon);
        }
        var values5 = Enum.GetValues(typeof(ECommanderPriority1)).Length;
        if (values5 != ShipTacticIcons.Count)
        {
            Debug.LogError($"not enought pilot ECommanderPriority1 tactics {values5}  !=  {ShipTacticIcons.Count } ");

        }

        //----------
        foreach (var actionIcon in ShipTacticSideIcons)
        {
            ShipTacticIconsSideDic.Add(actionIcon.Tactic, actionIcon.Icon);
        }
        var values8 = Enum.GetValues(typeof(ESideAttack)).Length;
        if (values8 != ShipTacticSideIcons.Count)
        {
            Debug.LogError("not enought pilot ESideAttack tactics");
        }

        //----------
        foreach (var actionIcon in GlobalTacticsIconsSide)
        {
            GlobalTacticsIconsSideDic.Add(actionIcon.Tactic, actionIcon.Icon);
        }
        var values9 = Enum.GetValues(typeof(EGlobalTactics)).Length;
        if (values9 != GlobalTacticsIconsSideDic.Count)
        {
            Debug.LogError("not enought pilot GlobalTactics tactics");
        }

        //----------
        foreach (var actionIcon in PilotTricksIconsSide)
        {
            PilotTricksIconsSideDic.Add(actionIcon.Trick, actionIcon.Icon);
        }
        var values10 = Enum.GetValues(typeof(EPilotTricks)).Length;
        if (values10 != PilotTricksIconsSideDic.Count)
        {
            Debug.LogError("not enought pilot tricks");
        }

    }

    private void LoadAchievementsIcon()
    {
        var singl = SteamStatsAndAchievements.Instance;
        var array = Enum.GetValues(typeof(SteamStatsAndAchievements.Achievement));
        var values3 = (SteamStatsAndAchievements.Achievement[])array;
        foreach (var achievement in values3)
        {
            var ss = singl.Achievements.FirstOrDefault(x => x.m_eAchievementID == achievement);
            if (ss != null)
            {
                var name = ss.m_eAchievementID.ToString();
                var str = Namings.Format("Icons/Achievements/{0}", name);
                var obj = Resources.Load<Sprite>(str);
                if (obj != null)
                    SpritesByName.Add(name, obj);
                else
                {

                    Debug.LogError($"No ICON path:{str}");
                }
            }
            else
            {
                Debug.LogError("can't find icon achivements Icons / achievements / " + achievement);
            }
        }


    }

    private void LoadModulsIcons()
    {
        var array = Enum.GetValues(typeof(SimpleModulType));
        var values3 = (SimpleModulType[])array;
        var noIcon = new HashSet<SimpleModulType>();
        foreach (var v in values3)
        {
            var obj = Resources.Load<Sprite>(Namings.Format("Icons/moduls/{0}", v.ToString()));
            ModulsIconsDic.Add(v, obj);
            if (obj == null)
            {
                noIcon.Add(v);
            }

        }

        foreach (var simpleModulType in noIcon)
        {
            Debug.LogError($"Modul {simpleModulType.ToString()} have no Icon");
        }
    }

    private void LoadRankIcons()
    {
        var array = Enum.GetValues(typeof(PilotRank));
        var values3 = (PilotRank[])array;
        foreach (var v in values3)
        {
            var obj = Resources.Load<Sprite>(Namings.Format("Icons/PilotRanks/{0}", v.ToString()));
            PilotRankIconsDic.Add(v, obj);

        }
    }

    public void CheckShipsWeaponsPosition()
    {
#if UNITY_EDITOR
        var p = new Player("Debug test player");
        foreach (var shipStruct in Ships)
        {
            var s = Library.CreateShip(shipStruct.ShipType, shipStruct.ShipConfig, p.SafeLinks, Library.CreateDebugPilot());
            if (s.WeaponModulsCount != shipStruct.ShipBase.WeaponPosition.Count && shipStruct.ShipType != ShipType.Base)
            {
                Debug.LogError(Namings.Format("wrong weapons postions count {0} and {1}.   target:{2}  onPrefab:{3}", shipStruct.ShipType,
                    shipStruct.ShipConfig, s.WeaponModulsCount, shipStruct.ShipBase.WeaponPosition.Count));
            }
        }
#endif
    }


    //    public Sprite GetActionIcon(ActionType actionType)
    //    {
    //#if UNITY_EDITOR
    //        if (!ActionIconsDic.ContainsKey(actionType))
    //        {
    //            Debug.LogError("have no icon " + actionType.ToString());
    //        }
    //#endif
    //
    //        return ActionIconsDic[actionType];
    //    }

    public Sprite GetTacticIcon(ECommanderPriority1 actionType)
    {
#if UNITY_EDITOR
        if (!ShipTacticIconsDic.ContainsKey(actionType))
        {
            Debug.LogError("have no pilot tactic icon" + actionType.ToString());
        }
#endif

        return ShipTacticIconsDic[actionType];
    }
    public Sprite GetTacticIcon(ESideAttack actionType)
    {
#if UNITY_EDITOR
        if (!ShipTacticIconsSideDic.ContainsKey(actionType))
        {
            Debug.LogError("have no pilot tactic icon" + actionType.ToString());
        }
#endif

        return ShipTacticIconsSideDic[actionType];
    }

    public Sprite GetTacticIcon(EGlobalTactics actionType)
    {
#if UNITY_EDITOR
        if (!GlobalTacticsIconsSideDic.ContainsKey(actionType))
        {
            Debug.LogError("have no pilot tactic icon" + actionType.ToString());
        }
#endif

        return GlobalTacticsIconsSideDic[actionType];
    }
    public Sprite GetTrickIcon(EPilotTricks trickType)
    {
#if UNITY_EDITOR
        if (!PilotTricksIconsSideDic.ContainsKey(trickType))
        {
            Debug.LogError("have no pilot trick icon" + trickType.ToString());
        }
#endif

        return PilotTricksIconsSideDic[trickType];
    }

    public Sprite GetSpellIcon(SpellType actionType)
    {
#if UNITY_EDITOR
        if (!SpellTypeIconsDic.ContainsKey(actionType))
        {
            Debug.LogError("have no icon " + actionType.ToString());
        }
#endif
        return SpellTypeIconsDic[actionType];
    }

    public Sprite GetShipTypeIcon(ShipType shipType)
    {
#if UNITY_EDITOR
        if (!ShipTypeIconsDic.ContainsKey(shipType))
        {
            Debug.LogError("have no icon " + shipType.ToString());
        }
#endif
        return ShipTypeIconsDic[shipType];
    }
    public Sprite GetWeaponIcon(WeaponType shipType)
    {
#if UNITY_EDITOR
        if (!WeaponTypeIconsDic.ContainsKey(shipType))
        {
            Debug.LogError("have no weapon icon " + shipType.ToString());
        }
#endif
        return WeaponTypeIconsDic[shipType];
    }
    public Sprite GetModulIcon(SimpleModulType modul)
    {
#if UNITY_EDITOR
        if (!ModulsIconsDic.ContainsKey(modul))
        {
            Debug.LogError("have no modul icon " + modul.ToString());
        }
#endif
        return ModulsIconsDic[modul];
    }

    public Sprite GetRankSprite(PilotRank statsCurRank)
    {
#if UNITY_EDITOR
        try
        {
            return PilotRankIconsDic[statsCurRank];
        }
        catch (Exception e)
        {
            Debug.LogError($"GetRankSprite {statsCurRank.ToString()}");
        }
#endif
        return PilotRankIconsDic[statsCurRank];
    }

    public Sprite GetAchiewementIcon(string achievementTMStrName)
    {
        if (SpritesByName.TryGetValue(achievementTMStrName, out var spr))
        {
            return spr;
        }
        return null;
    }

    public Sprite GetParameterItemIcon(ItemType parameterItemItemType, EParameterItemSubType subType)
    {
        ParameterItemIcon? itemIcon = ParametersItemsIcons.FirstOrDefault(x =>
            x.ItemType == parameterItemItemType && x.SubType == subType);
        if (itemIcon.HasValue)
        {
            return itemIcon.Value.Icon;
        }

        return null;

    }

    public Sprite GetParameterItemIcon(ItemType parameterItemItemType)
    {
        ParameterItemIconNoSubtype? itemIcon = ParametersItemsIconsNoSubType.FirstOrDefault(x =>
            x.ItemType == parameterItemItemType);
        if (itemIcon.HasValue)
        {
            return itemIcon.Value.Icon;
        }

        return null;

    }
}

