using System.Collections.Generic;
using UnityEngine;

public abstract class SetterElementBattleEvent : BattleFieldEvent
{
    private EBattlefildEventType _eventType;
    private int _minCount;
    private int _maxCount;
    protected List<GameObject> _createdElements = new List<GameObject>();
    private List<GameObject> _prefabsm;
    private float _insideRad;
    protected SetterElementBattleEvent(BattleController battle,
        int minCount,int maxCount, List<GameObject> prefabsm, EBattlefildEventType eventType, float insideRad) : base(battle)
    {
        _insideRad = insideRad;
        _prefabsm = prefabsm;
        _eventType = eventType;
        _minCount = minCount;
        _maxCount = maxCount;
    }

    public override List<Vector3> GetBlockedPosition()
    {
        var list = new List<Vector3>();
        foreach (var element in _createdElements)
        {
            list.Add(element.transform.position);
        }

        return list;
    }

    public override EBattlefildEventType Type => _eventType;
    public override void Init()
    {
        var cnt = MyExtensions.Random(_minCount, _maxCount);
        for (int i = 0; i < cnt; i++)
        {
            var prefab = _prefabsm.RandomElement();
            var element = DataBaseController.GetItem(prefab);
            var center = _battle.Battlefield.CellController.Data.CenterZone;
//            var rad = _battle.Battlefield.CellController.Data.InsideRadius;
//            var center = (min + max) / 2f;
            var isEven = i % 2 == 0;

            var dir1 = new Vector3(0, 0, -1);
            var diifAng = isEven?MyExtensions.Random(-90, -45): MyExtensions.Random(45, 90);
            dir1 = Utils.RotateOnAngUp(dir1, diifAng);
            var pos = center + dir1 * MyExtensions.Random(0.4f,1.1f) * _insideRad;


//            var rndX = isEven?
//                MyExtensions.Random(center.x, center.x + rad) : 
//                MyExtensions.Random(center.x - rad, center.x);
//
//            var rndZ = MyExtensions.Random(center.z - rad, center.z + rad);

            element.transform.position = pos;// new Vector3(rndX,0,rndZ);
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
