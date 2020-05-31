using UnityEngine;
using System.Collections;

public class QuestButton : MonoBehaviour
{
    private Player _player;
    private MapWindow _window;
    private bool _isInited = false;
    public GameObject QuestChange;
    public GameObject NewQuestChange;
    public Animator Animator;

    public void Init(Player player,MapWindow window)
    {
        if (_isInited)
        {
            return;
        }

        NewQuestChange.SetActive(false);
        QuestChange.SetActive(false);
_window = window;
_isInited = true;
        _player = player;
        _player.QuestData.OnComplete += OnComplete;
        _player.QuestData.OnReadyToComplete += OnReadyToComplete;
        _player.QuestData.OnStageChange += OnStageChange;
        _player.QuestData.OnQuestAdd += OnQuestAdd;
    }

    private void OnQuestAdd(QuestContainer obj)
    {
        QuestChange.SetActive(true);
    }

    public void OnClick()
    {
        _window.OnQuestsOpen();
        QuestChange.SetActive(false);
        NewQuestChange.SetActive(false);
    }

    private void OnStageChange(QuestContainer obj)
    {
        PlayGlow();
    }

    private void OnReadyToComplete(QuestContainer obj)
    {
        PlayGlow();
    }

    private void OnComplete(QuestContainer obj)
    {
        PlayGlow();
    }

    private void PlayGlow()
    {
        QuestChange.SetActive(true);
        Animator.SetTrigger("Play");
    }

    public void ClearAll()
    {
        _isInited = false;
        if (_player != null)
        {
            _player.QuestData.OnComplete -= OnComplete;
            _player.QuestData.OnReadyToComplete -= OnReadyToComplete;
            _player.QuestData.OnStageChange -= OnStageChange;
            _player.QuestData.OnQuestAdd -= OnQuestAdd;
            _player = null;
        }
    }

}
