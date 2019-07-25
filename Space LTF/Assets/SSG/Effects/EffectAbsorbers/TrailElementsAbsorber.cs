using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TrailElementsAbsorber :BaseEffectAbsorber
{
    public FXTrailSeries FXTrailSeries;

    public override void Play(Vector3 pos1, Vector3 pos2)
    {
        transform.position = pos1;
        FXTrailSeries.SetTarget(pos2);
        base.Play(pos1, pos2);
        //        FXTrailSeries.enableEmission = true;
    }

    public override void UpdatePositions(Vector3 pos1, Vector3 pos2)
    {
        transform.position = pos1;
        FXTrailSeries.SetTarget(pos2);
    }

    public override void Play()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        base.Play();
        //        FXTrailSeries.enableEmission = true;
    }

    public override void Stop()
    {
        gameObject.SetActive(false);
        base.Stop();
    }

    public override void StopEmmision()
    {
//        FXTrailSeries.enableEmission = false;
    }

    public override void StartEmmision()
    {
//        FXTrailSeries.enableEmission = true;
    }
}

