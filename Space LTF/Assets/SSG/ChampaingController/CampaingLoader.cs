using System;
using UnityEngine;
using System.Collections;
using System.IO;

public class CampaingLoader
{
    private const string PATH_CAMP = "CampSave/";
    public string[] GetAllSaves()
    {
        string[] fileEntries = null;
        string path = Application.persistentDataPath + PATH_CAMP;
        if (Directory.Exists(path))
        {

            fileEntries = Directory.GetFiles(path);
        }
        else
        {
            Directory.CreateDirectory(path);
        }

        if (fileEntries != null)
        {
            string[] fileNames = new string[fileEntries.Length];
            for (int i = 0; i < fileEntries.Length; i++)
            {
                var fileFull = fileEntries[i];
                var name = fileFull.Replace(path, "");
                fileNames[i] = name;
            }

            return fileNames;
        }

        return null;

    }

    public bool TryLoad(string name)
    {
        PlayerChampaingContainer player;
        var data = PlayerChampaingContainer.LoadGame(Application.persistentDataPath + PATH_CAMP + name, out player);
        if (data)
        {
            MainController.Instance.Campaing.Load(player);
            return true;
        }
        WindowManager.Instance.InfoWindow.Init(null,Namings.Tag("Errorload"));
        return false;
    }

    public bool CanUseName(string name)
    {
        var saves = GetAllSaves();
        if (saves == null)
        {
            return true;
        }

        foreach (var save in saves)
        {
            if (save.Equals(name))
            {
                return false;
            }
        }
        return true;
    }

    public bool SaveTo(string name, PlayerChampaingContainer playerChampaingContainer)
    {
        if (CanUseName(name))
        {
            string path = Application.persistentDataPath + PATH_CAMP + name;
            try
            {
                playerChampaingContainer.SaveTo(path);
            }
            catch (Exception e)
            {
                Debug.LogError($"save error {e}");
                return false;
            }
            return true;
        }

        return false;

    }

    public void DeleteSave(string name)
    {
        string path = Application.persistentDataPath + PATH_CAMP + name;
        File.Delete(path);
    }
}
