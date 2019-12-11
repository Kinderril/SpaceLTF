using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public enum PoolType
{
    flyNumberInGame,
    flyNumberInUI,
//    flyNumberWithPicture,
    effectVisual,
    bullet,
}

public class Pool
{
    private Tooltip _tooltip;

    private Dictionary<PoolType,List<PoolElement>> poolOfElements = new Dictionary<PoolType, List<PoolElement>>();
    private Dictionary<int,List<Bullet>> bulletsPool = new Dictionary<int, List<Bullet>>();
    private Dictionary<int,Bullet> registeredBulletsPrefabs = new Dictionary<int, Bullet>(); 

    private Dictionary<int, BaseEffectAbsorber> registeredEffects = new Dictionary<int, BaseEffectAbsorber>();
    private Dictionary<int, List<BaseEffectAbsorber>> effectsPool = new Dictionary<int, List<BaseEffectAbsorber>>();
    private Dictionary<BaseEffectAbsorber, List<BaseEffectAbsorber>> effectsPool2 = new Dictionary<BaseEffectAbsorber, List<BaseEffectAbsorber>>();
    

    private DataBaseController dataBaseController;
    private Transform _bulletContainer; 
    private Transform _canvasContainer; 

    public Pool(DataBaseController dataBaseController, Transform bulletContainer, Transform canvasContainer)
    {
        _bulletContainer = bulletContainer;
        _canvasContainer = canvasContainer;
//        _bulletContainer = Map.Instance.bulletContainer;
        foreach (PoolType pType in Enum.GetValues(typeof(PoolType)))
        {
            poolOfElements.Add(pType,new List<PoolElement>());
        }
        this.dataBaseController = dataBaseController;
        Prewarm();
    }

    private void Prewarm()
    {
        CreateNewTooltip();
        RegisterEffect(Utils.GetId(),DataBaseController.Instance.DataStructPrefabs.OnShipDeathEffect);
        RegisterEffect(Utils.GetId(),DataBaseController.Instance.DataStructPrefabs.ShieldChagedEffect);
        RegisterEffect(Utils.GetId(),DataBaseController.Instance.DataStructPrefabs.WeaponWaveStrike);
//        var baseT = _canvasContainer;
        for (int i = 0; i < 10; i++)
        {
            var element = DataBaseController.GetItem(dataBaseController.DataStructPrefabs.FlyNumberWithDependence);
            element.gameObject.SetActive(false);
            poolOfElements[PoolType.flyNumberInGame].Add(element);
            element.SetBaseParent(_canvasContainer);
        }
        for (int i = 0; i < 10; i++)
        {

            var element = DataBaseController.GetItem(dataBaseController.DataStructPrefabs.FlyingNumber);
            element.gameObject.SetActive(false);
            poolOfElements[PoolType.flyNumberInUI].Add(element);
            element.SetBaseParent(_canvasContainer);
        }
    }

    public Tooltip CreateNewTooltip()
    {

        Tooltip element = DataBaseController.GetItem(dataBaseController.DataStructPrefabs.TooltipPrefab);
        element.gameObject.SetActive(false);
        element.SetBaseParent(_canvasContainer);
        _tooltip =  (element);
        return element;
    }

    public void StartLevel()
    {
        bulletsPool = new Dictionary<int, List<Bullet>>();
    }

    public void RegisterBullet(Bullet bullet)
    {
//        bullet.ID == Utils.GetId() + 1000;
        if (!bulletsPool.ContainsKey(bullet.ID))
        {
            bulletsPool.Add(bullet.ID, new List<Bullet>());
        }
        else
        {
            Debug.LogWarning("Try to register bullet with same id:" + bullet.ID + "  " + bullet.GetType().ToString() + "   " + bullet.name);
        }
        if (!registeredBulletsPrefabs.ContainsKey(bullet.ID))
            registeredBulletsPrefabs.Add(bullet.ID,bullet);
    }

