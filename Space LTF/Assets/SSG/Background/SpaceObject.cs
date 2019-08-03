using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SpaceObject : MonoBehaviour
{
    public float MinScale = 0.7f;
    public float MaxScale = 2f;
    public bool AllSidesRotation = false;
    private bool _wannaRotate = false;
    private float _rotateSpeed;
    private Vector3 _rotateDir;

    void Awake()
    {
        _wannaRotate = MyExtensions.IsTrue01(0.7f);
        _rotateSpeed = MyExtensions.Random(0.1f, 0.25f);
        var xx = MyExtensions.Random(0f, 1f);
        var yy = MyExtensions.Random(0f, 1f);
        var zz = MyExtensions.Random(0f, 1f);
        _rotateDir = new Vector3(xx, yy, zz);
    }


    public MeshRenderer Renderer;

    public void Randomize()
    {
        Transform ObjecTransform = Renderer.transform;
        var delta = 1f;
        Quaternion q;
        var x = MyExtensions.Random(MinScale, MaxScale);
        Vector3 v = new Vector3(x, x, x);
        ObjecTransform.localScale = v;
        if (AllSidesRotation)
        {
            q = new Quaternion(MyExtensions.Random(-delta, delta),
            MyExtensions.Random(-delta, delta), MyExtensions.Random(-delta, delta), 1f);
        }
        else
        {
            q = new Quaternion(0,MyExtensions.Random(-delta, delta),0, 1f);
        }
        ObjecTransform.rotation = q;
    }

    void Update()
    {
        if (_wannaRotate)
            transform.Rotate(_rotateDir, _rotateSpeed);
    }

    public void SetColor(Color color)
    {
        Renderer.material.color = color;
    }
}

