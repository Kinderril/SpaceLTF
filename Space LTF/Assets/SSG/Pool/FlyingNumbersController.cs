using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class FlyingNumbersController 
{

    private List<FlyingNumberWithDependencesHoldin> allList = new List<FlyingNumberWithDependencesHoldin>();
    private Transform _flyingInfosContainer;

    public void Init(Transform flyingInfosContainer)
    {
        _flyingInfosContainer = flyingInfosContainer;
        allList.Clear();
    }

    public void AddShip(ShipBase ship)
    {
        var element = FlyingNumberWithDependencesHoldin.Create(_flyingInfosContainer);
        element.LinkWithShip(ship,false);
        allList.Add(element);
        var element2 = FlyingNumberWithDependencesHoldin.Create(_flyingInfosContainer);
        element2.LinkWithShip(ship,true);
        allList.Add(element);
    }
    

    public void Dispose()
    {
        foreach (var holdin in allList)
        {
            holdin.Dispose();
        }
        allList.Clear();
    }

    public void RemoveShip(ShipBase ship)
    {
        
    }
}