    public void RegisterEffect(int id,BaseEffectAbsorber effect)
    {
        if (!effectsPool.ContainsKey(id))
        {
            effectsPool.Add(id, new List<BaseEffectAbsorber>());
            if (effect != null)
                effectsPool2.Add(effect,new List<BaseEffectAbsorber>());
        }
        if (!registeredEffects.ContainsKey(id))
        {
            registeredEffects.Add(id, effect);

        }
    }


    public BaseEffectAbsorber GetEffect(BaseEffectAbsorber example)
    {
        BaseEffectAbsorber effect;
        var pool = effectsPool2[example];
        effect = pool.FirstOrDefault(x => !x.IsUsing);
        if (effect == null)
        {
            effect = DataBaseController.GetItem(example);
            pool.Add(effect);
        }
        return effect;
    }

    public BaseEffectAbsorber GetEffect(int type1)
    {
        BaseEffectAbsorber effect;
        var pool = effectsPool[type1];
        effect = pool.FirstOrDefault(x => !x.IsUsing);
        if (effect == null)
        {
            var prefab = registeredEffects[type1];
//            var prefab = DataBaseController.Instance.SpellDataBase.GetEffect(type1);
            effect = DataBaseController.GetItem(prefab);
            pool.Add(effect);
        }
        return effect;
    }

    public Bullet GetBullet(int id)
    {
        Bullet bullet;
        var pool = bulletsPool[id];
        bullet = pool.FirstOrDefault(x=>!x.IsUsing);
        if (bullet == null)
        {
            bullet =  GameObject.Instantiate(registeredBulletsPrefabs[id].gameObject).GetComponent<Bullet>();
            pool.Add(bullet);
        }
//        Debug.Log("Get bullet id:" + id);
        return bullet;
    }

    public T GetItemFromPool<T>(PoolType poolType, Vector3 pos = default(Vector3)) where T : PoolElement
    {
        PoolElement element = null;
        var dic = poolOfElements[poolType];
        element = GetNoUsed(dic);
        if (element == null)
        {
            switch (poolType)
            {
                case PoolType.flyNumberInGame:
                    element = DataBaseController.GetItem(dataBaseController.DataStructPrefabs.FlyNumberWithDependence);
                  break;
                case PoolType.flyNumberInUI:
                     element = DataBaseController.GetItem(dataBaseController.DataStructPrefabs.FlyingNumber);
                    break;
//                case PoolType.flyNumberWithPicture:
//                    element = DataBaseController.GetItem(dataBaseController.FlyingNumberWithPicture);
//                break;
            }

            dic.Add(element);
        }

        element.transform.localPosition = pos;
        element.Init();
        element.SetBaseParent(WindowManager.Instance.UIPool);
        return element as T;
    }

//    public VisualEffectBehaviour GetItemFromPool(EffectType effectType)
//    {
//        VisualEffectBehaviour element = null;
//        var dic = poolOfElements[PoolType.effectVisual];
//        for (int i = 0; i < dic.Count; i++)
//        {
//            var e = dic[i] as VisualEffectBehaviour;
//            if (!e.IsUsing && e.EffectType == effectType)
//                return e ;
//        }
//        element = DataBaseController.GetItem(dataBaseController.VisualEffectBehaviour(effectType));
//        dic.Add(element);
//        return element; 
//    }

    private PoolElement GetNoUsed(List<PoolElement> lis)
    {
        for (int i = 0; i < lis.Count; i++)
        {
            var e = lis[i];
            if (!e.IsUsing)
                return e;
        }
        return null;
    }

    public void Clear()
    {
        foreach (var poolOfElement in poolOfElements)
        {
            foreach (var element in poolOfElement.Value)
            {
                try
                {
                    element.EndUse();
                }
                catch (Exception ex)
                {
                    Debug.LogError("pool error " + element.name + "   " + ex);
                }
            }
        }
    }

    public Tooltip GetToolTip()
    {
        return _tooltip;
    }

    public void CloseTooltip()
    {
        _tooltip.Close();
    }
}

