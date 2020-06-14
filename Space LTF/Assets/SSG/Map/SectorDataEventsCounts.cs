using UnityEngine;
using System.Collections;

public class SectorDataEventsCounts 
{
    public int shopsCount { get; private set; }
    public int repairCount { get; private set; }
    public int bigEventsCount { get; private set; }
    public int minorEventsCount { get; private set; }
    public int armiesCount { get; private set; }


    public static SectorDataEventsCounts Exproler(int remainCount,int sectorSize)
    {
        var counts = new SectorDataEventsCounts();
        switch (sectorSize)
        {
            case 4:
                if (MyExtensions.IsTrueEqual())
                {
                    counts.shopsCount = 0;
                    counts.repairCount = 1;
                }
                else
                {
                    counts.shopsCount = 1;
                    counts.repairCount = 0;
                }

                counts.minorEventsCount = 1;
                counts.bigEventsCount = 1;
                break;   
            case 5:
                counts.shopsCount = 1;
                counts.repairCount = 1;
                counts.minorEventsCount = 2;
                counts.bigEventsCount = 1;

                break;   
            case 6:
            default:
                counts.shopsCount = 1;
                counts.repairCount = 1;
                counts.minorEventsCount = 2;
                counts.bigEventsCount = 2;

                break;
        }

        counts.armiesCount = remainCount - counts.shopsCount - counts.repairCount - counts.minorEventsCount -
                             counts.bigEventsCount;
        return counts;
    }

    public static SectorDataEventsCounts Standart(int remainCount, int sectorSize)
    {
        var counts = new SectorDataEventsCounts();
        if (remainCount < 7)
        {
            if (MyExtensions.IsTrueEqual())
            {
                counts.shopsCount = 1;
                counts.repairCount = 0;
            }
            else
            {
                counts.shopsCount = 0;
                counts.repairCount = 1;
            }

        }
        else
        {
            counts.shopsCount = remainCount > 12 ? MyExtensions.Random(1, 2) : 1;
            counts.repairCount = 1;
        }
        remainCount = remainCount - counts.shopsCount - counts.repairCount;
        counts.armiesCount = (int)(remainCount * MyExtensions.Random(0.6f, 0.8f));
        var allEventsCount = remainCount - counts.armiesCount;
        counts.bigEventsCount = Mathf.Clamp((int)(allEventsCount * 0.3f), 1, 4);
        counts.minorEventsCount = allEventsCount - counts.bigEventsCount;

        return counts;
    }
     
}
