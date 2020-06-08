using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WindowQuests : MonoBehaviour
{
    private PlayerQuestData _playerQuests;
    private bool _isInited;
//    private bool _firstOpen;
   public RectTransform Layout;
    private List<QuestContainerElement> _elelemtns = new List<QuestContainerElement>();
    private ContentSizeFitter fiter;
    public void Init(PlayerQuestData playerQuests)
    {
        gameObject.SetActive(false);
        if (_isInited)
        {
            return;
        }

        _isInited = true;
        _playerQuests = playerQuests;
        _playerQuests.OnQuestAdd += OnQuestAdd;
        foreach (var playerQuestsAllQuest in _playerQuests.AllQuests)
        {
            InitQuest(playerQuestsAllQuest);
        }
    }

    private void OnQuestAdd(QuestContainer obj)
    {
        InitQuest(obj);
    }

    private void InitQuest(QuestContainer questContainer)
    {
        QuestContainerElement prefab = DataBaseController.Instance.DataStructPrefabs.QuestContainerElement;
        var elelemt = DataBaseController.GetItem(prefab);
        elelemt.transform.SetParent(Layout);
        elelemt.Init(questContainer,Close);
        _elelemtns.Add(elelemt);
        LayoutRebuilder.ForceRebuildLayoutImmediate(Layout);
    }

    private IEnumerator RebuildCour()
    {
        yield return new WaitForFixedUpdate();
        Layout.gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(Layout);
    }

    public void ClearAll()
    {
        _isInited = false;
        if (_playerQuests != null)
        {
            _playerQuests.OnQuestAdd -= OnQuestAdd;
        }
        foreach (var questContainerElement in _elelemtns)
        {
            questContainerElement.ClearAll();
            GameObject.Destroy(questContainerElement.gameObject);
        }

        _elelemtns.Clear();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
        Layout.gameObject.SetActive(false);
        StartCoroutine(RebuildCour());

    }
}
