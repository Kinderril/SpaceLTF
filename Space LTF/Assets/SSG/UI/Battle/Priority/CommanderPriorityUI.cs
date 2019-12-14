using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class CommanderPriorityUI : MonoBehaviour
{
    private Commander _commander;
    public void Init(Commander commander)
    {
        _commander = commander;
    }

    //Base defence
    public void ClickDefenceBase()
    {
        _commander.Priority.ChangeTo(EBaseDefence.Yes);
    }  
    public void ClickStopDefenceBase()
    {
        _commander.Priority.ChangeTo(EBaseDefence.No);
    }
    //Straight|Flangs
    public void ClickStraight()
    {
        _commander.Priority.ChangeTo(ESideAttack.Straight);
    } 
    public void ClickFlangs()
    {
        _commander.Priority.ChangeTo(ESideAttack.Flangs);
    }

    //Base CommanderPriority2
    public void ClickECommanderPriority1MinShield()
    {
        _commander.Priority.ChangeTo(ECommanderPriority1.MinShield);
    }  
    public void ClickECommanderPriority1MinHealth()
    {
        _commander.Priority.ChangeTo(ECommanderPriority1.MinHealth);
    }  
    public void ClickECommanderPriority1MaxShield()
    {
        _commander.Priority.ChangeTo(ECommanderPriority1.MaxShield);
    }  
    public void ClickECommanderPriority1MaxHealth()
    {
        _commander.Priority.ChangeTo(ECommanderPriority1.MaxHealth);
    } 
    public void ClickECommanderPriority2Any()
    {
        _commander.Priority.ChangeTo(ECommanderPriority1.Any);
    }    
    public void ClickECommanderPriority2Light()
    {
        _commander.Priority.ChangeTo(ECommanderPriority1.Light);
    }    
    public void ClickECommanderPriority2Mid()
    {
        _commander.Priority.ChangeTo(ECommanderPriority1.Mid);
    }    
    public void ClickECommanderPriority2Heavy()
    {
        _commander.Priority.ChangeTo(ECommanderPriority1.Heavy);
    }


}

