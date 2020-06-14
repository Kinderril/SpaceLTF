using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;




[Serializable]
public class PlayerSlotsContainerSafeData
{

    public static string pathData = $"PlayerSlotsContainerSafeData.data";
    public List<PlayerSafe> _playerSafeContainers = new List<PlayerSafe>();

    public void Add(PlayerSafe safe)
    {
        _playerSafeContainers.Add(safe);
        Save();
    }


    public void Remove(PlayerSafe safe)
    {
        _playerSafeContainers.Remove(safe);
        Save();
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + pathData);
        //        MoneyData.Dispose();
        bf.Serialize(file, this);
        file.Close();
        Debug.Log("PlayerSlotsContainerSafeData Saved");

    }

    public static bool TryLoadGame(out PlayerSlotsContainerSafeData player)
    {
        var loadPath = Application.persistentDataPath + pathData;
        if (File.Exists(loadPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(loadPath, FileMode.Open);
            PlayerSlotsContainerSafeData save = (PlayerSlotsContainerSafeData)bf.Deserialize(file);
            file.Close();
            player = save;
            player.CheckAfterLoad();
            Debug.Log($"PlayerSlotsContainerSafeData Loaded : {loadPath}");
            return true;
        }
        Debug.Log($"No PlayerSlotsContainerSafeData saved! : {loadPath}");
        player = null;
        return false;
    }

    private void CheckAfterLoad()
    {
#if UNITY_EDITOR   
        foreach (var playerSafeContainer in _playerSafeContainers)
        {
            Debug.LogError($"PSF {playerSafeContainer.CreditsCoef}");
            playerSafeContainer.SetLowCoef();// = 10f;
        }

#endif

    }
}

public class PlayerSlotsContainer
{
    private PlayerSlotsContainerSafeData _data;
    private PlayerSlotsContainerIdsData _dataIds;
    public event Action<PlayerSafe> OnAddSafeContainer;
    public event Action<PlayerSafe> OnRemoveSafeContainer;
    public event Action OnUnblockId;

    public void Init()
    {
        PlayerSlotsContainerSafeData.TryLoadGame(out _data);
        if (_data == null)
        {
            _data = new PlayerSlotsContainerSafeData();
        }

        PlayerSlotsContainerIdsData.TryLoadGame(out _dataIds);
        if (_dataIds == null)
        {
            _dataIds = new PlayerSlotsContainerIdsData();
        }

        _dataIds.AddStartIds();
    }

    public List<PlayerSafe> PlayersProfiles()
    {
        return _data._playerSafeContainers;
    }

    public void AddNewContainer(PlayerSafe safe)
    {
        _data.Add(safe);
        OnAddSafeContainer?.Invoke(safe);
    }
    public void RemoveNewContainer(PlayerSafe safe)
    {
        _data.Remove(safe);
        OnRemoveSafeContainer?.Invoke(safe);
    }

    public void UnblockId(int id)
    {
        _dataIds.Unblock(id);
        OnUnblockId?.Invoke();
    }


    public bool ContainsUnblockId(int cellId)
    {
        return _dataIds.UnblockedIds.Contains(cellId);
    }

    public bool TryAddCreateNewProfile(ShipConfig shipConfig, EStartPair startPair, string nameFieldText)
    {
        if (nameFieldText.Length < 3)
        {
            WindowManager.Instance.InfoWindow.Init(null, Namings.Tag("NameNewTooShort"));
            return false;
        }

        foreach (var dataPlayerSafeContainer in _data._playerSafeContainers)
        {
            if (dataPlayerSafeContainer.Name == nameFieldText)
            {
                WindowManager.Instance.InfoWindow.Init(null, Namings.Tag("NameNewHaveName"));
                return false;
            }
        }
        PlayerSafe safe = new PlayerSafe(true,false);
        safe.CreateNew(shipConfig, startPair, nameFieldText);
        AddNewContainer(safe);
        return true;
    }

    public bool ContainsCompleteId(int cellId,out HashSet<int> keys)
    {
        var contains = _dataIds.Completes.ContainsKey(cellId);
        if (!contains)
        {
            keys = null;
            return false;
        }

        keys = _dataIds.Completes[cellId];
        return true;
    }

    public void SaveProfiles()
    {
        _data.Save();
    }

    public void CompleteId(int cellId,int size)
    {
        _dataIds.Complete(cellId, size);
    }
}
