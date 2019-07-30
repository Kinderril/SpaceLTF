﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class EngineEffect : MonoBehaviour
{
    private MovingObject _mobj;
    private float _lastFrameSpeed;
    public ParticleSystem[] Particles;
    private bool _failed;

    public void Init(MovingObject mobj)
    {
        _mobj = mobj;
    }

    void Update()
    {
        if (_failed)
        {
            return;
        }
        var delta = Mathf.Abs(_lastFrameSpeed - _mobj.CurSpeed);
        if (delta > Mathf.Epsilon)
        {
            _lastFrameSpeed = _mobj.CurSpeed/3f;
            var spd = _lastFrameSpeed*1f;
            for (int i = 0; i < Particles.Length; i++)
            {
                try
                {

                    var p = Particles[i];
                    var main = p.main;


                    var m = new ParticleSystem.MinMaxCurve(spd * 0.8f, spd * 1.2f);
                    main.startSpeed = m;
                }
                catch (Exception e)
                {
                    _failed = true;
                }
            }
        }
    }
}

