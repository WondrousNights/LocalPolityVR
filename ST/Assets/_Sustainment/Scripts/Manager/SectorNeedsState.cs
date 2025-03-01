using UnityEngine;

public class SectorNeedsState : TownGameState
{
    public override void EnterState(Town_GameManager gameManager)
    {
        Debug.Log("We be in the sector needs state");

        //Need to teleport all players to their prospective needs

        foreach(PlayerManager player in gameManager.playerManagers)
        {
            player.TeleportToSector();
        }
    }

    public override void ExitState(Town_GameManager gameManager)
    {
        
    }

    public override void UpdateState(Town_GameManager gameManager)
    {
        
    }
}
