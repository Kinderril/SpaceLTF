using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class DistortionTest : MonoBehaviour
{
    private Material _materialToChange;
    public float _curDistValue;
    public string valName = "_DissortAmt";

    void Awake()
    {
        try
        {

            var rnd = GetComponent<Renderer>();
            if (rnd != null)
                _materialToChange = Utils.CopyMaterial(rnd);
        }
        catch (Exception e)
        {

            Debug.LogError("Awake");
        }
    }
    public void Update()
    {
        try
        {
            if (_materialToChange != null)
                _materialToChange.SetFloat(valName, _curDistValue);

        }
        catch (Exception e)
        {
                   Debug.LogError("Update");
        }
    }
}

