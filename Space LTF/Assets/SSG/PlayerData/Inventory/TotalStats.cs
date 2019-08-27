using System;
using UnityEngine;
using System.Collections;

public enum PilotRank
{
    Cadet = 0,
    Private = 1,
    Lieutenant = 2,
//    Sergeant,
    Captain = 3,
    Major = 4,
    Colonel = 5,
}
[System.Serializable]
public class TotalStats
{
    public PilotRank CurRank;
    public int Kills { get; private set; }

    [field: NonSerialized] public event Action<PilotRank> OnRankChange; 

    public TotalStats()
    {
        CurRank = PilotRank.Cadet;
    }

    public void AddKills(int kills)
    {
        Kills += kills;
        var nextRankI = Kills / Library.RANK_ERIOD;
        PilotRank nextRank = (PilotRank) nextRankI;
        Debug.Log($"rank update : Kills {Kills}.  rank:{nextRank.ToString()}");
        if (nextRank != CurRank)
        {
            CurRank = nextRank;
            if (OnRankChange != null)
            {
                OnRankChange(CurRank);
            }
        }
    }

    public void AddHeathDamage(float healthDamage)
    {
        
    }

    public void AddShieldDamage(float shieldhDamage)
    {
        

    }
}
