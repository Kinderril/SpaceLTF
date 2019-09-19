using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class PlayerQuestData
{
    public int mainElementsFound = 0;
    public int MaxMainElements = 4;
    public FinalBattleData LastBattleData { get; private set; }

    [field: NonSerialized]
    public event Action OnElementFound;

    public PlayerQuestData(int targetElements)
    {
        MaxMainElements = targetElements;
    }

//    public bool CheckIfOver()
//    {
//        if (Completed())
//        {
//            MainController.Instance.BattleData.EndGameWin();
//            return true;
//        }
//        return false;
//    }

    public void ComeToLastPoint()
    {
        LastBattleData = new FinalBattleData();
//        LastBattleData.Init();
    }

    public void AddElement()
    {
        mainElementsFound++;
        if (mainElementsFound > MaxMainElements)
        {
            mainElementsFound = MaxMainElements;
            Debug.LogError("HOW CAN IT HAPPED");
        }
        if (OnElementFound != null)
        {
            OnElementFound();
        }
    }

    public bool Completed()
    {
        return mainElementsFound >= MaxMainElements;
    }
}
