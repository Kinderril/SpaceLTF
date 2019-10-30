using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundParticles : MonoBehaviour
{

    public List<ParticleSystem> GroundSystem;
//    public List<Gradient> gradients;
    private ParticleSystem _lastSystem = null;

    void Awake()
    {
//        Disable();
        foreach (var system in GroundSystem)
        {
            system.gameObject.SetActive(false);
        }
    }

    public void Init(Vector3 center,float size)
    {
//        gameObject.SetActive(true);
        _lastSystem = GroundSystem.RandomElement();
        _lastSystem.gameObject.SetActive(true);
        _lastSystem.transform.position = center;
    }

    public void Disable()
    {
        if (_lastSystem != null)
            _lastSystem.gameObject.SetActive(false);
//        gameObject.SetActive(false);
    }

}
