using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public class DialogParseWindow : EditorWindow
{


    [MenuItem("Tools/DialogParseWindow")]
    static void Init()
    {
        var w = GetWindow<DialogParseWindow>();
        w.Show();
        
    }
    
    private Editor _e;

    public void OnGUI()
    {
        if (!Application.isPlaying)
        {
            NoInGame();
        }
    }
    private string _stringToParse;
    private string _nameDialog;
    private string _lastLoc;
    private string _lastList;
    private void NoInGame()
    {
        EditorGUILayout.BeginHorizontal();
        DialogParse();
        EditorGUILayout.EndHorizontal();

    }

    private void DialogParse()
    {
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Parse to dialog"))
        {
            var  res = DialogParser.Parse(_stringToParse, _nameDialog);
            _lastList = res.val1.ToString();
            _lastLoc = res.val2.ToString();
        }
        _nameDialog = EditorGUILayout.TextField(_nameDialog);
        _stringToParse = EditorGUILayout.TextArea(_stringToParse);
        _lastList = EditorGUILayout.TextArea(_lastList);
        _lastLoc = EditorGUILayout.TextArea(_lastLoc);
        EditorGUILayout.EndVertical();

    }

    
}