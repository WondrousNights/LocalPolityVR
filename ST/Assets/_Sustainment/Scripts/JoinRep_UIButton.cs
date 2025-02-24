using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class JoinRep_UIButton : NetworkBehaviour
{
    Button button;
    PlayerManager playerManager;

    [SerializeField] TMP_Text buttonText;

    public NetworkVariable<bool> IsSelected = new NetworkVariable<bool>(false);
    public NetworkVariable<ulong> PlayerSelectedID = new NetworkVariable<ulong>(0);
    [SerializeField] SectorType sectorType;

    void Start()
    {
        button = GetComponent<Button>();
        

        button.onClick.AddListener(delegate {
            JoinRep(button);
        });

        IsSelected.OnValueChanged += (oldValue, newValue) => UpdateButtonText();
    }

    public override void OnNetworkSpawn()
    {
        
        StartCoroutine(AssignValues());
    }

    private IEnumerator AssignValues()
    {
        // Wait until the NetworkManager is running and the player object is spawned
        while (!NetworkManager.Singleton.IsConnectedClient || 
               !NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject())
        {
            yield return null; // Keep waiting
        }

        if(IsSelected.Value == true)
        {
            buttonText.text = "Rep taken";
        }
        else
        {
            buttonText.text = "Join";
        }
    }

    void JoinRep(Button change)
    {
        
        if(playerManager == null)
        {
            playerManager = GetComponent<UI_PlayerConnection>().connectedPlayer;
        }

        if(IsSelected.Value == false)
        {
            playerManager.SetSectorRpc(sectorType);
            playerManager.SetReadyRpc(true);
            ToggleButtonRpc(true, playerManager.NetworkObjectId);
        }
        else
        {
            if(PlayerSelectedID.Value == playerManager.NetworkObjectId)
            {
                playerManager.SetSectorRpc(SectorType.NotSet);
                 playerManager.SetReadyRpc(false);
                ToggleButtonRpc(false, 0);
            }
        }

    }

    [Rpc(SendTo.Server)]
    void ToggleButtonRpc(bool value, ulong playerSelected)
    {
        IsSelected.Value = value;
        PlayerSelectedID.Value = playerSelected;
    }

    void UpdateButtonText()
    {
        if(IsSelected.Value == true)
        {
            buttonText.text = "Rep taken";
        }
        else
        {
            buttonText.text = "Join";
        }

    }

}
