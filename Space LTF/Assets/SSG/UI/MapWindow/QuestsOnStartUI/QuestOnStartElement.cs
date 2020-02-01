using TMPro;
using UnityEngine;


public class QuestOnStartElement : MonoBehaviour
{
    private BaseQuestOnStart _quest;
    public TextMeshProUGUI CountField;
    public TextMeshProUGUI NameField;
    public GameObject ReadyObject;
    public GameObject CompleteObject;
    private CompleteQuestOnStartUI _takeRewardObject;

    private bool CanReward => _quest.IsReady && !_quest.IsCompleted;
    public void Init(BaseQuestOnStart quest, CompleteQuestOnStartUI takeRewardObject)
    {
        _takeRewardObject = takeRewardObject;
        _quest = quest;
        NameField.text = quest.Name;
        _quest.OnElementFound += OnElementFound;
        OnElementFound();
    }

    private void OnElementFound()
    {
        if (!_takeRewardObject.gameObject.activeSelf)
            _takeRewardObject.gameObject.SetActive(false);

        ReadyObject.gameObject.SetActive(CanReward);
        CompleteObject.gameObject.SetActive(_quest.IsCompleted);
        CountField.gameObject.SetActive(!_quest.IsCompleted);
        if (_quest.IsCompleted)
        {
            CountField.text = Namings.Tag("Completed");
        }
        else
        {
            CountField.text = $"{_quest.CurCount}/{_quest.TargetCounter}";
        }
    }

    public void Dispose()
    {
        _quest.OnElementFound -= OnElementFound;
    }

    public void OnClickReady()
    {
        if (CanReward)
            _takeRewardObject.Init(_quest, CloseReward);
    }

    private void CloseReward()
    {
        _takeRewardObject.gameObject.SetActive(false);
        OnElementFound();
    }

    public void UpdateInfo()
    {
        OnElementFound();
    }
}

