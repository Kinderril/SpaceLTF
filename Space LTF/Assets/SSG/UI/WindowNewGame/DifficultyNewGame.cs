using System;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class DifficultyNewGame : MonoBehaviour
{
    public int CurDifficulty { get; private set; }
    public ToggleWithTextMeshPro VeryEasy;
    public ToggleWithTextMeshPro Easy;
    public ToggleWithTextMeshPro Normal;
    public ToggleWithTextMeshPro Hard;
    public ToggleWithTextMeshPro Imposilbe;
    private Action _callback;

    public void Init()
    {
        Easy.Toggle.isOn = true;
        CurDifficulty = Library.MIN_GLOBAL_MAP_EASY_BASE_POWER;
        VeryEasy.Field.text = Namings.VeryEasy;
        Easy.Field.text = Namings.Easy;
        Normal.Field.text = Namings.Normal;
        Hard.Field.text = Namings.Hard;
        Imposilbe.Field.text = Namings.Imposilbe;
    }

    public void ONVeryWasyClick()
    {
        CurDifficulty = Library.MAX_GLOBAL_MAP_VERYEASY_BASE_POWER;
        if (_callback != null)
        {
            _callback();
        }
    }
    public void ONEasyClick()
    {
        CurDifficulty = Library.MIN_GLOBAL_MAP_EASY_BASE_POWER;
        if (_callback != null)
        {
            _callback();
        }

    }
    public void ONNormalClick()
    {
        CurDifficulty = Library.MIN_GLOBAL_MAP_NORMAL_BASE_POWER;
        if (_callback != null)
        {
            _callback();
        }
    } 
    public void ONHradClick()
    {

        CurDifficulty = Library.MIN_GLOBAL_MAP_HARD_BASE_POWER;
        if (_callback != null)
        {
            _callback();
        }
    } 
    public void ONImposilbeyClick()
    {

        CurDifficulty = Library.MIN_GLOBAL_MAP_IMPOSIBLE_BASE_POWER;
        if (_callback != null)
        {
            _callback();
        }
    }

    public void InitCallback(Action fieldChange)
    {
        _callback = fieldChange;
    }
}
