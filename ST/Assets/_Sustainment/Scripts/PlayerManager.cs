using Unity.Netcode;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    SustainmentGameManager gameManager;

    public NetworkVariable<bool> IsReady = new NetworkVariable<bool>();

    public NetworkVariable<SectorType> SectorRepresenting = new NetworkVariable<SectorType>();

    public override void OnNetworkSpawn()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<SustainmentGameManager>();

        if(IsOwner)
        {
            gameManager.playerManagers.Add(this);
            IsReady.OnValueChanged += OnReadyChanged;
            SectorRepresenting.OnValueChanged += OnSectorChanged;
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
    public void SetSectorRpc(SectorType sector)
    {
        SectorRepresenting.Value = sector;
    }

    public void OnSectorChanged(SectorType previous, SectorType current)
    {
        Debug.Log("My Sector : " + SectorRepresenting.Value);
    }


}
