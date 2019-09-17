using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class MapConsoleUI : MonoBehaviour
{
    public ObjectWithTextMeshPro SpaceConsoleElementPrefab;
    public Transform SpaceConsole;
    private List<ObjectWithTextMeshPro> existedElements = new List<ObjectWithTextMeshPro>(); 

    public void Appear()
    {
        var player = MainController.Instance.MainPlayer;
        player.MessagesToConsole.OnAddMessage += OnAddMessage;
        var allMessages = player.MessagesToConsole.GetAllAndClear();
        foreach (var msg in allMessages)
        {
            AddToConsole(msg);
        }
    }

    private void OnAddMessage(string obj)
    {
        var msg = PlayerMessagesToConsole.ModifWithDay(obj);
        AddToConsole(msg);
    }

    public void Close()
    {
        MainController.Instance.MainPlayer.MessagesToConsole.OnAddMessage -= OnAddMessage;
        var player = MainController.Instance.MainPlayer;
        player.MessagesToConsole.ClearAll();
    }

    private void AddToConsole(string msg)
    {
        var txt = DataBaseController.GetItem(SpaceConsoleElementPrefab);
        txt.transform.SetParent(SpaceConsole, false);
        txt.Field.text = msg;
    }

    public void ClearAll()
    {
        SpaceConsole.ClearTransform();
        existedElements.Clear();
    }
}

