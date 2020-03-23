using System;
using System.Collections.Generic;
using UnityEngine;

public enum EPilotTricks
{
    turn,     //Дрифт поворот
    twist,    //резкое смещение вбок
    loop,     //Мертвая петля для уворота
    frontStrike,     //таран
}

public enum PilotRank
{
    // Cadet = 0,
    Private = 0,
    Lieutenant = 1,
    Captain = 2,
    Major = 3,
    // Colonel = 5,
}
[System.Serializable]
public class TotalStats
{
    public PilotRank CurRank;
    public int Kills { get; private set; }
    public int Exp { get; private set; }
    public int UpgradePoints { get; private set; }

    private HashSet<EPilotTricks> _tricks = new HashSet<EPilotTricks>()
    {
        EPilotTricks.turn
    };
    private HashSet<EPilotTricks> _unLearnedtricks = new HashSet<EPilotTricks>()
    {
        EPilotTricks.loop,   EPilotTricks.twist,  EPilotTricks.frontStrike
    };

    [field: NonSerialized] public event Action<PilotRank> OnRankChange;
    [field: NonSerialized] public event Action<EPilotTricks> OnTrickLearned;

    public TotalStats()
    {
        CurRank = PilotRank.Private;
        if (DebugParamsController.AllTricks)
        {
            _tricks.Add(EPilotTricks.twist);
            _tricks.Add(EPilotTricks.turn);
            _tricks.Add(EPilotTricks.frontStrike);
            _tricks.Add(EPilotTricks.loop);
        }
    }
    public HashSet<EPilotTricks> GetTriks()
    {
        return _tricks;
    }

    public void AddExp(int getTotalExp)
    {
        Exp += getTotalExp;
        if (CurRank == PilotRank.Major)
        {
            return;
        }
        var nextRankExp = Library.PilotRankExp[CurRank];
        if (Exp > nextRankExp)
        {
            var nextRankInt = 1 + (int)CurRank;
            PilotRank nextRank = (PilotRank)nextRankInt;
            Debug.Log($"rank update : Kills {Kills}.  rank:{nextRank.ToString()}");
            if (nextRank != CurRank)
            {
                Exp -= nextRankExp;
                CurRank = nextRank;
                UpgradePoints++;
                OnRankChange?.Invoke(CurRank);
            }
        }
    }

    public void LearnTrick(EPilotTricks trick)
    {
        if (UpgradePoints > 0)
        {
            UpgradePoints--;
            _unLearnedtricks.Remove(trick);
            _tricks.Add(trick);
            OnTrickLearned?.Invoke(trick);
        }
    }

    public void AddKills(int kills)
    {
        Kills += kills;
    }

    public void AddHeathDamage(float healthDamage)
    {

    }

    public void AddShieldDamage(float shieldhDamage)
    {


    }

}
