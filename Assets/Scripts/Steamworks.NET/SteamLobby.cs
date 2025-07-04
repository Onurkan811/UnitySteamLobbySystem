using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class SteamLobby : MonoBehaviour
{
    public static SteamLobby Instance; // Singleton instance

    // Steamworks Callbacks
    protected Callback<LobbyCreated_t> lobbyCreated; // Callback for lobby creation result
    protected Callback<GameLobbyJoinRequested_t> joinRequest; // Callback for game lobby join requests from friends
    protected Callback<LobbyEnter_t> lobbyEntered; // Callback for entering a lobby successfully

    protected Callback<LobbyMatchList_t> lobbyList; // Callback for receiving a list of lobbies
    protected Callback<LobbyDataUpdate_t> lobbyData; // Callback for lobby data updates

    public List<CSteamID> lobbyID = new List<CSteamID>(); // List to store discovered lobby IDs

    // Manager reference
    private CustomNetworkManager _manager;

    public ulong CurrentLobbyID; // The Steam ID of the current active lobby

    public GameObject hostBtn; // Reference to the UI button for hosting a lobby

    private void Start()
    {
        if (!SteamManager.Initialized) return; // Ensure SteamManager is initialized
        if (Instance == null) Instance = this; // Initialize singleton
        _manager = GetComponent<CustomNetworkManager>(); // Get CustomNetworkManager component

        // Create Steamworks callbacks
        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        joinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

        lobbyList = Callback<LobbyMatchList_t>.Create(MatchLobby);
        lobbyData = Callback<LobbyDataUpdate_t>.Create(GetLobbyData);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        // Called when a lobby creation attempt completes
        if (callback.m_eResult != EResult.k_EResultOK) return; // Check for success

        Debug.Log("Lobby created");

        _manager.StartHost(); // Start hosting the Mirror server

        CSteamID ulSteamID = new CSteamID(callback.m_ulSteamIDLobby); // Get the lobby ID

        // Set lobby data: host's Steam ID and lobby name
        SteamMatchmaking.SetLobbyData(ulSteamID, "HostAddress",
            SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(ulSteamID, "name",
            SteamFriends.GetPersonaName().ToString());
    }

    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        // Called when a user is invited to or requests to join a game lobby
        Debug.Log("Requested");
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby); // Attempt to join the lobby
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        // Called when the client successfully enters a Steam lobby
        hostBtn.SetActive(false); // Hide the host button
        CurrentLobbyID = callback.m_ulSteamIDLobby; // Store the current lobby ID

        if (NetworkServer.active) return; // If already a server, do nothing

        CSteamID ulSteamID = new CSteamID(callback.m_ulSteamIDLobby);

        _manager.networkAddress = SteamMatchmaking.GetLobbyData(ulSteamID, "HostAddress"); // Get host address from lobby data
        _manager.StartClient(); // Start the Mirror client to connect to the host
    }

    public void HostLobby()
    {
        // Creates a public Steam lobby with a max of 10 members
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 10);
        hostBtn.SetActive(false); // Hide the host button
    }

    public void LeaveGame(CSteamID lobbyID)
    {
        // Placeholder for leaving the Steam lobby. Implementation needed.
    }
    public void JoinLobby(CSteamID _lobbyID)
    {
        // Attempts to join a specified Steam lobby
        SteamMatchmaking.JoinLobby(_lobbyID);
    }

    public void FindLobbies()
    {
        // Initiates a search for Steam lobbies
        if (lobbyID.Count > 0) lobbyID.Clear(); // Clear previous lobby list

        SteamMatchmaking.AddRequestLobbyListResultCountFilter(10); // Request up to 10 lobbies
        SteamMatchmaking.RequestLobbyList(); // Send the lobby list request
    }

    private void MatchLobby(LobbyMatchList_t callback)
    {
        // Callback when a list of matching lobbies is received
        if (FindLobbyManager.instance.lobbyList.Count > 0) FindLobbyManager.instance.ClearLobby(); // Clear existing displayed lobbies

        for (int i = 0; i < callback.m_nLobbiesMatching; i++)
        {
            CSteamID lobbyId = SteamMatchmaking.GetLobbyByIndex(i); // Get lobby ID by index
            lobbyID.Add(lobbyId); // Add to internal list
            SteamMatchmaking.RequestLobbyData(lobbyId); // Request detailed data for each lobby
        }
    }

    private void GetLobbyData(LobbyDataUpdate_t callback)
    {
        // Callback when lobby data is updated or received
        FindLobbyManager.instance.DisplayLobbies(lobbyID, callback); // Display the updated lobby information in the UI
    }
}