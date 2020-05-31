﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class QuestContainerElement : MonoBehaviour
{
    private QuestContainer _container;
    public RectTransform Layout;
    public GameObject ReadyToComplete;
    public GameObject Complete;
    public TextMeshProUGUI NameField;
    private List<QuestStageElement> _elements = new List<QuestStageElement>();
    private Action _closeCallback;
    public void Init(QuestContainer container,Action closeCallback)
    {
        _closeCallback = closeCallback;
        NameField.text = container.NameQuest();
        ReadyToComplete.gameObject.SetActive(false);
        Complete.gameObject.SetActive(false);
        _container = container;
        _container.OnReadyToComplete += OnReadyToComplete;
        _container.OnComplete += OnComplete;
        QuestStageElement prefab = DataBaseController.Instance.DataStructPrefabs.QuestStageElement;
        foreach (var stage in _container.Stages)
        {
            var element = DataBaseController.GetItem(prefab);
            element.Init(stage, closeCallback);
            element.transform.SetParent(Layout);
            _elements.Add(element);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(Layout);
        if (_container.IsComplete)
        {
            Complete.SetActive(true);
        }
        else
        {
            if (_container.ReadyIsComplete)
            {
                ReadyToComplete.SetActive(true);
            }
        }
    }

    public void OnClick()
    {
        _container.Complete(_closeCallback);
    }

    private void OnComplete(QuestContainer obj)
    {
        Complete.SetActive(true);
        NameField.text = $"{_container.NameQuest()} ({Namings.Tag("CompleteNodata")})";
    }

    private void OnReadyToComplete(QuestContainer obj)
    {
        ReadyToComplete.gameObject.SetActive(true);
        NameField.text = $"{_container.NameQuest()} ({Namings.Tag("ReadyToComplete")})";
    }

    public void ClearAll()
    {
        if (_container != null)
        {

            _container.OnComplete -= OnComplete;
            _container.OnReadyToComplete -= OnReadyToComplete;
        }

        foreach (var questStageElement in _elements)
        {
            questStageElement.ClearAll();
        }
        _elements.Clear();


    }
}
