using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


[CustomEditor(typeof (ShipBase))]
public class ShipInspector : Editor
{
    private ShipBase _shipBase;

    public void OnEnable()
    {
        _shipBase = target as ShipBase;
    }

    public override void OnInspectorGUI()
    {
//        GUILayout.Label("Trg acc:" + _shipBase.TargetAcceleration);
        if (Application.isPlaying)
        {

            string action = "null";
            if (_shipBase.CurAction != null)
            {
                action = _shipBase.CurAction.ActionType.ToString();
            }
            var maxSpeed = _shipBase.TargetPercentSpeed * _shipBase.ShipParameters.MaxSpeed;
            GUILayout.Label("CurSpeed:" + _shipBase.CurSpeed + "=>" + maxSpeed);
            EditorGUILayout.LabelField("action:" + action);
        }
        DrawDefaultInspector();
    }
}
