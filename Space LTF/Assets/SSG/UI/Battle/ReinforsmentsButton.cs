using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ReinforsmentsButton : MonoBehaviour
{

    private InGameMainUI _mainUI;
    private bool _canRetire;
    public Button Button;
//    public TextMeshProUGUI Field;
    public UIElementWithTooltipCache TooltipCache;

    public void Init(InGameMainUI mainUI)
    {
        _mainUI = mainUI;
        var repdata = MainController.Instance.MainPlayer.ReputationData;
        if (repdata.CanCallReinforsments(out var config))
        {
            _canRetire = true;
        }
        else
        {
            _canRetire = false;
        }
        TooltipCache.Cache = Namings.Tag("ReinforcmentsCall");
        Button.interactable = _canRetire;
    }



    public void OnClickRetire()
    {
        if (_canRetire)
        {
            Button.interactable = false;
            _mainUI.OnClickReinforsment();
            TooltipCache.Cache = Namings.Tag("ReinforcmentsCalled");

        }
    }
}

