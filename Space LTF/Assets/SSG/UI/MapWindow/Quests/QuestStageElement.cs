using System;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class QuestStageElement : MonoBehaviour
{
    public TextMeshProUGUI Field;
    public Image Complete;
    public Image NonActive;
    public Image Active;
    public Image Failed;
    private QuestStage _stage;
    private Action _closeWindowCallback;
    public void Init(QuestStage stage,Action closeWindowCallback)
    {
        _closeWindowCallback = closeWindowCallback;
        _stage = stage;
        UpdateBacks();
        UpdateText();
        stage.OnTextChange += OntextChange;
        stage.OnComplete += OnComplete;
        stage.OnFailed += OnFailed;
        stage.OnStageActivated += OnStageActivated;
    }

    private void OnFailed(QuestStage obj)
    {
        UpdateBacks();
        UpdateText();


    }

    private void UpdateBacks()
    {
        if (_stage.Failed)
        {
            Failed.enabled = true;
            NonActive.enabled = Complete.enabled = Active.enabled = false;
        }
        else
        {
            if (_stage.IsComplete)
            {

                Complete.enabled = true;
                Failed.enabled = NonActive.enabled = Active.enabled = false;
            }
            else
            {
                if (_stage.Activated)
                {
                    Active.enabled = true;
                    Failed.enabled = Complete.enabled = NonActive.enabled = false;
                }
                else
                {

                    NonActive.enabled = true;
                    Failed.enabled = Complete.enabled = Active.enabled = false;
                }
            }
        }

    }

    private void OnStageActivated(QuestStage obj)
    {
        UpdateBacks();
        UpdateText();
    }

    private void UpdateText()
    {
        if (_stage.Failed)
        {
            Field.text = Namings.Tag("Failed");
        }
        else if (_stage.IsComplete || _stage.Activated)
        {
            Field.text = _stage.GetDesc();
        }
        else
        {
            Field.text = Namings.Tag("Locked");
        }
    }

    public void OntextChange(QuestStage obj)
    {
        UpdateText();
    }
    public void OnClick()
    {
        if (_stage.Activated && !_stage.IsComplete)
        {
            if (_stage.CloseWindowOnClick)
            {
                _closeWindowCallback();
            }

            _stage.OnClick();
        }

    }

    private void OnComplete(QuestStage obj)
    {
        Field.text = $"{_stage.GetDesc()} ({Namings.Tag("CompleteNodata")})";
        UpdateBacks();
    }

    public void ClearAll()
    {
        

    }
}
