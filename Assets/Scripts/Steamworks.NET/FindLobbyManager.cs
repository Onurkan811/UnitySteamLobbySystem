using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindLobbyManager : MonoBehaviour
{
    public static FindLobbyManager instance; // Singleton instance

    private void Awake() => instance = this; // Initialize singleton

    public GameObject lobbyDataPrefab; // Prefab for displaying lobby data
    public Transform lobbiesMenuContent; // Parent transform for lobby entries in the UI

    public List<GameObject> lobbyList = new List<GameObject>(); // List of active lobby UI objects

    public void FindLobby()
    {
        // Triggers the SteamLobby to find available lobbies
        SteamLobby.Instance.FindLobbies();
    }

    public void DisplayLobbies(List<CSteamID> lobbyID, LobbyDataUpdate_t result)
    {
        // Displays the found lobbies in the UI
        for (int i = 0; i < lobbyID.Count; i++)
        {
            if (lobbyID[i].m_SteamID == result.m_ulSteamIDLobby)
            {
                GameObject lobby = Instantiate(lobbyDataPrefab, lobbiesMenuContent); // Instantiate lobby UI element
                LobbyData data = lobby.GetComponent<LobbyData>(); // Get LobbyData component
                data.lobbyID = (CSteamID)lobbyID[i].m_SteamID; // Set lobby Steam ID
                data.lobbyName = SteamMatchmaking.GetLobbyData((CSteamID)lobbyID[i].m_SteamID, "name"); // Get lobby name
                data.lobbyMembers = SteamMatchmaking.GetNumLobbyMembers((CSteamID)lobbyID[i]); // Get number of lobby members
                data.SetLobbyData(); // Set UI display for lobby data

                lobbyList.Add(lobby); // Add to the list of displayed lobbies
            }
        }
    }

    public void ClearLobby()
    {
        // Clears all displayed lobby UI elements
        foreach (GameObject lobby in lobbyList)
        {
            Destroy(lobby); // Destroy the UI object
        }
        lobbyList.Clear(); // Clear the list
    }
}