using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Serializable]
public  struct WeaponUpgradeData
{
    public Action UpgradeActiob;
    public string Desc;
    public string ShortDesc;

    public WeaponUpgradeData(Action action, string desc,string shortDesc)
    {
        UpgradeActiob = action;
        Desc = desc;
        ShortDesc = shortDesc;
    }
}

