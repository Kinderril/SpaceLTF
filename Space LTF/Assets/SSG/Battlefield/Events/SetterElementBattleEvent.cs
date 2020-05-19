using System.Collections.Generic;
using UnityEngine;

public abstract class SetterElementBattleEvent : BattleFieldEvent
{
    private BattlefildEventType _eventType;
    private int _minCount;
    private int _maxCount;
    private List<GameObject> _createdElements = new List<GameObject>();
    private List<GameObject> _prefabsm;
    protected SetterElementBattleEvent(BattleController battle,
        int minCount,int maxCount, List<GameObject> prefabsm, BattlefildEventType eventType) : base(battle)
    {
        _prefabsm = prefabsm;
        _eventType = eventType;
        _minCount = minCount;
        _maxCount = maxCount;
    }

    public override BattlefildEventType Type => _eventType;
    public override void Init()
    {
        var cnt = MyExtensions.Random(_minCount, _maxCount);
        for (int i = 0; i < cnt; i++)
        {
            var prefab = _prefabsm.RandomElement();
            var element = DataBaseController.GetItem(prefab);
            var max = _battle.Battlefield.CellController.Max;
            var min = _battle.Battlefield.CellController.Min;
            var center = (min + max) / 2f;
            var isEven = i % 2 == 0;

            var rndX = isEven?MyExtensions.Random(center.x, max.x): MyExtensions.Random(min.x, center.x);
            var rndZ = MyExtensions.Random(min.z, min.x);

            element.transform.position = new Vector3(rndX,0,rndZ);
            _createdElements.Add(element);
        }
    }

    public override void Dispose()
    {
        foreach (var createdElement in _createdElements)
        {
            GameObject.Destroy(createdElement);
        }
        _createdElements.Clear();
    }
}
