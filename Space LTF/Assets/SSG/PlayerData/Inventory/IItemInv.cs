using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum ItemType
{
    weapon,
    modul,
    spell
}

public interface IItemInv
{
    ItemType ItemType { get;}
    IInventory CurrentInventory { get; set; }
    int CostValue { get; }
    int RequireLevel { get; }
    string GetInfo();
    string WideInfo();
}

