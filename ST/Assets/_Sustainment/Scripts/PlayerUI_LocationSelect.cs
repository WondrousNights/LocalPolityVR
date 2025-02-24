using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[RequireComponent(typeof(UI_PlayerConnection))]
public class PlayerUI_LocationSelect : NetworkBehaviour
{
    TMP_Dropdown dropdown;

    PlayerManager playerManager;

    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();

        dropdown.onValueChanged.AddListener(SetLocation);
    }


    void SetLocation(int change)
    {
        if(playerManager == null)
        {
            playerManager = GetComponent<UI_PlayerConnection>().connectedPlayer;
        }

        if(change == 0)
        {
            //playerManager.SetDestinationRpc(Location.Forest);
        }
        if(change == 1)
        {
            //playerManager.SetDestinationRpc(Location.Quarry);
        }
        if(change == 2)
        {
            //playerManager.SetDestinationRpc(Location.House);
        }
    }
  
}
