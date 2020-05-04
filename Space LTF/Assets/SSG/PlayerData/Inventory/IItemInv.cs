using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum ItemType
{
    weapon,
    modul,
    spell,
    cocpit,
    engine,
    wings,

}

public interface IItemInv
{
    ItemType ItemType { get;}
    IInventory CurrentInventory { get; set; }
    int CostValue { get; }
    int RequireLevel(int posibleLevel = -1);
    string GetInfo();
    string WideInfo();
}

