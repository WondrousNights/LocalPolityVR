using UnityEngine;

public class VoteState : TownGameState
{
    public override void EnterState(Town_GameManager gameManager)
    {
        gameManager.EnableVotingScreensRpc();
        gameManager.SetTownHallTextRpc("Vote on proposal");
    }

    public override void ExitState(Town_GameManager gameManager)
    {
        gameManager.DisableVotingScreensRpc();
    }

    public override void UpdateState(Town_GameManager gameManager)
    {
        //throw new System.NotImplementedException();
    }


}
