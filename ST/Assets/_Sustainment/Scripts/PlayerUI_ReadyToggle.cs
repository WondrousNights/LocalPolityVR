using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UI_PlayerConnection))]
public class PlayerUI_ReadyToggle : NetworkBehaviour
{
    Toggle toggle;

    PlayerManager playerManager;

    void Start()
    {
        toggle = GetComponent<Toggle>();

        toggle.onValueChanged.AddListener(delegate {
            SetReady(toggle);
        });
    }


    void SetReady(Toggle change)
    {
        if(playerManager == null)
        {
            playerManager = GetComponent<UI_PlayerConnection>().connectedPlayer;
        }
        playerManager.SetReadyRpc(change.isOn);
    }
  
}
