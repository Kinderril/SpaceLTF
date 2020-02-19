using UnityEngine;
using System.Collections;

public class WeaponsAimSectorController 
{

    private CircleShader[] CircleShader;

    public void Init(WeaponAimedType[] weapons,ShipBase ship)
    {
        CircleShader = new CircleShader[weapons.Length];
        int i = 0;
        foreach (var weapon in weapons)
        {
            var shader = CacheAngCos(weapon, ship);
            CircleShader[i] = shader;
            i++;
        }
    }

    public CircleShader CacheAngCos(WeaponAimedType weapon, ShipBase ship)
    {
        var _sectorCos = Mathf.Cos(weapon.WeaponToAim.SetorAngle * Mathf.Deg2Rad / 2f);
        var circleShader = DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.CircleShader);
        circleShader.transform.SetParent(ship.transform, false);
        circleShader.transform.position = weapon.WeaponToAim.GetShootPos;
        circleShader.Init(ship, _sectorCos, weapon.WeaponToAim.AimRadius);
        return circleShader;
        //        var _sectorCos = Mathf.Cos(SetorAngle * Mathf.Deg2Rad / 2f);
        //        CircleShader = DataBaseController.GetItem(DataBaseController.Instance.DataStructPrefabs.CircleShader);
        //        CircleShader.transform.SetParent(Owner.transform, false);
        //        CircleShader.transform.position = GetShootPos;
        //        CircleShader.Init(Owner, _sectorCos, AimRadius);
    }
    public void IncreaseShootsDist(float coef)
    {
        for (int i = 0; i < CircleShader.Length; i++)
        {
            var shader = CircleShader[i];
            shader.IncreaseShootsDist(coef);
        }

    }

    public void Activate()
    {

    }   
    public void Disable()
    {

    }

    public void Select(bool val)
    {
        for (int i = 0; i < CircleShader.Length; i++)
        {
            var shader = CircleShader[i];
            shader.Select(val);
        }
                

    }

}
