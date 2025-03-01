using System.Numerics;
using Unity.Netcode;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerManager : NetworkBehaviour
{
    Town_GameManager gameManager;

    public NetworkVariable<bool> IsReady = new NetworkVariable<bool>();
    public NetworkVariable<SectorType> SectorRepresenting = new NetworkVariable<SectorType>();

    void Start()
    {
        gameManager = Town_GameManager.GetInstance();
    }

    public override void OnNetworkSpawn()
    {

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

    [Rpc(SendTo.Server)]
    private void SetPositionRpc(Vector3 position)
    {

    }

    public void TeleportToSector()
    {
        //
    }

    public void TeleportToTownHall()
    {
        //
    }


}
