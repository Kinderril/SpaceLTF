using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TacticsButton: MonoBehaviour
{
    private Action<PilotTcatic> OnChooseClickCllback;
    private List<TacticChooseButton> _allBtns = new List<TacticChooseButton>(); 

    void Awake()
    {
        var all = new List<PilotTcatic>()
        {
            PilotTcatic.attack,
//            PilotTcatic.sneakAttack,
            PilotTcatic.attackBase,
//            PilotTcatic.support,
            PilotTcatic.defenceBase,
        };
        var prefab = DataBaseController.Instance.DataStructPrefabs.TacticBtn;
        foreach (var pilotTcatic in all)
        {
            var tc1 = DataBaseController.GetItem(prefab);
            tc1.Init(pilotTcatic,OnBtnClick);
            tc1.transform.SetParent(transform,false);
            _allBtns.Add(tc1);
        }
    }

    private void OnBtnClick(TacticChooseButton btn)
    {
        OnChooseClickCllback(btn.Tcatic);
    }

    public void Init(PilotTcatic currentTatic,Action<PilotTcatic> OnChooseClick)
    {
        OnChooseClickCllback = OnChooseClick;
        foreach (var btn in _allBtns)
        {
            btn.gameObject.SetActive(btn.Tcatic != currentTatic);
        }
    }
}

