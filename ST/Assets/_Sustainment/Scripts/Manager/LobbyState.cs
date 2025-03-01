using UnityEngine;

public class LobbyState : TownGameState
{
    public override void EnterState(Town_GameManager gameManager)
    {
        Debug.Log("We be in the lobby");
    }

    public override void ExitState(Town_GameManager gameManager)
    {
        //throw new System.NotImplementedException();
    }

    public override void UpdateState(Town_GameManager gameManager)
    {
        //throw new System.NotImplementedException();
    }
}
