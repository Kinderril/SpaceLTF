using UnityEngine;
using UnityEngine.UI;


public class TrickPilotButton : MonoBehaviour
{
    public Button UpgradeButton;
    public EPilotTricks _trick;
    public GameObject Learned;
    private TotalStats _pilotStats;
    public Image Icon;
    // public TextMeshProUGUI Field;
    private bool _isLearned;
    public UIElementWithTooltipCache TooltipCache1;
    public UIElementWithTooltipCache TooltipCache2;

    public void Init(EPilotTricks trick, TotalStats pilotStats)
    {
        _trick = trick;
        _pilotStats = pilotStats;
        _pilotStats.OnRankChange += OnRankChange;
        _pilotStats.OnTrickLearned += OnTrickLearned;
        UpgradeButtonActive();
        TooltipCache2.Cache = TooltipCache1.Cache = Namings.Tag($"trick_{_trick.ToString()}");
        Icon.sprite = DataBaseController.Instance.DataStructPrefabs.GetTrickIcon(trick);
        // Field.text = Namings.Tag($"trick_name_{_trick.ToString()}");
    }

    private void OnTrickLearned(EPilotTricks obj)
    {
        UpgradeButtonActive();
    }

    public void SoftRefresh()
    {
        UpgradeButtonActive();
    }

    private void UpgradeButtonActive()
    {
        _isLearned = _pilotStats.GetTriks().Contains(_trick);
        UpgradeButton.interactable = _pilotStats.UpgradePoints > 0;
        UpgradeButton.gameObject.SetActive(!_isLearned);
        Learned.gameObject.SetActive(_isLearned);
    }

    public void OnClick()
    {
        _pilotStats.LearnTrick(_trick);
        UpgradeButtonActive();
    }

    private void OnRankChange(PilotRank obj)
    {
        if (_isLearned)
        {
            return;
        }
        UpgradeButtonActive();

    }

    public void Dispose()
    {
        _pilotStats.OnTrickLearned -= OnTrickLearned;
        _pilotStats.OnRankChange -= OnRankChange;
    }

}

