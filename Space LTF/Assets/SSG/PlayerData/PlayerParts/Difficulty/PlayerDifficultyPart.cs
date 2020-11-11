using System;
using UnityEngine;
using System.Collections;
using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;

[System.Serializable]
public class PlayerDifficultyPart
{
    private float a;
    private float b;
    private float c;

    private float offset = 1;
    private float coef = 0.35f;
    public PlayerDifficultyPart()
    {

    }
    public virtual void Init(EStartGameDifficulty dataDifficulty)
    {
        var x1 = -5f;
        var y1 = 3f;
        var x2 = 5f;
        var y2 = 0f;

        switch (dataDifficulty)
        {
            case EStartGameDifficulty.VeryEasy:
                y2 = -4;
                break;
            case EStartGameDifficulty.Easy:
                y2 = -2;
                break;
            case EStartGameDifficulty.Normal:
                y2 = -1;
                break;
            case EStartGameDifficulty.Hard:
                y2 = 0;
                break;
            case EStartGameDifficulty.Imposilbe:
                y2 = 1;
                break;
        }

        offset = y2;

        a = y1 - y2;
        b = x2 - x1;
        c = x1 * y2 - x2 * y1;

    }

    public virtual float CalcDelta(float enemyPower, float playerPower)
    {
        var delta = (playerPower - enemyPower + offset) * coef;
        return delta;
//        var delta = enemyPower - playerPower;
//        var yy = (-c - a * delta) / b;
//        return yy;

    }
}
