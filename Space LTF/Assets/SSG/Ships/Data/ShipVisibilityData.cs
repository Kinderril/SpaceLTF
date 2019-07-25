using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;


public class ShipVisibilityData : ShipData
{
    private bool _visible;
    private bool _inClouds;
    private bool _invisibleCast;
    private float _endInvisibleCast;
    public event Action<ShipBase, bool> OnVisibilityChange;

    public bool Visible
    {
        get { return _visible; }
        private set
        {
            var nv = value;
            bool act = (nv != _visible);
            _visible = nv;
            if (act && OnVisibilityChange != null)
            {
                OnVisibilityChange(_owner, _visible);
            }
        }
    }

    public ShipVisibilityData([NotNull] ShipBase owner)
        : base(owner)
    {

    }
    public void SetInClouds(bool inClouds)
    {
        _inClouds = inClouds;
        Visible = !(_inClouds || _invisibleCast);
    }

    public void SetInsivible(bool invisibleCast)
    {
        _invisibleCast = invisibleCast;
        Visible = !(_inClouds || _invisibleCast);
    }

    public void Update()
    {
        if (_invisibleCast)
        {
            if (_endInvisibleCast < Time.time)
            {
                SetInsivible(false);
            }
        }
    }

    public void SetInvisible(float period)
    {
        var effect2 = DataBaseController.Instance.SpellDataBase.InvisibleEffect;
        EffectController.Instance.Create(effect2, _owner.transform, period + 0.4f);
        SetInsivible(true);
        _endInvisibleCast = Time.time + period;
    }

    public void Dispose()
    {
        OnVisibilityChange = null;
    }
}

