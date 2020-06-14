using System;
using UnityEngine;
using System.Collections;
using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;

[System.Serializable]
public class PlayerDifficultyExprolerPart : PlayerDifficultyPart
{

    public PlayerDifficultyExprolerPart()
    {

    }
    public override void Init(EStartGameDifficulty dataDifficulty)
    {

    }

    public override float CalcDelta(float enemyPower, float playerPower)
    {
        return 0f;

    }
}
