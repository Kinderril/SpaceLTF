using System;
using UnityEngine;
using UnityEngine.UI;


public class RetirreButton : MonoBehaviour
{
    public Image CooldownImage;
    private float _secCooldown;
    private float _curReadyTime;
    public bool IsReady => _curReadyTime < Time.time;
    private InGameMainUI _mainUI;
    private bool _canRetire;
    public UIElementWithTooltipCache TooltipCache;

    public void Init(InGameMainUI mainUI, float initDelta, bool canRetire)
    {
        _mainUI = mainUI;
        _canRetire = canRetire;
        _secCooldown = initDelta;
        TooltipCache.Cache = Namings.Tag("Retreat");
        if (_canRetire)
        {
            StartCooldown(_secCooldown);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    protected void StartCooldown(float cd)
    {
        _curReadyTime = Time.time + cd;
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


    public void OnClickRetire()
    {
        if (!IsReady)
        {
            return;
            
        }
        BattleController.Instance.PauseData.Pause();

        void Unpause()
        {
            BattleController.Instance.PauseData.Unpase(1f);
        }

        WindowManager.Instance.ConfirmWindow.Init(() =>
        {
            StartCooldown(2f);
            if (_mainUI.MyCommander.Ships.Count == 1)
            {
                BattleController.Instance.RunAway();
            }
            else
            {
                foreach (var ship in _mainUI.MyCommander.Ships)
                {
                    if (ship.Value.ShipParameters.StartParams.ShipType != ShipType.Base)
                    {
                        ship.Value.RunAwayAction();
                    }
                }
            }

            Unpause();
        }, Unpause,
            Namings.Format(Namings.Tag("DoWantRetire")));
    }
}

