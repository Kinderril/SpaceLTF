using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class PlayerSlotsContainerIdsData
{
    public static string pathData = $"IdsData.data";
    public HashSet<int> UnblockedIds = new HashSet<int>() { 1, 2 };
//    public HashSet<int> CompleteIds = new HashSet<int>() { 1 };
    public Dictionary<int,HashSet<int>> Completes = new Dictionary<int, HashSet<int>>();
    public void Unblock(int id)
    {
        UnblockedIds.Add(id);
        MainController.Instance.Statistics.UnblockExprolerCells(UnblockedIds.Count);
        Save();
    }
    public void Complete(int id, int siz)
    {
        if (Completes.ContainsKey(id))
        {
            var set = Completes[id].Add(siz);
        }
        else
        {
            Completes.Add(id,new HashSet<int>(){siz});
        }
        Save();
    }
    private void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + pathData);
        //        MoneyData.Dispose();
        bf.Serialize(file, this);
        file.Close();
        Debug.Log("SlotsContainerIdsData Saved");

    }

    public static bool TryLoadGame(out PlayerSlotsContainerIdsData player)
    {
        if (File.Exists(Application.persistentDataPath + pathData))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + pathData, FileMode.Open);
            PlayerSlotsContainerIdsData save = (PlayerSlotsContainerIdsData)bf.Deserialize(file);
            file.Close();
            player = save;
            if (player.Completes == null)
            {
                player.Completes = new Dictionary<int, HashSet<int>>();
            }
            Debug.Log("PlayerSlotsContainerIdsData Loaded");
            return true;
        }
        Debug.Log("No PlayerSlotsContainerIdsData saved!");
        player = null;
        return false;
    }

    public void AddStartIds()
    {
        UnblockedIds.Add(100);
        UnblockedIds.Add(200);
        UnblockedIds.Add(300);
        UnblockedIds.Add(400);
        UnblockedIds.Add(500);
        UnblockedIds.Add(600);  

    }
}