using System;
using UnityEngine;

public enum EStartGameDifficulty
{
    VeryEasy,
    Easy,
    Normal,
    Hard,
    Imposilbe,
}

public class DifficultyNewGame : MonoBehaviour
{
    public EStartGameDifficulty CurDifficulty { get; private set; }
    public ToggleWithTextMeshPro VeryEasy;
    public ToggleWithTextMeshPro Easy;
    public ToggleWithTextMeshPro Normal;
    public ToggleWithTextMeshPro Hard;
    public ToggleWithTextMeshPro Imposilbe;
    private Action _callback;

    public void Init()
    {
        Easy.Toggle.isOn = true;
        CurDifficulty = EStartGameDifficulty.Easy;
        VeryEasy.Field.text = Namings.Tag("VeryEasy");
        Easy.Field.text = Namings.Tag("Easy");
        Normal.Field.text = Namings.Tag("Normal");
        Hard.Field.text = Namings.Tag("Hard");
        Imposilbe.Field.text = Namings.Tag("Imposilbe");
    }

    public void ONVeryWasyClick()
    {
        CurDifficulty = EStartGameDifficulty.VeryEasy;
        if (_callback != null)
        {
            _callback();
        }
    }
    public void ONEasyClick()
    {
        CurDifficulty = EStartGameDifficulty.Easy;
        if (_callback != null)
        {
            _callback();
        }

    }
    public void ONNormalClick()
    {
        CurDifficulty = EStartGameDifficulty.Normal;
        if (_callback != null)
        {
            _callback();
        }
    }
    public void ONHradClick()
    {

        CurDifficulty = EStartGameDifficulty.Hard;
        if (_callback != null)
        {
            _callback();
        }
    }
    public void ONImposilbeyClick()
    {

        CurDifficulty = EStartGameDifficulty.Imposilbe;
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
