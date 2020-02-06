using JetBrains.Annotations;
using TMPro;
using UnityEngine;


public class CellIinfoObjectUI : MonoBehaviour
{
    public GameObject MovingObject;
    public TextMeshProUGUI InfoField;
    private bool haveObj = false;
    private bool _disabled = true;
    private string _curText;
    private GlobalMapCellObject curObj;
    public bool IsDisabled => _disabled;
    private bool _isShort = true;

    public void Init([CanBeNull]GlobalMapCellObject obj)
    {
        _isShort = true;
        haveObj = obj != null;
        if (_disabled != haveObj || _disabled)
        {
            curObj = obj;
            _disabled = !haveObj;
            MovingObject.gameObject.SetActive(haveObj);
            if (curObj != null)
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
            var isWithArmy = cell.CurMovingArmy != null;
            if (isWithArmy)
            {
                var name = cell.CurMovingArmy.ShortDesc();
                _curText = name;
                TextUpdate(true);
                return;
            }
            string ending;
            if (cell.IsDestroyed)
            {
                ending = "\n (" + Namings.Tag("Destroyed") + ")";
            }
            else
            if (cell.Completed)
            {
                ending = "\n (" + Namings.Tag("Completed") + ")";
            }
            else
            {
                ending = "";
            }


            if (!cell.IsDestroyed)
            {
                //                Debug.LogError($"cell.IsScouted.id:{cell.Id}  scouted:{cell.IsScouted}  ");
                var isCore = cell as CoreGlobalMapCell;
                if (isCore != null)
                {
                    _curText = isCore.Desc();
                    TextUpdate(true);
                    return;
                }


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
                            desc = Namings.Tag("Fleet");
                        }
                        // #if UNITY_EDITOR
                        desc += $"({isArmy.PowerDesc()})";
                        // #endif
                        if (cell.IsScouted)
                        {
                            desc += $"({Namings.ShipConfig(isArmy.GetConfig())})";
                            if (isArmy.EventType.HasValue)
                            {
                                var nameEvent = Namings.BattleEvent(isArmy.EventType.Value);
                                desc = $"{desc}\n{Namings.Tag("Event")}:{nameEvent}";
                            }
                        }
                    }
                    else
                    {
                        desc = Namings.Tag("Unknown");
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
                        desc = Namings.Tag("Unknown");
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
                        desc = Namings.Tag("Unknown");
                    }
                }
                _curText = desc + ending;
                //                txt = shallShow ? desc + ending : Namings.Tag("Unknown");
            }
            else
            {
                var desc = cell.Desc();
                _curText = desc + ending;
            }

            TextUpdate(true);
        }
    }

    void Update()
    {
        if (haveObj)
        {
            MovingObject.transform.position = CamerasController.Instance.GlobalMapCamera.WorldToScreenPoint(curObj.transform.position) + Vector3.up * 20;
        }

        TextUpdate(false);
    }

    private void TextUpdate(bool updateAnyway)
    {
        var isShort = (Input.GetKey(KeyCode.LeftControl));
        //        Debug.LogError($"isShort:{isShort}");
        if ((isShort != _isShort || updateAnyway) && curObj != null)
        {
            _isShort = isShort;
            var index = $"{curObj.Cell.indX}.{curObj.Cell.indZ}";
            InfoField.text = !_isShort ? _curText : $"{_curText}\n{index}";
        }
    }

    public void Disable()
    {
        //        Debug.Log("Disable");
        haveObj = false;
        curObj = null;
        _disabled = true;
        MovingObject.gameObject.SetActive(false);
    }
}

