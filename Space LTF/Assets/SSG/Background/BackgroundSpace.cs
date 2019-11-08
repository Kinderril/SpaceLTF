using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BackgroundSpace : MonoBehaviour
{
    public MeshRenderer MainRenderer;
    public ParticleSystem MainParticleSystem;
    public List<ParticleSystem> effects;
    public List<SpaceObject> Stars;   //prefabs 
    public List<SpaceObject> Planets;//prefabs 
    public List<SpaceObject> BigPlanets;  //prefabs 
    public List<SpaceObject> Galaxies;   //prefabs 
    public List<Color> Colors; 

    public BoxCollider StartsContainer;
    public Transform Container;
    
//    public 

    public void Init()
    {
        Container.ClearTransform();
        var color = Colors.RandomElement();
        var backMats = DataBaseController.Instance.Backgrounds.BackMaterials;
        if (backMats.Count > 0)
        {
            var rnd = backMats.RandomElement();
            MainRenderer.material = rnd.MainMetarial;
            var renderParticle = MainParticleSystem.GetComponent<ParticleSystemRenderer>();
            renderParticle.material = rnd.ParticleMetarial;
        }

        int starsCount = (int)MyExtensions.Random(50, 90);
        int planetsCount = (int)MyExtensions.Random(3, 7);
        int galaxisCount = (int)MyExtensions.Random(1, 3);

        if (Stars.Count > 0)
        {
            for (int i = 0; i < starsCount; i++)
            {
                var p = GetRandomPositions();
                var pref = Stars.RandomElement();
                var star = DataBaseController.GetItem(pref);
                star.Randomize();
                star.SetColor(color);
                star.transform.SetParent(Container, true);
                star.transform.position = p;
            }
        }
        if (Planets.Count > 0)
        {
            for (int i = 0; i < planetsCount; i++)
            {
                var p = GetRandomPositions();
                var pref = Planets.RandomElement();
                var planet = DataBaseController.GetItem(pref);
                planet.Randomize();
                planet.SetColor(color);
                planet.transform.SetParent(Container, true);
                planet.transform.position = p;
            }
        }  
        if (Galaxies.Count > 0)
        {
            for (int i = 0; i < galaxisCount; i++)
            {
                var p = GetRandomPositions();
                var pref = Galaxies.RandomElement();
                var planet = DataBaseController.GetItem(pref);
                planet.Randomize();
                planet.SetColor(color);
                planet.transform.SetParent(Container, true);
                planet.transform.position = p;
            }
        }
    }

    private Vector3 GetRandomPositions()
    {
        var b = StartsContainer.bounds;
        var xx = MyExtensions.Random(b.min.x, b.max.x);
        var yy = MyExtensions.Random(b.min.y, b.max.y);
        var zz = MyExtensions.Random(b.min.z, b.max.z);
        Debug.DrawLine(b.min,b.max,Color.red,20f);
        return new Vector3(xx,yy,zz);
    }
}

