using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LobbyData : MonoBehaviour
{
    public CSteamID lobbyID; // Steam ID of the lobby
    public string lobbyName; // Name of the lobby
    public int lobbyMembers; // Current number of members in the lobby

    public Text lobbyNameText; // UI Text component for lobby name
    public Text lobbyMembersText; // UI Text component for lobby members count

    public void SetLobbyData()
    {
        // Sets the display text for lobby name and member count
        if (lobbyName == "") lobbyNameText.text = "Null"; // Handle empty lobby name
        else lobbyNameText.text = lobbyName; // Set lobby name text

        lobbyMembersText.text = lobbyMembers + "/4"; // Display current members out of 4 (assuming max 4)
    }

    public void JoinLobby()
    {
        // Triggers the SteamLobby to join this specific lobby
        SteamLobby.Instance.JoinLobby(lobbyID);
    }
}