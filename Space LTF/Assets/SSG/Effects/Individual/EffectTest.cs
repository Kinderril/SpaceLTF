using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class EffectTest : MonoBehaviour
{
    public ParticleSystem Particles;
    public float spd;

    void Update()
    {
        var m = new ParticleSystem.MinMaxCurve(spd * 0.8f, spd * 1.2f);
        var main = Particles.main;
        main.startSpeed = m;
    }
}

