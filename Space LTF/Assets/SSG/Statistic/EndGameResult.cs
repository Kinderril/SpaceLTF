using System;

[Serializable]
public class EndGameResult
{
    public float Difficulty;
    public ShipConfig Config;
    public int MapSize;
    public DateTime Date;
    public float FinalArmyPower;
    public bool Win;
    public int RecievedOpendPoints;

    public EndGameResult(
        bool win,
        float Difficulty,
        ShipConfig Config,
        int MapSize,
        DateTime Date,
        float FinalArmyPower,
        int recievedPoints
    )
    {
        RecievedOpendPoints = recievedPoints;
        Win = win;
        this.Difficulty = Difficulty;
        this.Config = Config;
        this.MapSize = MapSize;
        this.Date = Date;
        this.FinalArmyPower = FinalArmyPower;
    }
}