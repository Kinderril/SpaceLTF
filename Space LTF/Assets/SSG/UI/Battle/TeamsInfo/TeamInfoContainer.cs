using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class TeamInfoContainer : MonoBehaviour
{
    public Transform Layout;
    private Commander _commander;
//    private SideShipInfo pref;
    private Dictionary<ShipBase, SideShipInfo> _infosGreen  = new Dictionary<ShipBase, SideShipInfo>();
    private Dictionary<ShipBase, SideShipMiniInfo> _infosRight  = new Dictionary<ShipBase, SideShipMiniInfo>();
    private Action<ShipBase> _shipSelectedAction;
    private RectTransform _layoutRect;
//    private int _openCount;
    private int max_open = 2;
    private SideShipInfo _lastToggle = null;

    public void Init(Commander commander,Action<ShipBase> shipSelectedAction)
    {
        _infosGreen.Clear();
        _layoutRect = Layout.GetComponent<RectTransform>();
        Layout.ClearTransform();
        _shipSelectedAction = shipSelectedAction;
        _commander = commander;

        foreach (var shipBase in commander.Ships)
        {
            OnShipAdd(shipBase.Value);
        }
        commander.OnShipAdd += OnShipAdd;
        commander.OnShipDestroy += OnShipDestroy;
    }

    private void OnShipDestroy(ShipBase obj)
    {
        SideShipInfo info;
        if (_infosGreen.TryGetValue(obj, out info))
        {
            info.Dispose();
            GameObject.Destroy(info.gameObject);
        }
        SideShipMiniInfo info2;
        if (_infosRight.TryGetValue(obj, out info2))
        {
            info2.Dispose();
            GameObject.Destroy(info2.gameObject);
        }
    }

    private void OnShipAdd(ShipBase obj)
    {
        if (_commander.TeamIndex == TeamIndex.green)
        {
            var shallOpen = 1 == PlayerPrefs.GetInt(String.Format(SideShipInfo.PREFS_KEY, obj.Id),1);
            if (obj.ShipParameters.StartParams.ShipType == ShipType.Base)
            {
                shallOpen = false;
            }

            var pref = DataBaseController.Instance.DataStructPrefabs.SideShipInfoLeft;
            var s1 = DataBaseController.GetItem(pref);
            s1.Init(obj, _shipSelectedAction, ToggleChanges, shallOpen);
            if (shallOpen)
            {
                _lastToggle = s1;
            }
//            _openCount++;
//            Debug.LogError($"_openCount:{_openCount}");
            _infosGreen.Add(obj, s1);
            s1.transform.SetParent(Layout, false);
        }
        else
        {

            var pref = DataBaseController.Instance.DataStructPrefabs.SideShipInfoRight;
            var s1 = DataBaseController.GetItem(pref);
            s1.Init(obj);
            _infosRight.Add(obj, s1);
            s1.transform.SetParent(Layout, false);
        }
//        var pref = _commander.TeamIndex == TeamIndex.green ? DataBaseController.Instance.DataStructPrefabs.SideShipInfoLeft :
//            DataBaseController.Instance.DataStructPrefabs.SideShipInfoRight;
//        var s1 = DataBaseController.GetItem(pref);
//        s1.Init(obj, _shipSelectedAction,ToggleChanges);
//        _infos.Add(obj, s1);
//        s1.transform.SetParent(Layout, false);
    }

    private void ToggleChanges(SideShipInfo changedInfo)
    {
        if (changedInfo.IsOpen)
        {
            _lastToggle = changedInfo;
//            Debug.LogError($" _lastToggle.Id:{ _lastToggle.Id}");
           
        }
//        if (changedInfo.ToggleOpen.isOn)
//        {
//            _openCount++;
//            Debug.LogError($"_openCount:{_openCount}  ++:");
//        }
//        else
//        {
//            _openCount--;
//            Debug.LogError($"_openCount:{_openCount}  --:");
//        }

        var count = _infosGreen.Values.Sum(x => x.IsOpen?1:0);
        if (count > max_open)
        {
            if (_lastToggle != null && _lastToggle.IsOpen && _lastToggle != changedInfo)
            {
                _lastToggle.ToggleViaCode();
            }
            else
            {
                _lastToggle = _infosGreen.Values.FirstOrDefault(x => x.IsOpen && x != changedInfo);
                if (_lastToggle != null)
                {
                    _lastToggle.ToggleViaCode();
                }
                else
                {
                    Debug.LogError("can't find elemуtn to close");
                }
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutRect);
    }

    public void Dispose()
    {
        _infosGreen.Clear();
        Layout.ClearTransform();
        _commander.OnShipAdd -= OnShipAdd;
        _commander.OnShipDestroy -= OnShipDestroy;
    }
}

