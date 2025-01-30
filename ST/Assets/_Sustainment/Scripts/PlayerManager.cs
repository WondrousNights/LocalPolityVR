using Unity.Netcode;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    SustainmentGameManager gameManager;

    public NetworkVariable<bool> IsReady = new NetworkVariable<bool>();

    public NetworkVariable<Location> Destination = new NetworkVariable<Location>(Location.Forest);

    public override void OnNetworkSpawn()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<SustainmentGameManager>();

        if(IsOwner)
        {
            gameManager.playerManagers.Add(this);
            IsReady.OnValueChanged += OnReadyChanged;
            Destination.OnValueChanged += OnDestinationChanged;
            SetReadyRpc(false);
        }
        
  

        base.OnNetworkSpawn();
    }

    [Rpc(SendTo.Server)]
    public void SetReadyRpc(bool value)
    {
        IsReady.Value = value;
    }

    public void OnReadyChanged(bool previous, bool current)
    {
        if(current == true) gameManager.CheckPlayerReadyStatusRpc();
        Debug.Log("I am ready? : " + IsReady.Value);
    }

    [Rpc(SendTo.Server)]
    public void SetDestinationRpc(Location destination)
    {
        Destination.Value = destination;
    }

    public void OnDestinationChanged(Location previous, Location current)
    {
        Debug.Log("My Location : " + Destination.Value);
    }


}
