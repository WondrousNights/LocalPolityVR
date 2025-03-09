using System;
using System.Collections.Generic;
using UnityEngine;

public class PolicyIntroductionState : TownGameState
{

    int timeToStart = 15;
    int timePerPolicyIntroduction = 60;
    bool PolicyDiscussionStarted = false;
    float count;

    int sectorToSpeak = 0;


    List<String> SectorsNeedingToBeRepresented = new List<string>();
    public override void EnterState(Town_GameManager gameManager)
    {
        count = 0;
        PolicyDiscussionStarted = false;

        gameManager.TeleportPlayersToHallRpc();

        Debug.Log("We be in the Policy Introduction State");
        foreach(PlayerManager player in gameManager.playerManagers)
        {
            SectorsNeedingToBeRepresented.Add(player.SectorRepresenting.Value.ToString());
        }
    }

    public override void ExitState(Town_GameManager gameManager)
    {

    }

    public override void UpdateState(Town_GameManager gameManager)
    {
        count += Time.deltaTime;
        Debug.Log(count);
        if(!PolicyDiscussionStarted && count >= timeToStart)
        {
            PolicyDiscussionStarted = true;
            count = 0;

        }

        if(PolicyDiscussionStarted)
        {
            string textToDisplay = SectorsNeedingToBeRepresented[sectorToSpeak] + " has the floor for : " + Mathf.Round(timePerPolicyIntroduction - count);
            gameManager.SetTownHallTextRpc(textToDisplay);

            if(count >= timePerPolicyIntroduction)
            {
                sectorToSpeak += 1;
                count = 0;
            }

            if(sectorToSpeak > SectorsNeedingToBeRepresented.Count - 1)
            {
                gameManager.IncreaseGameStateIndex();
                gameManager.SwitchStateByInt(gameManager.GameStateIndex);
            }
        }

        
    }
}
