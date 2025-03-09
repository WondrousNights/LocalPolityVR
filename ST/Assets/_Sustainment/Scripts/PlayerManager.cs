using System.Numerics;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;
using XRMultiplayer;
using Vector3 = UnityEngine.Vector3;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField] SectorList sectorList;
    public NetworkVariable<bool> IsReady = new NetworkVariable<bool>();
    public NetworkVariable<SectorType> SectorRepresenting = new NetworkVariable<SectorType>();


    Town_GameManager gameManager;
    TeleportationProvider teleportation;
    public override void OnNetworkSpawn()
    {
        teleportation = GameObject.FindGameObjectWithTag("Teleporter").GetComponent<TeleportationProvider>();

        gameManager = Town_GameManager.GetInstance();
        gameManager.playerManagers.Add(this);

        if(IsOwner)
        {
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

    [Rpc(SendTo.Everyone)]
    private void SetPositionRpc(Vector3 position)
    {
        if(!IsOwner) return;

        TeleportRequest teleportRequest = new TeleportRequest
        {
            destinationPosition = position,
            destinationRotation = transform.rotation

        };

         Debug.Log($"[OWNER] TeleportRequest: Position = {teleportRequest.destinationPosition}, Rotation = {teleportRequest.destinationRotation}");

        teleportation.QueueTeleportRequest(teleportRequest);

        /*
        Debug.Log($"[Owner] Moving object to {position}");
        transform.position = position;

        foreach(NetworkTransform child in GetComponentsInChildren<NetworkTransform>())
        {
            Debug.Log($"[Owner] Teleporting child to {position}");
            child.Teleport(position, child.transform.rotation, child.transform.localScale);
        }
        */

    }
    [Rpc(SendTo.Everyone)]
    public void TeleportToSectorRpc()
    {
        if(!IsOwner) return;

        Debug.Log("TeleportToSectorRpc triggered!");

        foreach(SectorInformation sector in sectorList.Sectors)
        {
            if(sector.SectorType == SectorRepresenting.Value)
            {
                SetPositionRpc(sector.SectorBasePosition);
            }
        }
    }

    [Rpc(SendTo.Everyone)]
    public void TeleportToTownHallRpc()
    {
         if(!IsOwner) return;

        foreach(SectorInformation sector in sectorList.Sectors)
        {
            if(sector.SectorType == SectorRepresenting.Value)
            {
                SetPositionRpc(sector.SectorHallPosition);
            }
        }
    }


}
