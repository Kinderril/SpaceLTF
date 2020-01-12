using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class SteamScript : MonoBehaviour
{
    private CallResult<NumberOfCurrentPlayers_t> m_NumberOfCurrentPlayers;

    private void OnEnable()
    {
        if (SteamManager.Initialized)
        {
            m_NumberOfCurrentPlayers = CallResult<NumberOfCurrentPlayers_t>.Create(OnNumberOfCurrentPlayers);
        }
    }

    private void OnNumberOfCurrentPlayers(NumberOfCurrentPlayers_t pCallback, bool bIOFailure)
    {
        if (pCallback.m_bSuccess != 1 || bIOFailure)
        {
            Debug.Log("There was an error retrieving the NumberOfCurrentPlayers.");
        }
        else
        {
            Debug.Log("The number of players playing your game: " + pCallback.m_cPlayers);
        }
    }
}