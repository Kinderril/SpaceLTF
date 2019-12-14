using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class TryApplyToShip : UIElementWithTooltip
{
    protected ShipBase _ship;
    public Image CooldownImage;
    private float _secCooldown;
    private float _curReadyTime;

    public bool IsReady => _curReadyTime < Time.time;

    public void Init(ShipBase ship,float secCooldown )
    {
        _ship = ship;
        _secCooldown = secCooldown;
        CooldownImage.gameObject.SetActive(false);
    }

    protected void StartCooldown()
    {
        _curReadyTime = Time.time + _secCooldown;
        CooldownImage.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!IsReady)
        {
            CooldownImage.fillAmount = (_curReadyTime - Time.time) / _secCooldown;
        }
        else
        {
            if (!CooldownImage.gameObject.activeSelf)
            {
                CooldownImage.gameObject.SetActive(false);
            }
        }
    }

    protected override void OnPointEnterSub()
    {
        _ship.SelectedObject.SetActive(true);
        base.OnPointEnterSub();

    }

    protected override void OnPointExitSub()
    {
        _ship.SelectedObject.SetActive(false);
        base.OnPointExitSub();

    }
}
