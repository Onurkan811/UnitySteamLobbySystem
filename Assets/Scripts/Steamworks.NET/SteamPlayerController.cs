using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SteamPlayerController : NetworkBehaviour
{
    [SerializeField] private float speed; // Movement speed of the player
    [SerializeField] private float rotationSpeed; // Rotation speed of the player

    private float x, y; // Input axis values
    private Vector3 _movement; // Not currently used, could be for future movement logic

    private Rigidbody _rb; // Reference to the Rigidbody component

    void Start()
    {
        _rb = GetComponent<Rigidbody>(); // Get Rigidbody component
    }

    void Update()
    {
        if (!isLocalPlayer) return; // Only process input for the local player

        x = Input.GetAxis("Horizontal"); // Get horizontal input
        y = Input.GetAxis("Vertical"); // Get vertical input

        if (NetworkClient.isConnected)
        {
            cmdMove(x, y); // Send movement command to the server
        }

    }

    [Command]
    void cmdMove(float x, float y) => RpcMove(x, y); // Command to execute RpcMove on clients

    [ClientRpc]
    void RpcMove(float x, float y)
    {
        // ClientRpc to move the player on all clients
        Vector3 dir = new Vector3(x, 0, y).normalized;

        _rb.velocity = dir * speed * Time.deltaTime;

        if (dir == Vector3.zero) return; // If no input, stop here

        Quaternion lookDir = Quaternion.LookRotation(dir); // Calculate rotation towards movement direction
        transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, rotationSpeed * Time.deltaTime); // Smoothly rotate player
    }
}