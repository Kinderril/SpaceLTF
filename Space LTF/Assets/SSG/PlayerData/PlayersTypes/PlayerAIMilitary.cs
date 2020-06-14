using System.Collections.Generic;


[System.Serializable]
public class PlayerAIMilitary : PlayerAI
{
    private float _moneyCoef;
    public PlayerAIMilitary(string name,float moneyCoef)
        : base(name)
    {
        _moneyCoef = moneyCoef;
    }

    public override LastReward GetReward(Player winner)
    {
        MainController.Instance.Statistics.AddOpenPoints(2);
        var reward = new LastReward();
        if (_moneyCoef > 0f)
        {
            var power = Army.GetPower();
            int moneyToReward = (int) (_moneyCoef * power * Library.BATTLE_REWARD_WIN_MONEY_COEF *
                                       winner.SafeLinks.CreditsCoef);
            reward.Money = moneyToReward;
        }

        return reward;
    }
}

