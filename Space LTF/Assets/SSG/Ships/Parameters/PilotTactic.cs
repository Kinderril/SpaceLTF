using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[System.Serializable]
public class PilotTactic
{
    public ECommanderPriority1 Priority = ECommanderPriority1.Any;
    public ESideAttack SideAttack = ESideAttack.Straight;



    [field: NonSerialized]
    public event Action<ECommanderPriority1> OnPriorityChange;
    [field: NonSerialized]
    public event Action<ESideAttack> OnSideChange;

    public PilotTactic(ECommanderPriority1 priority,
        ESideAttack sideAttack)
    {
        Priority = priority;
        SideAttack = sideAttack;
    }

    public void ChangeTo(ESideAttack sideAttack)
    {
        if (sideAttack != SideAttack)
        {
            SideAttack = sideAttack;
            OnSideChange?.Invoke(SideAttack);
        }
    }  
    public void ChangeTo(ECommanderPriority1 priority1)
    {
        if (priority1 != Priority)
        {
            Priority = priority1;
            OnPriorityChange?.Invoke(Priority);
        }
    }
}

