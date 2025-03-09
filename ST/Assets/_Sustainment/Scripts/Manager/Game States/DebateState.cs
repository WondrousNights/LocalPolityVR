using System;
using UnityEngine;

public class DebateState : TownGameState
{
    float count = 0;

    float timeToDebate = 60f;
    public override void EnterState(Town_GameManager gameManager)
    {
        count = 0;
    }

    public override void ExitState(Town_GameManager gameManager)
    {
        //throw new System.NotImplementedException();
    }

    public override void UpdateState(Town_GameManager gameManager)
    {
       count += Time.deltaTime;
        string message = "Debate for " + Math.Round(timeToDebate - count);
        gameManager.SetTownHallTextRpc(message);

       if(count >= timeToDebate)
       {
            gameManager.IncreaseGameStateIndex();
            gameManager.SwitchStateByInt(gameManager.GameStateIndex);
       }
    }
}
