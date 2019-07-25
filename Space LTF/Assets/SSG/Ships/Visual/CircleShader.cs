using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class CircleShader : MonoBehaviour
{
    private const string LookDirection = "_LookDirection";
    private const string ObjectPos = "_ObjectPos";
    private const string Val = "_Val";

    private Material MatToCircle;
    private ShipBase _ship;
    private float _ang;
    private bool isEnable = false;

    public void Init(ShipBase ship,float ang,float range)
    {
        _ang = ang;
        _ship = ship;
        transform.localScale = transform.localScale * range * 2f;
        var renderer = GetComponent<MeshRenderer>();
        Utils.CopyMaterials(renderer,null);
        MatToCircle = renderer.material;
        if (MatToCircle == null)
        {
            Debug.LogError("MatToCircle is null");
            return;
        }
        MatToCircle.SetFloat(Val, _ang);
        _ship.OnDeath += OnDeath;
    }

    private void OnDeath(ShipBase obj)
    {
        obj.OnDeath -= OnDeath;
        GameObject.Destroy(gameObject);
    }

    void Update()
    {
        if (!isEnable)
        {
            return;
        }
        UpdateAng();
    }

    private void UpdateAng()
    {
        MatToCircle.SetVector(LookDirection, new Vector4(_ship.LookDirection.x, _ship.LookDirection.z, 0, 0));
        MatToCircle.SetVector(ObjectPos, new Vector4(_ship.Position.x, _ship.Position.y, _ship.Position.z, 0));
    }

    public void Select(bool val)
    {
        isEnable = val;
        gameObject.SetActive(val);
        if (val)
        {
            UpdateAng();
        }
    }
}

