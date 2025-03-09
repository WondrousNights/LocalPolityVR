using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UI_PlayerConnection))]
public class PlayerUI_ReadyToggle : NetworkBehaviour
{
    Button toggle;
    PlayerManager playerManager;

    void Start()
    {
        toggle = GetComponent<Button>();

        toggle.onClick.AddListener(delegate {
            SetReady(toggle);
        });
    }


    void SetReady(Button change)
    {
        if(playerManager == null)
        {
            playerManager = GetComponent<UI_PlayerConnection>().connectedPlayer;
        }
        playerManager.SetReadyRpc(true);
    }
  
}
