using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PilotInventoryUI : MonoBehaviour
{
    private IPilotParameters _pilot;
    private ShipInventory _ship;
    //    public SliderWithTextMeshPro DelaySlider;
    public ParameterWithLevelUp HealthField;
    public TextMeshProUGUI MoneyField;
    // public Slider LevelUpSlider;
    public ParameterWithLevelUp ShieldField;
    public ParameterWithLevelUp SpeedField;
    //    public TextMeshProUGUI TacticField;
    public TextMeshProUGUI RankField;
    public TextMeshProUGUI KillsField;
    public Image TacticPriorityIcon;
    public Image TacticSideIcon;
    public ImageWithTooltip RankIcon;
    public ParameterWithLevelUp TurnField;
    // public Button LevelUpButton;
    public PriorityTooltipInfo PriorityTooltipInfo;
    public SideAttackTooltipInfo SideAttackTooltipInfo;
    public Slider ExpirienceSlider;
    public UIElementWithTooltipCache ExpirienceToolitip;
    public TrickPilotButton TrickTrun;
    public TrickPilotButton TrickTwist;
    public TrickPilotButton TrickLoop;
    public TrickPilotButton TrickStrike;

    //    public GameObject LevelUpObject;
    //    public TextMeshProUGUI LevelUpField;

    public void Init(IPilotParameters pilot, ShipInventory ship)
    {
        _ship = ship;
        _pilot = pilot;
        _ship.OnShipRepaired += OnShipRepaired;
        _ship.OnItemAdded += OnItemAdded;
        _pilot.Tactic.OnPriorityChange += OnTacticPriorityChange;
        _pilot.Tactic.OnSideChange += OnTacticSideChange;
        _pilot.OnLevelUp += OnLevelUp;
        UpdateTacticField();

        RankUpdate();
        SetParamsAndMoney();
        UpdateTacticField();
        SetTricks();
    }

    private void SetTricks()
    {
        TrickTrun.Init(EPilotTricks.turn, _ship.PilotParameters.Stats);
        TrickTwist.Init(EPilotTricks.twist, _ship.PilotParameters.Stats);
        TrickLoop.Init(EPilotTricks.loop, _ship.PilotParameters.Stats);
        TrickStrike.Init(EPilotTricks.frontStrike, _ship.PilotParameters.Stats);
    }

    private void DisposeTricks()
    {
        TrickTrun.Dispose();
        TrickTwist.Dispose();
        TrickLoop.Dispose();
        TrickStrike.Dispose();
    }

    private void OnTacticSideChange(ESideAttack obj)
    {
        UpdateTacticField();
    }

    private void OnTacticPriorityChange(ECommanderPriority1 obj)
    {
        UpdateTacticField();
    }

    private void OnItemAdded(IItemInv item, bool val)
    {
        SetInfoParams();
    }
    public void SoftRefresh()
    {
        RankUpdate();
        TrickTrun.SoftRefresh();
        TrickTwist.SoftRefresh();
        TrickLoop.SoftRefresh();
        TrickStrike.SoftRefresh();
    }

    private void RefreshExpSlider()
    {
        if (_pilot.Stats.CurRank != PilotRank.Major)
        {
            var nextRankExp = Library.PilotRankExp[_pilot.Stats.CurRank];
            ExpirienceToolitip.Cache = $"{_pilot.Stats.Exp}/{nextRankExp}";
            var coef = ((float)(_pilot.Stats.Exp)) / ((float)(nextRankExp));
            ExpirienceSlider.value = coef;
        }
        else
        {
            ExpirienceToolitip.Cache = Namings.Tag("MaxLevel");
            ExpirienceSlider.value = 1;
        }
    }

    private void RankUpdate()
    {
        RefreshExpSlider();
        var reloadTime = Namings.Format(Namings.Tag("reloadBoost"), Library.CalcTrickReload(_pilot.Stats.CurRank, _ship.ShipType));
        RankIcon.Init(DataBaseController.Instance.DataStructPrefabs.GetRankSprite(_pilot.Stats.CurRank), reloadTime);
        RankField.text = _pilot.Stats.CurRank.ToString();
        var kills = _pilot.Stats.Kills;
        string info = Namings.Format(Namings.Tag("KillUIPilotMini"), kills);
        KillsField.text = info;
    }

    private void OnLevelUp(IPilotParameters obj)
    {
        SetParamsAndMoney();
    }

    private void SetParamsAndMoney()
    {
        var needToLvl = (float)Library.PilotLvlUpCost(_pilot.CurLevel);
        MoneyField.text = $"{_pilot.Money}/{needToLvl}";
        SetInfoParams();
    }

    private void UpdateTacticField()
    {
        TacticPriorityIcon.sprite = DataBaseController.Instance.DataStructPrefabs.GetTacticIcon(_pilot.Tactic.Priority);
        TacticSideIcon.sprite = DataBaseController.Instance.DataStructPrefabs.GetTacticIcon(_pilot.Tactic.SideAttack);
        PriorityTooltipInfo.SetData(_pilot.Tactic.Priority);
        SideAttackTooltipInfo.SetData(_pilot.Tactic.SideAttack);
    }

    private void OnShipRepaired(ShipInventory obj)
    {
        SetParamsAndMoney();
        SetInfoParams();
    }


    private void SetInfoParams()
    {
        var maxSpeed = ShipParameters.ParamUpdate(_ship.MaxSpeed, _pilot.SpeedLevel, ShipParameters.MaxSpeedCoef);
        var turnSpeed = ShipParameters.ParamUpdate(_ship.TurnSpeed, _pilot.TurnSpeedLevel, ShipParameters.TurnSpeedCoef);
        var maxShiled = ShipParameters.ParamUpdate(_ship.MaxShiled, _pilot.ShieldLevel, ShipParameters.MaxShieldCoef);
        var maxHealth = ShipParameters.ParamUpdate(_ship.MaxHealth, _pilot.HealthLevel, ShipParameters.MaxHealthCoef);

        PilotParamsInUI pilotParams = new PilotParamsInUI()
        { MaxSpeed = maxSpeed, MaxHealth = maxHealth, MaxShield = maxShiled, TurnSpeed = turnSpeed };

        foreach (var modul in _ship.Moduls.SimpleModuls)
        {
            if (modul != null)
            {
                var support = modul as BaseSupportModul;
                if (support != null)
                {
                    support.ChangeParamsShip(pilotParams);
                }
            }
        }

        var shiledInfo = LevelInfo(Namings.Tag("Shield"), _pilot.ShieldLevel, Info(pilotParams.MaxShield, _pilot.ShieldLevel));
        var speedInfo = LevelInfo(Namings.Tag("Speed"), _pilot.SpeedLevel, Info(pilotParams.MaxSpeed, _pilot.SpeedLevel));
        var turnInfo = LevelInfo(Namings.Tag("TurnSpeed"), _pilot.TurnSpeedLevel, Info(pilotParams.TurnSpeed, _pilot.TurnSpeedLevel));
        var curHp = pilotParams.MaxHealth * _ship.HealthPercent;
        var hpTxt = Namings.Format("{0}/{1}", curHp.ToString("0"), Info(pilotParams.MaxHealth, _pilot.HealthLevel,false));
        var txt = LevelInfo(Namings.Tag("Health"), _pilot.HealthLevel, hpTxt);

        ShieldField.SetData(shiledInfo, _pilot.ShieldLevel, _pilot, LibraryPilotUpgradeType.shield);
        SpeedField.SetData(speedInfo, _pilot.SpeedLevel, _pilot, LibraryPilotUpgradeType.speed);
        TurnField.SetData(turnInfo, _pilot.TurnSpeedLevel, _pilot, LibraryPilotUpgradeType.turnSpeed);
        HealthField.SetData(txt, _pilot.HealthLevel, _pilot, LibraryPilotUpgradeType.health);
    }

    private static string LevelInfo(string name, int level, string info)
    {
        return Namings.Format("{0}:{2}", name, level, info);
    }

    public static string Info(float p, int level,bool withFloatFormat = true)
    {
        var drob = p - (int)p;
        string format = "0";
        if (drob > 0f && withFloatFormat)
        {
            format = "0.0";
        }

        return p.ToString(format); ;
    }

    public void Dispose()
    {
        DisposeTricks();
        if (_pilot != null)
        {
            _pilot.Tactic.OnPriorityChange -= OnTacticPriorityChange;
            _pilot.Tactic.OnSideChange -= OnTacticSideChange;
        }
        if (_pilot != null)
        {
            _pilot.OnLevelUp -= OnLevelUp;
        }
        if (_ship != null)
        {
            _ship.OnItemAdded -= OnItemAdded;
            _ship.OnShipRepaired -= OnShipRepaired;
        }
    }



    //Straight|Flangs
    public void ClickStraight()
    {
        _pilot.Tactic.ChangeTo(ESideAttack.Straight);
    }
    public void ClickBaseDefence()
    {
        _pilot.Tactic.ChangeTo(ESideAttack.BaseDefence);
    }
    public void ClickFlangs()
    {
        _pilot.Tactic.ChangeTo(ESideAttack.Flangs);
    }

    //Base CommanderPriority   
    public void ClickECommanderPriority1Any()
    {
        _pilot.Tactic.ChangeTo(ECommanderPriority1.Any);
    }
    public void ClickECommanderPriority1MinShield()
    {
        _pilot.Tactic.ChangeTo(ECommanderPriority1.MinShield);
    }
    public void ClickECommanderPriority1MinHealth()
    {
        _pilot.Tactic.ChangeTo(ECommanderPriority1.MinHealth);
    }
    public void ClickECommanderPriority1MaxShield()
    {
        _pilot.Tactic.ChangeTo(ECommanderPriority1.MaxShield);
    }
    public void ClickECommanderPriority1MaxHealth()
    {
        _pilot.Tactic.ChangeTo(ECommanderPriority1.MaxHealth);
    }
    public void ClickECommanderPriority1Fast()
    {
        _pilot.Tactic.ChangeTo(ECommanderPriority1.Fast);
    }
    public void ClickECommanderPriority1Slow()
    {
        _pilot.Tactic.ChangeTo(ECommanderPriority1.Slow);
    }
    public void ClickECommanderPriority1Base()
    {
        _pilot.Tactic.ChangeTo(ECommanderPriority1.Base);
    }

}