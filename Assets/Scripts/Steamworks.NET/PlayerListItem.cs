using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerListItem : MonoBehaviour
{
    public string playerName; // Player's name for display
    public int ConnectionID; // Network connection ID
    public ulong playerSteamID; // Steam ID of the player

    private bool _avatarReceived; // Flag to check if avatar has been received

    public Text playerNameText; // UI Text component for player name
    public RawImage playerIcon; // UI RawImage component for player icon (avatar)

    protected Callback<AvatarImageLoaded_t> ImageLoaded; // Callback for Steam avatar image loaded event

    private void Start()
    {
        ImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnImageLoaded); // Initialize Steam callback
    }
    public void SetPlayerValues()
    {
        // Sets the player's name and icon in the UI
        if (playerNameText != null)
        {
            playerNameText.text = playerName; // Set player name text
        }

        if (!_avatarReceived) GetPlayerIcon(); // Get player icon if not already received
    }
    void GetPlayerIcon()
    {
        // Requests and sets the player's large Steam avatar
        int imageID = SteamFriends.GetLargeFriendAvatar((CSteamID)playerSteamID);
        if (imageID == -1) return; // If avatar is not available, return
        playerIcon.texture = GetSteamAsTexture(imageID); // Convert Steam image to Texture2D and set
    }
    private void OnImageLoaded(AvatarImageLoaded_t callback)
    {
        // Callback function when a Steam avatar image is loaded
        if (callback.m_steamID.m_SteamID == playerSteamID)
        {
            playerIcon.texture = GetSteamAsTexture(callback.m_iImage); // Set the loaded avatar
        }
        else return;
    }
    private Texture2D GetSteamAsTexture(int image)
    {
        // Converts a Steam image ID into a Unity Texture2D
        Texture2D texture = null;
        bool isValid = SteamUtils.GetImageSize(image, out uint width, out uint height);
        if (isValid)
        {
            byte[] _image = new byte[width * height * 4]; // Allocate byte array for RGBA image data
            isValid = SteamUtils.GetImageRGBA(image, _image, (int)(width * height * 4)); // Get image data

            if (isValid)
            {
                texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true); // Create new texture
                texture.LoadRawTextureData(_image); // Load raw image data
                texture.Apply(); // Apply changes to the texture
            }
        }
        _avatarReceived = true; // Mark avatar as received
        return texture;
    }

}