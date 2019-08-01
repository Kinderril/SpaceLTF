using JetBrains.Annotations;
using System;
using TMPro;
using UnityEngine;


public class CellIinfoObjectUI : MonoBehaviour
{
    public GameObject MovingObject;
    public TextMeshProUGUI InfoField;
    private bool haveObj = false;
    private bool _disabled = true;
    private GlobalMapCellObject curObj;
    public bool IsDisabled {
        get { return _disabled; }
        }

    public void Init([CanBeNull]GlobalMapCellObject obj)
    {
        haveObj = obj != null;
        if (_disabled != haveObj || _disabled)
        {
            curObj = obj;
            _disabled = !haveObj;
            MovingObject.gameObject.SetActive(haveObj);
            if (curObj!=null)
                Info(curObj.Cell);
//            Debug.Log("MovingObject:" + haveObj + "   " + _disabled);
     
        }

        if (obj != curObj)
        {
            curObj = obj;
            if (curObj != null)
            {
                _disabled = false;
                Info(curObj.Cell);
            }
        }
    }

    private void Info(GlobalMapCell cell)
    {
        if (cell != null)
        {
            string txt;
            string ending;
            if (cell.IsDestroyed)
            {
                ending = "\n (" + Namings.Destroyed + ")";
            }
            else
            if (cell.Completed)
            {
                ending = "\n (" + Namings.Completed + ")";
            }
            else
            {
                ending = "";
            }

            
            if (!cell.IsDestroyed)
            {
                var coreCell = cell is StartGlobalCell || cell is EndGlobalCell;
                var isArmy = cell as ArmyGlobalMapCell;
                var isEvent = cell is EventGlobalMapCell;
                //                bool shallShow = false;
                string desc;
                if (cell.Completed)
                {
                    desc = cell.Desc();
//                    shallShow = true;
                }
                else if (!coreCell && isArmy != null)
                {
//                    shallShow = false;
                    if (cell.InfoOpen || cell.IsScouted)
                    {
//                        shallShow = true;
                        if (MainController.Instance.MainPlayer.Parameters.Scouts.Level > 2)
                        {
                            desc = Namings.ShipConfig(isArmy.GetConfig());
                        }
                        else
                        {
                            desc = Namings.Fleet;
                        }
#if UNITY_EDITOR
                        desc += $"({isArmy.Power})";
#endif
                        if (cell.IsScouted)
                        {
                            desc += String.Format("({0})", Namings.ShipConfig(isArmy.GetConfig()));
                        }
                    }
                    else
                    {
                        desc = Namings.Unknown;
                    }
                }
                else if (isEvent)
                {
                    if (cell.IsScouted)
                    {
                        desc = cell.Desc();
                    }
                    else
                    {
                        desc = Namings.Unknown;
                    }
                }
                else
                {
                    if (coreCell || cell.IsScouted)
                    {
                        desc = cell.Desc();
                        //                        shallShow = coreCell.HaveInfo;
                    }
                    else
                    {
                        desc = Namings.Unknown;
                    }
                }
                txt = desc + ending;
//                txt = shallShow ? desc + ending : Namings.Unknown;
            }
            else
            {
                var desc = cell.Desc();
                txt = desc + ending;
            }

            //            Debug.Log("InfoField:" + haveObj + "   " + _disabled);
#if UNITY_EDITOR       
            InfoField.text = txt + $"{cell.indX}.{cell.indZ}";
            if (cell is CoreGlobalMapCell)
            {
                InfoField.text = $"{InfoField.text} [CORE!]";
            }
#else    
            InfoField.text = txt;
#endif
        }
    } 

    void Update()
    {
        if (haveObj)
        {
            MovingObject.transform.position = CamerasController.Instance.GlobalMapCamera.WorldToScreenPoint(curObj.transform.position) + Vector3.up * 20;
        }
    }

    public void Disable()
    {
//        Debug.Log("Disable");
        _disabled = true;
        MovingObject.gameObject.SetActive(false);
    }
}

