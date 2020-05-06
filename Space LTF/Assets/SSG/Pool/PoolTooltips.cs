using UnityEngine;
using System.Collections;

public class PoolTooltips
{
    private Tooltip _tooltip;
    private DataBaseController dataBaseController;
    private Transform _canvasContainer;
    public SpellBigTooltip _SpellBigTooltip;
    public WeaponBigTooltip _WeaponBigTooltip;
    public ModulBigTooltip _ModulBigTooltip;
    public ParamItemBigTooltip _ParamItemBigTooltip;
    public void CreateNewTooltip()
    {
        _tooltip = DataBaseController.GetItem(dataBaseController.DataStructPrefabs.TooltipPrefab);
        _SpellBigTooltip = DataBaseController.GetItem(dataBaseController.DataStructPrefabs.SpellBigTooltip);
        _WeaponBigTooltip = DataBaseController.GetItem(dataBaseController.DataStructPrefabs.WeaponBigTooltip);
        _ModulBigTooltip = DataBaseController.GetItem(dataBaseController.DataStructPrefabs.ModulBigTooltip);
        _ParamItemBigTooltip = DataBaseController.GetItem(dataBaseController.DataStructPrefabs.ParamItemBigTooltip);

        SetToolTip(_SpellBigTooltip);
        SetToolTip(_WeaponBigTooltip);
        SetToolTip(_ModulBigTooltip);
        SetToolTip(_ParamItemBigTooltip);
        SetToolTip(_tooltip);
    }

    private void SetToolTip(BaseTooltip element)
    {
        element.gameObject.SetActive(false);
        element.SetBaseParent(_canvasContainer);
    }

    public void Init(DataBaseController dataBaseController1, Transform canvasContainer)
    {
        this.dataBaseController = dataBaseController1;
        this._canvasContainer = canvasContainer;

    }

    public void Prewarm()
    {
        CreateNewTooltip();
    }

    public Tooltip GetToolTip()
    {
        return _tooltip;
    }

    public void CloseTooltip()
    {
        _SpellBigTooltip.Close();
        _WeaponBigTooltip.Close();
        _ModulBigTooltip.Close();
        _ParamItemBigTooltip.Close();
        _tooltip.Close();
    }
}
