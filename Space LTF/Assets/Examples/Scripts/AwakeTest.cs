using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class AwakeTest : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("Awake-");
    }
    void Start()
    {
        Debug.Log("Start-");
    }
    void OnDestroy()
    {
        Debug.Log("OnDestroy-");
    }
}

