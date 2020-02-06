using System.Collections.Generic;
using UnityEngine;


public class BattleBorderWall : MonoBehaviour
{
    public RoadMeshCreator ParticleAttractor;
    private CameraController _gameCamera;
    private const float distToMinShow = 10;
    private const float distToMaxShow = 20;
    // public List<Material> renderer;
    private const float delta = distToMaxShow - distToMinShow;
    private Color _color = new Color(61f/255f,202f/255f,212f/255f,0.5f);
    public float _prevAlpha = 1f;
    public float dist = 1f;
    private Vector3 _midPos;
    private Vector3 _posToCompare;
    private Material[] listToColor;
    private Vector3 _worldCenter;
    private float _distToShow;
    public void Init(Vector3 worldCenter, Vector3 p1, Vector3 p2, Transform container, CameraController gameCamera)
    {
        _worldCenter = worldCenter;
_midPos = (p1 + p2) / 2f;
        _posToCompare = _midPos + new Vector3(0,0,3f);
        _distToShow = (_worldCenter - _midPos).magnitude;
        _gameCamera = gameCamera;
        var list = new List<Vector3>() { p1, _midPos, p2 };
        transform.SetParent(container);
        ParticleAttractor.CreateByPoints(list);
        var renderer = ParticleAttractor.pathCreator.GetComponentInChildren<Renderer>();
        listToColor = Utils.CopyMaterials(renderer,null);

//        ParticleAttractor.roadMaterial = Utils.CopyMaterial(ParticleAttractor.roadMaterial);
//        ParticleAttractor.undersideMaterial = Utils.CopyMaterial(ParticleAttractor.undersideMaterial);
//        _color = ParticleAttractor.roadMaterial.color;

//        var listNames = new List<string>();
//        ParticleAttractor.roadMaterial.GetTexturePropertyNames(listNames);
//        foreach (var listName in listNames)
//        {
//            Debug.LogError($"listNames:{listName}");
//        }
    }

    void Update()
    {
        return;
        var dirCameraFromCenterdistCameraFromCenter = _worldCenter - _gameCamera.GameCameraHolder.position;
        dirCameraFromCenterdistCameraFromCenter.y = 0f;
        var distCamCenter = dirCameraFromCenterdistCameraFromCenter.magnitude;
        float curAlpha = 0f;
        if (distCamCenter > _distToShow)
        {
            curAlpha = 1f;
        }
        else
        {
            var dir = (_gameCamera.GameCameraHolder.position - _posToCompare);
            dir.y = 0f;
            dist = dir.magnitude;
            if (dist < distToMinShow)
            {
                curAlpha = 1f;
            }

            else if (dist > distToMaxShow)
            {
                curAlpha = 0f;
            }
            else
            {
                var d = dist - distToMinShow;
                var p = d / delta;
                curAlpha = p;
            }
        }

        var deltaPrevAlpha = Mathf.Abs(curAlpha - _prevAlpha);
        if (deltaPrevAlpha > 0.001f)
        {
            _prevAlpha = curAlpha;
            var color = new Color(_color.r,_color.g,_color.b,_prevAlpha);
            for (int i = 0; i < listToColor.Length; i++)
            {
                var mat = listToColor[i];
                mat.SetColor("_TintColor", color);

            }

        }

    }


}
