using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXTrailSeries : MonoBehaviour
{
    public List<FXTrailData> Datas;
    public Transform target;
    private bool _useGO;
    private Vector3 _posTarget;

    private float _nextActivationPeriod;
    public float ActivationPeriod;
    public float FlyPeriod;
    //    public int ActiveElements = 3;

    public void SetTarget(Vector3 pos2)
    {
        _useGO = false;
        _posTarget = pos2;
    }

    void Awake()
    {
        _useGO = (target != null);
//        if (ActiveElements > Datas.Count)
//        {
//            ActiveElements = Datas.Count;
//        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 trg = _useGO ? target.position : _posTarget;
        foreach (var fxTrailData in Datas)
        {
            fxTrailData.ManualUpdate(transform.position, trg);
            if (!fxTrailData.IsActive && _nextActivationPeriod < Time.time)
            {
                fxTrailData.Activate(FlyPeriod);
                _nextActivationPeriod = Time.time + ActivationPeriod;
            }
        }
    }

}
