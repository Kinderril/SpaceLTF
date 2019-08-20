using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class WeaponPlace : MonoBehaviour
{
    public Transform BulletOut;
    public GameObject PlaceVisual;
    public Renderer ReloadIndicator;
//    private Material _material;
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
           Utils.CopyMaterials(ReloadIndicator,Color.white);
//            _material = ReloadIndicator.material;
            weapon.SetTransform(this);
        }
        PlaceVisual.gameObject.SetActive(IsActivePlace);
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

