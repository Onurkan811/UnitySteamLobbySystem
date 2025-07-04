using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using System.Linq;

public class LobbyController : MonoBehaviour
{
    public static LobbyController Instance; // Singleton instance

    private void Awake()
    {
        Instance = this; // Initialize singleton
    }

    [Header("Player Item")]
    public GameObject playerListItemViewContent; // Parent transform for player list items in UI
    public GameObject playerListItemPrefab; // Prefab for player list item UI
    public GameObject localPlayerObject; // Reference to the local player's game object

    [Header("Lobby")]
    public ulong currentLobbyID; // The current lobby's Steam ID

    private List<PlayerListItem> playerList = new List<PlayerListItem>(); // List of displayed player list items
    public SteamPlayer localObject; // Reference to the local SteamPlayer component

    private CustomNetworkManager _manager; // Reference to the custom network manager

    private CustomNetworkManager Manager
    {
        get
        {
            if (_manager != null) return _manager;
            return _manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    public void UpdateLobbyName()
    {
        // Updates the displayed lobby name (though not explicitly shown in this script, likely for UI)
        currentLobbyID = Manager.GetComponent<SteamLobby>().CurrentLobbyID;
    }

    public void UpdatePlayerList()
    {
        // Manages updating the list of players displayed in the lobby UI
        RemovePlayerItems();
        CreatePlayerItems();
        UpdatePlayerItems();
    }

    private void CreatePlayerItems()
    {
        // Creates UI items for players that are in the game but not yet in the UI list
        foreach (SteamPlayer player in Manager.GamePlayer)
        {
            if (!playerList.Any(item => item.ConnectionID == player.ConnectionID))
            {
                CreatePlayerItem(player); // Create UI item for new player
            }
        }
    }

    private void RemovePlayerItems()
    {
        // Removes UI items for players that have left the game
        var itemsToRemove = playerList.Where(item => !Manager.GamePlayer.Any(player => player.ConnectionID == item.ConnectionID)).ToList();

        foreach (var item in itemsToRemove)
        {
            if (item != null)
            {
                Destroy(item.gameObject); // Destroy the UI object
            }
            playerList.Remove(item); // Remove from the list
        }
    }

    private void UpdatePlayerItems()
    {
        // Updates the details (like player name) of existing player UI items
        foreach (PlayerListItem item in playerList)
        {
            SteamPlayer player = Manager.GamePlayer.Find(p => p.ConnectionID == item.ConnectionID);
            if (player != null)
            {
                item.playerName = player.playerName; // Update player name
                item.SetPlayerValues(); // Refresh UI values
            }
        }
    }

    private void CreatePlayerItem(SteamPlayer player)
    {
        // Instantiates and sets up a single player list UI item
        GameObject newPlayerItem = Instantiate(playerListItemPrefab, playerListItemViewContent.transform);
        PlayerListItem newPlayerListItem = newPlayerItem.GetComponent<PlayerListItem>();
        newPlayerListItem.playerName = player.playerName; // Set player name
        newPlayerListItem.ConnectionID = player.ConnectionID; // Set connection ID
        newPlayerListItem.playerSteamID = player.playerSteamId; // Set Steam ID
        newPlayerListItem.SetPlayerValues(); // Set UI values
        playerList.Add(newPlayerListItem); // Add to list
    }

    public void FindLocalPlayer()
    {
        // Finds and sets the local player object and its SteamPlayer component
        localPlayerObject = GameObject.Find("LocalGamePlayer");
        localObject = localPlayerObject.GetComponent<SteamPlayer>();
    }

    public void StartGame(string sceneName)
    {
        // Tells the local player to send a command to start the game
        localObject.cmdStartGame(sceneName);
    }
}