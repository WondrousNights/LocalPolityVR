using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class UI_PlayerConnection : NetworkBehaviour
{
    public bool IsLocal;

    public PlayerManager connectedPlayer;

    public override void OnNetworkSpawn()
    {
        if (IsClient) // Ensures this only runs on clients
        {
            StartCoroutine(AssignLocalPlayer());
        }
    }

    private IEnumerator AssignLocalPlayer()
    {
        // Wait until the NetworkManager is running and the player object is spawned
        while (!NetworkManager.Singleton.IsConnectedClient || 
               !NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject())
        {
            yield return null; // Keep waiting
        }

        connectedPlayer = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<PlayerManager>();

        if (connectedPlayer != null)
        {
            Debug.Log("Successfully assigned local player: " + connectedPlayer.gameObject.name);
        }
        else
        {
            Debug.LogError("Failed to assign local player!");
        }
    }
}
