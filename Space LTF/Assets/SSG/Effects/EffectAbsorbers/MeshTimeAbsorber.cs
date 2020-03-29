using UnityEngine;

public class MeshTimeAbsorber : BaseEffectAbsorber
{
    public GameObject obj;
    public MeshRenderer Renderer;
    public TrailRenderer TrailRenderer;
    public float Delay = 0.5f;

    private Material Material1;
    private Material Material2;
    private float _endTime;
    private bool _doPeriod;
    private bool _onStart;
    private Color _color;
    private string _materialName1;
    private string _materialName2;

    private bool haveRenderer;
    private bool haveTrailRenderer;

    public override void Init()
    {
        haveRenderer = Renderer != null;
        haveTrailRenderer = TrailRenderer != null;
        if (haveRenderer)
            Material1 = Utils.CopyMaterial(Renderer.material);
        if (haveTrailRenderer)
            Material2 = Utils.CopyMaterial(TrailRenderer.material);
        if (haveRenderer)
            _materialName1 = NcCurveAnimation.Ng_GetMaterialColorName(Material1);
        if (haveTrailRenderer)
            _materialName2 = NcCurveAnimation.Ng_GetMaterialColorName(Material2);

        base.Init();
    }

    public override void Play()
    {
        _endTime = Time.time + Delay;
        obj.SetActive(true);
        _doPeriod = true;
        _onStart = true;
        base.Play();
    }

    protected override void UpdateManual()
    {
        if (!_doPeriod) return;
        var delta = _endTime - Time.time;
        if (delta < 0)
        {
            _doPeriod = false;
            _color.a = _onStart ? 1 : 0;
            if (!_onStart) Stop();
        }
        else
        {
            var percent = delta / Delay;
            percent = _onStart ? percent : 1 - percent;
            _color.a = percent;
        }

        if (haveRenderer)
            Material1.SetColor(_materialName1, _color);
        if (haveTrailRenderer)
            Material2.SetColor(_materialName2, _color);
        base.UpdateManual();
    }

    public override void StopEmmision()
    {
        _endTime = Time.time + Delay;
        _doPeriod = true;
        _onStart = false;
    }

    public override void Stop()
    {
        obj.SetActive(false);
        base.Stop();
    }
}