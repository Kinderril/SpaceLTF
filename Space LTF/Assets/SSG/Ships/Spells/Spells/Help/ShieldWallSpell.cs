//
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//
//
//[System.Serializable]
//public class ShieldWallSpell : BaseSpellModulInv 
//{
//    public const float WALL_PERIOD = 15f;
//    private const float rad = 9f;
//    [NonSerialized]
//    private SpellZoneVisualCircle ObjectToShow;
//
//    public ShieldWallSpell(int costCount, int costTime)
//        : base(SpellType.spaceWall, costCount, costTime)
//    {
//
//    }
//
//    public override void UpdateCast(Vector3 pos)
//    {
//        ObjectToShow.transform.position = pos;
//    }
//
//    public override void StartCast()
//    {
//        if (ObjectToShow == null)
//        {
//            ObjectToShow = DataBaseController.GetItem(DataBaseController.Instance.SpellDataBase.SpellZoneCircle);
//            ObjectToShow.transform.SetParent(BattleController.Instance.OneBattleContainer);
//            ObjectToShow.SetSize(rad);
//        }
//        ObjectToShow.gameObject.SetActive(true);
//    }
//    
//
//    protected override void CastAction(Vector3 pos)
//    {
//        var obj = DataBaseController.GetItem(DataBaseController.Instance.SpellDataBase.WallCatcher);
////        BattleController.Instance.
//        obj.transform.position = pos;
//        obj.Init(rad);
//    }
//
//    public override void EndCast()
//    {
//        ObjectToShow.gameObject.SetActive(false);
//    }
//    
//}
//
