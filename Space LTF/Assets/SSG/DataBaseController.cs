using System;
using System.Linq;
using UnityEngine;


[Serializable]
public struct ShipStruct
{
    public ShipBase ShipBase;
    public ShipType ShipType;
    public ShipConfig ShipConfig;
}

public class DataBaseController : Singleton<DataBaseController>
{
    //   public List<ShipControlCenter> ControlBase = new List<ShipControlCenter>();
    public DataStructPrefabs DataStructPrefabs;
    public SpellDataBase SpellDataBase;
    public AudioDataBase AudioDataBase;
    public BackgroundsDatabase Backgrounds;
    public Pool Pool;
    public Transform BulletContainer;
    public Transform AsteroidContainer;
    public SelectedElement SelectedElement;
    public GameObject PriorityTarget;
    public GameObject BaitPriorityTarget;
    public PoolRenderTextures PoolRenderTextures;
    public Transform CanvasContainer;

    public void Init()
    {
        DataStructPrefabs.Init();
        PoolRenderTextures.Init();
        Pool = new Pool(Instance, BulletContainer, CanvasContainer, AsteroidContainer);
        foreach (var bullet in DataStructPrefabs.Bullets)
        {
            Pool.RegisterBullet(bullet);
        }
        AudioDataBase.Init();
        SpellDataBase.Init();
    }

    public static T GetItem<T>(T item, Vector3 pos) where T : MonoBehaviour
    {
        return (Instantiate(item.gameObject, pos, Quaternion.identity) as GameObject).GetComponent<T>();
    }

    public static T GetItem<T>(T item) where T : MonoBehaviour
    {
        return (Instantiate(item.gameObject, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<T>();
    }

    public static GameObject GetItem(GameObject item)
    {
        return (Instantiate(item.gameObject, Vector3.zero, Quaternion.identity) as GameObject);
    }

    private int _lastIndex = 0;
    public int GetNewIndex()
    {
        _lastIndex++;
        return _lastIndex;
    }

    public Bullet GetBullet(WeaponType weaponType)
    {
        var posibleBullet = DataStructPrefabs.Bullets.FirstOrDefault(x => x.WeaponType == weaponType);
        if (posibleBullet == null)
        {
            Debug.LogError("can find bulet for parameters " + weaponType.ToString());
        }
        return posibleBullet;
    }

    public ShipBase GetShip(ShipType shipType, ShipConfig shipConfig)
    {
        var ship = DataStructPrefabs.Ships.FirstOrDefault(x => x.ShipType == shipType && x.ShipConfig == shipConfig);
        return ship.ShipBase;
    }
    public ShipBase GetShipTurret(WeaponType? type)
    {
        if (type.HasValue && DataStructPrefabs.Turrets.Count >= 3)
        {
            switch (type)
            {
                case WeaponType.laser:
                    return DataStructPrefabs.Turrets[0].ShipBase;
                case WeaponType.rocket:
                case WeaponType.casset:
                    return DataStructPrefabs.Turrets[1].ShipBase;
                case WeaponType.impulse:
                case WeaponType.eimRocket:
                    return DataStructPrefabs.Turrets[2].ShipBase;
            }
        }
        var ship = DataStructPrefabs.Turrets.RandomElement();
        return ship.ShipBase;
    }
}

