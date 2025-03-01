using System.Collections.Generic;
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
    
    private List<TownGameState> GameStates;
    private int GameStateIndex = 0;

    void Start()
    {

        GameStates = new List<TownGameState>
        {
            LobbyState,
            SectorNeedsState
        };

        currentState = LobbyState;
        currentState.EnterState(this);

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
                manager.IsReady.Value = false;
            }
            GameStateIndex += 1;
            SwitchStateByInt(GameStateIndex);
        }
    }
}
