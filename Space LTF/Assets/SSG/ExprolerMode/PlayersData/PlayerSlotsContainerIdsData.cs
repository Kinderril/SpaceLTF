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
    public HashSet<int> CompleteIds = new HashSet<int>() { 1 };
    public void Unblock(int id)
    {
        UnblockedIds.Add(id);
        Save();
    }
    public void Complete(int id)
    {
        CompleteIds.Add(id);
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
            Debug.Log("PlayerSlotsContainerIdsData Loaded");
            return true;
        }
        Debug.Log("No PlayerSlotsContainerIdsData saved!");
        player = null;
        return false;
    }
}