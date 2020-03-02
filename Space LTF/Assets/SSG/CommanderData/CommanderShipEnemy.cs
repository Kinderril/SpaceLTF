using UnityEngine;

public class CommanderShipEnemy
{
    public GameObject TargetPriority;
    public GameObject BaitPriority;
    public bool IsPriority { get; private set; }

    public CommanderShipEnemy(GameObject TargetPriority, GameObject baitPriority)
    {
        this.TargetPriority = TargetPriority;
        this.BaitPriority = baitPriority;
    }

    public void SetPriority(bool val, bool isBait)
    {
        IsPriority = val;
        if (isBait)
        {
            BaitPriority.SetActive(val);
        }
        else
        {
            TargetPriority.SetActive(val);
        }
    }
}