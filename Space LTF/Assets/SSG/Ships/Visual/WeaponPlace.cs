﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class WeaponPlace : MonoBehaviour
{
    public Transform BulletOut;
    private bool _lastFrame = false;
    private bool setted = false;
    private WeaponInGame weapon;
    private bool IsActivePlace;
    public AudioSource Source;


    public void SetWeapon(WeaponInGame weapon)
    {
        setted = true;
        IsActivePlace = weapon != null;
        if (IsActivePlace)
        {
            this.weapon = weapon;
//           Utils.CopyMaterials(ReloadIndicator,Color.white);
//            _material = ReloadIndicator.material;
            weapon.SetTransform(this);
        }

        if (Source != null)
        {
            Source.maxDistance = 40;
            Source.volume = 0.4f;
        }
//        PlaceVisual.gameObject.SetActive(IsActivePlace);
    }

    void Update()
    {
        if (!IsActivePlace || !setted)
        {
            return;
        }
        var isNowReady = weapon.IsLoaded();
//        if (isNowReady != _lastFrame)
//        {
////            _material.color = isNowReady ? Color.green : Color.red;
//        }
        _lastFrame = isNowReady;
    }
}

