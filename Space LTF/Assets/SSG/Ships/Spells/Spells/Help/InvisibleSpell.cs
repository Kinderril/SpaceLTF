//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//
//
//[System.Serializable]
//public class InvisibleSpell : BaseSpellModulInv
//{
//    private const float rad = 1f;
//    [NonSerialized]
//    private SpellZoneVisualCircle ObjectToShow;
//
//    public InvisibleSpell(int costCount, int costTime)
//        : base(SpellType.invisibility, costCount, costTime)
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
//    public override bool TryCast(CommanderCoinController coinController, Vector3 pos)
//    {
//        if (base.TryCast(coinController, pos))
//        {
//            float sDist;
//            var closest = BattleController.Instance.ClosestShipToPos(pos,ShipBase.TeamIndex,out sDist);
//            if (closest != null)
//            {
//                if (sDist < rad*rad)
//                {
//                    return false;
//                }
//            }
//        }
//        return false;
//    }
//
//    protected override void CastAction(Vector3 pos)
//    {
//        var c1 = BattleController.Instance.GetAllShipsInRadius(pos, TeamIndex.green, rad);
//        var c2 = BattleController.Instance.GetAllShipsInRadius(pos, TeamIndex.red, rad);
//        foreach (var shipBase in c1)
//        {
//            ActionShip(shipBase);
//        }
//        foreach (var shipBase in c2)
//        {
//            ActionShip(shipBase);
//        }
//    }
//
//    public override void EndCast()
//    {
//        ObjectToShow.gameObject.SetActive(false);
//    }
//
//    private void ActionShip(ShipBase shipBase)
//    {
//        int invinsablePeriod = 5;
//        shipBase.VisibilityData.SetInvisible(invinsablePeriod);
////        shipBase.ShipParameters.Damage(20, 0);
//
//    }
//}
//
