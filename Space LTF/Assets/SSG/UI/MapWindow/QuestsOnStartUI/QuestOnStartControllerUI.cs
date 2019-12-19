using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class QuestOnStartControllerUI : MonoBehaviour
{
    private bool _inited;
    private QuestsOnStartController _controller;
    private List<QuestOnStartElement> _elements = new List<QuestOnStartElement>();
    public Transform Layout;
    public CompleteQuestOnStartUI TakeRewardObject;
    public void Init(QuestsOnStartController controller)
    {
        TakeRewardObject.gameObject.SetActive(false);
        _controller = controller;
        if (!_inited)
        {
            _inited = true;
            foreach (var quest in _controller.ActiveQuests)
            {
                var element =
                    DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.QuestOnStartElement);
                element.transform.SetParent(Layout);
                element.Init(quest, TakeRewardObject);
                _elements.Add(element);
            }
        }
        else
        {
            foreach (var element in _elements)
            {
                element.UpdateInfo();
            }
        }
    }

    public void Dispose()
    {

    }

    public void ClearAll()
    {
        _inited = false;
        _elements.Clear();
        Layout.ClearTransform();
    }
}

