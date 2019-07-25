using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public enum ControlPointOffset
{
    none,
    soft,
    hard,
}

public class ArrowTarget : MonoBehaviour
{
    public ArrowShowElement Element;
    private const int MAX_COUNT = 30;
    private float CountElementsDelta = 2f;
    private ArrowShowElement[] Arrows = new ArrowShowElement[MAX_COUNT];
    private int elementsActivated = 0;
    private ShipBase _shipBase;
    private bool isInited = false;
    private float _randomizedControls;
    private float _randomizedControls2;

    void Awake()
    {
        for (int i = 0; i < Arrows.Length; i++)
        {
            ArrowShowElement a = DataBaseController.GetItem<ArrowShowElement>(Element);
            Arrows[i] = a;
            a.gameObject.SetActive(false);
            a.transform.SetParent(transform);
        }
    }

    public void Ready()
    {
        isInited = false;
        _shipBase = null;
    }

    private void ClearAll()
    {
        if (elementsActivated > 0)
        {
            for (int i = 0; i < elementsActivated; i++)
            {
                var arr = Arrows[i];
                arr.gameObject.SetActive(false);
            }
            elementsActivated = 0;
        }
    }

    void Update()
    {
        if (isInited)
        {
            if (!_shipBase.IsDead && _shipBase.CurAction != null)
            {
                var move = _shipBase.PathController.Target;
                if (_shipBase.CurAction != null)
                {
                    Vector3? target = _shipBase.CurAction.GetTargetToArrow();
                    if (target.HasValue)
                    {
                        UpdateTo(_shipBase.Position, target.Value);
                        return;
                    }
                }
            }
            ClearAll();
        }
    }
    
    private void UpdateTo(Vector3 from, Vector3 to)
    {
        var dist = (from - to).magnitude;
        var countElements =(int)(dist/CountElementsDelta);
        if (countElements < 2)
        {
            countElements = 0;
        }
        if (countElements > MAX_COUNT)
        {
            countElements = MAX_COUNT;
        }
        if (elementsActivated < countElements)
        {
            for (int i = elementsActivated; i < countElements; i++)
            {
                var arr = Arrows[i];
                arr.gameObject.SetActive(true);
            }
        }
        else if (elementsActivated > countElements)
        {
            for (int i = countElements; i < elementsActivated; i++)
            {
                var arr = Arrows[i];
                arr.gameObject.SetActive(false);
            }
        }
        elementsActivated = countElements;

        if (countElements == 0)
        {
            return;
        }
        var control = ContrtolPoint(from, to, ControlPointOffset.soft);
        float timeDelta = 1f/(1f+(float)countElements); 
        Vector3[] positions = new Vector3[countElements+1];
//        positions[0] = from;
        positions[positions.Length-1] = to;
        for (int i = 0; i < countElements; i++)
        {
            var p1 = UpdateQuadVector(from, to, timeDelta*(i+1f), control);
            positions[i] = p1;
        }
        for (int i = 0; i < positions.Length-1; i++)
        {
            var p1 = positions[i];
            var p2 = positions[i+1];
//            var dir = p2 - p1;
            var arr1 = Arrows[i];
            arr1.transform.position = p1;
            arr1.transform.LookAt(p2);
        }
    }
    
    protected Vector3 UpdateQuadVector(Vector3 start,Vector3 trg,float time,Vector3 control)
    {
        float vn = 1 - time;
        var v2 = vn * vn;
        var t2 = time * time;
        float px = v2 * start.x + 2 * time * vn * control.x + t2 * trg.x;
        float py = v2 * start.y + 2 * time * vn * control.y + t2 * trg.y;
        float pz = v2 * start.z + 2 * time * vn * control.z + t2 * trg.z;
        //        float y = start.y*vn + trg.y*time;

        return new Vector3(px, py, pz);
    }

    private Vector3 ContrtolPoint(Vector3 startPos, Vector3 end, ControlPointOffset offset)
    {
        float deltaOffset = 1f;

        var milldePoint = (startPos + end) / 2;
        var alpha = (startPos.z - end.z) / (startPos.x - end.x);
        var dist = (startPos - end).magnitude;
        var v = (new Vector3(1, -1 / alpha)).normalized;


        switch (offset)
        {
            case ControlPointOffset.none:
                deltaOffset = 0.1f;
                break;
            case ControlPointOffset.soft:
                var p = (dist * 0.25f + _randomizedControls * dist * 0.5f);
                deltaOffset = Mathf.Sign(_randomizedControls2 - 0.5f) * p;
                break;
            case ControlPointOffset.hard:
                deltaOffset = Mathf.Sign(UnityEngine.Random.value - 0.5f) * (dist * 0.75f + UnityEngine.Random.value * dist * 1.25f);
                break;
        }

        var contrtolPoint = milldePoint + v * deltaOffset;
        //        Debug.Log(startPos + "   " + contrtolPoint + "   " + end);
        contrtolPoint.y = milldePoint.y;
        return contrtolPoint;
    }

    public void SetOwner(ShipBase selectedShip)
    {
        _randomizedControls = UnityEngine.Random.value;
        _randomizedControls2 = UnityEngine.Random.value;
        isInited = true;
        _shipBase = selectedShip;
        _shipBase.OnDeath += OnDeath;
    }

    private void OnDeath(ShipBase obj)
    {
        obj.OnDeath -= OnDeath;
        Disable();
    }

    public void Disable()
    {

        isInited = false;
        _shipBase = null;
        ClearAll();
    }
}

