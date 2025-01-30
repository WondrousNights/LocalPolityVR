using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SustainmentGameManager : NetworkBehaviour
{
    Day_NightCycle daynight;

    public List<PlayerManager> playerManagers = new List<PlayerManager>();
    void Start()
    {
        daynight = GetComponent<Day_NightCycle>();
    }
    public override void OnNetworkSpawn()
    {

    }


    [Rpc(SendTo.Server, RequireOwnership = false)]
    public void CheckPlayerReadyStatusRpc()
    {
        int connectedPlayerCount = playerManagers.Count;
        int playersReady = 0;
        foreach(PlayerManager manager in playerManagers)
        {
            if(manager.IsReady.Value)
            {
                playersReady += 1;
            }
        }

        if(playersReady >= connectedPlayerCount)
        {
            daynight.ChangeStateRpc();

            foreach(PlayerManager manager in playerManagers)
            {
                manager.IsReady.Value = false;
            }
        }
    }
}
