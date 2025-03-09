using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Town_GameManager : NetworkBehaviour
{

    private Town_GameManager() { }
    private static Town_GameManager Instance;

    public static Town_GameManager GetInstance()
    {
        return Instance;
    }
      void Awake()
    {
       if (Instance != null)
        {
            Debug.Log("Duplicate GameManager found, destroying");
            Destroy(this);
            return;
        }
        Instance = this;
    }


    //Managing States
    TownGameState currentState;

    public LobbyState LobbyState = new LobbyState();
    public SectorNeedsState SectorNeedsState = new SectorNeedsState();
    
    public PolicyIntroductionState PolicyIntroductionState = new PolicyIntroductionState();
    public DebateState DebateState = new DebateState();
    public VoteState VoteState = new VoteState();

    private List<TownGameState> GameStates;
    public int GameStateIndex = -1;

    void Start()
    {
        currentState = LobbyState;
        currentState.EnterState(this);
        
        GameStates = new List<TownGameState>
        {
            SectorNeedsState,
            PolicyIntroductionState,
            DebateState,
            VoteState
        };



    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchStateByState(TownGameState state)
    {
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }

    public void SwitchStateByInt(int state)
    {
        currentState.ExitState(this);
        currentState = GameStates[state];
        currentState.EnterState(this);
    }



    //Managing Player
    public List<PlayerManager> playerManagers = new List<PlayerManager>();


    [Rpc(SendTo.Server, RequireOwnership = false)]
    public void CheckPlayerReadyStatusRpc()
    {
        int connectedPlayerCount = playerManagers.Count;

        //if(connectedPlayerCount < 3) return;

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
            foreach(PlayerManager manager in playerManagers)
            {
                manager.SetReadyRpc(false);
            }
            IncreaseGameStateIndex();
            SwitchStateByInt(GameStateIndex);
        }
    }

    public void IncreaseGameStateIndex()
    {
        GameStateIndex += 1;

        if(GameStateIndex > GameStates.Count - 1)
        {
            GameStateIndex = 0;
        }
    }

    [Rpc(SendTo.Everyone)]
    public void TeleportPlayersToSectorRpc()
    {
        foreach(PlayerManager player in playerManagers)
        {
            player.TeleportToSectorRpc();
        }
    }

    [Rpc(SendTo.Everyone)]
    public void TeleportPlayersToHallRpc()
    {
        foreach(PlayerManager player in playerManagers)
        {
            player.TeleportToTownHallRpc();
        }
    }


    //Managing UI

    public TextMeshProUGUI TownHallText;
    public List<GameObject> VotingScreens = new List<GameObject>();

    [Rpc(SendTo.Everyone)]
    public void SetTownHallTextRpc(string text)
    {
        TownHallText.text = text;
    }

    [Rpc(SendTo.Everyone)]
    public void EnableVotingScreensRpc()
    {
        foreach(GameObject gameObject in VotingScreens)
        {
            gameObject.SetActive(true);
        }
    }

    [Rpc(SendTo.Everyone)]
    public void DisableVotingScreensRpc()
    {
         foreach(GameObject gameObject in VotingScreens)
        {
            gameObject.SetActive(false);
        }
    }



}
