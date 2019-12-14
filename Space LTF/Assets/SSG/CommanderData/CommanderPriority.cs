using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum ESideAttack
{
    Straight,
    Flangs
}

public enum EBaseDefence
{
    No,
    Yes,
}

public enum ECommanderPriority1
{
    Any,
    MinShield,
    MinHealth,  
    MaxShield,
    MaxHealth,
    Light,
    Mid,
    Heavy,
} 






public class CommanderPriority
{
    private Commander _commander;
    public ESideAttack SideAttack;
    public EBaseDefence BaseDefence;
    public ECommanderPriority1 CommanderPriority1;

    public CommanderPriority(Commander commander)
    {
        _commander = commander;
    }

    public void ChangeTo(ESideAttack SideAttack)
    {
        this.SideAttack = SideAttack;
        foreach (var shipsValue in _commander.Ships.Values)
        {
            shipsValue.DesicionData.ChangePriority(this.SideAttack);
        }
    } 

    public void ChangeTo(EBaseDefence BaseDefence)
    {
        this.BaseDefence = BaseDefence;
        foreach (var shipsValue in _commander.Ships.Values)
        {
            shipsValue.DesicionData.ChangePriority(this.BaseDefence);
        }
    } 

    public void ChangeTo(ECommanderPriority1 CommanderPriority1)
    {
        this.CommanderPriority1 = CommanderPriority1;
        foreach (var shipsValue in _commander.Ships.Values)
        {
            shipsValue.DesicionData.ChangePriority(this.CommanderPriority1);
        }
    } 



}

