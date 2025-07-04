using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class CustomNetworkManager : NetworkManager
{
    public SteamPlayer playerPrefabs; // Prefab for the Steam player object
    public List<SteamPlayer> GamePlayer = new List<SteamPlayer>(); // List of active game players

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // Called on the server when a client connects and requests to add a player
        SteamPlayer gamePlayerInstance = Instantiate(playerPrefabs); // Instantiate player prefab

        gamePlayerInstance.ConnectionID = conn.connectionId; // Set connection ID
        gamePlayerInstance.PlayerIdNumber = GamePlayer.Count + 1; // Assign player ID number
        // Get player Steam ID from lobby members
        gamePlayerInstance.playerSteamId = (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.Instance.CurrentLobbyID, GamePlayer.Count);

        NetworkServer.AddPlayerForConnection(conn, gamePlayerInstance.gameObject); // Add player object to the network
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        // Called on the server after a scene change
        foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
        {
            if (conn.identity != null)
            {
                // Ensure the player's prefab is active
                conn.identity.gameObject.SetActive(true);
            }
        }
    }

    public void StartGame(string SceneName)
    {
        // Initiates a scene change on the server to start the game
        ServerChangeScene(SceneName);
    }
}