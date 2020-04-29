using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GlobalMapLighterUpCells : MonoBehaviour
{

    private List<LightUpObject> _allLightObjects = new List<LightUpObject>();
    private GlobalMapController _controller;
    private float _nextPosibleLightUp = 0f;
    private const float LightUpPeriod = 2f;

    public void Init(GlobalMapController controller)
    {
        _controller = controller;
        var db = DataBaseController.Instance.DataStructPrefabs;
        for (int i = 0; i < 12; i++)
        {
            var item = DataBaseController.GetItem(db.LightUpObject);
            _allLightObjects.Add(item);
            item.IsUsing = false;
            item.gameObject.SetActive(false);
            item.gameObject.transform.SetParent(transform);
        }
    }


    public void LightUpCells(HashSet<GlobalMapCell> cellsToLightUp)
    {
        if (Time.time > _nextPosibleLightUp)
        {
            _nextPosibleLightUp = Time.time + 0.1f + LightUpPeriod;
            foreach (var globalMapCell in cellsToLightUp)
            {
                var mapCell = _controller.GetCellObjectByCell(globalMapCell);
                if (mapCell != null)
                {
                    var notUsing = _allLightObjects.FirstOrDefault(x => !x.IsUsing);
                    if (notUsing != null)
                    {
                        notUsing.UseFor(LightUpPeriod);
                        notUsing.transform.position = mapCell.ModifiedPosition;
                    }
                }
            }
        }

    }

    public void Dispose()
    {
        foreach (var lightObject in _allLightObjects)
        {
            lightObject.IsUsing = false;
            lightObject.gameObject.SetActive(false);
        }
    }

}
