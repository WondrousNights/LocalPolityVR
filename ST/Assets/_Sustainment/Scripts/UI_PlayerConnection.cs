using Unity.Netcode;
using UnityEngine;

public class UI_PlayerConnection : NetworkBehaviour
{
    public bool IsLocal;

    public PlayerManager connectedPlayer;

    public override void OnNetworkSpawn()
    {
        if(IsLocal)
        {
            connectedPlayer = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
        }
    }
}
