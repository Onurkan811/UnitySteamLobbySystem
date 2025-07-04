using Mirror;
using Steamworks;
using UnityEngine;

public class SteamPlayer : NetworkBehaviour
{
    [SyncVar] public GameObject playerModel; // Reference to the player's visual model

    [SyncVar] public int ConnectionID, PlayerIdNumber; // Network connection ID and player number
    [SyncVar] public ulong playerSteamId; // Steam ID of the player

    [SyncVar(hook = nameof(PlayerNameUpdate))]
    public string playerName; // Player's name, synchronized across the network

    private CustomNetworkManager _manager; // Reference to the custom network manager

    private CustomNetworkManager Manager
    {
        get
        {
            if (_manager != null) return _manager;
            return _manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    public override void OnStartAuthority()
    {
        // Called when the local client gains authority over this player object
        SetPlayerName(SteamFriends.GetPersonaName().ToString()); // Set player name from Steam
        gameObject.name = "LocalGamePlayer"; // Tag the local player object

        if (LobbyController.Instance != null)
        {
            LobbyController.Instance.FindLocalPlayer(); // Find the local player in the lobby
            LobbyController.Instance.UpdateLobbyName(); // Update lobby name display
        }
    }

    public override void OnStartClient()
    {
        // Called when the player object starts on a client
        if (playerModel == null)
            playerModel = transform.Find("PlayerModel")?.gameObject; // Find the player model

        Manager.GamePlayer.Add(this); // Add this player to the list of game players

        if (LobbyController.Instance != null && LobbyController.Instance.gameObject.activeInHierarchy)
        {
            LobbyController.Instance.UpdateLobbyName(); // Update lobby name display
            LobbyController.Instance.UpdatePlayerList(); // Update the player list in the lobby UI
        }
    }

    public override void OnStopClient()
    {
        // Called when the player object stops on a client
        Manager.GamePlayer.Remove(this); // Remove this player from the list of game players
    }

    [Command]
    private void SetPlayerName(string _playerName)
    {
        // Command to set the player's name on the server
        PlayerNameUpdate(this.playerName, _playerName);
    }

    public void PlayerNameUpdate(string oldValue, string newValue)
    {
        // Hook method for playerName SyncVar, updates player name on server and clients
        if (isServer)
            this.playerName = newValue; // Update name on server

        if (isClient && LobbyController.Instance != null && LobbyController.Instance.gameObject.activeInHierarchy)
        {
            LobbyController.Instance.UpdatePlayerList(); // Update player list on clients
        }
    }

    public void CanStartGame(string sceneName)
    {
        // Initiates game start if the local player has authority
        if (authority)
            cmdStartGame(sceneName); // Command to start the game
    }

    [Command]
    public void cmdStartGame(string sceneName)
    {
        // Command executed on the server to start the game and load a new scene
        _manager.StartGame(sceneName);

        if (playerModel != null)
            playerModel.SetActive(true); // Activate the player model
    }
}