//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//
//
//[System.Serializable]
//public class AllToBaseSpell : BaseSpellModulInv
//{
//    private const float rad = 1.5f;
//    [NonSerialized]
//    private SpellZoneVisualCircle ObjectToShow;
//
//    public AllToBaseSpell(int costCount, int costTime)
//        : base(SpellType.allToBase, costCount, costTime)
//    {
//
//    }
////    public override string Name
////    {
////        get { return "All to base"; }
////    }
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
//            ObjectToShow.SetSize(rad);
//        }
//        ObjectToShow.gameObject.SetActive(true);
//    }
//
//    protected override void CastAction(Vector3 pos)
//    {
//        var allShips = ShipBase.Commander.Ships;
//        foreach (var shipBase in allShips.Values)
//        {
//            if (shipBase.ShipParameters.StartParams.ShipType != ShipType.Base)
//            {
//                ActionShip(shipBase, pos);
//            }
//        }
//
//    }
//
//    public override void EndCast()
//    {
//        ObjectToShow.gameObject.SetActive(false);
//    }
//
//    private void ActionShip(ShipBase shipBase,Vector3 closePos)
//    {
//        shipBase.Position = shipBase.Commander.StartMyPosition.Center;
//    }
//}
//
