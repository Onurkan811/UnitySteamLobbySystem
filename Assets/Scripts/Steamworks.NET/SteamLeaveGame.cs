using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamLeaveGame : MonoBehaviour
{
    public int steamID; // Unused, potential for future player ID management

    public CSteamID lobbyID; // The Steam ID of the current lobby

    private CustomNetworkManager _manager; // Reference to the custom network manager

    private CustomNetworkManager Manager
    {
        get
        {
            if (_manager != null) return _manager;
            return _manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    private void Start()
    {
        lobbyID = (CSteamID)SteamLobby.Instance.CurrentLobbyID; // Get the current lobby ID
    }

    private void Update()
    {
        // Currently, this script has a placeholder for leaving the game on Escape key press.
        // The actual lobby leaving logic is not implemented here.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (lobbyID != (CSteamID)0)
            {
                // This block is empty, actual leave game logic would go here
            }
        }
    }
}